using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BookingResponseDto> CreateAsync(CreateBookingRequest request)
        {
            var entity = _mapper.Map<Booking>(request);
            var created = await _repo.AddAsync(entity);
            return _mapper.Map<BookingResponseDto>(created);
        }

        public async Task<List<BookingResponseDto>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime from, DateTime to)
        {
            var bookings = await _repo.GetRoomBookingsAsync(roomInstanceId, from, to);
            return _mapper.Map<List<BookingResponseDto>>(bookings);
        }
    }
}
