using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> AddAsync(Comment comment);
        Task<Comment> DeleteAsync(int id);  
        Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject);
        Task<Comment> GetByIdAsync(int id);
        Task<Comment?> UpdateAsync(int id, Comment comment);
    }
}
