public class Player
	{
	private string v;
	private PlayerStats playerStats;

	public int PlayerId { get; set; }
	public string PlayerName { get; set; }
	public int TeamId { get; set; } // Team ID reference
	public PlayerStats Stats { get; set; } = new PlayerStats();

	// Constructor that aligns with CSV loading and DatabaseManager references
	public Player(int playerId, string playerName, int teamId, PlayerStats stats, int skillLevel)
		{
		PlayerId = playerId;
		PlayerName = playerName;
		TeamId = teamId;
		Stats = stats ?? new PlayerStats();
		}

	public Player(int playerId, string v, int teamId, PlayerStats playerStats)
		{
		PlayerId = playerId;
		this.v = v;
		TeamId = teamId;
		this.playerStats = playerStats;
		}

	// Shortcut property to fetch current skill level from PlayerStats
	public int SkillLevel => Stats.CurrentSeasonSkillLevel;

	// Fetch the team name based on TeamId
	public string TeamName => PlayersAndTeamsManager.Instance.GetTeamNameById(TeamId);
	}