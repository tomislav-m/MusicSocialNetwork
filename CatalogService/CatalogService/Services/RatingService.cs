using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Services
{
    public interface IRatingService
    {
        Task<IEnumerable<AlbumRating>> GetAlbumRatings(int albumId);
        Task<IEnumerable<AlbumRating>> GetUserRatings(int userId);
        Task<Tuple<float, int>> GetAlbumAverageRating(int albumId);
        Task<AlbumRating> RateAlbum(AlbumRating rating);
    }

    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AlbumRating>> GetAlbumRatings(int albumId)
        {
            return await _context.AlbumRatings.Where(x => x.AlbumId == albumId).ToListAsync();
        }

        public async Task<IEnumerable<AlbumRating>> GetUserRatings(int userId)
        {
            return await _context.AlbumRatings.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Tuple<float, int>> GetAlbumAverageRating(int albumId)
        {
            var ratings = _context.AlbumRatings.Where(x => x.AlbumId == albumId);
            var count = await ratings.CountAsync();

            var avgRating = count > 0 ? (await ratings.SumAsync(x => x.Rating) / count) : 0;

            return new Tuple<float, int>(avgRating, count);
        }

        public async Task<AlbumRating> RateAlbum(AlbumRating rating)
        {
            var oldRating = await _context.AlbumRatings
                .SingleOrDefaultAsync(x => x.AlbumId == rating.AlbumId && x.UserId == rating.UserId);

            if (oldRating == null)
            {
                _context.AlbumRatings.Add(rating);
            }
            else if (oldRating.Rating != rating.Rating)
            {
                oldRating.Rating = rating.Rating;
                _context.AlbumRatings.Update(oldRating);
            }

            await _context.SaveChangesAsync();

            return rating;
        }
    }
}
