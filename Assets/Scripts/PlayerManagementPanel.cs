using UnityEngine;
using UnityEngine.UI;
using TMPro;  // For using TMP components
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class PlayerManagementPanel:MonoBehaviour
	{
	// --- Region: Panel References --- //
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button backButton;
	public Button saveToCSVButton;  // New button for saving to CSV
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
		teamsCsvPath = System.IO.Path.Combine(Application.persistentDataPath, "teams.csv");
		playersCsvPath = System.IO.Path.Combine(Application.persistentDataPath, "players.csv");

		teams = LoadTeamsFromCSV();
		players = LoadPlayersFromCSV();

		addPlayerButton.onClick.AddListener(OnAddPlayer);
		deletePlayerButton.onClick.AddListener(OnDeletePlayer);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetails);
		backButton.onClick.AddListener(OnBackButton);
		saveToCSVButton.onClick.AddListener(OnSaveToCSV); // Link save to CSV functionality

		PopulateTeamDropdown();
		PopulatePlayerDropdown();
		}
	// --- End Region: Initialize Panel --- //

	// --- Region: Add Player --- //
	private void OnAddPlayer()
		{
		string playerName = playerNameInputField.text;
		int teamId = teamNameDropdown.value + 1; // Get selected team's ID (simple mapping)

		if (!string.IsNullOrEmpty(playerName))
			{
			Player newPlayer = new()
				{
				Name = playerName,
				TeamId = teamId
				};
			players.Add(newPlayer); // Add player to the list
			Debug.Log($"Player {playerName} added to Team {teamNameDropdown.options[teamNameDropdown.value].text}");
			PopulatePlayerDropdown(); // Refresh player dropdown
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
		if (playerToDelete != null)
			{
			players.Remove(playerToDelete); // Remove player from list
			Debug.Log($"Player {playerName} deleted.");
			PopulatePlayerDropdown(); // Refresh player dropdown
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
		// Logic to show player details input panel (expand as needed)
		Debug.Log($"Adding details for player: {playerName}");
		}
	// --- End Region: Add Player Details --- //

	// --- Region: Populate Team Dropdown --- //
	private void PopulateTeamDropdown()
		{
		teamNameDropdown.ClearOptions();  // Clear current dropdown options
		List<string> teamNames = teams.Select(t => t.Name).ToList();
		teamNameDropdown.AddOptions(teamNames);
		}
	// --- End Region: Populate Team Dropdown --- //

	// --- Region: Populate Player Dropdown --- //
	private void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();  // Clear current dropdown options
		List<string> playerNames = players.Select(p => p.Name).ToList();
		playerNameDropdown.AddOptions(playerNames);
		}
	// --- End Region: Populate Player Dropdown --- //

	// --- Region: Back Button --- //
	private void OnBackButton()
		{
		// Show the home panel or previous panel
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
				if (columns.Length == 2) // Assuming 2 columns: Id, Name
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
				if (columns.Length == 3) // Assuming 3 columns: Id, Name, TeamId
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
	public class Player
		{
		public int Id { get; set; }
		public string Name { get; set; }
		public int TeamId { get; set; }  // Foreign key to the team
		}
	// --- End Region: Player Class --- //

	// --- Region: Team Class --- //
	public class Team
		{
		public int Id { get; set; }
		public string Name { get; set; }
		}
	// --- End Region: Team Class --- //
	}
