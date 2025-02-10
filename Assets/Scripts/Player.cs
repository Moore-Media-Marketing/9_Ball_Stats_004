using System;

/// <summary>
/// Represents a player in the system.
/// </summary>
public class Player
	{
	public int PlayerId { get; set; }
	public string PlayerName { get; set; }
	public int TeamId { get; set; }
	public PlayerStats Stats { get; set; } = new PlayerStats();

	// Constructor matching CSV loading and DatabaseManager
	public Player(int playerId, string playerName, int teamId, PlayerStats stats)
		{
		PlayerId = playerId;
		PlayerName = playerName;
		TeamId = teamId;
		Stats = stats ?? new PlayerStats();
		}

	// Read-only property for skill level in the current season
	public int SkillLevel => Stats.CurrentSeasonSkillLevel;

	// Fetch the team name dynamically based on TeamId
	public string TeamName => PlayersAndTeamsManager.Instance.GetTeamNameById(TeamId);

	// Update the current season stats for the player
	public void UpdateCurrentSeasonStats(int gamesWon, int gamesPlayed, int totalPoints, int ppm, float pa, int breakAndRun,
		int miniSlams, int nineOnTheSnap, int shutouts, int skillLevel)
		{
		// Directly update the properties in Stats (CurrentSeasonStats)
		Stats.CurrentSeasonMatchesWon = gamesWon;
		Stats.CurrentSeasonMatchesPlayed = gamesPlayed;
		Stats.CurrentSeasonTotalPoints = totalPoints;
		Stats.CurrentSeasonPpm = ppm;
		Stats.CurrentSeasonPaPercentage = pa;
		Stats.CurrentSeasonBreakAndRun = breakAndRun;
		Stats.CurrentSeasonMiniSlams = miniSlams;
		Stats.CurrentSeasonNineOnTheSnap = nineOnTheSnap;
		Stats.CurrentSeasonShutouts = shutouts;

		// Update skill level
		Stats.CurrentSeasonSkillLevel = skillLevel;
		}

	// Helper method to validate player stats
	public bool ValidateStats()
		{
		return Stats.IsValid();
		}
	}