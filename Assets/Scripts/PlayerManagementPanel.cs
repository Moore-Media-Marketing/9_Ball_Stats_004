// --- Region: Player Management Panel --- //
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementPanel:MonoBehaviour
	{
	#region UI Elements

	[Header("UI Elements")]
	[Tooltip("Header text for the panel.")]
	public TMP_Text headerText;  // --- Header text for the panel ---

	[Tooltip("Dropdown for selecting team names.")]
	public TMP_Dropdown teamNameDropdown;  // --- Dropdown listing teams ---

	[Tooltip("Dropdown for selecting player names.")]
	public TMP_Dropdown playerNameDropdown;  // --- Dropdown listing players in the selected team ---

	[Tooltip("Input field for entering a new player name.")]
	public TMP_InputField playerNameInputField;  // --- Input field for new player names ---

	[Tooltip("Button to add a new player.")]
	public Button addPlayerButton;  // --- Button to add a new player to the team ---

	[Tooltip("Button to delete a selected player.")]
	public Button deletePlayerButton;  // --- Button to delete a selected player from the team ---

	[Tooltip("Button to add detailed data for a player (opens lifetime data panel).")]
	public Button addPlayerDetailsButton;  // --- Button to show detailed player data (lifetime data) ---

	[Tooltip("Button to go back to the previous panel.")]
	public Button backButton;  // --- Button to return to the previous panel ---

	#endregion UI Elements

	#region Private Fields

	private List<Team> teamList = new();  // --- List of teams loaded from the database ---
	private List<Player> currentTeamPlayers = new();  // --- List of players in the selected team ---
	private Team selectedTeam;  // --- Currently selected team ---
	private Player selectedPlayer;  // --- Currently selected player ---

	#endregion Private Fields

	#region Unity Methods

	private void Start()
		{
		// --- Assign button event listeners --- //
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		deletePlayerButton.onClick.AddListener(OnDeletePlayerClicked);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetailsButtonClicked);
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownValueChanged);

		// --- Load teams and update dropdowns --- //
		LoadTeams();
		UpdateTeamDropdown();
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		}

	#endregion Unity Methods

	#region Data Loading and UI Update

	private void LoadTeams()
		{
		if (DatabaseManager.Instance != null)
			{
			teamList = DatabaseManager.Instance.GetAllTeams();
			Debug.Log("Loaded " + teamList.Count + " teams.");
			}
		else
			{
			Debug.LogError("DatabaseManager.Instance is null! Cannot load teams.");
			}
		}

	private void UpdateTeamDropdown()
		{
		teamNameDropdown.ClearOptions();
		List<string> teamNames = new() { "Select Team" };
		if (teamList != null && teamList.Any())
			{
			teamNames.AddRange(teamList.Select(team => team.Name));
			}
		else
			{
			ShowFeedback("No teams available to select.");
			}
		teamNameDropdown.AddOptions(teamNames);
		}

	private void UpdatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		List<string> playerNames = new() { "Select Player" };
		if (currentTeamPlayers != null && currentTeamPlayers.Any())
			{
			playerNames.AddRange(currentTeamPlayers.Select(player => player.Name));
			}
		else
			{
			playerNames.Add("No players available");
			}
		playerNameDropdown.AddOptions(playerNames);
		selectedPlayer = null;
		}

	#endregion Data Loading and UI Update

	#region Button Event Handlers

	private void OnBackButtonClicked()
		{
		// --- Navigate back to the home panel using UIManager --- //
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("Back button clicked, showing home panel.");
		}

	public void OnAddPlayerClicked()
		{
		string playerName = playerNameInputField.text.Trim();

		// --- Check if player name is empty --- //
		if (string.IsNullOrEmpty(playerName))
			{
			ShowFeedback("Player name cannot be empty.");
			return;
			}

		// --- Check if a valid team is selected --- //
		if (selectedTeam == null)
			{
			ShowFeedback("Please select a team first.");
			return;
			}

		// --- Check if the player already exists in the team --- //
		if (currentTeamPlayers.Any(p => string.Equals(p.Name, playerName, System.StringComparison.OrdinalIgnoreCase)))
			{
			ShowFeedback("Player already exists in the team.");
			return;
			}

		// --- Create a new player with a placeholder skill level (5) --- //
		Player newPlayer = new(playerName, 5, selectedTeam.Id);
		currentTeamPlayers.Add(newPlayer);
		DatabaseManager.Instance.AddPlayer(newPlayer);

		// --- Show feedback and clear the input field --- //
		ShowFeedback($"Player '{playerName}' added to team '{selectedTeam.Name}'.");
		playerNameInputField.text = "";

		// --- Update the player dropdown --- //
		UpdatePlayerDropdown();
		}

	private void OnDeletePlayerClicked()
		{
		if (playerNameDropdown.value > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[playerNameDropdown.value].text;
			Player playerToDelete = currentTeamPlayers.FirstOrDefault(p => p.Name == selectedPlayerName);
			if (playerToDelete != null)
				{
				currentTeamPlayers.Remove(playerToDelete);
				// --- (Optional) Remove player from database if needed --- //
				ShowFeedback($"Player '{playerToDelete.Name}' removed from team '{selectedTeam.Name}'.");
				UpdatePlayerDropdown();
				}
			else
				{
				ShowFeedback("Player not found in the current team.");
				}
			}
		else
			{
			ShowFeedback("Please select a player to delete.");
			}
		}

	private void OnPlayerDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[index].text;
			selectedPlayer = currentTeamPlayers.FirstOrDefault(player => player.Name == selectedPlayerName);
			if (selectedPlayer != null)
				{
				Debug.Log($"Player selected: {selectedPlayer.Name}");
				}
			else
				{
				Debug.LogError("Selected player not found in the list!");
				}
			}
		else
			{
			selectedPlayer = null;
			}
		}

	private void OnTeamDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedTeamName = teamNameDropdown.options[index].text;
			selectedTeam = teamList.FirstOrDefault(team => team.Name == selectedTeamName);
			if (selectedTeam != null)
				{
				currentTeamPlayers = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.Id);
				UpdatePlayerDropdown();
				Debug.Log($"Team selected: {selectedTeam.Name}");
				}
			else
				{
				ShowFeedback("Selected team is not available.");
				}
			}
		else
			{
			playerNameDropdown.ClearOptions();
			selectedTeam = null;
			}
		}

	// --- Event handler for Add Player Details button --- //
	private void OnAddPlayerDetailsButtonClicked()
		{
		// --- Check if a player is selected --- //
		if (selectedTeam == null || selectedPlayer == null)
			{
			Debug.LogWarning("No team or player selected! Cannot open Lifetime Data panel.");
			ShowFeedback("Please select a team and a player first.");
			return;
			}

		// --- Link to PlayerLifetimeDataInputPanel via UIManager ---
		if (UIManager.Instance.playerLifetimeDataInputPanel != null)
			{
			UIManager.Instance.ShowPanel(UIManager.Instance.playerLifetimeDataInputPanel);
			Debug.Log($"Opening Lifetime Data panel for player: {selectedPlayer.Name}");
			}
		else
			{
			Debug.LogError("PlayerLifetimeDataInputPanel is not assigned in UIManager!");
			ShowFeedback("Lifetime Data panel is not available.");
			}
		}

	#endregion Button Event Handlers

	#region Feedback Functions

	private void ShowFeedback(string message)
		{
		if (FeedbackOverlay.Instance != null)
			{
			// --- Display feedback message for 2 seconds --- //
			FeedbackOverlay.Instance.ShowFeedback(message, 2f);
			}
		else
			{
			Debug.LogError("FeedbackOverlay.Instance is null!");
			}
		}

	#endregion Feedback Functions

	#region Additional Functions

	// --- Additional custom functions can be added here --- //

	#endregion Additional Functions
	}