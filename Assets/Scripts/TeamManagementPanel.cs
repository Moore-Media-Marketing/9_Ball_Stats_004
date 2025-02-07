using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// --- Region: TeamManagementPanel --- //
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
			List<Team> teams = DatabaseManager.Instance.LoadTeams(); // Load teams from in-memory storage
			Team existingTeam = teams.FirstOrDefault(t => t.TeamName == teamName); // Changed 'Name' to 'TeamName'

			if (existingTeam != null)
				{
				// Update existing team
				existingTeam.TeamName = teamName; // Changed 'Name' to 'TeamName'
				DatabaseManager.Instance.SaveTeams(teams); // Save updated teams to in-memory list
				Debug.Log("Team updated: " + teamName);
				}
			else
				{
				// Add new team
				int newTeamId = teams.Any() ? teams.Max(t => t.TeamId) + 1 : 1; // Auto-generate team ID
				Team newTeam = new Team(newTeamId, teamName);
				teams.Add(newTeam);
				DatabaseManager.Instance.SaveTeams(teams); // Save new team to in-memory list
				Debug.Log("Team added: " + teamName);
				}

			PopulateTeamDropdown();  // Refresh dropdown after adding/updating
			teamNameInputField.text = string.Empty; // Clear the input field
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
		string teamName = teamNameInputField.text;

		if (!string.IsNullOrEmpty(teamName))
			{
			List<Team> teams = DatabaseManager.Instance.LoadTeams();
			Team teamToDelete = teams.FirstOrDefault(t => t.TeamName == teamName); // Changed 'Name' to 'TeamName'

			if (teamToDelete != null)
				{
				teams.Remove(teamToDelete);
				DatabaseManager.Instance.SaveTeams(teams); // Remove team from list
				Debug.Log("Team deleted: " + teamName);
				}
			else
				{
				Debug.Log("Team not found: " + teamName);
				}

			PopulateTeamDropdown();  // Refresh dropdown after deleting
			teamNameInputField.text = string.Empty; // Clear input field
			}
		else
			{
			Debug.Log("Please enter a valid team name.");
			}
		}
	// --- End Region: Delete Team --- //

	// --- Region: Populate Team Dropdown --- //
	private void PopulateTeamDropdown()
		{
		List<Team> teams = DatabaseManager.Instance.LoadTeams();
		List<string> teamNames = teams.Select(t => t.TeamName).ToList();
		teamDropdown.ClearOptions();
		teamDropdown.AddOptions(teamNames); // Populate dropdown with current team names
		}
	// --- End Region: Populate Team Dropdown --- //

	// --- Region: Back Button --- //
	private void OnBackButton()
		{
		// Implement navigation back to previous screen
		}
	// --- End Region: Back Button --- //
	}
// --- End Region: TeamManagementPanel --- //
