using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            await _dbContext.Set<Comment>().AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> DeleteAsync(int id)
        {
            var comment = await _dbContext.Set<Comment>().FindAsync(id);

            _dbContext.Set<Comment>().Remove(comment);
            await _dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments = _dbContext.Set<Comment>()
                                     .Include(c => c.AppUser)
                                     .AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                comments = comments.Where(c => c.Stock.Symbol.Equals(queryObject.Symbol));
            }

            if(queryObject.IsDescending)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }

            return await comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Comment>().FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var entry = await _dbContext.Set<Comment>().FindAsync(id);

            if(entry is null)
            {
                return null;
            }

            comment.Id = id;

            _dbContext.Entry<Comment>(entry).CurrentValues.SetValues(comment);

            await _dbContext.SaveChangesAsync();

            return comment;
        }
    }
}
