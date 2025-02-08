using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class TeamManagementPanel:MonoBehaviour
	{
	private DatabaseManager databaseManager;

	// UI references
	public TMP_Text headerText;
	public TMP_Text teamNameText;
	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button clearTeamNameButton;
	public TMP_Dropdown teamDropdown;
	public Button modifyTeamNameButton;
	public Button deleteButton;
	public Button backButton;

	// Temporary variables for managing UI input
	private Team currentSelectedTeam;

	private void Start()
		{
		// Initialize DatabaseManager
		databaseManager = DatabaseManager.Instance;

		// Setup Button Listeners
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeamButtonClick);
		clearTeamNameButton.onClick.AddListener(OnClearTeamNameButtonClick);
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamNameButtonClick);
		deleteButton.onClick.AddListener(OnDeleteButtonClick);
		backButton.onClick.AddListener(OnBackButtonClick);

		// Setup Dropdown Listener
		teamDropdown.onValueChanged.AddListener(OnTeamDropdownChanged);

		// Initialize the dropdown with existing teams
		LoadTeamsIntoDropdown();
		}

	#region UI Management
	// Add or Update a Team
	public void OnAddUpdateTeamButtonClick()
		{
		string teamName = teamNameInputField.text;

		if (string.IsNullOrEmpty(teamName))
			{
			Debug.LogWarning("Team name cannot be empty.");
			return;
			}

		// Check if team already exists
		List<Team> existingTeams = databaseManager.LoadTeams();
		if (existingTeams.Any(t => t.TeamName == teamName))
			{
			Debug.LogWarning("A team with this name already exists.");
			return;
			}

		// Add new or update existing team
		if (currentSelectedTeam == null)
			{
			// Add new team
			Team newTeam = new(0, teamName);  // Assuming 0 for TeamId (to be updated after saving)
			databaseManager.SaveTeams(new List<Team> { newTeam });
			Debug.Log($"Team added: {teamName}");
			}
		else
			{
			// Update existing team
			currentSelectedTeam.UpdateTeamName(teamName);
			databaseManager.SaveTeams(new List<Team> { currentSelectedTeam });
			Debug.Log($"Team updated: {teamName}");
			}

		LoadTeamsIntoDropdown(); // Refresh dropdown list after adding/updating
		}

	// Clear the team name input field
	public void OnClearTeamNameButtonClick()
		{
		teamNameInputField.text = string.Empty;
		currentSelectedTeam = null;
		Debug.Log("Cleared team name input.");
		}

	// Modify the selected team name
	public void OnModifyTeamNameButtonClick()
		{
		if (currentSelectedTeam != null)
			{
			teamNameInputField.text = currentSelectedTeam.TeamName;
			}
		else
			{
			Debug.LogWarning("No team selected for modification.");
			}
		}

	// Delete the selected team
	public void OnDeleteButtonClick()
		{
		if (currentSelectedTeam != null)
			{
			databaseManager.DeleteTeam(currentSelectedTeam); // Assuming DeleteTeam method exists in DatabaseManager
			Debug.Log($"Team deleted: {currentSelectedTeam.TeamName}");
			LoadTeamsIntoDropdown(); // Refresh dropdown list after deleting
			OnClearTeamNameButtonClick(); // Clear the input field after deletion
			}
		else
			{
			Debug.LogWarning("No team selected for deletion.");
			}
		}

	// Update the back button text and handle back action
	public void OnBackButtonClick()
		{
		Debug.Log("Back button clicked.");
		// Logic to navigate back to previous screen or menu
		// If it's a scene change, use the following:
		// SceneManager.LoadScene("PreviousSceneName");

		// If it's navigating between UI panels:
		gameObject.SetActive(false);  // Hide current panel
									  // Optionally, show the previous panel (if needed)
		}
	#endregion

	#region Dropdown & Team Management
	// Load teams into the dropdown menu
	private void LoadTeamsIntoDropdown()
		{
		teamDropdown.ClearOptions();
		List<Team> teams = databaseManager.LoadTeams();

		// Add the teams to the dropdown
		List<string> teamNames = new();
		foreach (Team team in teams)
			{
			teamNames.Add(team.TeamName);  // Referencing the property correctly
			}

		teamDropdown.AddOptions(teamNames);
		}

	// Handle dropdown selection change
	private void OnTeamDropdownChanged(int index)
		{
		List<Team> teams = databaseManager.LoadTeams();

		if (index >= 0 && index < teams.Count)
			{
			currentSelectedTeam = teams[index];
			teamNameInputField.text = currentSelectedTeam.TeamName;
			Debug.Log($"Selected team: {currentSelectedTeam.TeamName}");
			}
		else
			{
			currentSelectedTeam = null;
			}
		}
	#endregion
	}
