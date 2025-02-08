using System.Collections.Generic;
using System.IO;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementPanel:MonoBehaviour
	{
	// --- Region: Panel References --- //
	[Header("Dropdowns")]
	[Tooltip("Dropdown to select a team.")]
	public TMP_Dropdown teamNameDropdown;

	[Tooltip("Dropdown to select a player.")]
	public TMP_Dropdown playerNameDropdown;

	[Header("Player Input")]
	[Tooltip("Input field for new player name.")]
	public TMP_InputField playerNameInputField;

	[Header("Buttons")]
	[Tooltip("Button to add a new player.")]
	public Button addPlayerButton;

	[Tooltip("Button to delete a selected player.")]
	public Button deletePlayerButton;

	[Tooltip("Button to add player details.")]
	public Button addPlayerDetailsButton;

	[Tooltip("Button to go back to the previous menu.")]
	public Button backButton;

	[Tooltip("Button to save data to CSV.")]
	public Button saveToCSVButton;
	// --- End Region: Panel References --- //

	// --- Region: CSV Data References --- //
	private string teamsCsvPath;
	private string playersCsvPath;
	private List<Team> teams;
	private List<Player> players;
	// --- End Region: CSV Data References --- //

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		// Define file paths for the CSVs
		teamsCsvPath = Path.Combine(Application.persistentDataPath, "teams.csv");
		playersCsvPath = Path.Combine(Application.persistentDataPath, "players.csv");

		teams = LoadTeamsFromCSV();
		players = LoadPlayersFromCSV();

		// Register button click listeners
		addPlayerButton.onClick.AddListener(OnAddPlayer);
		deletePlayerButton.onClick.AddListener(OnDeletePlayer);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetails);
		backButton.onClick.AddListener(OnBackButton);
		saveToCSVButton.onClick.AddListener(OnSaveToCSV);

		// Populate the dropdowns
		PopulateTeamDropdown();
		PopulatePlayerDropdown();
		}
	// --- End Region: Initialize Panel --- //

	// --- Region: Add Player --- //
	private void OnAddPlayer()
		{
		string playerName = playerNameInputField.text;
		int teamId = teamNameDropdown.value + 1; // Team ID is 1-indexed

		// Check if the player name is valid
		if (!string.IsNullOrEmpty(playerName))
			{
			Player newPlayer = new()
				{
				Name = playerName,
				TeamId = teamId
				};
			players.Add(newPlayer);
			Debug.Log($"Player {playerName} added to Team {teamNameDropdown.options[teamNameDropdown.value].text}");
			PopulatePlayerDropdown();
			}
		else
			{
			Debug.Log("Please enter a valid player name.");
			}
		}
	// --- End Region: Add Player --- //

	// --- Region: Delete Player --- //
	private void OnDeletePlayer()
		{
		string playerName = playerNameDropdown.options[playerNameDropdown.value].text;
		Player playerToDelete = players.FirstOrDefault(p => p.Name == playerName);

		// Delete the player if found
		if (playerToDelete != null)
			{
			players.Remove(playerToDelete);
			Debug.Log($"Player {playerName} deleted.");
			PopulatePlayerDropdown();
			}
		else
			{
			Debug.Log("Player not found.");
			}
		}
	// --- End Region: Delete Player --- //

	// --- Region: Add Player Details --- //
	private void OnAddPlayerDetails()
		{
		string playerName = playerNameDropdown.options[playerNameDropdown.value].text;
		Debug.Log($"Adding details for player: {playerName}");
		}
	// --- End Region: Add Player Details --- //

	// --- Region: Populate Team Dropdown --- //
	private void PopulateTeamDropdown()
		{
		teamNameDropdown.ClearOptions();
		List<string> teamNames = teams.Select(t => t.Name).ToList();
		teamNameDropdown.AddOptions(teamNames);
		}
	// --- End Region: Populate Team Dropdown --- //

	// --- Region: Populate Player Dropdown --- //
	private void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		List<string> playerNames = players.Select(p => p.Name).ToList();
		playerNameDropdown.AddOptions(playerNames);
		}
	// --- End Region: Populate Player Dropdown --- //

	// --- Region: Back Button --- //
	private void OnBackButton()
		{
		// Log back button action (could navigate to the home panel)
		Debug.Log("Back button clicked.");
		}
	// --- End Region: Back Button --- //

	// --- Region: Save Data to CSV --- //
	private void OnSaveToCSV()
		{
		SaveTeamsToCSV(teams);
		SavePlayersToCSV(players);
		Debug.Log("Data saved to CSV files.");
		}
	// --- End Region: Save Data to CSV --- //

	// --- Region: Load Teams from CSV --- //
	private List<Team> LoadTeamsFromCSV()
		{
		List<Team> loadedTeams = new();
		if (File.Exists(teamsCsvPath))
			{
			string[] lines = File.ReadAllLines(teamsCsvPath);
			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length == 2) // Assuming two columns: Id, Name
					{
					loadedTeams.Add(new Team
						{
						Id = int.Parse(columns[0]),
						Name = columns[1]
						});
					}
				}
			}
		return loadedTeams;
		}
	// --- End Region: Load Teams from CSV --- //

	// --- Region: Load Players from CSV --- //
	private List<Player> LoadPlayersFromCSV()
		{
		List<Player> loadedPlayers = new();
		if (File.Exists(playersCsvPath))
			{
			string[] lines = File.ReadAllLines(playersCsvPath);
			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length == 3) // Assuming three columns: Id, Name, TeamId
					{
					loadedPlayers.Add(new Player
						{
						Id = int.Parse(columns[0]),
						Name = columns[1],
						TeamId = int.Parse(columns[2])
						});
					}
				}
			}
		return loadedPlayers;
		}
	// --- End Region: Load Players from CSV --- //

	// --- Region: Save Teams to CSV --- //
	private void SaveTeamsToCSV(List<Team> teams)
		{
		List<string> lines = new();
		foreach (var team in teams)
			{
			lines.Add($"{team.Id},{team.Name}");
			}
		File.WriteAllLines(teamsCsvPath, lines);
		}
	// --- End Region: Save Teams to CSV --- //

	// --- Region: Save Players to CSV --- //
	private void SavePlayersToCSV(List<Player> players)
		{
		List<string> lines = new();
		foreach (var player in players)
			{
			lines.Add($"{player.Id},{player.Name},{player.TeamId}");
			}
		File.WriteAllLines(playersCsvPath, lines);
		}
	// --- End Region: Save Players to CSV --- //

	// --- Region: Player Class --- //
	[System.Serializable]
	public class Player
		{
		public int Id { get; set; }
		public string Name { get; set; }
		public int TeamId { get; set; }  // Foreign key to the team
		}
	// --- End Region: Player Class --- //

	// --- Region: Team Class --- //
	[System.Serializable]
	public class Team
		{
		public int Id { get; set; }
		public string Name { get; set; }
		}
	// --- End Region: Team Class --- //
	}
