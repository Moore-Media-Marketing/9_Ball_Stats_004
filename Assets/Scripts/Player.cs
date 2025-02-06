public class Player
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }
	public string PlayerName { get; set; }
	public int SkillLevel { get; set; }
	public int LifetimeMatchesPlayedInLast2Years { get; set; }
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeGamesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }
	public int CurrentSeasonPointsAwarded { get; set; }
	public float CurrentSeasonPointsPerMatch { get; set; }
	public int CurrentSeasonShutouts { get; set; }
	public int CurrentSeasonSkillLevel { get; set; }
	public int CurrentSeasonTotalPoints { get; set; }
	public float CurrentSeasonPpm { get; set; }
	public int CurrentSeasonMiniSlams { get; set; }
	public int CurrentSeasonNineOnTheSnap { get; set; }
	public float CurrentSeasonPaPercentage { get; set; }
	public float CurrentSeasonDefensiveShotAverage { get; set; }
	public int CurrentSeasonBreakAndRun { get; set; }
	public int LifetimeBreakAndRun { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }

	// Updated constructor to remove LifetimeDefensiveShotAvg
	public Player(
		int teamId, string teamName, string playerName, int skillLevel, int currentSeasonMatchesPlayed,
		int currentSeasonMatchesWon, int currentSeasonPointsAwarded, float currentSeasonPointsPerMatch,
		int currentSeasonShutouts, int currentSeasonSkillLevel, int currentSeasonTotalPoints, int lifetimeGamesPlayed,
		int lifetimeGamesWon, int lifetimeMiniSlams, int lifetimeNineOnTheSnap, int lifetimeShutouts,
		int lifetimeMatchesPlayedInLast2Years, int lifetimeBreakAndRun, float lifetimeDefensiveShotAverage)
		{
		TeamId = teamId;
		TeamName = teamName;
		PlayerName = playerName;
		SkillLevel = skillLevel;
		CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed;
		CurrentSeasonMatchesWon = currentSeasonMatchesWon;
		CurrentSeasonPointsAwarded = currentSeasonPointsAwarded;
		CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch;
		CurrentSeasonShutouts = currentSeasonShutouts;
		CurrentSeasonSkillLevel = currentSeasonSkillLevel;
		CurrentSeasonTotalPoints = currentSeasonTotalPoints;
		LifetimeGamesPlayed = lifetimeGamesPlayed;
		LifetimeGamesWon = lifetimeGamesWon;
		LifetimeMiniSlams = lifetimeMiniSlams;
		LifetimeNineOnTheSnap = lifetimeNineOnTheSnap;
		LifetimeShutouts = lifetimeShutouts;
		LifetimeMatchesPlayedInLast2Years = lifetimeMatchesPlayedInLast2Years;
		LifetimeBreakAndRun = lifetimeBreakAndRun;
		LifetimeDefensiveShotAverage = lifetimeDefensiveShotAverage;
		}

	public string ToCsv()
		{
		return $"{TeamName},{TeamId},{PlayerName},{LifetimeGamesPlayed},{LifetimeGamesWon},{CurrentSeasonPointsAwarded},{LifetimeGamesPlayed}, " +
			$"{CurrentSeasonBreakAndRun},{CurrentSeasonDefensiveShotAverage},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon}," +
			$"{CurrentSeasonMiniSlams},{CurrentSeasonNineOnTheSnap},{CurrentSeasonPaPercentage},{CurrentSeasonPointsAwarded},{CurrentSeasonPointsPerMatch}," +
			$"{CurrentSeasonPpm},{CurrentSeasonShutouts},{CurrentSeasonSkillLevel},{CurrentSeasonTotalPoints},{LifetimeBreakAndRun}," +
			$"{LifetimeDefensiveShotAverage},{LifetimeGamesPlayed},{LifetimeGamesWon},{LifetimeMatchesPlayed},{LifetimeMatchesWon}," +
			$"{LifetimeMiniSlams},{LifetimeNineOnTheSnap},{LifetimeShutouts},{LifetimeMatchesPlayedInLast2Years}";
		}


	public static Player FromCsv(string csvLine)
		{
		string[] values = csvLine.Split(',');

		return new Player(
			int.Parse(values[1]), values[0], values[2], int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5]),
			int.Parse(values[6]), float.Parse(values[7]), int.Parse(values[8]), int.Parse(values[9]), int.Parse(values[10]),
			int.Parse(values[11]), int.Parse(values[12]), int.Parse(values[13]), int.Parse(values[14]), int.Parse(values[15]),
			int.Parse(values[16]), int.Parse(values[17]), int.Parse(values[18])
		);
		}
	}