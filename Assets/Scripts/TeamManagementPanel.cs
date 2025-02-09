using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	[Header("UI Elements")]
	[Tooltip("Text displaying the header of the Team Management panel.")]
	public TMP_Text headerText;

	[Tooltip("Text displaying the name of the team.")]
	public TMP_Text teamNameText;

	[Tooltip("Input field for adding or modifying a team name.")]
	public TMP_InputField teamNameInputField;

	[Tooltip("Button to add or update the team.")]
	public Button addUpdateTeamButton;

	[Tooltip("Button to clear the team name input field.")]
	public Button clearTeamNameButton;

	[Tooltip("Dropdown for selecting a team.")]
	public TMP_Dropdown teamDropdown;

	[Tooltip("Button to modify the selected team's name.")]
	public Button modifyTeamNameButton;

	[Tooltip("Button to delete the selected team.")]
	public Button deleteButton;

	[Tooltip("Button to navigate back.")]
	public Button backButton;

	[Tooltip("Text displaying the back button.")]
	public TMP_Text backButtonText;

	// --- Data Management --- //
	private List<Team> teams = new();

	private int selectedTeamId = -1;

	// --- Initialization --- //
	private void Start()
		{
		// Populate the team dropdown with available teams
		PopulateTeamDropdown();

		// Set up button listeners for various interactions
		SetupButtonListeners();
		}

	// --- Region: Button Setup --- //
	private void SetupButtonListeners()
		{
		// Add or Update team button
		if (addUpdateTeamButton != null)
			addUpdateTeamButton.onClick.AddListener(AddOrUpdateTeam);

		// Clear team name button
		if (clearTeamNameButton != null)
			clearTeamNameButton.onClick.AddListener(ClearTeamName);

		// Modify selected team button
		if (modifyTeamNameButton != null)
			modifyTeamNameButton.onClick.AddListener(ModifySelectedTeam);

		// Delete selected team button
		if (deleteButton != null)
			deleteButton.onClick.AddListener(DeleteSelectedTeam);

		// Back button
		if (backButton != null)
			backButton.onClick.AddListener(HandleBackButton);

		// Team dropdown selection change
		if (teamDropdown != null)
			teamDropdown.onValueChanged.AddListener(OnTeamSelected);
		}

	// --- Region: Dropdown Population --- //
	private void PopulateTeamDropdown()
		{
		if (teamDropdown == null)
			{
			Debug.LogError("TeamDropdown reference is missing!");
			return;
			}

		// Fetch the teams from the manager
		var manager = PlayersAndTeamsManager.Instance;
		if (manager == null)
			{
			Debug.LogError("PlayersAndTeamsManager instance is missing!");
			return;
			}

		teams = manager.GetAllTeams();
		teamDropdown.ClearOptions();

		// Handle case where there are no teams
		if (teams.Count == 0)
			{
			Debug.LogWarning("No teams found! Clearing dropdown.");
			return;
			}

		// Add teams to the dropdown
		List<string> teamNames = new();
		foreach (var team in teams)
			{
			teamNames.Add(team.TeamName);
			}

		teamDropdown.AddOptions(teamNames);
		teamDropdown.RefreshShownValue();
		}

	// --- Region: Team Actions --- //
	private void AddOrUpdateTeam()
		{
		var manager = PlayersAndTeamsManager.Instance;
		if (manager == null)
			{
			Debug.LogError("PlayersAndTeamsManager instance is missing!");
			return;
			}

		string newTeamName = teamNameInputField.text.Trim();
		if (string.IsNullOrEmpty(newTeamName))
			{
			Debug.LogWarning("Team name cannot be empty.");
			return;
			}

		if (selectedTeamId == -1)
			{
			manager.AddTeam(newTeamName); // Add new team
			}
		else
			{
			manager.ModifyTeam(selectedTeamId, newTeamName); // Modify existing team
			}

		// Refresh dropdown and clear input field
		PopulateTeamDropdown();
		ClearTeamName();
		}

	private void ModifySelectedTeam()
		{
		if (selectedTeamId == -1)
			{
			Debug.LogWarning("No team selected for modification.");
			return;
			}

		string newTeamName = teamNameInputField.text.Trim();
		if (string.IsNullOrEmpty(newTeamName))
			{
			Debug.LogWarning("Team name cannot be empty.");
			return;
			}

		var manager = PlayersAndTeamsManager.Instance;
		if (manager == null)
			{
			Debug.LogError("PlayersAndTeamsManager instance is missing!");
			return;
			}

		manager.ModifyTeam(selectedTeamId, newTeamName); // Modify selected team
		PopulateTeamDropdown();
		ClearTeamName();
		}

	private void DeleteSelectedTeam()
		{
		if (selectedTeamId == -1)
			{
			Debug.LogWarning("No team selected for deletion.");
			return;
			}

		var manager = PlayersAndTeamsManager.Instance;
		if (manager == null)
			{
			Debug.LogError("PlayersAndTeamsManager instance is missing!");
			return;
			}

		manager.DeleteTeam(selectedTeamId); // Delete selected team
		PopulateTeamDropdown();
		ClearTeamName();
		}

	// --- Region: Navigation --- //
	private void HandleBackButton()
		{
		Debug.Log("Back button pressed.");
		gameObject.SetActive(false); // Hides the panel
		}

	// --- Region: Dropdown Selection --- //
	private void OnTeamSelected(int index)
		{
		if (index < 0 || index >= teams.Count)
			{
			Debug.LogWarning("Invalid team selection.");
			return;
			}

		selectedTeamId = teams[index].TeamId;
		teamNameInputField.text = teams[index].TeamName; // Prepopulate the team name input field
		}

	// --- Region: Utility Functions --- //
	private void ClearTeamName()
		{
		teamNameInputField.text = "";
		selectedTeamId = -1;
		}
	}