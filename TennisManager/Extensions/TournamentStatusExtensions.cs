using MudBlazor;
using TennisManager.Models;

namespace TennisManager.Extensions;

public static class TournamentStatusExtensions
{
    public static Color GetColor(this TournamentStatus status)
    {
        return status switch
        {
            TournamentStatus.RegistrationOpen => Color.Success,
            TournamentStatus.RegistrationClosed => Color.Default,
            TournamentStatus.InProgress => Color.Info,
            TournamentStatus.Finished => Color.Warning,
            _ => Color.Default
        };
    }

    public static string GetIcon(this TournamentStatus status)
    {
        return status switch
        {
            TournamentStatus.RegistrationOpen => Icons.Material.Filled.HowToReg,
            TournamentStatus.RegistrationClosed => Icons.Material.Filled.Lock,
            TournamentStatus.InProgress => Icons.Material.Filled.PlayArrow,
            TournamentStatus.Finished => Icons.Material.Filled.EmojiEvents,
            _ => Icons.Material.Filled.Info
        };
    }

    public static string GetText(this TournamentStatus status)
    {
        return status switch
        {
            TournamentStatus.RegistrationOpen => "Anmeldung geöffnet",
            TournamentStatus.RegistrationClosed => "Anmeldung geschlossen",
            TournamentStatus.InProgress => "Turnier läuft",
            TournamentStatus.Finished => "Turnier beendet",
            _ => status.ToString()
        };
    }
}