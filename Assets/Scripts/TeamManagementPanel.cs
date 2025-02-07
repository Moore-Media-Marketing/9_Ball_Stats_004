using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

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
			List<Team> teams = LoadTeamsFromCsv(); // Load teams from CSV
			Team existingTeam = teams.FirstOrDefault(t => t.TeamName == teamName); // Changed 'Name' to 'TeamName'

			if (existingTeam != null)
				{
				// Update existing team
				existingTeam.TeamName = teamName; // Changed 'Name' to 'TeamName'
				SaveTeamsToCsv(teams); // Save updated teams to CSV
				Debug.Log("Team updated: " + teamName);
				}
			else
				{
				// Add new team
				int newTeamId = teams.Any() ? teams.Max(t => t.TeamId) + 1 : 1; // Auto-generate team ID
				Team newTeam = new(newTeamId, teamName);
				teams.Add(newTeam);
				SaveTeamsToCsv(teams); // Save new team to CSV
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
			List<Team> teams = LoadTeamsFromCsv(); // Load teams from CSV
			Team teamToDelete = teams.FirstOrDefault(t => t.TeamName == teamName); // Changed 'Name' to 'TeamName'

			if (teamToDelete != null)
				{
				teams.Remove(teamToDelete);
				SaveTeamsToCsv(teams); // Remove team from CSV
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
		List<Team> teams = LoadTeamsFromCsv(); // Load teams from CSV
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

	// --- Region: Load Teams from CSV --- //
	private List<Team> LoadTeamsFromCsv()
		{
		// Replace with your method to load teams from the CSV
		return CsvHelper.LoadTeamsFromCsv();
		}

	// --- Region: Save Teams to CSV --- //
	private void SaveTeamsToCsv(List<Team> teams)
		{
		// Replace with your method to save teams to the CSV
		CsvHelper.SaveTeamsToCsv(teams);
		}
	// --- End Region: Save Teams to CSV --- //
	}
// --- End Region: TeamManagementPanel --- //
