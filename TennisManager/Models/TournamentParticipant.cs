using TennisManager.Data;

namespace TennisManager.Models
{
    public class TournamentParticipant
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }

    }
}
