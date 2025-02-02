// --- Region: Using Directives --- //
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
// --- End Region: Using Directives --- //


// --- Region: Class Definition --- //
public class TeamManagementPanel:MonoBehaviour
	{
	#region UI Elements

	[Header("UI Elements")]
	[Tooltip("Header text for the panel.")]
	public TMP_Text headerText;  // --- Header text for the panel ---

	[Tooltip("Input field for team name.")]
	public TMP_InputField teamNameInputField;

	[Tooltip("Button to add/update a team.")]
	public Button addUpdateTeamButton;

	[Tooltip("Button to clear the team name input field.")]
	public Button clearTeamNameButton;

	[Tooltip("Dropdown listing teams.")]
	public TMP_Dropdown teamDropdown;

	[Tooltip("Button to modify the team name.")]
	public Button modifyTeamNameButton;

	[Tooltip("Button to delete the selected team.")]
	public Button deleteButton;

	[Tooltip("Button to return to the previous panel.")]
	public Button backButton;

	[Header("Overlay Feedback")]
	public GameObject overlayFeedbackPanel;
	public TMP_Text feedbackText;

	#endregion UI Elements

	#region Private Fields

	private List<Team> teamList = new List<Team>();

	#endregion Private Fields

	#region Unity Methods

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeamClicked);
		clearTeamNameButton.onClick.AddListener(OnClearTeamNameClicked);
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamNameClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);

		teamDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);

		// Update dropdown via DropdownManager rather than directly from DatabaseManager
		DropdownManager.Instance.UpdateTeamDropdown(teamDropdown);
		}

	#endregion Unity Methods

	#region Data Loading and UI Update

	private void OnTeamDropdownValueChanged(int value)
		{
		if (value > 0)
			teamNameInputField.text = teamDropdown.options[value].text;
		else
			teamNameInputField.text = "";
		}

	#endregion Data Loading and UI Update

	#region Button Event Handlers

	public void OnAddUpdateTeamClicked()
		{
		string teamName = teamNameInputField.text.Trim();
		if (string.IsNullOrEmpty(teamName))
			{
			ShowFeedback("Team name cannot be empty.");
			return;
			}

		teamList = DatabaseManager.Instance.GetAllTeams();
		Team existingTeam = teamList.FirstOrDefault(t => t.name.Equals(teamName, System.StringComparison.OrdinalIgnoreCase));
		if (existingTeam != null)
			{
			// For update, we assume the team to update is the currently selected team in the dropdown.
			string oldName = teamDropdown.options[teamDropdown.value].text;
			DatabaseManager.Instance.UpdateTeamName(oldName, teamName);
			ShowFeedback($"Team '{teamName}' updated.");
			}
		else
			{
			Team newTeam = new Team(teamName);
			DatabaseManager.Instance.AddTeam(newTeam);
			ShowFeedback($"Team '{teamName}' added.");
			}

		teamNameInputField.text = "";
		// Refresh the dropdown via DropdownManager
		DropdownManager.Instance.UpdateTeamDropdown(teamDropdown);
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
			// Update using both the old team name and the new team name
			DatabaseManager.Instance.UpdateTeamName(selectedTeamName, teamName);
			ShowFeedback($"Team name updated to '{teamName}'.");
			DropdownManager.Instance.UpdateTeamDropdown(teamDropdown);
			teamNameInputField.text = "";
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
			ShowFeedback($"Are you sure you want to delete '{selectedTeamName}'?");
			StartCoroutine(ConfirmDeletion(selectedTeamName));
			}
		else
			{
			ShowFeedback("Please select a team to delete.");
			}
		}

	private IEnumerator ConfirmDeletion(string teamName)
		{
		yield return new WaitForSeconds(3f);
		DatabaseManager.Instance.RemoveTeam(teamName);
		ShowFeedback($"Team '{teamName}' deleted.");
		DropdownManager.Instance.UpdateTeamDropdown(teamDropdown);
		teamNameInputField.text = "";
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}

	#endregion Button Event Handlers

	#region Feedback Functions

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

	#endregion Feedback Functions

	#region Additional Functions

	// --- Additional custom functions can be added here --- //

	#endregion Additional Functions
	}
// --- End Region: Class Definition --- //
