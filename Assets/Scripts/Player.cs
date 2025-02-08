using System;

public class Player
	{
	// Properties
	public string PlayerName { get; set; }
	public int PlayerId { get; set; }
	public int SkillLevel { get; set; }
	public string TeamName { get; set; }
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }
	public float CurrentSeasonDefensiveShotAverage { get; set; }
	public int CurrentSeasonMiniSlams { get; set; }
	public int CurrentSeasonNineOnTheSnap { get; set; }
	public int CurrentSeasonPointsAwarded { get; set; }
	public int CurrentSeasonPointsPerMatch { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }

	// Constructor with 16 parameters
	public Player(string playerName, int playerId, int skillLevel, string teamName,
				  int currentSeasonMatchesPlayed, int currentSeasonMatchesWon,
				  float currentSeasonDefensiveShotAverage, int currentSeasonMiniSlams,
				  int currentSeasonNineOnTheSnap, int currentSeasonPointsAwarded,
				  int currentSeasonPointsPerMatch, int lifetimeMatchesPlayed,
				  int lifetimeMatchesWon, int lifetimeMiniSlams, int lifetimeNineOnTheSnap,
				  int lifetimeShutouts)
		{
		PlayerName = playerName;
		PlayerId = playerId;
		SkillLevel = skillLevel;
		TeamName = teamName;
		CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed;
		CurrentSeasonMatchesWon = currentSeasonMatchesWon;
		CurrentSeasonDefensiveShotAverage = currentSeasonDefensiveShotAverage;
		CurrentSeasonMiniSlams = currentSeasonMiniSlams;
		CurrentSeasonNineOnTheSnap = currentSeasonNineOnTheSnap;
		CurrentSeasonPointsAwarded = currentSeasonPointsAwarded;
		CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch;
		LifetimeMatchesPlayed = lifetimeMatchesPlayed;
		LifetimeMatchesWon = lifetimeMatchesWon;
		LifetimeMiniSlams = lifetimeMiniSlams;
		LifetimeNineOnTheSnap = lifetimeNineOnTheSnap;
		LifetimeShutouts = lifetimeShutouts;
		}

	// CSV handling methods (if needed)
	public string ToCsv()
		{
		return $"{PlayerId},{PlayerName},{SkillLevel},{TeamName},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon},{CurrentSeasonDefensiveShotAverage},{CurrentSeasonMiniSlams},{CurrentSeasonNineOnTheSnap},{CurrentSeasonPointsAwarded},{CurrentSeasonPointsPerMatch},{LifetimeMatchesPlayed},{LifetimeMatchesWon},{LifetimeMiniSlams},{LifetimeNineOnTheSnap},{LifetimeShutouts}";
		}

	public static Player FromCsv(string csv)
		{
		var values = csv.Split(',');
		return new Player(
			values[1], int.Parse(values[0]), int.Parse(values[2]), values[3],
			int.Parse(values[4]), int.Parse(values[5]), float.Parse(values[6]),
			int.Parse(values[7]), int.Parse(values[8]), int.Parse(values[9]),
			int.Parse(values[10]), int.Parse(values[11]), int.Parse(values[12]),
			int.Parse(values[13]), int.Parse(values[14]), int.Parse(values[15])
		);
		}
	}
