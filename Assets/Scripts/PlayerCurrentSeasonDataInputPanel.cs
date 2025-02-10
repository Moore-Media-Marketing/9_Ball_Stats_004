using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentSeasonDataInputPanel:MonoBehaviour
	{
	// UI Elements
	public TMP_Text headerText;

	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField gamesWonInputField;
	public TMP_InputField gamesPlayedInputField;
	public TMP_InputField totalPointsInputField;
	public TMP_InputField ppmInputField;
	public TMP_InputField paInputField;
	public TMP_InputField breakAndRunInputField;
	public TMP_InputField miniSlamsInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField shutoutsInputField;
	public TMP_Dropdown skillLevelDropdown;
	public Button matchupComparisonButton;
	public Button saveToCSVButton;
	public Button backButton;

	private List<Player> players;

	private void Start()
		{
		// Initialize player list and dropdowns
		players = DatabaseManager.Instance.GetAllPlayers();
		PopulateTeamDropdown();
		PopulateSkillLevelDropdown();

		// Button listeners
		teamNameDropdown.onValueChanged.AddListener(OnTeamSelected);
		saveToCSVButton.onClick.AddListener(SaveToCSV);
		backButton.onClick.AddListener(() => UIManager.Instance.GoBackToPreviousPanel());
		matchupComparisonButton.onClick.AddListener(OpenMatchupComparisonPanel);
		}

	// Populate the team dropdown with team names
	private void PopulateTeamDropdown()
		{
		List<Team> teams = DatabaseManager.Instance.GetAllTeams();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.TeamName).ToList());
		teamNameDropdown.AddOptions(new List<string> { "All Teams" }); // Option for all teams
		teamNameDropdown.onValueChanged.AddListener(OnTeamSelected);
		OnTeamSelected(0); // Set default to show all players
		}

	// Populate skill level dropdown with values from 1 to 9
	private void PopulateSkillLevelDropdown()
		{
		skillLevelDropdown.ClearOptions();
		skillLevelDropdown.AddOptions(Enumerable.Range(1, 9).Select(i => i.ToString()).ToList());
		}

	// Update player dropdown when a team is selected
	private void OnTeamSelected(int index)
		{
		string selectedTeamName = teamNameDropdown.options[index].text;
		List<Player> filteredPlayers;

		if (selectedTeamName == "All Teams")
			{
			filteredPlayers = players; // Show all players
			}
		else
			{
			filteredPlayers = players.Where(p => p.TeamName == selectedTeamName).ToList();
			}

		PopulatePlayerDropdown(filteredPlayers);
		}

	// Populate the player dropdown based on the selected team
	private void PopulatePlayerDropdown(List<Player> filteredPlayers)
		{
		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(filteredPlayers.Select(p => p.PlayerName).ToList());
		}

	// Save the inputted data to the database and CSV
	private void SaveToCSV()
		{
		string playerName = playerNameDropdown.options[playerNameDropdown.value].text;
		Player selectedPlayer = players.FirstOrDefault(p => p.PlayerName == playerName);

		if (selectedPlayer == null)
			{
			Debug.LogWarning("No player selected.");
			return;
			}

		// Validate inputs
		if (!ValidateInputs())
			{
			Debug.LogWarning("Please fill in all fields.");
			return;
			}

		// Save current season data
		selectedPlayer.Stats.CurrentSeasonMatchesWon = int.Parse(gamesWonInputField.text);
		selectedPlayer.Stats.CurrentSeasonMatchesPlayed = int.Parse(gamesPlayedInputField.text);
		selectedPlayer.Stats.CurrentSeasonTotalPoints = int.Parse(totalPointsInputField.text);
		selectedPlayer.Stats.CurrentSeasonPpm = int.Parse(ppmInputField.text);
		selectedPlayer.Stats.CurrentSeasonPaPercentage = float.Parse(paInputField.text);
		selectedPlayer.Stats.CurrentSeasonBreakAndRun = int.Parse(breakAndRunInputField.text);
		selectedPlayer.Stats.CurrentSeasonMiniSlams = int.Parse(miniSlamsInputField.text);
		selectedPlayer.Stats.CurrentSeasonNineOnTheSnap = int.Parse(nineOnTheSnapInputField.text);
		selectedPlayer.Stats.CurrentSeasonShutouts = int.Parse(shutoutsInputField.text);
		selectedPlayer.Stats.CurrentSeasonSkillLevel = int.Parse(skillLevelDropdown.options[skillLevelDropdown.value].text);

		// Save to database (CSV)
		DatabaseManager.Instance.UpdatePlayerStats(selectedPlayer.PlayerId, selectedPlayer.Stats);
		DatabaseManager.Instance.SavePlayersToCSV();

		Debug.Log($"Current season data for {selectedPlayer.PlayerName} saved.");
		}

	// Validate all input fields
	private bool ValidateInputs()
		{
		return !string.IsNullOrEmpty(gamesWonInputField.text) &&
			   !string.IsNullOrEmpty(gamesPlayedInputField.text) &&
			   !string.IsNullOrEmpty(totalPointsInputField.text) &&
			   !string.IsNullOrEmpty(ppmInputField.text) &&
			   !string.IsNullOrEmpty(paInputField.text) &&
			   !string.IsNullOrEmpty(breakAndRunInputField.text) &&
			   !string.IsNullOrEmpty(miniSlamsInputField.text) &&
			   !string.IsNullOrEmpty(nineOnTheSnapInputField.text) &&
			   !string.IsNullOrEmpty(shutoutsInputField.text);
		}

	// Open Matchup Comparison Panel
	private void OpenMatchupComparisonPanel()
		{
		UIManager.Instance.ShowComparisonPanel(); // Replace GoToPanel with ShowComparisonPanel
		}
	}