public class Player
	{
	private string playerName;
	private string teamName;

	public int Id { get; set; }
	public string PlayerName { get; set; }
	public string TeamName { get; set; }
	public int SkillLevel { get; set; }
	public PlayerStats Stats { get; set; }
	public int TeamId { get; set; }

	// Player constructor
	public Player(int id, string name, int skillLevel, PlayerStats stats, int teamId)
		{
		Id = id;
		PlayerName = name;
		SkillLevel = skillLevel;
		Stats = stats;
		TeamId = teamId;
		}

	public Player(string playerName, string teamName, int skillLevel, PlayerStats stats, int teamId)
		{
		this.playerName = playerName;
		this.teamName = teamName;
		SkillLevel = skillLevel;
		Stats = stats;
		TeamId = teamId;
		}
	}
