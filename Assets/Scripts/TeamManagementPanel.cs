using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages UI interactions for adding, modifying, and deleting teams.
/// </summary>
public class TeamManagementPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TMP_Text headerText;
	public TMP_Text teamNameText;
	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button clearTeamNameButton;
	public TMP_Dropdown teamDropdown;
	public Button modifyTeamNameButton;
	public Button deleteButton;
	public Button backButton;
	public TMP_Text backButtonText;

	private List<Team> teams = new();
	private int selectedTeamId = -1;

	private void Start()
		{
		PopulateTeamDropdown();
		SetupButtonListeners();
		}

	/// <summary>
	/// Sets up button listeners for UI interactions.
	/// </summary>
	private void SetupButtonListeners()
		{
		if (addUpdateTeamButton != null)
			addUpdateTeamButton.onClick.AddListener(AddOrUpdateTeam);

		if (clearTeamNameButton != null)
			clearTeamNameButton.onClick.AddListener(ClearTeamName);

		if (modifyTeamNameButton != null)
			modifyTeamNameButton.onClick.AddListener(ModifySelectedTeam);

		if (deleteButton != null)
			deleteButton.onClick.AddListener(DeleteSelectedTeam);

		if (backButton != null)
			backButton.onClick.AddListener(HandleBackButton);

		if (teamDropdown != null)
			teamDropdown.onValueChanged.AddListener(OnTeamSelected);
		}

	/// <summary>
	/// Populates the team dropdown with available teams.
	/// </summary>
	private void PopulateTeamDropdown()
		{
		if (teamDropdown == null)
			{
			Debug.LogError("TeamDropdown reference is missing!");
			return;
			}

		var manager = PlayersAndTeamsManager.Instance;
		if (manager == null)
			{
			Debug.LogError("PlayersAndTeamsManager instance is missing!");
			return;
			}

		teams = manager.GetAllTeams();
		teamDropdown.ClearOptions();

		if (teams.Count == 0)
			{
			Debug.LogWarning("No teams found! Clearing dropdown.");
			return;
			}

		List<string> teamNames = new();
		foreach (var team in teams)
			{
			teamNames.Add(team.TeamName);
			}

		teamDropdown.AddOptions(teamNames);
		teamDropdown.RefreshShownValue();
		}

	/// <summary>
	/// Adds a new team or updates an existing team.
	/// </summary>
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
			manager.AddTeam(newTeamName);
			}
		else
			{
			manager.ModifyTeam(selectedTeamId, newTeamName);
			}

		PopulateTeamDropdown();
		ClearTeamName();
		}

	/// <summary>
	/// Modifies the selected team's name.
	/// </summary>
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

		manager.ModifyTeam(selectedTeamId, newTeamName);
		PopulateTeamDropdown();
		ClearTeamName();
		}

	/// <summary>
	/// Deletes the selected team.
	/// </summary>
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

		manager.DeleteTeam(selectedTeamId);
		PopulateTeamDropdown();
		ClearTeamName();
		}

	/// <summary>
	/// Handles the back button action.
	/// </summary>
	private void HandleBackButton()
		{
		Debug.Log("Back button pressed.");
		gameObject.SetActive(false); // Hides the panel
		}

	/// <summary>
	/// Handles dropdown selection change.
	/// </summary>
	private void OnTeamSelected(int index)
		{
		if (index < 0 || index >= teams.Count)
			{
			Debug.LogWarning("Invalid team selection.");
			return;
			}

		selectedTeamId = teams[index].TeamId;
		teamNameInputField.text = teams[index].TeamName;
		}

	/// <summary>
	/// Clears the team name input field.
	/// </summary>
	private void ClearTeamName()
		{
		teamNameInputField.text = "";
		selectedTeamId = -1;
		}
	}
