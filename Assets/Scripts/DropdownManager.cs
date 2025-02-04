// --- Region: Using Directives --- //
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
// --- End Region: Using Directives --- //

// --- Region: Class Definition --- //
public class DropdownManager:MonoBehaviour
	{
	// --- Region: Singleton Setup --- //
	public static DropdownManager Instance;

	private void Awake()
		{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
		}
	// --- End Region: Singleton Setup --- //

	// --- Region: Public Dropdown References --- //
	// --- Header: Home Panel Dropdowns --- //
	[Header("Home Panel Dropdowns")]
	[Tooltip("Dropdown for selecting teams in the Home Panel.")]
	public TMP_Dropdown teamDropdown;

	// --- Header: Team Management Panel Dropdowns --- //
	[Header("Team Management Panel Dropdowns")]
	[Tooltip("Dropdown for selecting teams in Team Management Panel.")]
	public TMP_Dropdown teamManagementDropdown;

	// --- Header: Player Management Panel Dropdowns --- //
	[Header("Player Management Panel Dropdowns")]
	[Tooltip("Dropdown for selecting players in Player Management Panel.")]
	public TMP_Dropdown playerNameDropdown;

	[Tooltip("Dropdown for selecting teams in Player Management Panel.")]
	public TMP_Dropdown playerTeamDropdown;

	// --- Header: Player Lifetime Data Input Panel Dropdowns --- //
	[Header("Player Lifetime Data Input Panel Dropdowns")]
	[Tooltip("Dropdown for selecting teams in Player Lifetime Data Input Panel.")]
	public TMP_Dropdown lifetimeDataTeamDropdown;

	[Tooltip("Dropdown for selecting players in Player Lifetime Data Input Panel.")]
	public TMP_Dropdown lifetimeDataPlayerDropdown;

	// --- Header: Player Current Season Data Input Panel Dropdowns --- //
	[Header("Player Current Season Data Input Panel Dropdowns")]
	[Tooltip("Dropdown for selecting teams in Player Current Season Data Input Panel.")]
	public TMP_Dropdown currentSeasonDataTeamDropdown;

	[Tooltip("Dropdown for selecting players in Player Current Season Data Input Panel.")]
	public TMP_Dropdown currentSeasonDataPlayerDropdown;

	[Tooltip("Dropdown for selecting skill level in Player Current Season Data Input Panel.")]
	public TMP_Dropdown currentSeasonSkillLevelDropdown;

	// --- Header: Matchup Comparison Panel Dropdowns --- //
	[Header("Matchup Comparison Panel Dropdowns")]
	[Tooltip("Dropdown for selecting Team A in Matchup Comparison Panel.")]
	public TMP_Dropdown teamADropdown;

	[Tooltip("Dropdown for selecting Team B in Matchup Comparison Panel.")]
	public TMP_Dropdown teamBDropdown;

	// --- Header: Skill Level Dropdown --- //
	[Header("Skill Level Dropdown")]
	[Tooltip("Dropdown for selecting skill level from 1 to 9.")]
	public TMP_Dropdown skillLevelDropdown;
	// --- End Region: Public Dropdown References --- //

	// --- Region: Unity Methods --- //
	private void Start()
		{
		RefreshAllDropdowns();
		}
	// --- End Region: Unity Methods --- //

	// --- Region: Dropdown Population Methods --- //

	// --- Comment: Refreshes all dropdowns by populating team, player, and skill level data --- //
	public void RefreshAllDropdowns()
		{
		PopulateTeamsDropdown();
		PopulatePlayersDropdown();
		PopulateSkillLevelDropdown();
		}

	// --- Comment: Populates the team dropdowns using data from SQLite --- //
	public void PopulateTeamsDropdown()
		{
		List<string> allTeams = GetAllTeamNamesFromDatabase(); // Fetch team names from DB
		UpdateDropdown(teamDropdown, allTeams);
		UpdateDropdown(teamADropdown, allTeams);
		UpdateDropdown(teamBDropdown, allTeams);
		UpdateDropdown(teamManagementDropdown, allTeams);
		UpdateDropdown(lifetimeDataTeamDropdown, allTeams);
		UpdateDropdown(currentSeasonDataTeamDropdown, allTeams);
		}

	// --- Comment: Updates a specific team dropdown with the latest team names --- //
	public void UpdateTeamDropdown(TMP_Dropdown dropdown, List<Team> teams)
		{
		List<string> allTeams = teams.Select(t => t.name).ToList(); // Extract team names
		UpdateDropdown(dropdown, allTeams); // Update the dropdown
		}

	// --- Comment: Populates the player dropdown using data from SQLite --- //
	public void PopulatePlayersDropdown()
		{
		List<string> allPlayers = GetAllPlayerNamesFromDatabase(); // Fetch player names from DB
		UpdateDropdown(playerNameDropdown, allPlayers);
		UpdateDropdown(lifetimeDataPlayerDropdown, allPlayers);
		UpdateDropdown(currentSeasonDataPlayerDropdown, allPlayers);
		}

	// --- Comment: Populates the skill level dropdown with levels 1 through 9 --- //
	public void PopulateSkillLevelDropdown()
		{
		List<string> skillLevels = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
		UpdateDropdown(skillLevelDropdown, skillLevels);
		UpdateDropdown(currentSeasonSkillLevelDropdown, skillLevels);
		}

	// --- Comment: Helper method that clears and updates the given dropdown with new options --- //
	public void UpdateDropdown(TMP_Dropdown dropdown, List<string> items)
		{
		if (dropdown == null) return;
		dropdown.ClearOptions();
		dropdown.AddOptions(items);
		}

	// --- Comment: Fetches all team names from SQLite database --- //
	private List<string> GetAllTeamNamesFromDatabase()
		{
		List<string> teamNames = new List<string>();
		using (var connection = new SqliteConnection("URI=file:YourDatabaseFilePathHere"))
			{
			connection.Open();
			using (var command = connection.CreateCommand())
				{
				command.CommandText = "SELECT name FROM Teams"; // Assuming the table name is 'Teams' and column is 'name'
				using (var reader = command.ExecuteReader())
					{
					while (reader.Read())
						{
						teamNames.Add(reader.GetString(0)); // Add team name to list
						}
					}
				}
			}
		return teamNames;
		}

	// --- Comment: Fetches all player names from SQLite database --- //
	private List<string> GetAllPlayerNamesFromDatabase()
		{
		List<string> playerNames = new List<string>();
		using (var connection = new SqliteConnection("URI=file:YourDatabaseFilePathHere"))
			{
			connection.Open();
			using (var command = connection.CreateCommand())
				{
				command.CommandText = "SELECT name FROM Players"; // Assuming the table name is 'Players' and column is 'name'
				using (var reader = command.ExecuteReader())
					{
					while (reader.Read())
						{
						playerNames.Add(reader.GetString(0)); // Add player name to list
						}
					}
				}
			}
		return playerNames;
		}
	// --- End Region: Dropdown Population Methods --- //
	}
// --- End Region: Class Definition --- //
