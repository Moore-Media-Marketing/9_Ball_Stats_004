using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SQLite;
using System.Collections.Generic;
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

	// --- Region: SQLite References --- //
	private SQLiteConnection db;
	private string dbPath;
	// --- End Region: SQLite References --- //

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		dbPath = System.IO.Path.Combine(Application.persistentDataPath, "sampleData.db");
		db = new SQLiteConnection(dbPath);
		db.CreateTable<Team>(); // Ensure the Team table exists

		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeam);
		deleteTeamButton.onClick.AddListener(OnDeleteTeam);
		backButton.onClick.AddListener(OnBackButton);

		PopulateTeamDropdown();
		}
	// --- End Region: Initialize Panel --- //

	// --- Region: Add or Update Team --- //
	private void OnAddUpdateTeam()
		{
		string teamName = teamNameInputField.text;
		if (!string.IsNullOrEmpty(teamName))
			{
			Team existingTeam = db.Table<Team>().FirstOrDefault(t => t.Name == teamName);
			if (existingTeam != null)
				{
				// Update existing team
				existingTeam.Name = teamName;
				db.Update(existingTeam);
				Debug.Log("Team updated: " + teamName);
				}
			else
				{
				// Add new team
				Team newTeam = new Team { Name = teamName };
				db.Insert(newTeam);
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
		Team teamToDelete = db.Table<Team>().FirstOrDefault(t => t.Name == teamName);
		if (teamToDelete != null)
			{
			db.Delete(teamToDelete);  // Remove team from database
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
		List<Team> teams = db.Table<Team>().ToList();  // Get teams from the database
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

	// --- Region: Team Class --- //
	public class Team
		{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		}
	// --- End Region: Team Class --- //
	}
