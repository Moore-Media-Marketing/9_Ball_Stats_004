[System.Serializable]
public class Player
	{
	public string name;
	public int skillLevel;
	public int gamesPlayed;
	public int gamesWon;
	public float ppm;
	public float pa;
	public string TeamName; // Add this property to associate a player with a team
	public float WinPercentage => CalculateWinPercentage();

	public Player(string name, int skillLevel, int gamesPlayed, int gamesWon, string teamName)
		{
		this.name = name;
		this.skillLevel = skillLevel;
		this.gamesPlayed = gamesPlayed;
		this.gamesWon = gamesWon;
		this.ppm = 0;
		this.pa = 0;
		this.TeamName = teamName; // Assign the team name
		}

	private float CalculateWinPercentage()
		{
		if (this.gamesPlayed == 0) return 0f;
		return (float) this.gamesWon / this.gamesPlayed * 100f;
		}

	public void UpdateStats(bool won, float pointsPerMatch, float pointsAwarded)
		{
		if (pointsPerMatch < 0 || pointsAwarded < 0)
			{
			return;
			}

		this.gamesPlayed++;
		if (won) this.gamesWon++;
		this.ppm = pointsPerMatch;
		this.pa = pointsAwarded;
		}
	}