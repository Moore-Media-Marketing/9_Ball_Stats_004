using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MatchupComparisonManager:MonoBehaviour
	{
	// Reference to the MatchupResultsPanel
	public MatchupResultsPanel matchupResultsPanel;

	// CSV file paths
	private string teamsCsvPath;
	private string playersCsvPath;

	// Loaded data
	private List<Team> teams;
	private List<Player> players;

	// --- Region: Initialization --- //
	private void Start()
		{
		// Define file paths for the CSVs
		teamsCsvPath = Path.Combine(Application.persistentDataPath, "teams.csv");
		playersCsvPath = Path.Combine(Application.persistentDataPath, "players.csv");

		// Load data from CSV files
		teams = LoadTeamsFromCSV();
		players = LoadPlayersFromCSV();

		// Example usage: Compare teams with their players and stats
		Team team1 = teams.FirstOrDefault(t => t.Name == "Team 1");
		Team team2 = teams.FirstOrDefault(t => t.Name == "Team 2");

		if (team1 != null && team2 != null)
			{
			CompareMatchups(team1, team2);
			}
		else
			{
			Debug.Log("Teams not found.");
			}
		}
	// --- End Region: Initialization --- //

	// --- Comment: Method to compare matchups between two teams --- //
	public void CompareMatchups(Team team1, Team team2)
		{
		List<Player> team1Players = players.Where(p => p.TeamId == team1.Id).ToList();
		List<Player> team2Players = players.Where(p => p.TeamId == team2.Id).ToList();

		int team1SkillLevel = 0;
		int team2SkillLevel = 0;

		// Calculate total skill level for team 1 and team 2
		foreach (Player player in team1Players)
			{
			team1SkillLevel += player.CurrentSeasonSkillLevel;
			}

		foreach (Player player in team2Players)
			{
			team2SkillLevel += player.CurrentSeasonSkillLevel;
			}

		Debug.Log($"Team 1 Skill Level: {team1SkillLevel}, Team 2 Skill Level: {team2SkillLevel}");

		// --- Region: Handicap Logic --- //
		// Implement handicap system based on the given formula
		// Determine the points required to win based on Skill Levels
		float team1Odds = 0;
		float team2Odds = 0;

		foreach (Player player in team1Players)
			{
			team1Odds += CalculateWinningPoints(player.CurrentSeasonSkillLevel);
			}

		foreach (Player player in team2Players)
			{
			team2Odds += CalculateWinningPoints(player.CurrentSeasonSkillLevel);
			}

		Debug.Log($"Team 1 odds: {team1Odds}, Team 2 odds: {team2Odds}");
		// --- End Region: Handicap Logic --- //

		// --- Region: Prepare Data for Results Panel --- //
		List<MatchupResult> matchupResults = new();

		var sortedTeam1Players = team1Players.OrderByDescending(p => p.CurrentSeasonSkillLevel).Take(5).ToList();
		var sortedTeam2Players = team2Players.OrderByDescending(p => p.CurrentSeasonSkillLevel).Take(5).ToList();

		for (int i = 0; i < 5; i++)
			{
			Player team1Player = sortedTeam1Players[i];
			Player team2Player = sortedTeam2Players[i];

			MatchupResult result = new()
				{
				Team1PlayerName = team1Player.Name,
				Team2PlayerName = team2Player.Name,
				Team1PointsRequired = CalculateWinningPoints(team1Player.CurrentSeasonSkillLevel),
				Team2PointsRequired = CalculateWinningPoints(team2Player.CurrentSeasonSkillLevel),
				};

			matchupResults.Add(result);
			}

		// Send the matchup results to the MatchupResultsPanel
		if (matchupResultsPanel != null)
			{
			matchupResultsPanel.DisplayMatchupResults(matchupResults);
			}
		// --- End Region: Prepare Data for Results Panel --- //
		}

	// --- Region: Calculate Winning Points --- //
	private float CalculateWinningPoints(int skillLevel)
		{
		switch (skillLevel)
			{
			case 1: return 14f;
			case 2: return 19f;
			case 3: return 25f;
			case 4: return 31f;
			case 5: return 38f;
			case 6: return 46f;
			case 7: return 55f;
			case 8: return 65f;
			case 9: return 75f;
			default: return 0f;
			}
		}
	// --- End Region: Calculate Winning Points --- //

	// --- Region: Load Teams from CSV --- //
	private List<Team> LoadTeamsFromCSV()
		{
		List<Team> loadedTeams = new();
		if (File.Exists(teamsCsvPath))
			{
			string[] lines = File.ReadAllLines(teamsCsvPath);
			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length == 2) // Assuming 2 columns: Id, Name
					{
					loadedTeams.Add(new Team
						{
						Id = int.Parse(columns[0]),
						Name = columns[1]
						});
					}
				}
			}
		return loadedTeams;
		}
	// --- End Region: Load Teams from CSV --- //

	// --- Region: Load Players from CSV --- //
	private List<Player> LoadPlayersFromCSV()
		{
		List<Player> loadedPlayers = new();
		if (File.Exists(playersCsvPath))
			{
			string[] lines = File.ReadAllLines(playersCsvPath);
			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length == 4) // Assuming 4 columns: Id, Name, TeamId, CurrentSeasonSkillLevel
					{
					loadedPlayers.Add(new Player
						{
						Id = int.Parse(columns[0]),
						Name = columns[1],
						TeamId = int.Parse(columns[2]),
						CurrentSeasonSkillLevel = int.Parse(columns[3])
						});
					}
				}
			}
		return loadedPlayers;
		}
	// --- End Region: Load Players from CSV --- //

	// --- Region: Player Class --- //
	public class Player
		{
		public int Id { get; set; }
		public string Name { get; set; }
		public int TeamId { get; set; }  // Foreign key to the team
		public int CurrentSeasonSkillLevel { get; set; }
		}
	// --- End Region: Player Class --- //

	// --- Region: Team Class --- //
	public class Team
		{
		public int Id { get; set; }
		public string Name { get; set; }
		}
	// --- End Region: Team Class --- //

	// --- Region: MatchupResult Class --- //
	public class MatchupResult
		{
		public string Team1PlayerName { get; set; }
		public string Team2PlayerName { get; set; }
		public float Team1PointsRequired { get; set; }
		public float Team2PointsRequired { get; set; }
		}
	// --- End Region: MatchupResult Class --- //
	}
