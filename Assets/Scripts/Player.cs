public class Player
	{
	// Player's unique ID
	public int PlayerId { get; set; }

	// Player's name
	public string PlayerName { get; set; }

	// Player's skill level (1-9)
	public int SkillLevel { get; set; }

	// Team ID that this player belongs to
	public int TeamId { get; set; }

	// Player's Team Name
	public string TeamName { get; set; } // Added TeamName property

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

	// New property for lifetime matches played in the last 2 years
	public int LifetimeMatchesPlayedInLast2Years { get; set; }

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
	public Player()
		{ }

	// Method to get points required to win based on skill level
	public int GetPointsRequired(int skillLevel)
		{
		return skillLevel switch
			{
				1 => 14,
				2 => 19,
				3 => 25,
				4 => 31,
				5 => 38,
				6 => 46,
				7 => 55,
				8 => 65,
				9 => 75,
				_ => 0, // Default to 0 if invalid skill level
				};
		}

	// --- CSV Helper Methods ---
	// Converts the Player object to a CSV string
	public string ToCsv()
		{
		return $"{TeamName},{PlayerId},{PlayerName},{TotalGames},{TotalWins},{TotalPoints},{PointsRequiredToWin}," +
			   $"{CurrentSeasonBreakAndRun},{CurrentSeasonDefensiveShotAverage},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon}," +
			   $"{CurrentSeasonMiniSlams},{CurrentSeasonNineOnTheSnap},{CurrentSeasonPaPercentage},{CurrentSeasonPointsAwarded}," +
			   $"{CurrentSeasonPointsPerMatch},{CurrentSeasonPpm},{CurrentSeasonShutouts},{CurrentSeasonSkillLevel},{CurrentSeasonTotalPoints}," +
			   $"{LifetimeBreakAndRun},{LifetimeDefensiveShotAverage},{LifetimeDefensiveShotAvg},{LifetimeGamesPlayed},{LifetimeGamesWon}," +
			   $"{LifetimeMatchesPlayed},{LifetimeMatchesWon},{LifetimeMiniSlams},{LifetimeNineOnTheSnap},{LifetimeShutouts}," +
			   $"{LifetimeMatchesPlayedInLast2Years}"; // Add the new property to the CSV string
		}

	// Converts a CSV string into a Player object
	public static Player FromCsv(string csvLine)
		{
		string[] values = csvLine.Split(',');

		Player player = new()
			{
			TeamName = values[0], // Populate TeamName from CSV
			PlayerId = int.Parse(values[1]),
			PlayerName = values[2],

			TotalGames = int.Parse(values[3]),
			TotalWins = int.Parse(values[4]),
			TotalPoints = int.Parse(values[5]),
			PointsRequiredToWin = int.Parse(values[6]),
			CurrentSeasonBreakAndRun = int.Parse(values[7]),
			CurrentSeasonDefensiveShotAverage = float.Parse(values[8]),
			CurrentSeasonMatchesPlayed = int.Parse(values[9]),
			CurrentSeasonMatchesWon = int.Parse(values[10]),
			CurrentSeasonMiniSlams = int.Parse(values[11]),
			CurrentSeasonNineOnTheSnap = int.Parse(values[12]),
			CurrentSeasonPaPercentage = float.Parse(values[13]),
			CurrentSeasonPointsAwarded = int.Parse(values[14]),
			CurrentSeasonPointsPerMatch = float.Parse(values[15]),
			CurrentSeasonPpm = float.Parse(values[16]),
			CurrentSeasonShutouts = int.Parse(values[17]),
			CurrentSeasonSkillLevel = int.Parse(values[18]),
			CurrentSeasonTotalPoints = int.Parse(values[19]),
			LifetimeBreakAndRun = int.Parse(values[20]),
			LifetimeDefensiveShotAverage = float.Parse(values[21]),
			LifetimeDefensiveShotAvg = float.Parse(values[22]),
			LifetimeGamesPlayed = int.Parse(values[23]),
			LifetimeGamesWon = int.Parse(values[24]),
			LifetimeMatchesPlayed = int.Parse(values[25]),
			LifetimeMatchesWon = int.Parse(values[26]),
			LifetimeMiniSlams = int.Parse(values[27]),
			LifetimeNineOnTheSnap = int.Parse(values[28]),
			LifetimeShutouts = int.Parse(values[29]),
			LifetimeMatchesPlayedInLast2Years = int.Parse(values[30]) // Read the new property from CSV
			};

		return player;
		}
	}