using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamManagementPanel:MonoBehaviour
	{
	// Reference to UI elements
	[Header("Team Management UI Elements")]
	public TMP_InputField teamNameInputField;
	public TMP_Dropdown teamDropdown;
	public TMP_Text feedbackText;

	private List<Team> teamList = new();  // List of Team objects

	// Populate dropdown when the panel is opened
	public void OnEnable()
		{
		UpdateDropdowns();
		}

	// Fetch and update the dropdown with teams from the database
	public void UpdateDropdowns()
		{
		// Clear existing options
		teamDropdown.ClearOptions();

		// Get the list of teams from the database (SQLite)
		teamList = DatabaseManager.Instance.GetAllTeams();

		// Extract just the team names for display
		List<string> teamNames = new();
		foreach (var team in teamList)
			{
			teamNames.Add(team.Name);
			}

		// Add team names to the dropdown
		teamDropdown.AddOptions(teamNames);
		}

	// Method to add a new team
	public void OnAddTeamButtonClicked()
		{
		string teamName = teamNameInputField.text.Trim();

		if (string.IsNullOrEmpty(teamName))
			{
			feedbackText.text = "Team name cannot be empty.";
			return;
			}

		// Create a new team object and add it to the database (SQLite)
		Team newTeam = new(teamName);
		DatabaseManager.Instance.AddTeam(newTeam);

		// Update dropdown and reset input field
		UpdateDropdowns();
		teamNameInputField.text = string.Empty;
		feedbackText.text = "Team added successfully!";
		}

	// Method to remove a team
	public void OnRemoveTeamButtonClicked()
		{
		if (teamDropdown.value < 0 || teamDropdown.value >= teamList.Count)
			{
			feedbackText.text = "Select a team to remove.";
			return;
			}

		// Find the selected team
		string selectedTeamName = teamDropdown.options[teamDropdown.value].text;
		Team teamToRemove = teamList.FirstOrDefault(t => t.Name == selectedTeamName);

		if (teamToRemove != null)
			{
			// Remove the team from the database (SQLite)
			DatabaseManager.Instance.RemoveTeam(teamToRemove);

			// Update dropdown
			UpdateDropdowns();
			feedbackText.text = "Team removed successfully!";
			}
		else
			{
			feedbackText.text = "Team not found.";
			}
		}
	}
