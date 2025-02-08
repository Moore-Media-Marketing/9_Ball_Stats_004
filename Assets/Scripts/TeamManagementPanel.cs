using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

	private void SetupButtonListeners()
		{
		if (addUpdateTeamButton != null) addUpdateTeamButton.onClick.AddListener(AddOrUpdateTeam);
		if (clearTeamNameButton != null) clearTeamNameButton.onClick.AddListener(ClearTeamName);
		if (modifyTeamNameButton != null) modifyTeamNameButton.onClick.AddListener(ModifySelectedTeam);
		if (deleteButton != null) deleteButton.onClick.AddListener(DeleteSelectedTeam);
		if (backButton != null) backButton.onClick.AddListener(HandleBackButton);
		if (teamDropdown != null) teamDropdown.onValueChanged.AddListener(OnTeamSelected);
		}

	private void PopulateTeamDropdown()
		{
		if (teamDropdown == null)
			{
			Debug.LogError("TeamDropdown reference is missing! Assign it in the Unity Inspector.");
			return;
			}

		if (PlayersAndTeamsManager.Instance == null)
			{
			Debug.LogError("PlayersAndTeamsManager instance is missing!");
			return;
			}

		teams = PlayersAndTeamsManager.Instance.GetAllTeams();

		if (teams == null || teams.Count == 0)
			{
			Debug.LogWarning("No teams found!");
			teamDropdown.ClearOptions();
			return;
			}

		teamDropdown.ClearOptions();
		List<string> teamNames = new();
		foreach (var team in teams)
			{
			teamNames.Add(team.TeamName);
			}

		teamDropdown.AddOptions(teamNames);
		teamDropdown.value = 0; // Default to the first team
		}

	private void AddOrUpdateTeam()
		{
		if (PlayersAndTeamsManager.Instance == null)
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
			// Adding a new team
			PlayersAndTeamsManager.Instance.AddTeam(newTeamName);
			Debug.Log($"Added new team: {newTeamName}");
			}
		else
			{
			// Updating existing team
			PlayersAndTeamsManager.Instance.ModifyTeam(selectedTeamId, newTeamName);
			Debug.Log($"Updated team ID {selectedTeamId} to {newTeamName}");
			}

		PopulateTeamDropdown();
		ClearTeamName();
		}

	private void ClearTeamName()
		{
		if (teamNameInputField != null)
			{
			teamNameInputField.text = "";
			}
		selectedTeamId = -1; // Reset selected ID
		}

	private void ModifySelectedTeam()
		{
		if (teamDropdown == null || PlayersAndTeamsManager.Instance == null)
			{
			Debug.LogError("TeamDropdown or PlayersAndTeamsManager is missing!");
			return;
			}

		if (teamDropdown.value < 0 || teams.Count == 0)
			{
			Debug.LogWarning("No team selected for modification.");
			return;
			}

		selectedTeamId = teams[teamDropdown.value].TeamId;
		if (teamNameInputField != null)
			{
			teamNameInputField.text = teams[teamDropdown.value].TeamName;
			}
		}

	private void DeleteSelectedTeam()
		{
		if (teamDropdown == null || PlayersAndTeamsManager.Instance == null)
			{
			Debug.LogError("TeamDropdown or PlayersAndTeamsManager is missing!");
			return;
			}

		if (teamDropdown.value < 0 || teams.Count == 0)
			{
			Debug.LogWarning("No team selected for deletion.");
			return;
			}

		int teamIdToDelete = teams[teamDropdown.value].TeamId;
		PlayersAndTeamsManager.Instance.DeleteTeam(teamIdToDelete);
		PopulateTeamDropdown();
		Debug.Log($"Deleted team with ID {teamIdToDelete}");
		}

	private void OnTeamSelected(int index)
		{
		if (index < 0 || index >= teams.Count)
			{
			return;
			}

		selectedTeamId = teams[index].TeamId;
		if (teamNameInputField != null)
			{
			teamNameInputField.text = teams[index].TeamName;
			}
		}

	private void HandleBackButton()
		{
		Debug.Log("Back button clicked. Returning to the previous screen...");
		}

	public void UpdateBackButtonText(string newText)
		{
		if (backButtonText != null)
			{
			backButtonText.text = newText;
			}
		}
	}
