using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SQLite;
using System.Collections.Generic;
using System.Linq;

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
	// --- End Region: Panel References --- //

	// --- Region: SQLite References --- //
	private SQLiteConnection db;
	private string dbPath;
	// --- End Region: SQLite References --- //

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		dbPath = System.IO.Path.Combine(Application.persistentDataPath, "sampleData.db");
		db = new SQLiteConnection(dbPath);
		db.CreateTable<Player>(); // Ensure the Player table exists
		db.CreateTable<Team>(); // Ensure the Team table exists

		addPlayerButton.onClick.AddListener(OnAddPlayer);
		deletePlayerButton.onClick.AddListener(OnDeletePlayer);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetails);
		backButton.onClick.AddListener(OnBackButton);

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
			Player newPlayer = new Player
				{
				Name = playerName,
				TeamId = teamId
				};
			db.Insert(newPlayer);
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
		Player playerToDelete = db.Table<Player>().FirstOrDefault(p => p.Name == playerName);
		if (playerToDelete != null)
			{
			db.Delete(playerToDelete); // Remove player from database
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
		List<Team> teams = db.Table<Team>().ToList();  // Get teams from the database
		teamNameDropdown.ClearOptions();  // Clear current dropdown options

		// Add team names to dropdown
		List<string> teamNames = teams.Select(t => t.Name).ToList();
		teamNameDropdown.AddOptions(teamNames);
		}
	// --- End Region: Populate Team Dropdown --- //

	// --- Region: Populate Player Dropdown --- //
	private void PopulatePlayerDropdown()
		{
		List<Player> players = db.Table<Player>().ToList();  // Get players from the database
		playerNameDropdown.ClearOptions();  // Clear current dropdown options

		// Add player names to dropdown
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

	// --- Region: Player Class --- //
	public class Player
		{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public int TeamId { get; set; }  // Foreign key to the team
		}
	// --- End Region: Player Class --- //

	// --- Region: Team Class --- //
	public class Team
		{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		}
	// --- End Region: Team Class --- //
	}
