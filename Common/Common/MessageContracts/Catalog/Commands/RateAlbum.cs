namespace Common.MessageContracts.Catalog.Commands
{
    public class RateAlbum
    {
        public int UserId { get; set; }
        public int AlbumId { get; set; }
        public float Rating { get; set; }
    }
}
