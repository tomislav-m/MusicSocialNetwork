using Microsoft.EntityFrameworkCore;
using RecommenderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenderService.Services
{
    public interface IRecommendationService
    {
        Task Catalogize(CatalogModel model);
        Task<IEnumerable<int>> GetPopularAlbums(IEnumerable<int> userAlbumIds);
        IEnumerable<int> GetUserAlbumIds(int userId);
        Task<IEnumerable<int>> FilterAlbumsByStylesAndGenres(IEnumerable<int> userAlbumIds);
    }

    public class RecommendationService : IRecommendationService
    {
        private readonly CatalogDbContext _context;
        private readonly MusicDbContext _musicContext;

        public RecommendationService(CatalogDbContext context, MusicDbContext musicContext)
        {
            _context = context;
            _musicContext = musicContext;
        }

        public async Task Catalogize(CatalogModel model)
        {
            await _context.CatalogModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetPopularAlbums(IEnumerable<int> userAlbumIds)
        {
            var dict = await _context.CatalogModels
                .Where(x => !userAlbumIds.Contains(x.AlbumId))
                .GroupBy(x => x.AlbumId)
                .ToDictionaryAsync(x => x.Key, x => x.Count());
            
            return dict.OrderByDescending(x => x.Value)
                .Take(1000)
                .Select(x => x.Key);
        }

        public IEnumerable<int> GetUserAlbumIds(int userId)
        {
            return _context.CatalogModels.Where(x => x.UserId == userId).Select(x => x.AlbumId);
        }

        public async Task<IEnumerable<int>> FilterAlbumsByStylesAndGenres(IEnumerable<int> userAlbumIds)
        {
            var genreIds = new HashSet<int>();
            var styleIds = new HashSet<int>();

            foreach(var id in userAlbumIds)
            {
                var album = await _musicContext.Albums.FindAsync(id);
                genreIds.Add(album.GenreId);
                styleIds.Add(album.StyleId);
            }

            return _musicContext.Albums
                .Where(x => !userAlbumIds.Contains(x.Id) && genreIds.Contains(x.GenreId) && styleIds.Contains(x.StyleId))
                .Select(x => x.Id);
        }
    }
}
