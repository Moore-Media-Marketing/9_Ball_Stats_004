public class Player
	{
	public string PlayerName { get; set; }
	public int SkillLevel { get; set; }
	public int TeamId { get; set; }
	public string TeamName { get; set; }
	public int LifetimeGamesWon { get; set; }
	public int LifetimeGamesPlayed { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }
	public int LifetimeMatchesPlayedInLast2Years { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeShutouts { get; set; }
	public int LifetimeBreakAndRun { get; set; } // **Added missing property**
	public int LifetimeNineOnTheSnap { get; set; } // **Added missing property**
	public int CurrentSeasonPointsAwarded { get; set; }
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }

	// Constructor to initialize the player
	public Player(string playerName, int skillLevel, int teamId, string teamName, int lifetimeGamesWon, int lifetimeGamesPlayed,
				  float lifetimeDefensiveShotAverage, int lifetimeMatchesPlayedInLast2Years, int lifetimeMiniSlams,
				  int lifetimeShutouts, int lifetimeBreakAndRun, int lifetimeNineOnTheSnap, // **Added these two**
				  int currentSeasonPointsAwarded, int currentSeasonMatchesPlayed, int currentSeasonMatchesWon)
		{
		PlayerName = playerName;
		SkillLevel = skillLevel;
		TeamId = teamId;
		TeamName = teamName;
		LifetimeGamesWon = lifetimeGamesWon;
		LifetimeGamesPlayed = lifetimeGamesPlayed;
		LifetimeDefensiveShotAverage = lifetimeDefensiveShotAverage;
		LifetimeMatchesPlayedInLast2Years = lifetimeMatchesPlayedInLast2Years;
		LifetimeMiniSlams = lifetimeMiniSlams;
		LifetimeShutouts = lifetimeShutouts;
		LifetimeBreakAndRun = lifetimeBreakAndRun; // **Initialize new property**
		LifetimeNineOnTheSnap = lifetimeNineOnTheSnap; // **Initialize new property**
		CurrentSeasonPointsAwarded = currentSeasonPointsAwarded;
		CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed;
		CurrentSeasonMatchesWon = currentSeasonMatchesWon;
		}

	// Method to convert a player to CSV
	public string ToCsv()
		{
		return $"{PlayerName},{SkillLevel},{TeamId},{TeamName},{LifetimeGamesWon},{LifetimeGamesPlayed}," +
			   $"{LifetimeDefensiveShotAverage},{LifetimeMatchesPlayedInLast2Years},{LifetimeMiniSlams},{LifetimeShutouts}," +
			   $"{LifetimeBreakAndRun},{LifetimeNineOnTheSnap}," + // **Added to CSV output**
			   $"{CurrentSeasonPointsAwarded},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon}";
		}

	// Static method to parse a CSV line and create a Player object
	public static Player FromCsv(string csvLine)
		{
		string[] values = csvLine.Split(',');

		return new Player(values[0], int.Parse(values[1]), int.Parse(values[2]), values[3],
						  int.Parse(values[4]), int.Parse(values[5]), float.Parse(values[6]),
						  int.Parse(values[7]), int.Parse(values[8]), int.Parse(values[9]),
						  int.Parse(values[10]), int.Parse(values[11]), int.Parse(values[12]), // **Parsing new properties**
						  int.Parse(values[13]), int.Parse(values[14]), int.Parse(values[15]));
		}

	// --- New Method: CalculateOverallScore --- //
	public int CalculateOverallScore(PlayerWeightSettings weightSettings)
		{
		// Example of calculating the overall score: you can adjust this based on your stats
		int score = CurrentSeasonPointsAwarded + (LifetimeGamesWon * 2) - (LifetimeGamesPlayed / 10);
		return score;
		}
	}
