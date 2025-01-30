using System.Collections.Generic;

using UnityEngine;

public class ComparisonManager:MonoBehaviour
	{
	public static ComparisonManager Instance { get; private set; }

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			}
		}

	// --- References --- //
	public DataManager dataManager; // Reference to DataManager

	public UIManager uiManager; // Reference to UIManager
	public ComparisonResultsPanel resultsPanel; // Reference to the comparison results panel

	// --- Player vs Player Comparison --- //
	public void ComparePlayers(Player homePlayer, Player awayPlayer)
		{
		// Calculate and display the comparison between two players
		float homePlayerWinPercentage = homePlayer.WinPercentage;
		float awayPlayerWinPercentage = awayPlayer.WinPercentage;

		string result = homePlayerWinPercentage > awayPlayerWinPercentage ?
			$"{homePlayer.name} wins against {awayPlayer.name}" :
			$"{awayPlayer.name} wins against {homePlayer.name}";

		Debug.Log(result);
		}

	// --- Team vs Team Comparison --- //
	public void CompareTeams(Team homeTeam, Team awayTeam)
		{
		// Get all possible matchups between players from both teams
		List<ComparisonResult> comparisonResults = new();

		foreach (var homePlayer in homeTeam.Players) // FIXED: changed team.players to team.Players
			{
			foreach (var awayPlayer in awayTeam.Players) // FIXED: changed team.players to team.Players
				{
				// Compare the players
				ComparisonResult result = ComparePlayersForMatchup(homePlayer, awayPlayer);
				comparisonResults.Add(result);
				}
			}

		// Sort the results based on favoring the home team
		comparisonResults.Sort((result1, result2) => result2.HomePlayerWinPercentage.CompareTo(result1.HomePlayerWinPercentage));

		// Display the results in the UI (This is where you would update your UI with the matchups)
		DisplayComparisonResults(comparisonResults, homeTeam, awayTeam);
		}

	// --- Helper: Compare Player Matchup --- //
	private ComparisonResult ComparePlayersForMatchup(Player homePlayer, Player awayPlayer)
		{
		// Calculate win percentages and points per match (PPM)
		float homePlayerWinPercentage = homePlayer.WinPercentage;
		float awayPlayerWinPercentage = awayPlayer.WinPercentage;
		float homePlayerPPM = homePlayer.ppm;
		float awayPlayerPPM = awayPlayer.ppm;

		// Create the result for this matchup
		ComparisonResult result = new()
			{
			HomePlayer = homePlayer,
			AwayPlayer = awayPlayer,
			HomePlayerWinPercentage = homePlayerWinPercentage,
			AwayPlayerWinPercentage = awayPlayerWinPercentage,
			HomePlayerPPM = homePlayerPPM,
			AwayPlayerPPM = awayPlayerPPM,
			};

		return result;
		}

	// --- Helper: Display Comparison Results --- //
	private void DisplayComparisonResults(List<ComparisonResult> results, Team homeTeam, Team awayTeam)
		{
		// Display the comparison results in the UI (e.g., on the results panel)
		resultsPanel.ClearResults();

		foreach (var result in results)
			{
			// Pass a string version of the result
			resultsPanel.AddResult($"{result.HomePlayer.name} vs {result.AwayPlayer.name}: {result.HomePlayerWinPercentage}% vs {result.AwayPlayerWinPercentage}%");
			}

		// Calculate which team has more favorable matchups (e.g., based on total win percentages)
		float homeTeamSkillLevel = CalculateTeamSkillLevel(homeTeam);
		float awayTeamSkillLevel = CalculateTeamSkillLevel(awayTeam);

		// Pass the winner as a string
		resultsPanel.DisplayWinner(homeTeamSkillLevel > awayTeamSkillLevel ? homeTeam.Name : awayTeam.Name);
		}

	// --- Helper: Calculate Team Skill Level --- //
	private float CalculateTeamSkillLevel(Team team)
		{
		float skillLevel = 0;
		foreach (var player in team.Players) // FIXED: changed team.players to team.Players
			{
			skillLevel += player.skillLevel; // Assuming the player's skill level is a float or int
			}
		return skillLevel;
		}
	}

// --- Comparison Result Class --- //
public class ComparisonResult
	{
	public Player HomePlayer;
	public Player AwayPlayer;
	public float HomePlayerWinPercentage;
	public float AwayPlayerWinPercentage;
	public float HomePlayerPPM;
	public float AwayPlayerPPM;
	}