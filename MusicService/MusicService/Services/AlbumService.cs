using Microsoft.EntityFrameworkCore;
using MusicService.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Service.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAll(int page, int count);
        Task<Album> GetById(long id);
        Task<Album> Create(Album album);
        void Update(Album album);
        void Delete(Album album);
        Task<IEnumerable<Album>> Search(string searchTerm, int page = 1, int size = 10);
    }

    public class AlbumService : IAlbumService
    {
        private readonly MusicDbContext _context;

        public AlbumService(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Album> GetById(long id)
        {
            return await _context.Albums.Where(x => x.Id == id)
                .Include(x => x.Genre)
                .Include(x => x.Style)
                .Include(x => x.Format)
                .Include(x => x.Tracks)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Album>> GetAll(int page, int count)
        {
            return await _context.Albums.Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Album>> Search(string searchTerm, int page = 1, int size = 10)
        {
            return await _context.Albums
                .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<Album> Create(Album album)
        {
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();

            return album;
        }

        public async void Delete(Album album)
        {
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
        }

        public async void Update(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
        }
    }
}
