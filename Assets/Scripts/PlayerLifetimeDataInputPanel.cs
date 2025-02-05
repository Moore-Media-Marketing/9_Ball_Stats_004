using UnityEngine;
using TMPro; // Corrected: Using TextMeshPro for text and dropdown
using System.Collections.Generic;
using System.Linq;

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	[Header("UI Elements")]
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;

	public TMP_InputField lifetimeGamesWonInputField;
	public TMP_InputField lifetimeGamesPlayedInputField;
	public TMP_InputField lifetimeDefensiveShotAvgInputField;
	public TMP_InputField matchesPlayedInLast2YearsInputField;
	public TMP_InputField lifetimeBreakAndRunInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField lifetimeMiniSlamsInputField;
	public TMP_InputField lifetimeShutoutsInputField;

	public TMP_Text updateLifetimeButtonText;
	public TMP_Text saveToCSVButtonText;
	public TMP_Text backButtonText;

	private List<Team> teams = new();  // List of available teams
	private List<Player> players = new();  // List of available players

	// --- Initialization --- //
	private void Start()
		{
		// Setup initial UI and data loading
		PopulateTeamDropdown();
		PopulatePlayerDropdown();

		// Add event listeners for dropdown changes
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownChanged);
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownChanged);

		// Add event listeners for the buttons
		updateLifetimeButtonText.text = "Update Lifetime";
		saveToCSVButtonText.text = "Save to CSV";
		backButtonText.text = "Back";
		}

	// --- Dropdown Population --- //
	private void PopulateTeamDropdown()
		{
		teams = DatabaseManager.Instance.LoadTeamsFromCsv(); // Corrected method call
		List<string> teamNames = teams.Select(t => t.TeamName).ToList();

		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teamNames);
		}

	private void PopulatePlayerDropdown()
		{
		List<string> playerNames = players.Select(p => p.PlayerName).ToList();

		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(playerNames);
		}

	// --- Dropdown Value Changed Handlers --- //
	private void OnTeamDropdownChanged(int selectedIndex)
		{
		// Handle team selection change (e.g., filter players based on team)
		Team selectedTeam = teams[selectedIndex];
		players = DatabaseManager.Instance.LoadPlayersFromCsv(); // Corrected method call
		PopulatePlayerDropdown();
		}

	private void OnPlayerDropdownChanged(int selectedIndex)
		{
		// Handle player selection change
		Player selectedPlayer = players[selectedIndex];
		PopulatePlayerLifetimeData(selectedPlayer);
		}

	// --- Player Lifetime Data --- //
	private void PopulatePlayerLifetimeData(Player player)
		{
		lifetimeGamesWonInputField.text = player.LifetimeGamesWon.ToString();
		lifetimeGamesPlayedInputField.text = player.LifetimeGamesPlayed.ToString();
		lifetimeDefensiveShotAvgInputField.text = player.LifetimeDefensiveShotAvg.ToString("F2");
		matchesPlayedInLast2YearsInputField.text = player.LifetimeMatchesPlayedInLast2Years.ToString();
		lifetimeBreakAndRunInputField.text = player.LifetimeBreakAndRun.ToString();
		nineOnTheSnapInputField.text = player.LifetimeNineOnTheSnap.ToString();
		lifetimeMiniSlamsInputField.text = player.LifetimeMiniSlams.ToString();
		lifetimeShutoutsInputField.text = player.LifetimeShutouts.ToString();
		}

	// --- Button Handlers --- //
	public void OnUpdateLifetimeButtonClicked()
		{
		// Handle the Update Lifetime button logic here (e.g., save the data back to the player object)
		int selectedPlayerIndex = playerNameDropdown.value;
		Player selectedPlayer = players[selectedPlayerIndex];

		// Update player stats from input fields
		selectedPlayer.LifetimeGamesWon = int.Parse(lifetimeGamesWonInputField.text);
		selectedPlayer.LifetimeGamesPlayed = int.Parse(lifetimeGamesPlayedInputField.text);
		selectedPlayer.LifetimeDefensiveShotAvg = float.Parse(lifetimeDefensiveShotAvgInputField.text);
		selectedPlayer.LifetimeMatchesPlayedInLast2Years = int.Parse(matchesPlayedInLast2YearsInputField.text);
		selectedPlayer.LifetimeBreakAndRun = int.Parse(lifetimeBreakAndRunInputField.text);
		selectedPlayer.LifetimeNineOnTheSnap = int.Parse(nineOnTheSnapInputField.text);
		selectedPlayer.LifetimeMiniSlams = int.Parse(lifetimeMiniSlamsInputField.text);
		selectedPlayer.LifetimeShutouts = int.Parse(lifetimeShutoutsInputField.text);

		// Optionally, save these updated values to CSV here.
		Debug.Log($"Updated lifetime data for player: {selectedPlayer.PlayerName}");
		}

	public void OnSaveToCSVButtonClicked()
		{
		// Save the updated player data to the CSV (or database)
		DatabaseManager.Instance.SavePlayersToCsv(players); // Corrected method call
		Debug.Log("Player data saved to CSV.");
		}

	public void OnBackButtonClicked()
		{
		// Navigate back to the previous screen (e.g., close panel)
		UIManager.Instance.GoBackToPreviousPanel(); // Handle going back in your UIManager
		Debug.Log("Back button clicked. Navigating back...");
		}

	// --- CSV Helpers --- //
	// Helper methods for loading data from CSV files and saving the updated data
	}
