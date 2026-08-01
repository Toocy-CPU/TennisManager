using TennisManager.Data;

namespace TennisManager.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;
        public int Round { get; set; }
        public DateTime Deadline { get; set; }
        public MatchStatus Status { get; set; }
        public MatchOutcome Outcome { get; set; }
        public string PlayerAId { get; set; } = string.Empty;
        public ApplicationUser PlayerA { get; set; } = null!;
        public string PlayerBId { get; set; } = string.Empty;
        public ApplicationUser PlayerB { get; set; } = null!;
        public string? WinnerId { get; set; }
        public ApplicationUser? Winner { get; set; }
        public ICollection<SetResult> Sets { get; set; } = new List<SetResult>();
    }
    public enum MatchStatus
    {
        Pending,
        Scheduled,
        Completed
    }
    public enum MatchOutcome
    {
        Pending,
        Played,
        WalkoverA,
        WalkoverB,
        DoubleForfeit
    }
}
