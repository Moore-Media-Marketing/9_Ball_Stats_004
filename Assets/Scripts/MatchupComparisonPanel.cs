using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MatchupComparisonPanel:MonoBehaviour
	{
	// --- Panel References --- //
	public TMP_Text headerText;
	public TMP_Text selectTeamAText;
	public TMP_Dropdown teamADropdown;
	public GameObject teamAPlayerScrollView;
	public TMP_Text teamBTeamText;
	public TMP_Dropdown teamBDropdown;
	public GameObject teamBPlayerScrollView;
	public TMP_Text compareButtonText;
	public TMP_Text backButtonText;

	// --- Player Toggle Prefab Reference --- //
	public GameObject playerTogglePrefab;

	// --- Initialization --- //
	private void Start()
		{
		// Populate team dropdowns with team data
		PopulateTeamDropdowns();
		compareButtonText.text = "Compare";
		backButtonText.text = "Back";
		}

	// --- Populate Team Dropdowns --- //
	private void PopulateTeamDropdowns()
		{
		List<Team> teams = DatabaseManager.Instance.LoadTeamsFromCsv();  // Get teams from DatabaseManager
		List<string> teamNames = new();

		foreach (var team in teams)
			{
			teamNames.Add(team.TeamName);
			}

		teamADropdown.ClearOptions();
		teamADropdown.AddOptions(teamNames);
		teamBDropdown.ClearOptions();
		teamBDropdown.AddOptions(teamNames);
		}

	// --- On Team Selection Changed --- //
	public void OnTeamADropdownChanged(int index)
		{
		// Get the selected team and populate its players
		Team selectedTeam = DatabaseManager.Instance.LoadTeamsFromCsv()[index];
		PopulateTeamPlayerScrollView(teamAPlayerScrollView, selectedTeam);
		}

	public void OnTeamBDropdownChanged(int index)
		{
		// Get the selected team and populate its players
		Team selectedTeam = DatabaseManager.Instance.LoadTeamsFromCsv()[index];
		PopulateTeamPlayerScrollView(teamBPlayerScrollView, selectedTeam);
		}

	// --- Populate Team Players ScrollView --- //
	private void PopulateTeamPlayerScrollView(GameObject scrollView, Team team)
		{
		// Clear existing player toggles
		foreach (Transform child in scrollView.transform)
			{
			Destroy(child.gameObject);
			}

		// Retrieve players for the selected team and create toggles
		List<Player> players = DatabaseManager.Instance.LoadPlayersFromCsv();  // Get players
		List<Player> teamPlayers = players.FindAll(p => p.TeamId == team.TeamId);  // Filter by team ID

		foreach (var player in teamPlayers)
			{
			GameObject playerToggle = Instantiate(playerTogglePrefab, scrollView.transform);
			TMP_Text playerNameText = playerToggle.GetComponentInChildren<TMP_Text>();
			playerNameText.text = player.PlayerName;
			Toggle playerToggleComponent = playerToggle.GetComponent<Toggle>();
			playerToggleComponent.onValueChanged.AddListener((bool value) => { OnPlayerToggleChanged(value, player); });
			}
		}

	// --- Handle Player Toggle Changes --- //
	private void OnPlayerToggleChanged(bool isSelected, Player player)
		{
		// Handle logic when a player toggle is selected or deselected (e.g., store player selection for comparison)
		Debug.Log($"Player {player.PlayerName} selected: {isSelected}");
		}

	// --- On Compare Button Clicked --- //
	public void OnCompareButtonClicked()
		{
		// Gather selected data for team A and team B
		string teamAName = teamADropdown.options[teamADropdown.value].text;
		string teamBName = teamBDropdown.options[teamBDropdown.value].text;

		// You would need to gather scores and winning information, possibly based on selected players
		int teamAScore = 30; // Example score, replace with actual score logic
		int teamBScore = 25; // Example score, replace with actual score logic

		// Example calculation of win probabilities (replace with actual calculation logic)
		float teamAWinProbability = 0.65f;
		float teamBWinProbability = 0.35f;

		// Example of determining the winning team
		string winningTeam = teamAScore > teamBScore ? teamAName : teamBName;

		// Create a new MatchupResultData with gathered data
		MatchupResultData matchupResult = new(teamAName, teamBName, teamAScore, teamBScore,
												teamAWinProbability, teamBWinProbability, winningTeam);

		// Now pass the matchupResult to the ShowMatchupResultsPanel method
		UIManager.Instance.ShowMatchupResultsPanel(matchupResult);
		}

	// --- Back Button --- //
	public void OnBackButtonClicked()
		{
		// Navigate back to the previous panel
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}
