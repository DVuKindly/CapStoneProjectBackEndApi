using AutoMapper;
using MembershipService.API.Data;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Enums;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;
        private readonly MembershipDbContext _context;

        public BookingService(IBookingRepository repo, IMapper mapper, MembershipDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BookingResponseDto> CreateAsync(CreateBookingRequest request)
        {
            bool isOverlap = await _context.Bookings.AnyAsync(b =>
                b.RoomInstanceId == request.RoomInstanceId &&
                b.Status != BookingStatus.Cancelled &&
                b.StartDate < request.EndDate &&
                b.EndDate > request.StartDate);

            if (isOverlap)
                throw new InvalidOperationException("Phòng đã có người đặt trong thời gian này.");

            var entity = _mapper.Map<Booking>(request);
            entity.Status = BookingStatus.Confirmed;
            entity.CreatedAt = DateTime.UtcNow;
            entity.MemberId = request.MemberId;



            var created = await _repo.AddAsync(entity);
            return _mapper.Map<BookingResponseDto>(created);
        }



        //public async Task<List<BookingResponseDto>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime from, DateTime to)
        //{
        //    var bookings = await _repo.GetRoomBookingsAsync(roomInstanceId, from, to);
        //    return _mapper.Map<List<BookingResponseDto>>(bookings);
        //}

        public async Task<List<BookingResponseDto>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime? from, DateTime? to)
        {
            // Lấy toàn bộ bookings đã confirmed
            var allBookings = await _repo.GetRoomBookingsAsync(roomInstanceId, null, null);
            if (!allBookings.Any()) // nếu không có gì response avalable
            {
                return new List<BookingResponseDto>
                {
                    new BookingResponseDto
                    {
                        BookingId = Guid.Empty,
                        RoomInstanceId = roomInstanceId,
                        StartDate = null,
                        EndDate = null,
                        Note = null,
                        Status = null,
                        ViewedBookingStatus = "available"
                    }
                };
            }
            // TH1: from và to đều null
            if (from == null && to == null)
            {
                // Tìm booking muộn nhất từ hôm nay trở đi
                var latestBooking = allBookings
                .OrderByDescending(b => b.EndDate)
                .FirstOrDefault();
                return new List<BookingResponseDto>
                {
                    new BookingResponseDto
                    {
                        BookingId = Guid.Empty,
                        RoomInstanceId = roomInstanceId,
                        StartDate = latestBooking.StartDate,
                        EndDate = latestBooking.EndDate,
                        Status = "available",
                        ViewedBookingStatus = $"available from {latestBooking.EndDate.AddDays(1):yyyy-MM-dd}",
                        Note = null
                    }
                };
            }
    
            // TH2 & TH3: from và to có giá trị
            var fromDate = from.Value.Date;
            var toDate = to.Value.Date;
    
            // Kiểm tra xem khoảng thời gian [from, to] có overlap với bất kỳ booking nào không
            var overlappingBookings = allBookings
                .Where(b => b.StartDate <= toDate && b.EndDate >= fromDate)
                .OrderBy(b => b.StartDate)
                .ToList();
    
            if (overlappingBookings.Any())
            {
                // TH2: Có overlap → hiển thị available sau ngày EndDate của booking cuối cùng
                var lastBooking = overlappingBookings.OrderBy(b => b.EndDate).Last();
                return new List<BookingResponseDto>
                {
                    new BookingResponseDto
                    {
                        BookingId = Guid.Empty,
                        RoomInstanceId = roomInstanceId,
                        ViewedBookingStatus = $"available from {lastBooking.EndDate.AddDays(1):yyyy-MM-dd}",
                        StartDate = fromDate,
                        EndDate = toDate,
                        Status = "available",
                        Note = null
                    }
                };
            }
            else
            {
                // TH3: Không có overlap → hiển thị available
                return new List<BookingResponseDto>
                {
                    new BookingResponseDto
                    {
                        BookingId = Guid.Empty,
                        RoomInstanceId = roomInstanceId,
                        ViewedBookingStatus = "available",
                        StartDate = fromDate,
                        EndDate = toDate,
                        Status = "available",
                        Note = null
                    }
                };
            }
        }







        public async Task<bool> ValidateBookingAsync(Guid roomInstanceId, DateTime startDate, DateTime endDate)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.RoomInstanceId == roomInstanceId &&
                b.Status != BookingStatus.Cancelled &&
                b.StartDate < endDate &&
                b.EndDate > startDate); // 🔥 Giao thời gian
        }



    }
}
