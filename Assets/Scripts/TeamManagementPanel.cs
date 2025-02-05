using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	public TMP_Dropdown teamDropdown;

	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button deleteTeamButton;
	public Button backButton;
	// --- End Region: UI References --- //

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		// Add listeners for button clicks
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeam);
		deleteTeamButton.onClick.AddListener(OnDeleteTeam);
		backButton.onClick.AddListener(OnBackButton);

		PopulateTeamDropdown(); // Populate the dropdown with existing teams
		}

	// --- End Region: Initialize Panel --- //

	// --- Region: Add or Update Team --- //
	private void OnAddUpdateTeam()
		{
		string teamName = teamNameInputField.text;

		if (!string.IsNullOrEmpty(teamName))
			{
			List<Team> teams = DatabaseManager.Instance.LoadTeams(); // Load teams from database
			Team existingTeam = teams.FirstOrDefault(t => t.TeamName == teamName); // Changed 'Name' to 'TeamName'

			if (existingTeam != null)
				{
				// Update existing team
				existingTeam.TeamName = teamName; // Changed 'Name' to 'TeamName'
				DatabaseManager.Instance.SaveTeams(teams); // Save updated teams to database
				Debug.Log("Team updated: " + teamName);
				}
			else
				{
				// Add new team
				int newTeamId = teams.Any() ? teams.Max(t => t.TeamId) + 1 : 1; // Auto-generate team ID
				Team newTeam = new(newTeamId, teamName);
				teams.Add(newTeam);
				DatabaseManager.Instance.SaveTeams(teams); // Save new team to database
				Debug.Log("Team added: " + teamName);
				}

			PopulateTeamDropdown();  // Refresh dropdown after adding/updating
			}
		else
			{
			Debug.Log("Please enter a valid team name.");
			}
		}

	// --- End Region: Add or Update Team --- //

	// --- Region: Delete Team --- //
	private void OnDeleteTeam()
		{
		string teamName = teamDropdown.options[teamDropdown.value].text;
		List<Team> teams = DatabaseManager.Instance.LoadTeams(); // Load teams from database
		Team teamToDelete = teams.FirstOrDefault(t => t.TeamName == teamName); // Changed 'Name' to 'TeamName'

		if (teamToDelete != null)
			{
			teams.Remove(teamToDelete);  // Remove team from list
			DatabaseManager.Instance.SaveTeams(teams);  // Save updated list to database
			Debug.Log("Team deleted: " + teamName);
			PopulateTeamDropdown();  // Refresh dropdown after deletion
			}
		else
			{
			Debug.Log("Team not found.");
			}
		}

	// --- End Region: Delete Team --- //

	// --- Region: Populate Team Dropdown --- //
	private void PopulateTeamDropdown()
		{
		List<Team> teams = DatabaseManager.Instance.LoadTeams(); // Load teams from database
		teamDropdown.ClearOptions();  // Clear current dropdown options

		// Add team names to dropdown
		List<string> teamNames = teams.Select(t => t.TeamName).ToList(); // Changed 'Name' to 'TeamName'
		teamDropdown.AddOptions(teamNames);
		}

	// --- End Region: Populate Team Dropdown --- //

	// --- Region: Back Button --- //
	private void OnBackButton()
		{
		// Show the home panel or previous panel
		Debug.Log("Back button clicked.");
		}

	// --- End Region: Back Button --- //
	}