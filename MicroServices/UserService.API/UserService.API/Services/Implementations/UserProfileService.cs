using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserDbContext _db;

        public UserProfileService(UserDbContext db)
        {
            _db = db;
        }

        private async Task<(List<Guid> skillIds, List<Guid> traitIds)> SplitTraitsAndSkillsAsync(List<Guid> ids)
        {
            var skillIds = await _db.Skills
                .Where(s => ids.Contains(s.Id))
                .Select(s => s.Id)
                .ToListAsync();

            var traitIds = ids.Except(skillIds).ToList();
            return (skillIds, traitIds);
        }

        public async Task<BaseResponse> CreateAsync(UserProfilePayload request)
        {
            try
            {
                if (await _db.UserProfiles.AnyAsync(x => x.AccountId == request.AccountId))
                    return new BaseResponse { Success = false, Message = "Tài khoản đã có profile." };

                var entity = new UserProfile
                {
                    Id = request.AccountId,
                    AccountId = request.AccountId,
                    FullName = request.FullName,
                    RoleType = request.RoleType,
                    LocationId = request.LocationId == Guid.Empty ? null : request.LocationId,
                    CityId = request.CityId,
                    IsCompleted = false,
                    Interests = string.Join(",", request.InterestIds ?? new List<Guid>()),
                    PersonalityTraits = string.Join(",", request.PersonalityTraitIds ?? new List<Guid>()),
                    Phone = request.Phone,
                    Gender = request.Gender,
                    DOB = string.IsNullOrEmpty(request.DOB) ? null : DateTime.Parse(request.DOB),
                    Address = request.Address,
                    Note = request.Note,
                    OnboardingStatus = request.OnboardingStatus ?? "Pending",
                    CreatedByAdminId = request.CreatedByAdminId,
                    CreatedAt = DateTime.UtcNow
                };

                await _db.UserProfiles.AddAsync(entity);

                if (request.InterestIds != null)
                {
                    foreach (var id in request.InterestIds)
                    {
                        _db.UserInterests.Add(new UserInterest
                        {
                            UserProfileId = request.AccountId,
                            InterestId = id
                        });
                    }
                }

                if (request.PersonalityTraitIds != null)
                {
                    var (skillIds, traitIds) = await SplitTraitsAndSkillsAsync(request.PersonalityTraitIds);

                    foreach (var id in skillIds)
                    {
                        _db.UserSkills.Add(new UserSkill
                        {
                            UserProfileId = request.AccountId,
                            SkillId = id
                        });
                    }

                    foreach (var id in traitIds)
                    {
                        _db.UserPersonalityTraits.Add(new UserPersonalityTrait
                        {
                            UserProfileId = request.AccountId,
                            PersonalityTraitId = id
                        });
                    }
                }

                await _db.SaveChangesAsync();

                return new BaseResponse { Success = true, Message = "Tạo hồ sơ thành công." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = $"Lỗi khi lưu profile: {ex.Message}" };
            }
        }

        public async Task<UserProfileDto> GetProfileAsync(Guid accountId)
        {
            var user = await _db.UserProfiles
                .Include(u => u.Property)
                .FirstOrDefaultAsync(u => u.AccountId == accountId);

            if (user == null) return null!;

            var interestNames = await _db.UserInterests
                .Where(ui => ui.UserProfileId == accountId)
                .Include(ui => ui.Interest)
                .Select(ui => ui.Interest.Name)
                .ToListAsync();

            var traitNames = await _db.UserPersonalityTraits
                .Where(up => up.UserProfileId == accountId)
                .Include(up => up.PersonalityTrait)
                .Select(up => up.PersonalityTrait.Name)
                .ToListAsync();

            var skillNames = await _db.UserSkills
                .Where(us => us.UserProfileId == accountId)
                .Include(us => us.Skill)
                .Select(us => us.Skill.Name)
                .ToListAsync();

            return new UserProfileDto
            {
                AccountId = user.AccountId,
                FullName = user.FullName,
                Phone = user.Phone,
                Gender = user.Gender,
                DOB = user.DOB,
                AvatarUrl = user.AvatarUrl,
                SocialLinks = user.SocialLinks,
                LocationId = user.LocationId,
                LocationName = user.Property?.Name,
                Address = user.Address,
                OnboardingStatus = user.OnboardingStatus,
                Note = user.Note,
                Interests = interestNames,
                PersonalityTraits = traitNames.Concat(skillNames).ToList(),
                CvUrl = user.CvUrl,
                Introduction = user.Introduction
            };
        }

        public async Task<UserProfileResponseDto> UpdateProfileAsync(Guid accountId, UpdateUserProfileDto dto)
        {
            var user = await _db.UserProfiles
                .Include(u => u.Property)
                .FirstOrDefaultAsync(u => u.AccountId == accountId);

            if (user == null)
            {
                return new UserProfileResponseDto
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            if (dto.FullName != null) user.FullName = dto.FullName;
            if (dto.Phone != null) user.Phone = dto.Phone;
            if (dto.Gender != null) user.Gender = dto.Gender;
            if (dto.DOB.HasValue) user.DOB = dto.DOB;
            if (dto.AvatarUrl != null) user.AvatarUrl = dto.AvatarUrl;
            if (dto.SocialLinks != null) user.SocialLinks = dto.SocialLinks;
            if (dto.Address != null) user.Address = dto.Address;
            if (dto.Introduction != null) user.Introduction = dto.Introduction;
            if (dto.CvUrl != null) user.CvUrl = dto.CvUrl;
            if (dto.Note != null) user.Note = dto.Note;

            // Cập nhật InterestIds
            if (dto.InterestIds != null)
            {
                var validInterestIds = await _db.Interests
                    .Where(i => dto.InterestIds.Contains(i.Id))
                    .Select(i => i.Id)
                    .ToListAsync();

                var invalidIds = dto.InterestIds.Except(validInterestIds).ToList();
                if (invalidIds.Any())
                {
                    return new UserProfileResponseDto
                    {
                        Success = false,
                        Message = $"Invalid InterestId(s): {string.Join(", ", invalidIds)}"
                    };
                }

                var oldInterests = await _db.UserInterests.Where(x => x.UserProfileId == accountId).ToListAsync();
                _db.UserInterests.RemoveRange(oldInterests);

                if (dto.InterestIds.Any())
                {
                    foreach (var id in dto.InterestIds)
                    {
                        _db.UserInterests.Add(new UserInterest
                        {
                            UserProfileId = accountId,
                            InterestId = id
                        });
                    }
                    user.Interests = string.Join(",", dto.InterestIds);
                }
                else
                {
                    user.Interests = "";
                }
            }

            // Cập nhật PersonalityTraitIds (gồm cả Trait và Skill)
            if (dto.PersonalityTraitIds != null)
            {
                var allTraitIds = dto.PersonalityTraitIds;

                var validSkillIds = await _db.Skills
                    .Where(s => allTraitIds.Contains(s.Id))
                    .Select(s => s.Id)
                    .ToListAsync();

                var validTraitIds = await _db.PersonalityTraits
                    .Where(t => allTraitIds.Contains(t.Id))
                    .Select(t => t.Id)
                    .ToListAsync();

                var validIds = validSkillIds.Concat(validTraitIds).ToHashSet();
                var invalidIds = allTraitIds.Where(id => !validIds.Contains(id)).ToList();

                if (invalidIds.Any())
                {
                    return new UserProfileResponseDto
                    {
                        Success = false,
                        Message = $"Invalid PersonalityTraitId(s): {string.Join(", ", invalidIds)}"
                    };
                }

                _db.UserPersonalityTraits.RemoveRange(_db.UserPersonalityTraits.Where(x => x.UserProfileId == accountId));
                _db.UserSkills.RemoveRange(_db.UserSkills.Where(x => x.UserProfileId == accountId));

                if (dto.PersonalityTraitIds.Any())
                {
                    var (skillIds, traitIds) = await SplitTraitsAndSkillsAsync(dto.PersonalityTraitIds);

                    foreach (var id in skillIds)
                    {
                        _db.UserSkills.Add(new UserSkill
                        {
                            UserProfileId = accountId,
                            SkillId = id
                        });
                    }

                    foreach (var id in traitIds)
                    {
                        _db.UserPersonalityTraits.Add(new UserPersonalityTrait
                        {
                            UserProfileId = accountId,
                            PersonalityTraitId = id
                        });
                    }

                    user.PersonalityTraits = string.Join(",", dto.PersonalityTraitIds);
                }
                else
                {
                    user.PersonalityTraits = "";
                }
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var Property = await _db.Propertys
                .FirstOrDefaultAsync(r => r.Id == user.LocationId);

            var personalityNames = await _db.UserPersonalityTraits
                .Where(x => x.UserProfileId == accountId)
                .Include(x => x.PersonalityTrait)
                .Select(x => x.PersonalityTrait.Name)
                .ToListAsync();

            var skillNames = await _db.UserSkills
                .Where(x => x.UserProfileId == accountId)
                .Include(x => x.Skill)
                .Select(x => x.Skill.Name)
                .ToListAsync();

            return new UserProfileResponseDto
            {
                Success = true,
                Message = "Profile updated successfully.",
                Data = new UserProfileDto
                {
                    AccountId = user.AccountId,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Gender = user.Gender,
                    DOB = user.DOB,
                    AvatarUrl = user.AvatarUrl,
                    SocialLinks = user.SocialLinks,
                    Address = user.Address,
                    LocationId = user.LocationId,
                    LocationName = Property?.Name,
                    OnboardingStatus = user.OnboardingStatus,
                    Note = user.Note,
                    UpdatedAt = user.UpdatedAt,
                    Interests = await _db.UserInterests
                        .Where(i => i.UserProfileId == accountId)
                        .Include(i => i.Interest)
                        .Select(i => i.Interest.Name)
                        .ToListAsync(),
                    PersonalityTraits = personalityNames.Concat(skillNames).ToList(),
                    Introduction = user.Introduction,
                    CvUrl = user.CvUrl
                }
            };
        }



        public async Task<List<UserProfileShortDto>> GetProfilesByAccountIdsAsync(List<Guid> accountIds)
        {
            var profiles = await _db.UserProfiles
                .Where(p => accountIds.Contains(p.AccountId))
                .Include(p => p.Property)
                .Select(p => new UserProfileShortDto
                {
                    AccountId = p.AccountId,
                    LocationId = p.LocationId,
                    LocationName = p.Property != null ? p.Property.Name : null,
                    RoleType = p.RoleType
                })
                .ToListAsync();

            return profiles;
        }

        public async Task<BaseResponse> UpdateStatusAsync(Guid accountId, UpdateUserProfileStatusDto dto)
        {
            try
            {
                var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);

                if (user == null)
                    return new BaseResponse { Success = false, Message = "Không tìm thấy hồ sơ người dùng." };

                user.OnboardingStatus = "ApprovedMember";

                if (user.RoleType?.ToLower() != "member")
                {
                    user.RoleType = "member";
                }

                user.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return new BaseResponse { Success = true, Message = "Cập nhật trạng thái hồ sơ thành công." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = $"Lỗi khi cập nhật trạng thái hồ sơ: {ex.Message}" };
            }
        }

        public async Task<List<UserProfileShortDto>> GetProfilesByRoleKeysAsync(string[] roleKeys)
        {
            var profiles = await _db.UserProfiles
                .Include(p => p.Property)
                .Where(p => roleKeys.Contains(p.RoleType))
                .ToListAsync();

            return profiles.Select(p => new UserProfileShortDto
            {
                AccountId = p.AccountId,
                RoleType = p.RoleType,
                LocationId = p.LocationId,
                LocationName = p.Property?.Name ?? ""
            }).ToList();
        }

        public async Task<UserProfileShortDto?> GetProfileShortDtoAsync(Guid accountId)
        {
            return await _db.UserProfiles
                .Where(x => x.AccountId == accountId)
                .Select(x => new UserProfileShortDto
                {
                    AccountId = x.AccountId,
                    LocationId = x.LocationId,
                    LocationName = x.Property.Name,
                    RoleType = x.RoleType,
                    CityId = x.CityId // 👈 THÊM DÒNG NÀY
                })
                .FirstOrDefaultAsync();
        }




        public async Task<UserProfile?> GetCurrentUserProfileAsync(Guid accountId)
        {
            return await _db.UserProfiles
                .Include(u => u.UserInterests).ThenInclude(i => i.Interest)
                .Include(u => u.UserPersonalityTraits).ThenInclude(p => p.PersonalityTrait)
                .Include(u => u.UserSkills).ThenInclude(s => s.Skill)
                .FirstOrDefaultAsync(u => u.AccountId == accountId);
        }


    }
}
