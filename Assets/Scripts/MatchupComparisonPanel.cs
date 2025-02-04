using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite; // Import SQLite namespace

public class MatchupComparisonPanel:MonoBehaviour
	{
	#region UI Elements

	[Tooltip("Header text for the matchup comparison panel.")]
	public TMP_Text headerText;  // --- Displays the panel header ---

	[Tooltip("Instruction text to select Team A.")]
	public TMP_Text selectTeamAText;  // --- Text for selecting Team A ---

	[Tooltip("Dropdown for selecting Team A.")]
	public TMP_Dropdown teamADropdown;  // --- Dropdown for Team A selection ---

	[Tooltip("Scroll view that lists players for Team A.")]
	public ScrollRect teamAPlayerScrollView;  // --- Scroll view for Team A players ---

	[Tooltip("Text label for Team B.")]
	public TMP_Text teamBTeamText;  // --- Label for Team B ---

	[Tooltip("Dropdown for selecting Team B.")]
	public TMP_Dropdown teamBDropdown;  // --- Dropdown for Team B selection ---

	[Tooltip("Scroll view that lists players for Team B.")]
	public ScrollRect teamBPlayerScrollView;  // --- Scroll view for Team B players ---

	[Tooltip("Button to compare the matchups between Team A and Team B.")]
	public Button compareButton;  // --- Button to perform matchup comparison ---

	[Tooltip("Button to go back to the previous panel.")]
	public Button backButton;  // --- Back button for panel navigation ---

	[Tooltip("Matchup comparison panel for toggling visibility.")]
	public GameObject matchupComparisonPanel;  // --- Panel for matchup comparison ---

	[Tooltip("Matchup results panel for toggling visibility.")]
	public GameObject matchupResultsPanel;  // --- Panel for matchup results ---

	#endregion UI Elements

	#region Unity Methods

	private void Start()
		{
		// Set up button listeners
		backButton.onClick.AddListener(OnBackButtonClicked);
		compareButton.onClick.AddListener(OnCompareButtonClicked);

		// Refresh dropdowns
		if (DropdownManager.Instance != null)
			{
			List<string> teamNames = GetAllTeamNamesFromDatabase();
			if (teamNames != null && teamNames.Count > 0)
				{
				DropdownManager.Instance.UpdateDropdown(teamADropdown, teamNames);
				DropdownManager.Instance.UpdateDropdown(teamBDropdown, teamNames);
				}
			else
				{
				Debug.LogWarning("No team names available to populate dropdowns.");
				}
			}
		else
			{
			Debug.LogWarning("DropdownManager instance not found. Ensure it is added to the scene.");
			}
		}

	#endregion Unity Methods

	#region Button Event Handlers

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("MatchupComparisonPanel: Back button clicked, returning to home panel.");
		}

	public void OnCompareButtonClicked()
		{
		// Debugging visibility
		Debug.Log("Teams are valid, proceeding with comparison.");

		// Hide the MatchupComparisonPanel
		matchupComparisonPanel.SetActive(false);

		// Show the MatchupResultsPanel
		matchupResultsPanel.SetActive(true);

		// Perform the matchup comparison
		Team selectedTeamA = GetTeamFromDatabase(teamADropdown);
		Team selectedTeamB = GetTeamFromDatabase(teamBDropdown);
		MatchupResultData resultData = PerformMatchupComparison(selectedTeamA, selectedTeamB);

		// Get the selected team names from the dropdowns
		string teamAName = teamADropdown.options[teamADropdown.value].text;
		string teamBName = teamBDropdown.options[teamBDropdown.value].text;

		// Calculate average skill levels for both teams
		float teamASkillLevel = CalculateTeamSkillLevel(selectedTeamA);
		float teamBSkillLevel = CalculateTeamSkillLevel(selectedTeamB);

		// Get the MatchupResultsPanel component using TryGetComponent
		if (matchupResultsPanel.TryGetComponent(out MatchupResultsPanel resultsPanel))
			{
			Debug.Log($"Team A Skill Level: {teamASkillLevel}, Team B Skill Level: {teamBSkillLevel}");

			resultsPanel.SetMatchupData(resultData, teamAName, teamBName);
			}
		else
			{
			Debug.LogError("MatchupResultsPanel component not found on the results panel.");
			}

		Debug.Log("Matchup results set and panel switched.");
		}

	#endregion Button Event Handlers

	#region Matchup Comparison Logic

	private Team GetTeamFromDatabase(TMP_Dropdown dropdown)
		{
		string selectedTeamName = dropdown.options[dropdown.value].text;
		Team selectedTeam = GetTeamByNameFromDatabase(selectedTeamName);
		return selectedTeam;
		}

	private List<string> GetAllTeamNamesFromDatabase()
		{
		List<string> teamNames = new List<string>();
		string connectionString = "URI=file:" + Application.persistentDataPath + "/gameDatabase.db"; // Path to SQLite database

		using (var connection = new SqliteConnection(connectionString))
			{
			connection.Open();
			string query = "SELECT teamName FROM Teams";  // Query to retrieve all team names
			using (var command = new SqliteCommand(query, connection))
				{
				using (var reader = command.ExecuteReader())
					{
					while (reader.Read())
						{
						teamNames.Add(reader.GetString(0));  // Add team name to the list
						}
					}
				}
			}

		return teamNames;
		}

	private Team GetTeamByNameFromDatabase(string teamName)
		{
		Team team = null;
		string connectionString = "URI=file:" + Application.persistentDataPath + "/gameDatabase.db"; // Path to SQLite database

		using (var connection = new SqliteConnection(connectionString))
			{
			connection.Open();
			string query = "SELECT * FROM Teams WHERE teamName = @teamName";  // Query to get team details
			using (var command = new SqliteCommand(query, connection))
				{
				command.Parameters.AddWithValue("@teamName", teamName);
				using (var reader = command.ExecuteReader())
					{
					if (reader.Read())
						{
						team = new Team
							{
							teamName = reader.GetString(0),
							players = GetPlayersForTeamFromDatabase(reader.GetInt32(0))  // Assuming the first column is team name and second is team ID
							};
						}
					}
				}
			}

		return team;
		}

	private List<Player> GetPlayersForTeamFromDatabase(int teamId)
		{
		List<Player> players = new List<Player>();
		string connectionString = "URI=file:" + Application.persistentDataPath + "/gameDatabase.db"; // Path to SQLite database

		using (var connection = new SqliteConnection(connectionString))
			{
			connection.Open();
			string query = "SELECT * FROM Players WHERE teamId = @teamId";  // Query to get players for a team
			using (var command = new SqliteCommand(query, connection))
				{
				command.Parameters.AddWithValue("@teamId", teamId);
				using (var reader = command.ExecuteReader())
					{
					while (reader.Read())
						{
						Player player = new Player
							{
							name = reader.GetString(1),  // Assuming column 1 is player name
							currentSeasonSkillLevel = reader.GetInt32(2)  // Assuming column 2 is current season skill level
							};
						players.Add(player);
						}
					}
				}
			}

		return players;
		}

	private float GetPointsRequiredToWin(float skillLevel)
		{
		return (int) skillLevel switch
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
				_ => 0f,
				};
		}

	private MatchupResultData PerformMatchupComparison(Team teamA, Team teamB)
		{
		float teamAWinsCount = 0;
		float teamBWinsCount = 0;

		foreach (var playerA in teamA.players)
			{
			foreach (var playerB in teamB.players)
				{
				float pointsRequiredA = GetPointsRequiredToWin(playerA.currentSeasonSkillLevel);
				float pointsRequiredB = GetPointsRequiredToWin(playerB.currentSeasonSkillLevel);

				// Debugging calculation values
				Debug.Log($"Player A: {playerA.name}, Skill Level: {playerA.currentSeasonSkillLevel}, Points Required: {pointsRequiredA}");
				Debug.Log($"Player B: {playerB.name}, Skill Level: {playerB.currentSeasonSkillLevel}, Points Required: {pointsRequiredB}");

				if (pointsRequiredA > pointsRequiredB)
					teamAWinsCount++;
				else if (pointsRequiredA < pointsRequiredB)
					teamBWinsCount++;
				}
			}

		// Debug the results before division
		Debug.Log($"Team A Wins Count: {teamAWinsCount}, Team B Wins Count: {teamBWinsCount}");

		float totalComparisons = teamA.players.Count * teamB.players.Count;

		// Avoid division by zero and validate total comparisons
		if (totalComparisons == 0)
			{
			Debug.LogError("Total comparisons count is zero! Returning default values.");
			return new MatchupResultData(0, 0);  // Prevent division by zero
			}

		// Calculate percentages safely
		float teamAWinsPercentage = (teamAWinsCount / totalComparisons) * 100;
		float teamBWinsPercentage = (teamBWinsCount / totalComparisons) * 100;

		// Validate percentages and debug log them
		Debug.Log($"Team A Win Percentage: {teamAWinsPercentage}, Team B Win Percentage: {teamBWinsPercentage}");

		return new MatchupResultData(Mathf.RoundToInt(teamAWinsPercentage), Mathf.RoundToInt(teamBWinsPercentage));
		}

	// New method to calculate the average skill level of a team
	private float CalculateTeamSkillLevel(Team team)
		{
		if (team.players.Count == 0) return 0f;

		float totalSkillLevel = 0f;
		foreach (var player in team.players)
			{
			totalSkillLevel += player.currentSeasonSkillLevel;
			}

		return totalSkillLevel / team.players.Count;
		}

	#endregion Matchup Comparison Logic
	}
