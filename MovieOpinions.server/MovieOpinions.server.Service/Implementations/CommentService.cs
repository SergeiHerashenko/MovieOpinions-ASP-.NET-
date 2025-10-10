using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.Comments;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MovieOpinions.server.Service.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserService _userService;

        public CommentService(ICommentRepository commentRepository, IUserService userService)
        {
            _userService = userService;
            _commentRepository = commentRepository;
        }

        public async Task<BaseResponse<List<Comment>>> GetAllCommentFilm(int idFilm)
        {
            var getAllCommentByFilm = await _commentRepository.GetCommentFilm(idFilm);

            if(getAllCommentByFilm.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<List<Comment>>()
                {
                    StatusCode = getAllCommentByFilm.StatusCode,
                    Description = getAllCommentByFilm.Description,
                };
            }
            else
            {
                foreach(var comment in getAllCommentByFilm.Data)
                {
                    var getUserProfile = await _userService.GetUserProfileById(comment.IdUser);

                    if (getUserProfile.StatusCode != Domain.Enum.StatusCode.OK)
                    {
                        return new BaseResponse<List<Comment>>()
                        {
                            StatusCode = getUserProfile.StatusCode,
                            Description = getUserProfile.Description,
                        };
                    }

                    if(string.IsNullOrEmpty(getUserProfile.Data.FirstName))
                    {
                        comment.User.FirstName = "User" + Guid.NewGuid().ToString("N").Substring(0, 8);
                    }
                    else
                    {
                        comment.User.FirstName = getUserProfile.Data.FirstName;
                    }

                    if (string.IsNullOrWhiteSpace(getUserProfile.Data.LastName))
                    {
                        comment.User.LastName = "Anonymous";
                    }
                    else
                    {
                        comment.User.LastName = getUserProfile.Data.LastName;
                    }

                    comment.User.AvatarUrl = getUserProfile.Data.AvatarUrl ?? "/Images/Default/default-avatar.png";
                    comment.User.Bio = getUserProfile.Data.Bio ?? "No bio provided.";
                    comment.User.CreatedAt = getUserProfile.Data.CreatedAt;
                    comment.User.UpdatedAt = getUserProfile.Data.UpdatedAt;
                }

                Dictionary<int, Comment> allComments = new Dictionary<int, Comment>();

                foreach(var comment in getAllCommentByFilm.Data)
                {
                    allComments[comment.IdComment] = comment;
                    comment.Replies = new List<Comment>();
                }

                foreach (var comment in getAllCommentByFilm.Data)
                {
                    if (comment.ParentCommentId.HasValue)
                    {
                        allComments[comment.ParentCommentId.Value].Replies.Add(comment);
                    }
                }

                var topLevelComments = allComments
                    .Where(c => c.Value.ParentCommentId == null)
                    .Select(c => c.Value)
                    .ToList();

                return new BaseResponse<List<Comment>>()
                {
                    StatusCode = Domain.Enum.StatusCode.OK,
                    Data = topLevelComments
                };
            }
        }
    }
}
