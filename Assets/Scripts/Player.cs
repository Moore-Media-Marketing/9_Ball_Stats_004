using SQLite;  // Ensure this is included for SQLite attributes

[Table("Players")]  // SQLite Table attribute to map the class to the 'Players' table
public class Player
	{
	// SQLite attributes for Primary Key and Auto Increment
	[PrimaryKey, AutoIncrement]
	public int PlayerId { get; set; } // Unique ID for each player

	// Player's name
	public string PlayerName { get; set; } // Add get/set

	// Player's skill level (1-9)
	public int SkillLevel { get; set; }

	// Career Statistics
	public int TotalGames { get; set; } // Total games played

	public int TotalWins { get; set; }  // Total wins
	public int TotalPoints { get; set; } // Total points scored
	public int PointsRequiredToWin { get; set; } // Points required to win based on skill level

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
	public float LifetimeDefensiveShotAvg { get; set; }  // Same as LifetimeDefensiveShotAverage
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeGamesWon { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }

	// Parameterless constructor required for SQLite
	public Player()
		{ }

	// Constructor to initialize the Player with given parameters
	public Player(string playerName, int skillLevel, int totalGames, int totalWins, int totalPoints)
		{
		this.PlayerName = playerName;
		this.SkillLevel = skillLevel;
		this.TotalGames = totalGames;
		this.TotalWins = totalWins;
		this.TotalPoints = totalPoints;
		this.PointsRequiredToWin = GetPointsRequired(skillLevel); // Calculate points required based on skill level
																  // Initialize other stats to default
		}

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
	}