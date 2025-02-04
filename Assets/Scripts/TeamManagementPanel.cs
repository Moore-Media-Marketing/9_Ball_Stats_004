using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	#region UI Elements
	[Header("UI Elements")]
	[Tooltip("Header text for the panel.")]
	public TMP_Text headerText;

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
	private List<Team> teamList = new();
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

		// Load teams from DatabaseManager
		teamList = DatabaseManager.Instance.GetAllTeams();
		DropdownManager.Instance.UpdateTeamDropdown(teamDropdown, teamList); // Ensure this method is updated
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

		Team existingTeam = teamList.FirstOrDefault(t => t.name.Equals(teamName, System.StringComparison.OrdinalIgnoreCase));

		if (existingTeam != null)
			{
			// Update team name in the list
			existingTeam.name = teamName;
			DatabaseManager.Instance.SaveTeams(); // Save to JSON without arguments
			ShowFeedback($"Team '{teamName}' updated.");
			}
		else
			{
			Team newTeam = new(teamName);
			teamList.Add(newTeam);
			DatabaseManager.Instance.SaveTeams(); // Save to JSON without arguments
			ShowFeedback($"Team '{teamName}' added.");
			}

		teamNameInputField.text = "";
		DropdownManager.Instance.UpdateTeamDropdown(teamDropdown, teamList);
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
			// Find the selected team ID
			int selectedTeamId = teamDropdown.value > 0
				? teamList.FirstOrDefault(t => t.name == teamDropdown.options[teamDropdown.value].text)?.id ?? -1
				: -1;

			if (selectedTeamId == -1)
				{
				ShowFeedback("Error: Selected team ID not found.");
				return;
				}

			// Update team name in the list
			var team = teamList.FirstOrDefault(t => t.id == selectedTeamId);
			if (team != null)
				{
				team.name = teamName;
				DatabaseManager.Instance.SaveTeams(); // Save to JSON without arguments
				ShowFeedback($"Team name updated to '{teamName}'.");

				DropdownManager.Instance.UpdateTeamDropdown(teamDropdown, teamList);
				teamNameInputField.text = "";
				}
			}
		else
			{
			ShowFeedback("Please select a team to modify.");
			}
		}

	public void OnClearTeamNameClicked()
		{
		teamNameInputField.text = "";
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
		Team teamToDelete = teamList.FirstOrDefault(t => t.name == teamName);
		if (teamToDelete != null)
			{
			teamList.Remove(teamToDelete);
			DatabaseManager.Instance.SaveTeams(); // Save to JSON without arguments
			ShowFeedback($"Team '{teamName}' deleted.");
			DropdownManager.Instance.UpdateTeamDropdown(teamDropdown, teamList);
			teamNameInputField.text = "";
			}
		else
			{
			ShowFeedback("Error: Team not found.");
			}
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
