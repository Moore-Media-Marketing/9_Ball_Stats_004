using UnityEngine;

public class Player
	{
	// Player's unique ID
	public int PlayerId { get; set; }

	// Player's name
	public string PlayerName { get; set; }

	// Player's skill level (1-9)
	public int SkillLevel { get; set; }

	// Team ID that this player belongs to
	public int TeamId { get; set; }  // This is the missing property

	// Career Statistics
	public int TotalGames { get; set; }
	public int TotalWins { get; set; }
	public int TotalPoints { get; set; }
	public int PointsRequiredToWin { get; set; }

	// Current Season Statistics
	public int CurrentSeasonBreakAndRun { get; set; }
	public float CurrentSeasonDefensiveShotAverage { get; set; }
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }
	public int CurrentSeasonMiniSlams { get; set; }
	public int CurrentSeasonNineOnTheSnap { get; set; }
	public float CurrentSeasonPaPercentage { get; set; }
	public int CurrentSeasonPointsAwarded { get; set; }
	public float CurrentSeasonPointsPerMatch { get; set; }
	public float CurrentSeasonPpm { get; set; }
	public int CurrentSeasonShutouts { get; set; }
	public int CurrentSeasonSkillLevel { get; set; }
	public int CurrentSeasonTotalPoints { get; set; }

	// Lifetime Statistics
	public int LifetimeBreakAndRun { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }
	public float LifetimeDefensiveShotAvg { get; set; }
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeGamesWon { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }

	// Constructor to initialize the Player with given parameters
	public Player(string playerName, int skillLevel, int totalGames, int totalWins, int totalPoints)
		{
		this.PlayerName = playerName;
		this.TotalGames = totalGames;
		this.TotalWins = totalWins;
		this.TotalPoints = totalPoints;
		this.PointsRequiredToWin = GetPointsRequired(skillLevel); // Calculate points required based on skill level
		}

	// Default constructor
	public Player() { }

	// Method to get points required to win based on skill level
	public int GetPointsRequired(int skillLevel)
		{
		switch (skillLevel)
			{
			case 1: return 14;
			case 2: return 19;
			case 3: return 25;
			case 4: return 31;
			case 5: return 38;
			case 6: return 46;
			case 7: return 55;
			case 8: return 65;
			case 9: return 75;
			default: return 0; // Default to 0 if invalid skill level
			}
		}

	// --- CSV Helper Methods ---
	// Converts the Player object to a CSV string
	public string ToCsv()
		{
		return $"{PlayerId},{PlayerName},{TotalGames},{TotalWins},{TotalPoints},{PointsRequiredToWin}," +
			   $"{CurrentSeasonBreakAndRun},{CurrentSeasonDefensiveShotAverage},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon}," +
			   $"{CurrentSeasonMiniSlams},{CurrentSeasonNineOnTheSnap},{CurrentSeasonPaPercentage},{CurrentSeasonPointsAwarded}," +
			   $"{CurrentSeasonPointsPerMatch},{CurrentSeasonPpm},{CurrentSeasonShutouts},{CurrentSeasonSkillLevel},{CurrentSeasonTotalPoints}," +
			   $"{LifetimeBreakAndRun},{LifetimeDefensiveShotAverage},{LifetimeDefensiveShotAvg},{LifetimeGamesPlayed},{LifetimeGamesWon}," +
			   $"{LifetimeMatchesPlayed},{LifetimeMatchesWon},{LifetimeMiniSlams},{LifetimeNineOnTheSnap},{LifetimeShutouts}";
		}

	// Converts a CSV string into a Player object
	public static Player FromCsv(string csvLine)
		{
		string[] values = csvLine.Split(',');

		Player player = new()
			{
			PlayerId = int.Parse(values[0]),
			PlayerName = values[1],

			TotalGames = int.Parse(values[2]),
			TotalWins = int.Parse(values[3]),
			TotalPoints = int.Parse(values[4]),
			PointsRequiredToWin = int.Parse(values[5]),
			CurrentSeasonBreakAndRun = int.Parse(values[6]),
			CurrentSeasonDefensiveShotAverage = float.Parse(values[7]),
			CurrentSeasonMatchesPlayed = int.Parse(values[8]),
			CurrentSeasonMatchesWon = int.Parse(values[9]),
			CurrentSeasonMiniSlams = int.Parse(values[10]),
			CurrentSeasonNineOnTheSnap = int.Parse(values[11]),
			CurrentSeasonPaPercentage = float.Parse(values[12]),
			CurrentSeasonPointsAwarded = int.Parse(values[13]),
			CurrentSeasonPointsPerMatch = float.Parse(values[14]),
			CurrentSeasonPpm = float.Parse(values[15]),
			CurrentSeasonShutouts = int.Parse(values[16]),
			CurrentSeasonSkillLevel = int.Parse(values[17]),
			CurrentSeasonTotalPoints = int.Parse(values[18]),
			LifetimeBreakAndRun = int.Parse(values[19]),
			LifetimeDefensiveShotAverage = float.Parse(values[20]),
			LifetimeDefensiveShotAvg = float.Parse(values[21]),
			LifetimeGamesPlayed = int.Parse(values[22]),
			LifetimeGamesWon = int.Parse(values[23]),
			LifetimeMatchesPlayed = int.Parse(values[24]),
			LifetimeMatchesWon = int.Parse(values[25]),
			LifetimeMiniSlams = int.Parse(values[26]),
			LifetimeNineOnTheSnap = int.Parse(values[27]),
			LifetimeShutouts = int.Parse(values[28])
			};

		return player;
		}
	}
