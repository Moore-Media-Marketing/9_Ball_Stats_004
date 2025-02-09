using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

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

	public List<Player> players;

	public void Start()
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
	public void PopulateTeamDropdown()
		{
		List<Team> teams = DatabaseManager.Instance.GetAllTeams();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.TeamName).ToList());
		}

	// Populate player dropdown
	public void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		players = DatabaseManager.Instance.GetAllPlayers();
		playerNameDropdown.AddOptions(players.Select(p => p.PlayerName).ToList());
		}

	// Add player to database
	public void AddPlayer()
		{
		string playerName = playerNameInputField.text;
		string teamName = teamNameDropdown.options[teamNameDropdown.value].text;
		int skillLevel = 10;  // Example skill level (could be added via input fields as well)

		if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrEmpty(teamName))
			{
			// Get teamId based on the selected team name
			int teamId = DatabaseManager.Instance.GetAllTeams()
				.FirstOrDefault(t => t.TeamName == teamName)?.TeamId ?? -1;

			if (teamId == -1)
				{
				Debug.LogWarning($"Team '{teamName}' not found.");
				return;
				}

			DatabaseManager.Instance.AddPlayer(playerName, teamId, skillLevel);
			PopulatePlayerDropdown();  // Update player dropdown after adding a new player
			}
		else
			{
			Debug.LogWarning("Please fill in both player name and team.");
			}
		}

	// Delete player from database
	public void DeletePlayer()
		{
		string playerName = playerNameDropdown.options[playerNameDropdown.value].text;
		if (!string.IsNullOrEmpty(playerName))
			{
			// Get playerId based on the selected player name
			int playerId = players.FirstOrDefault(p => p.PlayerName == playerName)?.PlayerId ?? -1;

			if (playerId == -1)
				{
				Debug.LogWarning($"Player '{playerName}' not found.");
				return;
				}

			DatabaseManager.Instance.DeletePlayer(playerId);
			PopulatePlayerDropdown();  // Update player dropdown after deleting a player
			}
		else
			{
			Debug.LogWarning("No player selected to delete.");
			}
		}

	// Add or update player details (stats)
	public void AddPlayerDetails()
		{
		// Ensure we have a player selected
		if (playerNameDropdown.value < 0 || playerNameDropdown.value >= players.Count)
			{
			Debug.LogWarning("No player selected to update details.");
			return;
			}

		// Fetch the player to update
		Player selectedPlayer = players[playerNameDropdown.value];

		// Log the update
		Debug.Log($"Updated player {selectedPlayer.PlayerName} with new stats.");

		// Save updated player stats back to DatabaseManager
		DatabaseManager.Instance.UpdatePlayerStats(selectedPlayer.PlayerId, selectedPlayer.Stats);

		// Optionally refresh the player dropdown if needed
		PopulatePlayerDropdown();
		}

	// Save to CSV
	public void SaveToCSV()
		{
		// Manually trigger saving of player data to CSV
		Debug.Log("Saving player data to CSV...");

		// If needed, you can call the specific save function from the DatabaseManager
		CSVManager.SavePlayersToCSV("Assets/PlayerData.csv", players);  // Save players data manually to CSV

		// Optionally, log confirmation message
		Debug.Log("Player data saved successfully.");
		}
	}
