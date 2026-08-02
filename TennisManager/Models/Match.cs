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
        public string? PlayerAId { get; set; }
        public ApplicationUser? PlayerA { get; set; }
        public string? PlayerBId { get; set; }
        public ApplicationUser? PlayerB { get; set; }
        public string? WinnerId { get; set; }
        public ApplicationUser? Winner { get; set; }
        public int? NextMatchId { get; set; }
        public Match? NextMatch { get; set; }
        public NextMatchSlot NextMatchSlot { get; set; } = NextMatchSlot.None;
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
    public enum NextMatchSlot
    {
        None,
        PlayerA,
        PlayerB
    }
}
