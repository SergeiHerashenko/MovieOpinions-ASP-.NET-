using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<BaseResponse<UserEntity>> GetUserByIdAsync(Guid idUser)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<UserProfile>> GetUserProfileById(Guid idUser)
        {
            var getProfileUser = await _userRepository.GetUserId(idUser);

            if (getProfileUser.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<UserProfile>()
                {
                    Description = getProfileUser.Description,
                    StatusCode = getProfileUser.StatusCode,
                };
            }

            var profileUser = getProfileUser.Data.Profile;

            UserProfile userProfile = new UserProfile()
            {
                FirstName = profileUser.FirstName,
                LastName = profileUser.LastName,
                AvatarUrl = profileUser.AvatarUrl,
                Bio = profileUser.Bio,
                CreatedAt = profileUser.CreatedAt,
                UpdatedAt = profileUser.UpdatedAt
            };

            return new BaseResponse<UserProfile>()
            {
                Data = userProfile,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
    }
}
