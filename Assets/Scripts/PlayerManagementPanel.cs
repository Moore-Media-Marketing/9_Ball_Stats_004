using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI and logic for player management, including adding, deleting, and updating players.
/// </summary>
public class PlayerManagementPanel:MonoBehaviour
	{
	// --- UI Elements ---
	public TMP_Text headerText;

	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button saveToCSVButton;
	public Button backButton;

	// --- Data Storage ---
	private List<Player> players;

	private List<Team> teams;

	/// <summary>
	/// Initializes the panel, populates dropdowns, and assigns button listeners.
	/// </summary>
	private void Start()
		{
		// Load data from the database
		players = DatabaseManager.Instance.GetAllPlayers();
		teams = DatabaseManager.Instance.GetAllTeams();

		// Populate dropdowns
		PopulateTeamDropdown();
		PopulatePlayerDropdown();

		// Attach button listeners
		addPlayerButton.onClick.AddListener(AddPlayer);
		deletePlayerButton.onClick.AddListener(DeletePlayer);
		addPlayerDetailsButton.onClick.AddListener(AddPlayerDetails);
		saveToCSVButton.onClick.AddListener(SaveToCSV);
		backButton.onClick.AddListener(() => UIManager.Instance.GoBackToPreviousPanel());
		}

	/// <summary>
	/// Populates the team dropdown with team names from the database.
	/// </summary>
	private void PopulateTeamDropdown()
		{
		teamNameDropdown.ClearOptions();
		teams = DatabaseManager.Instance.GetAllTeams();

		if (teams != null && teams.Count > 0)
			{
			teamNameDropdown.AddOptions(teams.Select(t => t.TeamName).ToList());
			}
		else
			{
			Debug.LogWarning("No teams available to populate the dropdown.");
			}
		}

	/// <summary>
	/// Populates the player dropdown with player names from the database.
	/// </summary>
	private void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		players = DatabaseManager.Instance.GetAllPlayers();

		if (players != null && players.Count > 0)
			{
			playerNameDropdown.AddOptions(players.Select(p => p.PlayerName).ToList());
			}
		else
			{
			Debug.LogWarning("No players available to populate the dropdown.");
			}
		}

	/// <summary>
	/// Adds a new player to the database and refreshes the dropdown.
	/// </summary>
	private void AddPlayer()
		{
		string playerName = playerNameInputField.text;
		if (string.IsNullOrEmpty(playerName))
			{
			Debug.LogWarning("Player name cannot be empty.");
			return;
			}

		if (teamNameDropdown.options.Count == 0)
			{
			Debug.LogWarning("No teams available to assign the player.");
			return;
			}

		string teamName = teamNameDropdown.options[teamNameDropdown.value].text;
		int teamId = teams.FirstOrDefault(t => t.TeamName == teamName)?.TeamId ?? -1;

		if (teamId == -1)
			{
			Debug.LogWarning($"Team '{teamName}' not found.");
			return;
			}

		// Generate a unique player ID
		int playerId = GeneratePlayerId();

		// Default skill level (can be modified based on UI input later)
		int skillLevel = 5;

		// Generate initial player stats
		PlayerStats stats = SampleDataGenerator.Instance.GeneratePlayerStats(skillLevel);

		// Add player to database
		DatabaseManager.Instance.AddPlayer(playerId, playerName, teamId, stats);

		Debug.Log($"Player '{playerName}' added to team '{teamName}'.");

		// Refresh UI
		PopulatePlayerDropdown();
		playerNameInputField.text = "";
		}

	/// <summary>
	/// Deletes the selected player from the database.
	/// </summary>
	private void DeletePlayer()
		{
		if (playerNameDropdown.value < 0 || playerNameDropdown.value >= players.Count)
			{
			Debug.LogWarning("No player selected to delete.");
			return;
			}

		string playerName = playerNameDropdown.options[playerNameDropdown.value].text;
		int playerId = players.FirstOrDefault(p => p.PlayerName == playerName)?.PlayerId ?? -1;

		if (playerId == -1)
			{
			Debug.LogWarning($"Player '{playerName}' not found.");
			return;
			}

		DatabaseManager.Instance.DeletePlayer(playerId);
		Debug.Log($"Player '{playerName}' deleted.");

		// Refresh UI
		PopulatePlayerDropdown();
		}

	/// <summary>
	/// Updates the selected player's stats.
	/// </summary>
	private void AddPlayerDetails()
		{
		if (playerNameDropdown.value < 0 || playerNameDropdown.value >= players.Count)
			{
			Debug.LogWarning("No player selected to update details.");
			return;
			}

		Player selectedPlayer = players[playerNameDropdown.value];

		// Apply new stats (modify this part to allow user inputs for stats)
		selectedPlayer.Stats = SampleDataGenerator.Instance.GeneratePlayerStats(selectedPlayer.Stats.CurrentSeasonSkillLevel);

		// Save updated stats to the database
		DatabaseManager.Instance.UpdatePlayerStats(selectedPlayer.PlayerId, selectedPlayer.Stats);

		Debug.Log($"Updated player {selectedPlayer.PlayerName} with new stats.");

		// Refresh UI
		PopulatePlayerDropdown();
		}

	/// <summary>
	/// Saves player data to a CSV file.
	/// </summary>
	private void SaveToCSV()
		{
		string path = System.IO.Path.Combine(Application.persistentDataPath, "PlayerData.csv");
		CSVManager.SavePlayersToCSV(path, players);

		Debug.Log($"Player data saved successfully at: {path}");
		}

	/// <summary>
	/// Generates a unique player ID by finding the max ID and incrementing it.
	/// </summary>
	private int GeneratePlayerId()
		{
		players = DatabaseManager.Instance.GetAllPlayers();
		return players.Count > 0 ? players.Max(p => p.PlayerId) + 1 : 1;
		}
	}