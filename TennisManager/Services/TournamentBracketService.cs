using Microsoft.EntityFrameworkCore;
using TennisManager.Data;
using TennisManager.Models;

namespace TennisManager.Services
{
    public class TournamentBracketService
    {
        private readonly ApplicationDbContext _context;

        public TournamentBracketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task GenerateBracketAsync(int tournamentId)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants)
                    .ThenInclude(p => p.User)
                .Include(t => t.Matches)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament is null)
            {
                return;
            }

            if (tournament.Matches.Any())
            {
                // Baum wurde bereits erstellt
                return;
            }

            var players = tournament.Participants
                .OrderBy(_ => Random.Shared.Next())
                .ToList();

            // guard as long as we dont handle odd playerCount
            if (!IsPowerOfTwo(players.Count))
            {
                throw new InvalidOperationException(
                    "Die Teilnehmerzahl muss eine Zweierpotenz sein.");
            }

            var rounds = (int)Math.Log2(players.Count);

            // generate matches in a list for each round
            List<List<Match>> bracket = [];

            int matchesInRound = players.Count / 2;

            for (int round = 1; round <= rounds; round++)
            {
                List<Match> roundMatches = [];

                for (int i = 0; i < matchesInRound; i++)
                {
                    roundMatches.Add(new Match
                    {
                        TournamentId = tournament.Id,
                        Round = round
                    });
                }

                bracket.Add(roundMatches);

                matchesInRound /= 2;
            }
            // add players to the matches of the 1st round
            for (int i = 0; i < players.Count; i++)
            {
                int matchIndex = i / 2;

                if (i % 2 == 0)
                {
                    bracket[0][matchIndex].PlayerAId = players[i].UserId;
                }
                else
                {
                    bracket[0][matchIndex].PlayerBId = players[i].UserId;
                }
            }
            // add nextMatchId and nextMatchSlot to the bracket
            for (int i = 0; i < bracket.Count - 1; i++)
            {
                var bracketA = bracket[i];
                var bracketB = bracket[i + 1];
                for (int j = 0; j < bracketA.Count; j++)
                {
                    bracketA[j].NextMatch = bracketB[j / 2];
                    bracketA[j].NextMatchSlot =
                        j % 2 == 0 ? NextMatchSlot.PlayerA : NextMatchSlot.PlayerB;
                }
            }
            // save in database
            _context.Matches.AddRange(
                bracket.SelectMany(x => x)
                );

            await _context.SaveChangesAsync();
        }

        private static bool IsPowerOfTwo(int value)
        {
            return value > 1 && (value & (value - 1)) == 0;
        }

        public async Task SaveMatchResultsAsync(List<SetResult> setResults,Match match)
        {
            var matchId = setResults.First().MatchId;

            var existingSets = await _context.SetResults
                .Where(s => s.MatchId == matchId)
                .ToListAsync();

            foreach (var set in existingSets)
            {
                if (!setResults.Any(s => s.Id == set.Id))
                    _context.SetResults.Remove(set);
            }

            foreach (var set in setResults)
            {
                if (set.Id == 0)
                    _context.SetResults.Add(set);
                else
                    _context.SetResults.Update(set);
            }

            match.WinnerId = DetermineWinner(match, setResults);

            TransferWinnerToNextMatch(match);

            await _context.SaveChangesAsync();
        }

        private void TransferWinnerToNextMatch(Match match)
        {
            if (match.WinnerId != null && match.NextMatch != null)
            {
                if (match.NextMatchSlot == NextMatchSlot.PlayerA)
                {
                    match.NextMatch.PlayerAId = match.WinnerId;
                }
                else if (match.NextMatchSlot == NextMatchSlot.PlayerB)
                {
                    match.NextMatch.PlayerBId = match.WinnerId;
                }
            }
        }

        private string? DetermineWinner(Match match,List<SetResult> setResults)
        {
            int playerAWins = 0;
            int playerBWins = 0;

            foreach (var set in setResults)
            {
                if(DidPlayerAWinSet(set))
                {
                    playerAWins++;
                }
                else
                {
                    playerBWins++;
                }
            }
            return playerAWins > playerBWins ? match.PlayerAId : match.PlayerBId;
        }

        private bool DidPlayerAWinSet(SetResult set)
        {
            if (set.TieBreakA.HasValue && set.TieBreakB.HasValue)
                return set.TieBreakA > set.TieBreakB;

            return set.PlayerAGames > set.PlayerBGames;
        }
    }
}
