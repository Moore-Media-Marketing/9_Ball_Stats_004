using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages team-related operations such as adding, modifying, and deleting teams.
/// Integrates with DatabaseManager for persistent data storage.
/// Displays user feedback using OverlayFeedbackPanel.
/// </summary>
public class TeamManagementPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	[Header("UI Elements")]
	[Tooltip("Text displaying the header of the Team Management panel.")]
	public TMP_Text headerText;

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

	// --- Data Management --- //
	private List<Team> teams = new();

	private int selectedTeamId = -1;

	// --- Initialization --- //
	private void Start()
		{
		PopulateTeamDropdown();
		SetupButtonListeners();
		}

	// --- Setup Buttons --- //
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

	// --- Populate Dropdown with Teams --- //
	private void PopulateTeamDropdown()
		{
		if (teamDropdown == null)
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("Error: TeamDropdown reference is missing.");
			return;
			}

		teams = DatabaseManager.Instance.GetAllTeams();
		teamDropdown.ClearOptions();

		if (teams.Count == 0)
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("No teams found. Please add a new team.");
			teamDropdown.AddOptions(new List<string> { "No Teams Available" });
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

	// --- Add or Update Team --- //
	private void AddOrUpdateTeam()
		{
		string newTeamName = teamNameInputField.text.Trim();
		if (string.IsNullOrEmpty(newTeamName))
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("Team name cannot be empty.");
			return;
			}

		if (selectedTeamId == -1)
			{
			DatabaseManager.Instance.AddTeam(newTeamName); // Add new team
			OverlayFeedbackPanel.Instance.ShowFeedback($"Team '{newTeamName}' added successfully.");
			}
		else
			{
			DatabaseManager.Instance.ModifyTeam(selectedTeamId, newTeamName); // Modify existing team
			OverlayFeedbackPanel.Instance.ShowFeedback($"Team '{newTeamName}' updated successfully.");
			}

		RefreshUI();
		}

	// --- Modify Selected Team --- //
	private void ModifySelectedTeam()
		{
		if (selectedTeamId == -1)
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("No team selected for modification.");
			return;
			}

		string newTeamName = teamNameInputField.text.Trim();
		if (string.IsNullOrEmpty(newTeamName))
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("Team name cannot be empty.");
			return;
			}

		DatabaseManager.Instance.ModifyTeam(selectedTeamId, newTeamName);
		OverlayFeedbackPanel.Instance.ShowFeedback($"Team '{newTeamName}' modified successfully.");
		RefreshUI();
		}

	// --- Delete Selected Team --- //
	private void DeleteSelectedTeam()
		{
		if (selectedTeamId == -1)
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("No team selected for deletion.");
			return;
			}

		string teamName = teams.Find(t => t.TeamId == selectedTeamId)?.TeamName;

		OverlayFeedbackPanel.Instance.ShowFeedback(
			$"Are you sure you want to delete '{teamName}'? This will move its players to 'Unassigned Players'.",
			() =>
			{
				DatabaseManager.Instance.DeleteTeam(selectedTeamId);
				OverlayFeedbackPanel.Instance.ShowFeedback($"Team '{teamName}' deleted successfully.");
				RefreshUI();
			},
			() => OverlayFeedbackPanel.Instance.ShowFeedback("Deletion canceled.")
		);
		}

	// --- Handle Dropdown Selection --- //
	private void OnTeamSelected(int index)
		{
		if (index < 0 || index >= teams.Count)
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("Invalid team selection.");
			return;
			}

		selectedTeamId = teams[index].TeamId;
		teamNameInputField.text = teams[index].TeamName;
		}

	// --- Refresh UI After Changes --- //
	private void RefreshUI()
		{
		PopulateTeamDropdown();
		ClearTeamName();
		}

	// --- Clear Input Field --- //
	private void ClearTeamName()
		{
		teamNameInputField.text = "";
		selectedTeamId = -1;
		}

	// --- Handle Back Button --- //
	private void HandleBackButton()
		{
		Debug.Log("HandleBackButton called");

		if (OverlayFeedbackPanel.Instance != null)
			{
			OverlayFeedbackPanel.Instance.ShowFeedback("Returning to the previous menu...");
			}
		else
			{
			Debug.LogError("OverlayFeedbackPanel instance is null.");
			}

		if (UIManager.Instance != null)
			{
			Debug.Log("UIManager instance found, calling GoBackToPreviousPanel");
			UIManager.Instance.GoBackToPreviousPanel();
			}
		else
			{
			Debug.LogError("UIManager.Instance is null.");
			}

		gameObject.SetActive(false);
		}
	}