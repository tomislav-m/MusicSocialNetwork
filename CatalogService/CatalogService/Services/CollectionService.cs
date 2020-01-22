using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Services
{
    public interface ICollectionService
    {
        Task<IEnumerable<UserAlbum>> AddToCollection(UserAlbum userAlbum);
        Task<IEnumerable<UserAlbum>> GetCollection(int userId);
    }

    public class CollectionService : ICollectionService
    {
        private readonly ApplicationDbContext _context;

        public CollectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAlbum>> AddToCollection(UserAlbum userAlbum)
        {
            await _context.UserAlbums.AddAsync(userAlbum);
            await _context.SaveChangesAsync();

            return await _context.UserAlbums.Where(x => x.UserId == userAlbum.UserId).ToListAsync();
        }

        public async Task<IEnumerable<UserAlbum>> GetCollection(int userId)
        {
            return await _context.UserAlbums.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetPopularAlbums()
        {
            var dict = await _context.AlbumRatings
                .GroupBy(x => x.AlbumId)
                .ToDictionaryAsync(x => x.Key, x => x.Count());
            return dict.OrderByDescending(x => x.Value)
                .Take(1000)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
