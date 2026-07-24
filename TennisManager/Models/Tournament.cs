namespace TennisManager.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public TournamentMode Mode { get; set; }
        public int MaxPlayers { get; set; }
        public string Description { get; set; } = string.Empty;
        public TournamentStatus Status { get; set; }
        public ICollection<TournamentParticipant> Participants { get; set; } = new List<TournamentParticipant>();
        public ICollection<Match> Matches { get; set; } = new List<Match>();

    }

    public enum TournamentMode
    {
        SingleKnockout,
        DoubleKnockout,
        GroupStage
    }

    public enum TournamentStatus
    {
        RegistrationOpen,
        Running,
        Finished
    }
}
