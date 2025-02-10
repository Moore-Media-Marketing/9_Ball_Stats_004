using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MatchupComparisonPanel:MonoBehaviour
	{
	// References to UI elements
	public TMP_Dropdown team1Dropdown;
	public TMP_Dropdown team2Dropdown;
	public GameObject playerTogglePrefab;
	public Transform team1PlayerScrollViewContent;
	public Transform team2PlayerScrollViewContent;
	public GameObject matchupResultsPanel;
	public GameObject matchupComparisonPanel;
	public Button compareButton; // Added the reference for the Compare button

	private List<Player> selectedTeam1Players = new();
	private List<Player> selectedTeam2Players = new();

	private void Start()
		{
		// Populate the team dropdowns when the panel is activated
		PopulateTeamDropdowns();

		// Add listeners to dropdowns to load players when the team changes
		team1Dropdown.onValueChanged.AddListener(delegate { LoadTeamPlayers(true); });
		team2Dropdown.onValueChanged.AddListener(delegate { LoadTeamPlayers(false); });

		// Add listener for the Compare button to trigger the matchup comparison
		compareButton.onClick.AddListener(CompareMatchup);
		}

	// Populates the dropdowns with team names
	private void PopulateTeamDropdowns()
		{
		// Clear existing options
		team1Dropdown.ClearOptions();
		team2Dropdown.ClearOptions();

		// Get all teams from the database
		List<Team> allTeams = DatabaseManager.Instance.GetAllTeams();
		if (allTeams == null || allTeams.Count == 0)
			{
			Debug.LogError("No teams available to load into the dropdowns.");
			return;
			}

		// Add the teams to the dropdown lists
		List<string> teamNames = allTeams.Select(team => team.TeamName).ToList();
		team1Dropdown.AddOptions(teamNames);
		team2Dropdown.AddOptions(teamNames);

		// Optionally select the first team by default
		if (teamNames.Count > 0)
			{
			team1Dropdown.value = 0;
			team2Dropdown.value = 0;
			LoadTeamPlayers(true); // Load players for team 1 by default
			LoadTeamPlayers(false); // Load players for team 2 by default
			}
		}

	// Loads players for the selected team into the player selection UI
	private void LoadTeamPlayers(bool isTeam1)
		{
		TMP_Dropdown teamDropdown = isTeam1 ? team1Dropdown : team2Dropdown;
		Transform contentContainer = isTeam1 ? team1PlayerScrollViewContent : team2PlayerScrollViewContent;
		List<Player> selectedPlayers = isTeam1 ? selectedTeam1Players : selectedTeam2Players;

		// Clear previous selections
		foreach (Transform child in contentContainer)
			{
			Destroy(child.gameObject);
			}
		selectedPlayers.Clear();

		string selectedTeamName = teamDropdown.options[teamDropdown.value].text;
		Team selectedTeam = DatabaseManager.Instance.GetAllTeams().FirstOrDefault(t => t.TeamName == selectedTeamName);

		if (selectedTeam == null)
			{
			Debug.LogError($"Team not found: {selectedTeamName}");
			return;
			}

		List<Player> teamPlayers = DatabaseManager.Instance.GetAllPlayers().Where(p => p.TeamId == selectedTeam.TeamId).ToList();

		if (teamPlayers.Count == 0)
			{
			Debug.LogError($"No players found for team: {selectedTeamName}");
			return;
			}

		// Debug log the number of players being loaded
		Debug.Log($"Loaded {teamPlayers.Count} players for team: {selectedTeamName}");

		foreach (Player player in teamPlayers)
			{
			GameObject toggleObj = Instantiate(playerTogglePrefab, contentContainer);
			Toggle playerToggle = toggleObj.GetComponent<Toggle>();
			TMP_Text playerText = toggleObj.GetComponentInChildren<TMP_Text>();

			playerText.text = $"{player.PlayerName} (Skill {player.Stats.CurrentSeasonSkillLevel})";

			// Log for each toggle created
			Debug.Log($"Created toggle for player: {player.PlayerName}");

			playerToggle.onValueChanged.AddListener(delegate
				{
					if (playerToggle.isOn)
						{
						if (!selectedPlayers.Contains(player))
							{
							selectedPlayers.Add(player);
							Debug.Log($"{player.PlayerName} added to the selected players.");
							}
						}
					else
						{
						selectedPlayers.Remove(player);
						Debug.Log($"{player.PlayerName} removed from the selected players.");
						}

					// Log selected players count
					Debug.Log($"Selected players for team {(isTeam1 ? "1" : "2")}: {selectedPlayers.Count}");
					});
			}
		}

	// Compares the selected teams and players
	public void CompareMatchup()
		{
		Debug.Log("CompareMatchup called");

		// Debug log the number of selected players for both teams
		Debug.Log($"Team 1 selected players count: {selectedTeam1Players.Count}");
		Debug.Log($"Team 2 selected players count: {selectedTeam2Players.Count}");

		// Check if both teams have at least one player selected
		if (selectedTeam1Players.Count == 0 || selectedTeam2Players.Count == 0)
			{
			Debug.LogError("Both teams must have at least one selected player!");
			return;
			}

		// Debug log the selected players
		Debug.Log($"Team 1 selected players: {selectedTeam1Players.Count}");
		foreach (var player in selectedTeam1Players)
			{
			Debug.Log($"- {player.PlayerName}");
			}

		Debug.Log($"Team 2 selected players: {selectedTeam2Players.Count}");
		foreach (var player in selectedTeam2Players)
			{
			Debug.Log($"- {player.PlayerName}");
			}

		// Open the MatchupResultsPanel and pass data for display
		MatchupResultsPanel.Instance.DisplayMatchupResults(selectedTeam1Players, selectedTeam2Players);

		// Close the current MatchupComparisonPanel
		matchupComparisonPanel.SetActive(false);
		}
	}
