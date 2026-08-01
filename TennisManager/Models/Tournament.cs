using System.ComponentModel.DataAnnotations;
using TennisManager.Data;

namespace TennisManager.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = DateTime.Today;
        public DateTime? RegistrationEnds { get; set; }
        public int RoundDurationInDays { get; set; } = 7;
        public string Location { get; set; } = string.Empty;
        public TournamentMode Mode { get; set; }
        public int CurrentRound { get; set; } = 0;
        public int MaxPlayers { get; set; } = 8;
        public string Description { get; set; } = string.Empty;
        public TournamentStatus Status { get; set; } = TournamentStatus.RegistrationOpen;
        public ICollection<TournamentParticipant> Participants { get; set; } = new List<TournamentParticipant>(); // or []
        public ICollection<Match> Matches { get; set; } = new List<Match>();
        public string CreatedByUserId { get; set; } = string.Empty;
        public ApplicationUser CreatedByUser { get; set; } = null!;

        // methods
        public Tournament Clone()
        {
            return new Tournament
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Location = Location,
                StartDate = StartDate,
                RegistrationEnds = RegistrationEnds,
                RoundDurationInDays = RoundDurationInDays,
                MaxPlayers = MaxPlayers,
                Mode = Mode,
                Status = Status,
                CreatedByUserId = CreatedByUserId,

                Participants = Participants
                    .Select(p => new TournamentParticipant
                    {
                        Id = p.Id,
                        TournamentId = p.TournamentId,
                        UserId = p.UserId,
                        User = p.User
                    })
                    .ToList()
            };
        }
    }

    public enum TournamentMode
    {
        [Display(Name ="Single Out")]
        SingleKnockout,
        [Display(Name ="Double Out")]
        DoubleKnockout,
        [Display(Name = "Gruppenphase")]
        GroupStage
    }

    public enum TournamentStatus
    {
        RegistrationOpen,
        RegistrationClosed,
        InProgress,
        Finished,
        Cancelled
    }
}
