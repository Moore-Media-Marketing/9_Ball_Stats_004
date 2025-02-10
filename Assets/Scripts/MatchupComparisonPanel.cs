using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MatchupComparisonPanel:MonoBehaviour
	{
	// References to UI elements
	public TMP_Dropdown teamADropdown;
	public TMP_Dropdown teamBDropdown;
	public GameObject playerTogglePrefab;
	public Transform teamAPlayerScrollViewContent;
	public Transform teamBPlayerScrollViewContent;
	public GameObject matchupResultsPanel;
	public GameObject matchupComparisonPanel;
	public Button compareButton; // Added the reference for the Compare button

	private List<Player> selectedTeamAPlayers = new();
	private List<Player> selectedTeamBPlayers = new();

	private void Start()
		{
		// Populate the team dropdowns when the panel is activated
		PopulateTeamDropdowns();

		// Add listeners to dropdowns to load players when the team changes
		teamADropdown.onValueChanged.AddListener(delegate { LoadTeamPlayers(true); });
		teamBDropdown.onValueChanged.AddListener(delegate { LoadTeamPlayers(false); });

		// Add listener for the Compare button to trigger the matchup comparison
		compareButton.onClick.AddListener(CompareMatchup);
		}

	// Populates the dropdowns with team names
	private void PopulateTeamDropdowns()
		{
		// Clear existing options
		teamADropdown.ClearOptions();
		teamBDropdown.ClearOptions();

		// Get all teams from the database
		List<Team> allTeams = DatabaseManager.Instance.GetAllTeams();
		if (allTeams == null || allTeams.Count == 0)
			{
			Debug.LogError("No teams available to load into the dropdowns.");
			return;
			}

		// Add the teams to the dropdown lists
		List<string> teamNames = allTeams.Select(team => team.TeamName).ToList();
		teamADropdown.AddOptions(teamNames);
		teamBDropdown.AddOptions(teamNames);

		// Optionally select the first team by default
		if (teamNames.Count > 0)
			{
			teamADropdown.value = 0;
			teamBDropdown.value = 0;
			LoadTeamPlayers(true); // Load players for team A by default
			LoadTeamPlayers(false); // Load players for team B by default
			}
		}

	// Loads players for the selected team into the player selection UI
	private void LoadTeamPlayers(bool isTeamA)
		{
		TMP_Dropdown teamDropdown = isTeamA ? teamADropdown : teamBDropdown;
		Transform contentContainer = isTeamA ? teamAPlayerScrollViewContent : teamBPlayerScrollViewContent;
		List<Player> selectedPlayers = isTeamA ? selectedTeamAPlayers : selectedTeamBPlayers;

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
					Debug.Log($"Selected players for team {(isTeamA ? "A" : "B")}: {selectedPlayers.Count}");
					});
			}
		}

	// Compares the selected teams and players
	public void CompareMatchup()
		{
		Debug.Log("CompareMatchup called");

		// Debug log the number of selected players for both teams
		Debug.Log($"Team A selected players count: {selectedTeamAPlayers.Count}");
		Debug.Log($"Team B selected players count: {selectedTeamBPlayers.Count}");

		// Check if the matchup results panel reference is missing
		if (matchupResultsPanel == null)
			{
			Debug.LogError("MatchupResultsPanel reference is missing!");
			return;
			}

		// Check if both teams have at least one player selected
		if (selectedTeamAPlayers.Count == 0 || selectedTeamBPlayers.Count == 0)
			{
			Debug.LogError("Both teams must have at least one selected player!");
			return;
			}

		// Debug log the selected players
		Debug.Log($"Team A selected players: {selectedTeamAPlayers.Count}");
		Debug.Log($"Team B selected players: {selectedTeamBPlayers.Count}");

		// Open the MatchupResultsPanel and pass data for display
		MatchupResultsPanel.Instance.DisplayMatchupResults(selectedTeamAPlayers, selectedTeamBPlayers);

		// Close the current MatchupComparisonPanel
		matchupComparisonPanel.SetActive(false);
		}

	}
