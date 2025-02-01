using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	// --- UI Elements --- //
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

	public Button updateLifetimeButton;
	public Button backButton;

	// Store player data
	private Team selectedTeam;

	private Player selectedPlayer;

	// Singleton Instance
	public static PlayerLifetimeDataInputPanel Instance { get; private set; }

	private void Awake()
		{
		// Ensure there is only one instance of PlayerLifetimeDataInputPanel
		if (Instance == null)
			{
			Instance = this;
			Debug.Log("PlayerLifetimeDataInputPanel Instance set.");
			}
		else
			{
			Destroy(gameObject); // Destroy duplicates
			Debug.LogError("Duplicate PlayerLifetimeDataInputPanel instance destroyed.");
			}
		}

	private void Start()
		{
		if (Instance == null)
			{
			Debug.LogError("PlayerLifetimeDataInputPanel instance is still null at Start!");
			return;
			}

		// --- Button Listeners --- //
		backButton.onClick.AddListener(OnBackButtonClicked);
		updateLifetimeButton.onClick.AddListener(OnUpdateLifetimeButtonClicked);

		// Initialize dropdowns
		InitializeDropdowns();
		}

	// --- Method to initialize and populate dropdowns --- //
	private void InitializeDropdowns()
		{
		// Populate team dropdown
		var teams = DatabaseManager.Instance.GetAllTeams();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.name).ToList());

		// Populate player dropdown when a team is selected
		teamNameDropdown.onValueChanged.AddListener(OnTeamSelected);
		Debug.Log("Team dropdown initialized.");
		}


	private Player currentPlayer;

	// --- Set the player data for the panel --- //
	public void SetPlayerData(Player player)
		{
		selectedPlayer = player;

		// --- Update UI elements with player's data --- //

		// Populate the input fields with player's lifetime data
		lifetimeGamesWonInputField.text = selectedPlayer.lifetimeGamesWon.ToString();
		lifetimeGamesPlayedInputField.text = selectedPlayer.lifetimeGamesPlayed.ToString();
		lifetimeDefensiveShotAvgInputField.text = selectedPlayer.lifetimeDefensiveShotAvg.ToString();
		matchesPlayedInLast2YearsInputField.text = selectedPlayer.matchesPlayedInLast2Years.ToString();
		lifetimeBreakAndRunInputField.text = selectedPlayer.lifetimeBreakAndRun.ToString();
		nineOnTheSnapInputField.text = selectedPlayer.lifetimeNineOnTheSnap.ToString();
		lifetimeMiniSlamsInputField.text = selectedPlayer.lifetimeMiniSlams.ToString();
		lifetimeShutoutsInputField.text = selectedPlayer.lifetimeShutouts.ToString();

		Debug.Log($"Populated lifetime data for player {selectedPlayer.name}");
		}




	// --- Method when team is selected from dropdown --- //
	private void OnTeamSelected(int teamIndex)
		{
		if (teamIndex > 0) // Make sure "Select Team" is not selected
			{
			selectedTeam = DatabaseManager.Instance.GetAllTeams().ElementAt(teamIndex - 1); // Offset by 1 for "Select Team" option

			// Fetch players for the selected team
			var players = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.id);

			// Debug the player list
			Debug.Log($"Players for {selectedTeam.name}: {players.Count} players found.");

			// If players list is empty, log that info
			if (players.Count == 0)
				{
				Debug.LogError($"No players found for team: {selectedTeam.name}");
				}

			playerNameDropdown.ClearOptions();

			// Check if players exist before adding options
			if (players.Any())
				{
				playerNameDropdown.AddOptions(players.Select(p => p.name).ToList());
				}
			else
				{
				playerNameDropdown.AddOptions(new List<string> { "No players available" });
				Debug.LogWarning("No players available for the selected team.");
				}

			playerNameDropdown.onValueChanged.AddListener(OnPlayerSelected);
			Debug.Log($"Team selected: {selectedTeam.name}");
			}
		else
			{
			playerNameDropdown.ClearOptions(); // Clear player dropdown if no team is selected
			Debug.Log("No team selected.");
			}
		}

	// --- When a player is selected from PlayerNameDropdown, populate data --- //
	private void OnPlayerSelected(int playerIndex)
		{
		if (playerIndex > 0) // Make sure "Select Player" is not selected
			{
			selectedPlayer = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.id)
				.FirstOrDefault(p => p.name == playerNameDropdown.options[playerIndex].text);

			if (selectedPlayer != null)
				{
				PopulateLifetimeData(selectedPlayer);
				Debug.Log($"Player selected: {selectedPlayer.name}");
				}
			else
				{
				Debug.LogError("Selected player not found.");
				}
			}
		else
			{
			// Clear player data if no player is selected
			ResetLifetimeDataFields();
			Debug.Log("No player selected.");
			}
		}

	// --- Populate lifetime data for selected player --- //
	private void PopulateLifetimeData(Player player)
		{
		// Fill input fields with saved data for the selected player
		lifetimeGamesWonInputField.text = player.lifetimeGamesWon.ToString();
		lifetimeGamesPlayedInputField.text = player.lifetimeGamesPlayed.ToString();
		lifetimeDefensiveShotAvgInputField.text = player.lifetimeDefensiveShotAvg.ToString();
		matchesPlayedInLast2YearsInputField.text = player.matchesPlayedInLast2Years.ToString();
		lifetimeBreakAndRunInputField.text = player.lifetimeBreakAndRun.ToString();
		nineOnTheSnapInputField.text = player.lifetimeNineOnTheSnap.ToString();
		lifetimeMiniSlamsInputField.text = player.lifetimeMiniSlams.ToString();
		lifetimeShutoutsInputField.text = player.lifetimeShutouts.ToString();

		Debug.Log($"Populated lifetime data for player {player.name}");
		}

	// --- Reset lifetime data fields --- //
	private void ResetLifetimeDataFields()
		{
		lifetimeGamesWonInputField.text = "";
		lifetimeGamesPlayedInputField.text = "";
		lifetimeDefensiveShotAvgInputField.text = "";
		matchesPlayedInLast2YearsInputField.text = "";
		lifetimeBreakAndRunInputField.text = "";
		nineOnTheSnapInputField.text = "";
		lifetimeMiniSlamsInputField.text = "";
		lifetimeShutoutsInputField.text = "";

		Debug.Log("Reset lifetime data fields.");
		}

	// --- On Back button clicked --- //
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("Back button clicked, showing home panel.");
		}

	// --- On Update Lifetime button clicked --- //
	private void OnUpdateLifetimeButtonClicked()
		{
		// Check if player data exists, if not return early
		if (selectedPlayer == null)
			{
			Debug.LogError("No player selected!");
			return;
			}

		// Check if all input fields are valid
		if (!int.TryParse(lifetimeGamesWonInputField.text, out int lifetimeGamesWon) ||
			!int.TryParse(lifetimeGamesPlayedInputField.text, out int lifetimeGamesPlayed) ||
			!float.TryParse(lifetimeDefensiveShotAvgInputField.text, out float lifetimeDefensiveShotAvg) ||
			!int.TryParse(matchesPlayedInLast2YearsInputField.text, out int matchesPlayedInLast2Years) ||
			!int.TryParse(lifetimeBreakAndRunInputField.text, out int lifetimeBreakAndRun) ||
			!int.TryParse(nineOnTheSnapInputField.text, out int nineOnTheSnap) ||
			!int.TryParse(lifetimeMiniSlamsInputField.text, out int lifetimeMiniSlams) ||
			!int.TryParse(lifetimeShutoutsInputField.text, out int lifetimeShutouts))
			{
			Debug.LogError("Invalid input data! Please check the fields.");
			return;
			}

		// Update player lifetime data
		selectedPlayer.lifetimeGamesWon = lifetimeGamesWon;
		selectedPlayer.lifetimeGamesPlayed = lifetimeGamesPlayed;
		selectedPlayer.lifetimeDefensiveShotAvg = lifetimeDefensiveShotAvg;
		selectedPlayer.matchesPlayedInLast2Years = matchesPlayedInLast2Years;
		selectedPlayer.lifetimeBreakAndRun = lifetimeBreakAndRun;
		selectedPlayer.lifetimeNineOnTheSnap = nineOnTheSnap;
		selectedPlayer.lifetimeMiniSlams = lifetimeMiniSlams;
		selectedPlayer.lifetimeShutouts = lifetimeShutouts;

		// Save the updated player data to the database
		DatabaseManager.Instance.SavePlayer(selectedPlayer);

		Debug.Log($"Updated lifetime data for player {selectedPlayer.name}");
		}

	// --- Methods to open the panel and populate data for a specific player --- //
	// --- Open the panel and populate data for a specific player --- //
	public void OpenWithPlayerData(Player player)
		{
		SetPlayerData(player); // Populate data before showing the panel
		UIManager.Instance.ShowPanel(this.gameObject); // Ensure the panel is shown
		Debug.Log($"Opened panel with player data: {player.name}");
		}

	// Open the panel without player data (if no player is selected)
	// --- Open the panel without player data --- //
	public void OpenWithNoData()
		{
		selectedPlayer = null; // No player selected
		ResetLifetimeDataFields(); // Clear all data fields
		UIManager.Instance.ShowPanel(this.gameObject); // Ensure the panel is shown
		Debug.Log("Opening without player data. Please select a player.");
		}
	}
