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
            var dbModel = await _context.CatalogModels.SingleOrDefaultAsync(x => x.AlbumId == model.AlbumId && x.UserId == model.UserId);
            if (dbModel == null)
            {
                await _context.CatalogModels.AddAsync(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<int>> GetPopularAlbums(IEnumerable<int> userAlbumIds)
        {
            var uaList = userAlbumIds.ToList();

            var catalogList = await _context.CatalogModels
                .Where(x => uaList.Contains(x.AlbumId))
                .ToListAsync();
            var dict = catalogList.GroupBy(x => x.AlbumId)
                .ToDictionary(x => x.Key, x => x.Count());
            
            var albumIds = dict.OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();
            albumIds.AddRange(uaList.Where(x => !albumIds.Contains(x)).OrderBy(a => Guid.NewGuid()).Take(100));

            return albumIds;
        }

        public IEnumerable<int> GetUserAlbumIds(int userId)
        {
            return _context.CatalogModels.Where(x => x.UserId == userId).Select(x => x.AlbumId).ToList();
        }

        public async Task<IEnumerable<int>> FilterAlbumsByStylesAndGenres(IEnumerable<int> userAlbumIds)
        {
            var genreIds = new HashSet<int>();
            var styleIds = new HashSet<int>();
            var artistIds = new HashSet<int>();

            foreach(var id in userAlbumIds)
            {
                var album = await _musicContext.Albums.FindAsync(id);
                genreIds.Add(album.GenreId);
                styleIds.Add(album.StyleId);
                artistIds.Add(album.ArtistId);
            }

            return _musicContext.Albums
                .Where(x => !userAlbumIds.Contains(x.Id) && genreIds.Contains(x.GenreId) && styleIds.Contains(x.StyleId) && !artistIds.Contains(x.ArtistId))
                .Select(x => x.Id);
        }
    }
}
