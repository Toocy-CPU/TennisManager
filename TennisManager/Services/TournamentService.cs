using Microsoft.EntityFrameworkCore;
using TennisManager.Data;
using TennisManager.Models;

namespace TennisManager.Services
{
    public class TournamentService
    {
        private readonly ApplicationDbContext _context;

        public TournamentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tournament>> GetTournamentsAsync()
        {
            return await _context.Tournaments
                .Include(p => p.Participants)
                .ToListAsync();
        }
        public async Task<Tournament?> GetTournamentAsync(int tournamentId)
        {
            return await _context.Tournaments
                .Include(t => t.Participants)
                    .ThenInclude(p => p.User)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.PlayerA)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.PlayerB)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Winner)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Sets)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);
        }
        //public async Task<List<Match>> GetBracketAsync(int tournamentId)
        //{
        //    return await _context.Matches
        //        .Where(m => m.TournamentId == tournamentId)
        //        .Include(m => m.PlayerA)
        //        .Include(m => m.PlayerB)
        //        .Include(m => m.Winner)
        //        .Include(m => m.Sets)
        //        .OrderBy(m => m.Round)
        //        .ToListAsync();
        //}
        public async Task CreateTournamentAsync(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);

            await _context.SaveChangesAsync();
        }
        public async Task UpdateTournamentAsync(Tournament editedTournament)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants)
                .FirstAsync(t => t.Id == editedTournament.Id);


            // normale Eigenschaften aktualisieren
            tournament.Name = editedTournament.Name;
            tournament.StartDate = editedTournament.StartDate;
            tournament.RegistrationEnds = editedTournament.RegistrationEnds;
            tournament.Location = editedTournament.Location;
            tournament.Description = editedTournament.Description;
            tournament.Mode = editedTournament.Mode;
            tournament.MaxPlayers = editedTournament.MaxPlayers;
            tournament.RoundDurationInDays = editedTournament.RoundDurationInDays;


            // entfernte Teilnehmer
            var removedParticipants = tournament.Participants
                .ExceptBy(
                    editedTournament.Participants.Select(p => p.Id),
                    p => p.Id)
                .ToList();

            foreach (var participant in removedParticipants)
            {
                tournament.Participants.Remove(participant);
                _context.TournamentParticipants.Remove(participant);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<RegistrationResult> RegisterPlayerAsync(int tournamentId, string userId)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament is null)
            {
                return RegistrationResult.TournamentNotFound;
            }
            if (tournament.Status != TournamentStatus.RegistrationOpen)
            {
                return RegistrationResult.RegistrationClosed;
            }
            if(tournament.Participants.Any(p => p.UserId == userId))
            {
                return RegistrationResult.AlreadyRegistered;
            }
            if(tournament.Participants.Count >= tournament.MaxPlayers)
            {
                return RegistrationResult.TournamentFull;
            }
            var participant = new TournamentParticipant
            {
                TournamentId = tournamentId,
                UserId = userId,
            };
            // insert into join-Entity bcs of Include
            tournament.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return RegistrationResult.Registered;
        }
        public async Task<RegistrationResult> UnregisterPlayerAsync(int tournamentId, string userId)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament is null)
            {
                return RegistrationResult.TournamentNotFound;
            }
            if (tournament.Status != TournamentStatus.RegistrationOpen)
            {
                return RegistrationResult.RegistrationClosed;
            }
            var participant = tournament.Participants
                .FirstOrDefault(p => p.UserId == userId);

            if (participant == null)
            {
                return RegistrationResult.PlayerNotFound;       
            }

            tournament.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            return RegistrationResult.Unregistered;
        }
    }

    public enum RegistrationResult
    {
        Registered,
        AlreadyRegistered,
        TournamentFull,
        RegistrationClosed,
        TournamentNotFound,
        Unregistered,
        PlayerNotFound
    }
}
