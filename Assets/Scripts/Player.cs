public class Player
	{
	public string PlayerName { get; set; }
	public string TeamName { get; set; }
	public int SkillLevel { get; set; }
	public PlayerStats Stats { get; set; }
	public int TeamId { get; set; }

	public Player(string playerName, string teamName, int skillLevel, PlayerStats stats, int teamId)
		{
		PlayerName = playerName;
		TeamName = teamName;
		SkillLevel = skillLevel;
		Stats = stats;
		TeamId = teamId;
		}
	}
