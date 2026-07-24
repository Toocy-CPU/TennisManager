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
            return await _context.Tournaments.ToListAsync();
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
    }
}
