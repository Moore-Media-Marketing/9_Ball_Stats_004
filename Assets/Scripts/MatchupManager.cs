using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// --- Region: Matchup Manager --- //
public class MatchupManager:MonoBehaviour
	{
	// --- Comment: Method to get the combined skill level of a team --- //
	public int GetTeamSkillLevel(Team team)
		{
		int skillLevel = 0;
		foreach (Player player in team.Players)
			{
			skillLevel += player.SkillLevel;
			}
		return skillLevel;
		}

	// --- Comment: Method to compare two teams --- //
	public void CompareTeams(Team team1, Team team2)
		{
		int team1SkillLevel = GetTeamSkillLevel(team1); // Get combined skill level for team 1
		int team2SkillLevel = GetTeamSkillLevel(team2); // Get combined skill level for team 2

		Debug.Log($"Team 1 Skill Level: {team1SkillLevel}, Team 2 Skill Level: {team2SkillLevel}");

		// Logic to compare teams and decide the winner can be added here
		if (team1SkillLevel > team2SkillLevel)
			{
			Debug.Log("Team 1 wins!");
			}
		else if (team2SkillLevel > team1SkillLevel)
			{
			Debug.Log("Team 2 wins!");
			}
		else
			{
			Debug.Log("It's a tie!");
			}
		}

	// --- Region: Load Teams and Players from CSV --- //
	private string teamsCsvPath;
	private string playersCsvPath;

	private List<Team> teams = new();
	private List<Player> players = new();

	// --- Comment: Initialization method for loading data --- //
	private void Start()
		{
		// Define file paths for the CSVs
		teamsCsvPath = Path.Combine(Application.persistentDataPath, "teams.csv");
		playersCsvPath = Path.Combine(Application.persistentDataPath, "players.csv");

		// Load data from CSV files
		teams = LoadTeamsFromCSV();
		players = LoadPlayersFromCSV();

		// Example: Compare teams based on loaded data
		Team team1 = teams.FirstOrDefault(t => t.Name == "Team 1");
		Team team2 = teams.FirstOrDefault(t => t.Name == "Team 2");

		if (team1 != null && team2 != null)
			{
			CompareTeams(team1, team2);
			}
		else
			{
			Debug.Log("Teams not found.");
			}
		}
	// --- End Region: Load Teams and Players from CSV --- //

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
				if (columns.Length == 4) // Assuming 4 columns: Id, Name, TeamId, SkillLevel
					{
					loadedPlayers.Add(new Player
						{
						Id = int.Parse(columns[0]),
						Name = columns[1],
						TeamId = int.Parse(columns[2]),
						SkillLevel = int.Parse(columns[3])
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
		public int SkillLevel { get; set; }  // Player's skill level
		}
	// --- End Region: Player Class --- //

	// --- Region: Team Class --- //
	public class Team
		{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Player> Players { get; set; } = new List<Player>();  // List of players in the team
		}
	// --- End Region: Team Class --- //
	}
// --- End Region: Matchup Manager --- //
