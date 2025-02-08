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

		if (DatabaseManager.Instance == null)
			{
			Debug.LogError("DatabaseManager instance is missing!");
			return;
			}

		teams = DatabaseManager.Instance.LoadTeams();
		if (teams == null || teams.Count == 0)
			{
			Debug.LogWarning("No teams found in the database!");
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
		if (DatabaseManager.Instance == null)
			{
			Debug.LogError("DatabaseManager instance is missing!");
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
			int newTeamId = teams.Count + 1;
			Team newTeam = new(newTeamId, newTeamName);
			teams.Add(newTeam);
			Debug.Log($"Added new team: {newTeamName}");
			}
		else
			{
			// Updating existing team
			Team teamToUpdate = teams.Find(t => t.TeamId == selectedTeamId);
			if (teamToUpdate != null)
				{
				teamToUpdate.UpdateTeamName(newTeamName);
				Debug.Log($"Updated team {selectedTeamId} to {newTeamName}");
				}
			}

		DatabaseManager.Instance.SaveTeams(teams);
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
		if (teamDropdown == null || DatabaseManager.Instance == null)
			{
			Debug.LogError("TeamDropdown or DatabaseManager is missing!");
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
		if (teamDropdown == null || DatabaseManager.Instance == null)
			{
			Debug.LogError("TeamDropdown or DatabaseManager is missing!");
			return;
			}

		if (teamDropdown.value < 0 || teams.Count == 0)
			{
			Debug.LogWarning("No team selected for deletion.");
			return;
			}

		int teamIdToDelete = teams[teamDropdown.value].TeamId;
		DatabaseManager.Instance.DeleteTeam(teamIdToDelete);
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
