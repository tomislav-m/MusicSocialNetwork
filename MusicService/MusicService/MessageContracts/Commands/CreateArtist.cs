namespace MusicService.MessageContracts
{
    public class CreateArtist
    {
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string PhotoUrl { get; set; }
        public string Bio { get; set; }
        public int YearFormed { get; set; }
        public int YearBorn { get; set; }
        public string Country { get; set; }
        public string MbId { get; set; }
    }
}
