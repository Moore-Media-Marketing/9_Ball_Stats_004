using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementPanel:MonoBehaviour
	{
	// UI Elements
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button saveToCSVButton;
	public Button backButton;

	private List<Player> players;

	void Start()
		{
		// Initialize players and dropdowns
		players = DatabaseManager.Instance.GetAllPlayers();
		PopulateTeamDropdown();
		PopulatePlayerDropdown();

		// Button listeners
		addPlayerButton.onClick.AddListener(AddPlayer);
		deletePlayerButton.onClick.AddListener(DeletePlayer);
		addPlayerDetailsButton.onClick.AddListener(AddPlayerDetails);
		saveToCSVButton.onClick.AddListener(SaveToCSV);
		backButton.onClick.AddListener(() => UIManager.Instance.GoBackToPreviousPanel());
		}

	// Populate team dropdown
	private void PopulateTeamDropdown()
		{
		List<Team> teams = DatabaseManager.Instance.GetAllTeams();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.TeamName).ToList());
		}

	// Populate player dropdown
	private void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		players = DatabaseManager.Instance.GetAllPlayers();
		playerNameDropdown.AddOptions(players.Select(p => p.PlayerName).ToList());
		}

	// Add player to database
	private void AddPlayer()
		{
		string playerName = playerNameInputField.text;
		string teamName = teamNameDropdown.options[teamNameDropdown.value].text;

		if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(teamName))
			{
			Debug.LogWarning("Please fill in both player name and team.");
			return;
			}

		// Get teamId based on selected team name
		int teamId = DatabaseManager.Instance.GetAllTeams()
						.FirstOrDefault(t => t.TeamName == teamName)?.TeamId ?? -1;

		if (teamId == -1)
			{
			Debug.LogWarning($"Team '{teamName}' not found.");
			return;
			}

		// Generate unique player ID
		int playerId = GeneratePlayerId();

		// Get the skill level from user input or set a default (example: 5)
		int skillLevel = 5;  // Example skill level (you can modify this based on your UI/input)

		// Generate player stats using SampleDataGenerator, passing skillLevel
		PlayerStats stats = SampleDataGenerator.Instance.GeneratePlayerStats(skillLevel);

		// Create a new player object
		Player newPlayer = new(playerId, playerName, teamId, stats);

		// Add player to the database
		DatabaseManager.Instance.AddPlayer(newPlayer.PlayerName, newPlayer.PlayerId, newPlayer.TeamId, newPlayer.Stats);

		// Refresh the dropdown after adding the player
		PopulatePlayerDropdown();
		}

	// Delete player from database
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
		PopulatePlayerDropdown(); // Refresh dropdown after deletion
		}

	// Add or update player details (stats)
	private void AddPlayerDetails()
		{
		if (playerNameDropdown.value < 0 || playerNameDropdown.value >= players.Count)
			{
			Debug.LogWarning("No player selected to update details.");
			return;
			}

		Player selectedPlayer = players[playerNameDropdown.value];

		// Log the update
		Debug.Log($"Updated player {selectedPlayer.PlayerName} with new stats.");

		// Save updated player stats back to DatabaseManager
		DatabaseManager.Instance.UpdatePlayerStats(selectedPlayer.PlayerId, selectedPlayer.Stats);

		// Refresh the dropdown
		PopulatePlayerDropdown();
		}

	// Save to CSV
	private void SaveToCSV()
		{
		string path = System.IO.Path.Combine(Application.persistentDataPath, "PlayerData.csv");
		CSVManager.SavePlayersToCSV(path, players);

		Debug.Log($"Player data saved successfully at: {path}");
		}

	// Generate a unique player ID (simple method, could be improved)
	private int GeneratePlayerId()
		{
		return players.Count > 0 ? players.Max(p => p.PlayerId) + 1 : 1;
		}
	}
