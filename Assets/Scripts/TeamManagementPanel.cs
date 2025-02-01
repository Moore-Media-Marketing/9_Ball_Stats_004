using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- UI Elements ---
	public TMP_Text headerText;

	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button clearTeamNameButton;
	public TMP_Dropdown teamDropdown;
	public Button modifyTeamNameButton;
	public Button deleteButton;
	public Button backButton;

	// --- Overlay Feedback Panel ---
	public GameObject overlayFeedbackPanel;

	public TMP_Text feedbackText;

	private List<Team> teamList = new();

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeamClicked);
		clearTeamNameButton.onClick.AddListener(OnClearTeamNameClicked);
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamNameClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);

		UpdateTeamDropdown();
		teamDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		}

	private void OnTeamDropdownValueChanged(int value)
		{
		if (value > 0)
			teamNameInputField.text = teamDropdown.options[value].text;
		else
			teamNameInputField.text = "";
		}

	private void UpdateTeamDropdown()
		{
		// Fetch teams from the database
		teamList = DatabaseManager.Instance.GetAllTeams();

		if (teamList == null || teamList.Count == 0)
			{
			Debug.LogWarning("No teams found in the database!");
			teamDropdown.ClearOptions();
			teamDropdown.AddOptions(new List<string> { "No Teams Available" });
			headerText.text = "No Teams Available";
			return;
			}

		Debug.Log($"Number of teams retrieved: {teamList.Count}");
		foreach (var team in teamList)
			{
			Debug.Log($"Team: {team.Name}");
			}

		// Extract unique team names
		List<string> uniqueTeamNames = teamList.Select(t => t.Name).Distinct().ToList();
		Debug.Log($"Unique team names: {string.Join(", ", uniqueTeamNames)}");

		// Update dropdown options
		teamDropdown.ClearOptions();
		uniqueTeamNames.Insert(0, "Select Team");
		teamDropdown.AddOptions(uniqueTeamNames);

		// Refresh dropdown UI
		teamDropdown.RefreshShownValue();

		// Set header text
		headerText.text = uniqueTeamNames.Count > 1 ? uniqueTeamNames[1] : "No Teams Available";
		}

	public void OnAddUpdateTeamClicked()
		{
		string teamName = teamNameInputField.text.Trim();

		if (string.IsNullOrEmpty(teamName))
			{
			ShowFeedback("Team name cannot be empty.");
			return;
			}

		Team existingTeam = teamList.FirstOrDefault(t => t.Name == teamName);
		if (existingTeam != null)
			{
			existingTeam.Name = teamName;
			DatabaseManager.Instance.SaveData();
			ShowFeedback($"Team '{teamName}' updated.");
			}
		else
			{
			Team newTeam = new(teamName);
			DatabaseManager.Instance.AddTeam(newTeam);
			ShowFeedback($"Team '{teamName}' added.");
			}

		teamNameInputField.text = "";
		UpdateTeamDropdown();
		}

	public void OnClearTeamNameClicked()
		{
		teamNameInputField.text = "";
		}

	public void OnModifyTeamNameClicked()
		{
		string teamName = teamNameInputField.text.Trim();
		if (string.IsNullOrEmpty(teamName))
			{
			ShowFeedback("Please enter a team name.");
			return;
			}

		if (teamDropdown.value > 0)
			{
			string selectedTeamName = teamDropdown.options[teamDropdown.value].text;
			Team selectedTeam = teamList.FirstOrDefault(t => t.Name == selectedTeamName);
			if (selectedTeam != null)
				{
				selectedTeam.Name = teamName;
				DatabaseManager.Instance.SaveData();
				ShowFeedback($"Team name updated to '{teamName}'.");
				UpdateTeamDropdown();
				teamNameInputField.text = "";
				}
			}
		else
			{
			ShowFeedback("Please select a team to modify.");
			}
		}

	public void OnDeleteButtonClicked()
		{
		if (teamDropdown.value > 0)
			{
			string selectedTeamName = teamDropdown.options[teamDropdown.value].text;
			Team selectedTeam = teamList.FirstOrDefault(t => t.Name == selectedTeamName);
			if (selectedTeam != null)
				{
				ShowFeedback($"Are you sure you want to delete '{selectedTeam.Name}'?");
				StartCoroutine(ConfirmDeletion(selectedTeam));
				}
			}
		else
			{
			ShowFeedback("Please select a team to delete.");
			}
		}

	private IEnumerator ConfirmDeletion(Team selectedTeam)
		{
		yield return new WaitForSeconds(3f);
		DatabaseManager.Instance.RemoveTeam(selectedTeam);
		ShowFeedback($"Team '{selectedTeam.Name}' deleted.");
		UpdateTeamDropdown();
		teamNameInputField.text = "";
		}

	private void ShowFeedback(string message)
		{
		feedbackText.text = message;
		overlayFeedbackPanel.SetActive(true);
		StartCoroutine(HideFeedbackPanel());
		}

	private IEnumerator HideFeedbackPanel()
		{
		yield return new WaitForSeconds(3f);
		overlayFeedbackPanel.SetActive(false);
		}

	public void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}
	}