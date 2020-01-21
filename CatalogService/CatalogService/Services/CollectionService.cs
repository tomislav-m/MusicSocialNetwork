using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Services
{
    public interface ITagService
    {
        Task<IEnumerable<UserAlbum>> AddToCollection(UserAlbum userAlbum);
        Task<Tag> CreateTag(Tag Tag);
        Task<Tag> GetTag(int TagId);
        Task<Tag> GetTag(string TagName, int userId);
    }

    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAlbum>> AddToCollection(UserAlbum userAlbum)
        {
            await _context.UserAlbums.AddAsync(userAlbum);
            await _context.SaveChangesAsync();

            return await _context.UserAlbums.Where(x => x.UserId == userAlbum.UserId).ToListAsync();
        }

        public async Task<Tag> CreateTag(Tag Tag)
        {
            await _context.Tags.AddAsync(Tag);
            await _context.SaveChangesAsync();

            return Tag;
        }

        public async Task<Tag> GetTag(int TagId)
        {
            return await _context.Tags.Where(x => x.Id == TagId)
                //.Include(x => x.Albums)
                .SingleOrDefaultAsync();
        }

        public async Task<Tag> GetTag(string TagName, int userId)
        {
            return await _context.Tags.Where(x => x.Name == TagName && x.UserId == userId)
                //.Include(x => x.Albums)
                .SingleOrDefaultAsync();
        }
    }
}
