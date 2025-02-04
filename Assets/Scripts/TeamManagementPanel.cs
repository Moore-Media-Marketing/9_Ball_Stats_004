using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	public TMP_Dropdown teamDropdown;
	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button deleteTeamButton;
	public Button backButton;
	// --- End Region: UI References --- //

	// --- Region: CSV References --- //
	private string csvFilePath;
	// --- End Region: CSV References --- //

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		csvFilePath = Path.Combine(Application.persistentDataPath, "teams.csv");

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
			List<Team> teams = ReadTeamsFromCSV();
			Team existingTeam = teams.FirstOrDefault(t => t.Name == teamName);

			if (existingTeam != null)
				{
				// Update existing team
				existingTeam.Name = teamName;
				WriteTeamsToCSV(teams);
				Debug.Log("Team updated: " + teamName);
				}
			else
				{
				// Add new team
				Team newTeam = new() { Name = teamName };
				teams.Add(newTeam);
				WriteTeamsToCSV(teams);
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
		List<Team> teams = ReadTeamsFromCSV();
		Team teamToDelete = teams.FirstOrDefault(t => t.Name == teamName);

		if (teamToDelete != null)
			{
			teams.Remove(teamToDelete);  // Remove team from list
			WriteTeamsToCSV(teams);  // Write updated list to CSV
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
		List<Team> teams = ReadTeamsFromCSV();
		teamDropdown.ClearOptions();  // Clear current dropdown options

		// Add team names to dropdown
		List<string> teamNames = teams.Select(t => t.Name).ToList();
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

	// --- Region: CSV Helper Methods --- //
	private List<Team> ReadTeamsFromCSV()
		{
		List<Team> teams = new();

		if (File.Exists(csvFilePath))
			{
			var lines = File.ReadAllLines(csvFilePath);

			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length > 1)  // Assuming each line has a team name
					{
					teams.Add(new Team { Name = columns[0] });
					}
				}
			}

		return teams;
		}

	private void WriteTeamsToCSV(List<Team> teams)
		{
		List<string> lines = new();

		foreach (var team in teams)
			{
			lines.Add(team.Name);
			}

		File.WriteAllLines(csvFilePath, lines);
		}
	// --- End Region: CSV Helper Methods --- //

	// --- Region: Team Class --- //
	public class Team
		{
		public string Name { get; set; }
		}
	// --- End Region: Team Class --- //
	}
