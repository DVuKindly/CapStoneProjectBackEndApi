﻿namespace MembershipService.API.Dtos.Response
{
    public class EcosystemResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
