namespace TennisManager.Models
{
    public class SetResult
    {
        public int Id { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;


        public int SetNumber { get; set; }

        public int PlayerAGames { get; set; }

        public int PlayerBGames { get; set; }

        public int? TieBreakA { get; set; }

        public int? TieBreakB { get; set; }
    }
}
