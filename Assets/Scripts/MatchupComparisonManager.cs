using UnityEngine;
using SQLite;
using System.Collections.Generic;
using System.Linq;

// --- Region: Matchup Comparison Manager --- //
public class MatchupComparisonManager:MonoBehaviour
	{
	// --- Region: SQLite References --- //
	private SQLiteConnection db;
	private string dbPath;
	// --- End Region: SQLite References --- //

	// Reference to the MatchupResultsPanel
	public MatchupResultsPanel matchupResultsPanel;

	// --- Region: Initialization --- //
	private void Start()
		{
		dbPath = System.IO.Path.Combine(Application.persistentDataPath, "sampleData.db");
		db = new SQLiteConnection(dbPath);
		db.CreateTable<Player>(); // Ensure the Player table exists
		db.CreateTable<Team>(); // Ensure the Team table exists

		// Example usage: Compare teams with their players and stats
		Team team1 = db.Table<Team>().FirstOrDefault(t => t.Name == "Team 1");
		Team team2 = db.Table<Team>().FirstOrDefault(t => t.Name == "Team 2");

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
		List<Player> team1Players = db.Table<Player>().Where(p => p.TeamId == team1.Id).ToList();
		List<Player> team2Players = db.Table<Player>().Where(p => p.TeamId == team2.Id).ToList();

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
		List<MatchupResult> matchupResults = new List<MatchupResult>();

		var sortedTeam1Players = team1Players.OrderByDescending(p => p.CurrentSeasonSkillLevel).Take(5).ToList();
		var sortedTeam2Players = team2Players.OrderByDescending(p => p.CurrentSeasonSkillLevel).Take(5).ToList();

		for (int i = 0; i < 5; i++)
			{
			Player team1Player = sortedTeam1Players[i];
			Player team2Player = sortedTeam2Players[i];

			MatchupResult result = new MatchupResult()
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
	}
// --- End Region: Matchup Comparison Manager --- //

// --- Region: MatchupResult Class --- //
public class MatchupResult
	{
	public string Team1PlayerName { get; set; }
	public string Team2PlayerName { get; set; }
	public float Team1PointsRequired { get; set; }
	public float Team2PointsRequired { get; set; }
	}
// --- End Region: MatchupResult Class --- //
