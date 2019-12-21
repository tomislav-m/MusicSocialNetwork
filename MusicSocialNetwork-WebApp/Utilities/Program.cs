using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Model;

namespace Utilities
{
    class Program
    {
        static readonly string apiKey = "819f524777ce392140458d440e48ebbc";
        static readonly string url = @"http://ws.audioscrobbler.com/2.0/?method=";
        static readonly string username = "twosuitsluke83";
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            //await AddArtist();
            var albumUrl = "http://localhost:57415/api/albums";
            var response = await client.GetAsync(albumUrl);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var json = await response.Content.ReadAsStringAsync();
                var root = JsonDocument.Parse(json).RootElement;
                var albums = JsonSerializer.Deserialize<List<Album>>(root.ToString(), options);
                foreach (var album in albums)
                {
                    await AddTrack(album);
                    await Task.Delay(300);
                }
            }

            //var artistUrl = "http://localhost:57415/api/artist";
            //var response = await client.GetAsync(artistUrl);
            //if (response.IsSuccessStatusCode)
            //{
            //    var options = new JsonSerializerOptions
            //    {
            //        PropertyNameCaseInsensitive = true
            //    };

            //    var json = await response.Content.ReadAsStringAsync();
            //    var root = JsonDocument.Parse(json).RootElement;
            //    var artists = JsonSerializer.Deserialize<List<Artist>>(root.ToString(), options);
            //    foreach (var artist in artists)
            //    {
            //        await AddAlbum(artist);
            //        await Task.Delay(300);
            //    }
            //}
        }

        static async Task AddArtist()
        {
            var methodUrl = string.Format("{0}library.getartists&api_key={1}&user={2}&format=json&limit=2000&page=2", url, apiKey, username);
            var response = await client.GetAsync(@methodUrl);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var json = await response.Content.ReadAsStringAsync();
                var root = JsonDocument.Parse(json).RootElement.GetProperty("artists").GetProperty("artist").ToString();
                var artists = JsonSerializer.Deserialize<List<ArtistName>>(root, options);

                foreach (var artist in artists)
                {
                    var artistInfo = await AddArtistInfo(artist.Name);
                    if (artistInfo != null)
                    {
                        artistInfo.Name = artist.Name;
                        artistInfo.MbId = artist.MbId;
                    }
                    else
                    {
                        artistInfo.Name = artist.Name;
                        artistInfo.MbId = artist.MbId;
                    }
                    var stringContent = new StringContent(JsonSerializer.Serialize(artistInfo), Encoding.UTF8, "application/json");
                    var postResponse = await client.PostAsync("http://localhost:57415/api/artists", stringContent);
                    Console.WriteLine("{0}: {1}", artist.Name, postResponse.StatusCode);
                }
            }
        }

        static async Task<Artist> AddArtistInfo(string name)
        {
            var artistInfoUrl = "https://www.theaudiodb.com/api/v1/json/195003/search.php?s=" + name;
            var response = await client.GetAsync(artistInfoUrl);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var root = JsonDocument.Parse(json).RootElement.GetProperty("artists");
                    var artist = JsonSerializer.Deserialize<List<Artist>>(root.ToString(), options).FirstOrDefault();

                    try
                    {
                        artist.YearFormed = int.Parse(artist.YearStr);
                    }
                    catch
                    {
                        int.TryParse(artist.YearBornStr, out var result);
                        artist.YearBorn = result;
                    }
                    return artist;
                }
                catch
                {
                    return new Artist();
                }
            }
            return new Artist();
        }

        static async Task AddAlbum(Artist artist)
        {
            var albumUrl = "https://www.theaudiodb.com/api/v1/json/195003/searchalbum.php?s=" + artist.Name;
            var response = await client.GetAsync(albumUrl);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var json = await response.Content.ReadAsStringAsync();
                var root = JsonDocument.Parse(json).RootElement.GetProperty("album");
                if (root.ValueKind == JsonValueKind.Null) return;

                var albums = JsonSerializer.Deserialize<List<Album>>(root.ToString(), options);

                foreach(var album in albums)
                {
                    var dbAlbum = artist.Albums.FirstOrDefault(x => x.Name == album.Name);
                    dbAlbum.TMDBId = album.TMDBId;

                    var stringContent = new StringContent(JsonSerializer.Serialize(dbAlbum), Encoding.UTF8, "application/json");
                    var postResponse = await client.PutAsync("http://localhost:57415/api/albums/" + dbAlbum.Id, stringContent);
                    Console.WriteLine("{0}: {1}", dbAlbum.Name, postResponse.StatusCode);
                }
            }
        }

        static async Task AddInfo(Album album)
        {
            var albumUrl = "https://www.theaudiodb.com/api/v1/json/195003/searchalbum.php?a=" + album.Name;
            var response = await client.GetAsync(albumUrl);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var json = await response.Content.ReadAsStringAsync();
                var root = JsonDocument.Parse(json).RootElement.GetProperty("album");
                if (root.ValueKind == JsonValueKind.Null) return;

                album.TMDBId = JsonSerializer.Deserialize<List<Album>>(root.ToString(), options).FirstOrDefault().TMDBId;

                var stringContent = new StringContent(JsonSerializer.Serialize(album), Encoding.UTF8, "application/json");
                var postResponse = await client.PutAsync("http://localhost:57415/api/albums/" + album.Id, stringContent);
                Console.WriteLine("{0}: {1}", album.Name, postResponse.StatusCode);
            }
        }

        static async Task AddTrack(Album album)
        {
            var albumUrl = "https://theaudiodb.com/api/v1/json/195003/track.php?m=" + album.TMDBId;
            var response = await client.GetAsync(albumUrl);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var json = await response.Content.ReadAsStringAsync();
                    var root = JsonDocument.Parse(json).RootElement.GetProperty("track");
                    if (root.ValueKind == JsonValueKind.Null) return;

                    var tracks = JsonSerializer.Deserialize<List<Track>>(root.ToString(), options);

                    foreach (var track in tracks)
                    {
                        track.AlbumId = album.Id;
                        track.Duration = double.Parse(track.DurationStr) / 1000;
                        var stringContent = new StringContent(JsonSerializer.Serialize(track), Encoding.UTF8, "application/json");
                        var postResponse = await client.PostAsync("http://localhost:57415/api/tracks/", stringContent);
                        Console.WriteLine("{0}: {1}", track.Title, postResponse.StatusCode);
                    }
                }
            } catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }
    }

    class ArtistName
    {
        public string Name { get; set; }
        public string MbId { get; set; }
    }
}
