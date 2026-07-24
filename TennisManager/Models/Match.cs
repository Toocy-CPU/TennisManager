using TennisManager.Data;

namespace TennisManager.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;
        public string PlayerAId { get; set; } = string.Empty;   
        public ApplicationUser PlayerA { get; set; } = null!;
        public string PlayerBId { get; set; } = string.Empty;
        public ApplicationUser PlayerB { get; set; } = null!;

        public ICollection<SetResult> Sets { get; set; } = new List<SetResult>();
    }
}
