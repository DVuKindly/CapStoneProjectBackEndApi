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




        public async Task<bool> CreateHoldBookingAsync(CreateHoldBookingRequest request)
        {
            var endDate = CalculateEndDate(request.StartDate, request.DurationValue, request.DurationUnit);
            var expireAt = request.PackageType.ToLower() switch
            {
                "basic" => DateTime.UtcNow.AddMinutes(15),
                "combo" => DateTime.UtcNow.AddDays(3),
                _ => DateTime.UtcNow.AddMinutes(10)
            };

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                MemberId = request.MemberId,
                RoomInstanceId = request.RoomInstanceId,
                StartDate = request.StartDate,
                EndDate = endDate,
                Status = BookingStatus.Hold,
                ExpireAt = expireAt,
                Note = "Hold booking created before payment"
            };

            _context.Bookings.Add(booking);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> ConfirmBookingAsync(ConfirmBookingRequest request)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b =>
                b.MemberId == request.MemberId &&
                b.RoomInstanceId == request.RoomInstanceId &&
                b.StartDate.Date == request.StartDate.Date &&
                b.Status == BookingStatus.Hold);

            if (booking == null)
                return false;

            booking.Status = BookingStatus.Confirmed;
            booking.ExpireAt = null;

            await  _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> AutoCancelExpiredBookingsAsync()
        {
            var now = DateTime.UtcNow;
            var expired = await _context.Bookings
                .Where(b => b.Status == BookingStatus.Hold && b.ExpireAt != null && b.ExpireAt < now)
                .ToListAsync();

            foreach (var booking in expired)
            {
                booking.Status = BookingStatus.Cancelled;
            }

            var updated = await _context.SaveChangesAsync();
            return updated;
        }

        private DateTime CalculateEndDate(DateTime start, int value, string unit)
        {
            return unit.ToLower() switch
            {
                "day" => start.AddDays(value),
                "week" => start.AddDays(value * 7),
                "month" => start.AddMonths(value),
                "year" => start.AddYears(value),
                _ => start
            };
        }

        public async Task<bool> CancelHoldBookingAsync(CancelHoldBookingRequest request)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b =>
                b.MemberId == request.MemberId &&
                b.RoomInstanceId == request.RoomInstanceId &&
                b.StartDate == request.StartDate &&
                b.Status == BookingStatus.Hold);

            if (booking == null) return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
