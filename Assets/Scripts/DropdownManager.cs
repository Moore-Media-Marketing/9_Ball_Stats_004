// --- Region: Using Directives --- //
using System.Collections.Generic;

using TMPro;

using UnityEngine;

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
	[Header("Team Dropdowns")]
	[Tooltip("Dropdown for all teams.")]
	public TMP_Dropdown teamDropdown;

	[Tooltip("Dropdown for Team A.")]
	public TMP_Dropdown teamADropdown;

	[Tooltip("Dropdown for Team B.")]
	public TMP_Dropdown teamBDropdown;

	[Header("Player Dropdowns")]
	[Tooltip("Dropdown for player names.")]
	public TMP_Dropdown playerNameDropdown;

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

	// --- Comment: Populates the team dropdowns using data from DatabaseManager --- //
	public void PopulateTeamsDropdown()
		{
		List<string> allTeams = DatabaseManager.Instance.GetAllTeamNames(); // Fetch team names from DB
		UpdateDropdown(teamDropdown, allTeams);
		UpdateDropdown(teamADropdown, allTeams);
		UpdateDropdown(teamBDropdown, allTeams);
		}

	// --- Comment: Updates a specific team dropdown with the latest team names --- //
	public void UpdateTeamDropdown(TMP_Dropdown dropdown)
		{
		List<string> allTeams = DatabaseManager.Instance.GetAllTeamNames();
		UpdateDropdown(dropdown, allTeams);
		}

	// --- Comment: Populates the player dropdown using data from DatabaseManager --- //
	public void PopulatePlayersDropdown()
		{
		List<string> allPlayers = DatabaseManager.Instance.GetAllPlayerNames(); // Fetch player names from DB
		UpdateDropdown(playerNameDropdown, allPlayers);
		}

	// --- Comment: Populates the skill level dropdown with levels 1 through 9 --- //
	public void PopulateSkillLevelDropdown()
		{
		List<string> skillLevels = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
		UpdateDropdown(skillLevelDropdown, skillLevels);
		}

	// --- Comment: Helper method that clears and updates the given dropdown with new options --- //
	public void UpdateDropdown(TMP_Dropdown dropdown, List<string> items)
		{
		if (dropdown == null) return;
		dropdown.ClearOptions();
		dropdown.AddOptions(items);
		}

	// --- End Region: Dropdown Population Methods --- //
	}

// --- End Region: Class Definition --- //