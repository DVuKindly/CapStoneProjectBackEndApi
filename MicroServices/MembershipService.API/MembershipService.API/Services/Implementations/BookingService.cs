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
            entity.Status = BookingStatus.Hold;
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

        public async Task<List<BookingResponseDto>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime from, DateTime to)
        {
            var bookings = await _repo.GetRoomBookingsAsync(roomInstanceId, from, to); // chỉ các booking trùng khoảng

            var bookingResponses = _mapper.Map<List<BookingResponseDto>>(bookings);

            // Nếu không có booking nào trùng: phòng rảnh hoàn toàn
            if (!bookings.Any())
            {
                return new List<BookingResponseDto>
        {
            new BookingResponseDto
            {
                RoomInstanceId = roomInstanceId,
                ViewedBookingStatus = "available",
                StartDate = from,
                EndDate = to
            }
        };
            }

            // Nếu có ít nhất 1 booking trùng → vướng lịch → phải ghi Available từ ngày trống tiếp theo
            var maxEndDate = bookings.Max(b => b.EndDate).AddDays(1);

            return new List<BookingResponseDto>
    {
        new BookingResponseDto
        {
            RoomInstanceId = roomInstanceId,
            ViewedBookingStatus = $"available from {maxEndDate:yyyy-MM-dd}",
            StartDate = from,
            EndDate = to
        }
    };
        }



        public async Task<bool> ValidateBookingAsync(Guid roomInstanceId, Guid bookingId, DateTime startDate)
        {
            return await _context.Bookings
                .AnyAsync(b =>
                    b.RoomInstanceId == roomInstanceId &&
                    b.StartDate.Date == startDate.Date &&
                    b.Status != BookingStatus.Cancelled);
        }


    }
}
