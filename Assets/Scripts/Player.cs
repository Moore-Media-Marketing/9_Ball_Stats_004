// --- Region: Player Class --- //
public class Player
	{
	// --- Comment: Player's statistics --- //
	public string Name { get; set; } // Player's name
	public int SkillLevel { get; set; } // Player's skill level (1 to 9)

	// --- Comment: Additional current season statistics --- //
	public int currentSeasonGamesPlayed { get; set; } // Games played this season
	public int currentSeasonGamesWon { get; set; } // Games won this season
	public float currentSeasonPaPercentage { get; set; } // Percentage of successful shots
	public float currentSeasonPpm { get; set; } // Points per match
	public int currentSeasonSkillLevel { get; set; } // Skill level this season
	public int currentSeasonTotalPoints { get; set; } // Total points earned this season

	// --- Comment: Lifetime statistics --- //
	public int lifetimeGamesWon { get; set; }
	public int lifetimeGamesPlayed { get; set; }
	public float lifetimeDefensiveShotAvg { get; set; }
	public int matchesPlayedInLast2Years { get; set; }
	public int lifetimeBreakAndRun { get; set; }
	public int lifetimeNineOnTheSnap { get; set; }
	public int lifetimeMiniSlams { get; set; }
	public int lifetimeShutouts { get; set; }

	// --- Comment: Player ID and Team ID --- //
	public string Id { get; set; } // Unique player ID
	public string TeamId { get; set; } // Associated team ID

	// --- Comment: Constructor with parameters --- //
	public Player(string name, string id, int skillLevel, string teamId)
		{
		Name = name;
		Id = id;
		SkillLevel = skillLevel;
		TeamId = teamId;
		currentSeasonGamesPlayed = 0;
		currentSeasonGamesWon = 0;
		currentSeasonPaPercentage = 0;
		currentSeasonPpm = 0;
		currentSeasonSkillLevel = skillLevel;
		currentSeasonTotalPoints = 0;

		lifetimeGamesWon = 0;
		lifetimeGamesPlayed = 0;
		lifetimeDefensiveShotAvg = 0;
		matchesPlayedInLast2Years = 0;
		lifetimeBreakAndRun = 0;
		lifetimeNineOnTheSnap = 0;
		lifetimeMiniSlams = 0;
		lifetimeShutouts = 0;
		}

	// --- Comment: Parameterless constructor for SQLite compatibility --- //
	public Player()
		{
		// This constructor is required for SQLite and similar systems.
		}
	}
// --- End Region --- //
