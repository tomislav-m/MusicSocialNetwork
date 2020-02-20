using System.Collections.Generic;

namespace Common.MessageContracts.Catalog.Events
{
    public class PopularAlbums
    {
        public IEnumerable<int> TodayAlbums { get; set; }
        public IEnumerable<int> WeekAlbums { get; set; }
        public IEnumerable<int> MonthAlbums { get; set; }
    }
}
