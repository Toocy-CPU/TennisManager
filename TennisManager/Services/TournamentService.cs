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
            return await _context.Tournaments.Include(p => p.Participants).ToListAsync();
        }
        public async Task<Tournament?> GetTurnamentAsync(int id)
        {
            return await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task CreateTournamentAsync(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);

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

            return RegistrationResult.Success;
        }
    }

    public enum RegistrationResult
    {
        Success,
        AlreadyRegistered,
        TournamentFull,
        RegistrationClosed,
        TournamentNotFound
    }
}
