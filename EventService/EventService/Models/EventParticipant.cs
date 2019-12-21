using EventService.Models.Enums;

namespace EventService.Models
{
    public class EventParticipant
    {
        public int ArtistId { get; set; }
        public ParticipantRole ParticipantRole { get; set; }
    }
}
