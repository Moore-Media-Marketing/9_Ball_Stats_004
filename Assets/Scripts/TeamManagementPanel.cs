using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class TeamManagementPanel:MonoBehaviour
	{
	// UI Elements
	public TMP_Text headerText;
	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button clearTeamNameButton;
	public TMP_Dropdown teamDropdown;
	public Button modifyTeamNameButton;
	public Button deleteButton;
	public Button backButton;

	// Overlay Feedback Panel
	public GameObject overlayFeedbackPanel;
	public TMP_Text feedbackText;

	private List<Team> teamList = new();

	private void Start()
		{
		// Assign button actions
		backButton.onClick.AddListener(OnBackButtonClicked);
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeamClicked);
		clearTeamNameButton.onClick.AddListener(OnClearTeamNameClicked);
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamNameClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);

		// Populate the dropdown with existing teams
		UpdateTeamDropdown();
		}

	private void UpdateTeamDropdown()
		{
		teamList = DatabaseManager.Instance.GetAllTeams();
		teamDropdown.ClearOptions();

		List<string> options = new List<string> { "Select Team" };
		options.AddRange(teamList.Select(team => team.Name));

		teamDropdown.AddOptions(options);
		teamDropdown.value = 0;
		teamDropdown.RefreshShownValue();

		headerText.text = teamList.Count > 0 ? "Select a Team" : "No Teams Available";
		}

	public void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}

	public void OnAddUpdateTeamClicked()
		{
		string teamName = teamNameInputField.text.Trim();

		if (string.IsNullOrEmpty(teamName))
			{
			ShowFeedback("Team name cannot be empty.");
			return;
			}

		if (teamDropdown.value > 0)
			{
			Team selectedTeam = teamList[teamDropdown.value - 1];
			selectedTeam.Name = teamName;
			DatabaseManager.Instance.UpdateTeam(selectedTeam);
			ShowFeedback($"Team '{teamName}' updated.");
			}
		else
			{
			Team newTeam = new() { Name = teamName };
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
		if (teamDropdown.value > 0)
			{
			teamNameInputField.text = teamList[teamDropdown.value - 1].Name;
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
			Team selectedTeam = teamList[teamDropdown.value - 1];
			ShowFeedback($"Are you sure you want to delete '{selectedTeam.Name}'?");
			StartCoroutine(ConfirmDeletion(selectedTeam));
			}
		else
			{
			ShowFeedback("Please select a team to delete.");
			}
		}

	private IEnumerator ConfirmDeletion(Team selectedTeam)
		{
		yield return new WaitForSeconds(3f);
		overlayFeedbackPanel.SetActive(false);
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
	}
