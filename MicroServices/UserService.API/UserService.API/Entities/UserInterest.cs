using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.Entities
{
    public class UserInterest
    {
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public Guid InterestId { get; set; }
        public Interest Interest { get; set; }
    }
}
