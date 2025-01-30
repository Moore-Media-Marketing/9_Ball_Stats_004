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

	private void Start()
		{
		// Raise the event when a team is selected
		EventManager.Instance.TriggerTeamSelectedEvent();
		}

	public void UpdatePlayerStats()
		{
		// Raise an event when player data is updated
		EventManager.Instance.TriggerPlayerDataUpdatedEvent();
		}

	// --- References --- //
	public DataManager dataManager; // Reference to DataManager
	public UIManager uiManager; // Reference to UIManager
	public ComparisonResultsPanel resultsPanel; // Reference to the comparison results panel

	// --- Helper: Get Points Required to Win Based on Skill Level --- //
	private int GetPointsRequiredToWin(int skillLevel)
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
				_ => 0, // Invalid skill level
				};
		}

	// --- Helper: Compare Player Matchup with Handicap --- //
	private ComparisonResult ComparePlayersForMatchup(Player homePlayer, Player awayPlayer)
		{
		// Get the points required for each player based on their skill level
		int homePlayerPointsRequired = GetPointsRequiredToWin(homePlayer.skillLevel);
		int awayPlayerPointsRequired = GetPointsRequiredToWin(awayPlayer.skillLevel);

		// Calculate the win percentage based on the points required
		float homePlayerWinPercentage = (float) homePlayerPointsRequired / (homePlayerPointsRequired + awayPlayerPointsRequired) * 100;
		float awayPlayerWinPercentage = (float) awayPlayerPointsRequired / (homePlayerPointsRequired + awayPlayerPointsRequired) * 100;

		// Create the result for this matchup
		ComparisonResult result = new()
			{
			HomePlayer = homePlayer,
			AwayPlayer = awayPlayer,
			HomePlayerWinPercentage = homePlayerWinPercentage,
			AwayPlayerWinPercentage = awayPlayerWinPercentage,
			HomePlayerPPM = homePlayer.ppm,
			AwayPlayerPPM = awayPlayer.ppm,
			};

		return result;
		}

	// --- Helper: Calculate Team Skill Level --- //
	private float CalculateTeamSkillLevel(Team team)
		{
		float skillLevel = 0;
		foreach (var player in team.Players)
			{
			skillLevel += player.skillLevel; // Assuming the player's skill level is a float or int
			}
		return skillLevel;
		}

	// --- Helper: Check if Team is Valid According to 23-Rule --- //
	private bool IsTeamValid(Team team)
		{
		int totalSkillLevel = 0;
		int seniorPlayerCount = 0;

		foreach (var player in team.Players)
			{
			totalSkillLevel += player.skillLevel;
			if (player.skillLevel >= 6) // Senior skill level players are 6-9
				{
				seniorPlayerCount++;
				}
			}

		// Ensure total skill level is <= 23 and only 2 senior players
		return totalSkillLevel <= 23 && seniorPlayerCount <= 2;
		}

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
		// Ensure teams are valid according to the 23-rule
		if (!IsTeamValid(homeTeam) || !IsTeamValid(awayTeam))
			{
			Debug.LogError("One or both teams do not meet the 23-rule.");
			return;
			}

		// Get all possible matchups between players from both teams
		List<ComparisonResult> comparisonResults = new();

		foreach (var homePlayer in homeTeam.Players)
			{
			foreach (var awayPlayer in awayTeam.Players)
				{
				// Compare the players with the handicap system applied
				ComparisonResult result = ComparePlayersForMatchup(homePlayer, awayPlayer);
				comparisonResults.Add(result);
				}
			}

		// Sort the results based on favoring the home team
		comparisonResults.Sort((result1, result2) => result2.HomePlayerWinPercentage.CompareTo(result1.HomePlayerWinPercentage));

		// Display the results in the UI (This is where you would update your UI with the matchups)
		DisplayComparisonResults(comparisonResults, homeTeam, awayTeam);
		}

	// --- Helper: Display Comparison Results --- //
	private void DisplayComparisonResults(List<ComparisonResult> results, Team homeTeam, Team awayTeam)
		{
		// Display the comparison results in the UI
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
