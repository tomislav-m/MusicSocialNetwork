using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Services
{
    public interface IRatingService
    {
        Task<List<AlbumRating>> GetAlbumRatings(int albumId);
        Task<float> GetAlbumAverageRating(int albumId);
        Task<AlbumRating> RateAlbum(AlbumRating rating);
    }

    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AlbumRating>> GetAlbumRatings(int albumId)
        {
            return await _context.AlbumRatings.Where(x => x.AlbumId == albumId).ToListAsync();
        }

        public async Task<float> GetAlbumAverageRating(int albumId)
        {
            var ratings = _context.AlbumRatings.Where(x => x.AlbumId == albumId);

            return (await ratings.SumAsync(x => x.Rating) / await ratings.CountAsync());
        }

        public async Task<AlbumRating> RateAlbum(AlbumRating rating)
        {
            _context.AlbumRatings.Add(rating);
            await _context.SaveChangesAsync();

            return rating;
        }
    }
}
