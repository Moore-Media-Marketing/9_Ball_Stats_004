public class Player
	{
	public int PlayerId { get; set; }
	public string PlayerName { get; set; }
	public int TeamId { get; set; } // Team ID reference
	public PlayerStats Stats { get; set; } = new PlayerStats();

	// Constructor that aligns with CSV loading and DatabaseManager references
	public Player(int playerId, string playerName, int teamId, PlayerStats stats)
		{
		PlayerId = playerId;
		PlayerName = playerName;
		TeamId = teamId;
		Stats = stats ?? new PlayerStats();
		}

	// Shortcut property to fetch current skill level from PlayerStats
	public int SkillLevel => Stats.CurrentSeasonSkillLevel;

	// Fetch the team name based on TeamId
	public string TeamName => PlayersAndTeamsManager.Instance.GetTeamNameById(TeamId);
	}
