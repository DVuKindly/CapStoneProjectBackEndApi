using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.Entities
{

    public class UserPersonalityTrait
    {
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public Guid PersonalityTraitId { get; set; }
        public PersonalityTrait PersonalityTrait { get; set; }
    }
}
