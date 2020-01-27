using Microsoft.EntityFrameworkCore;
using MusicService.DomainModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Service.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetAll(int page, int count);
        Task<Artist> GetById(long id);
        Task<Artist> Create(Artist artist);
        void Update(Artist artist);
        void Delete(Artist artist);
        Task<IEnumerable<Artist>> Search(string searchTerm, int page = 1, int size = 10);
    }

    public class ArtistService : IArtistService
    {
        private readonly MusicDbContext _context;

        public ArtistService(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetAll(int page = 1, int count = 20)
        {
            return await _context.Artists.Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<Artist> GetById(long id)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(x => x.Id == id);
            var albums = await _context.Albums.Where(x => x.ArtistId == id)
                .Include(x => x.Genre)
                .Include(x => x.Style)
                .OrderBy(x => x.YearReleased)
                .ToListAsync();
            artist.Albums = albums;

            return artist; 
        }

        public async Task<Artist> Create(Artist artist)
        {
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();

            return artist;
        }

        public async void Update(Artist artist)
        {
            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
        }

        public async void Delete(Artist artist)
        {
            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Artist>> Search(string searchTerm, int page = 1, int size = 10)
        {
            return await _context.Artists
                .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }
    }
}
