using NickWasHere;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	[Header("UI Elements")]
	public TMP_InputField teamNameInputField; // Input field for entering team name

	public TMP_Text teamNameText; // Text to display the team name
	public Button modifyTeamNameButton; // Button to modify the selected team name
	public Button addUpdateTeamButton; // Button to add or update a team
	public Button deleteButton; // Button to delete a team
	public Button clearTeamNameButton; // Button to clear the team name input
	public Button backButton; // Button to go back to the previous panel

	// --- Dropdown --- //
	[Header("Dropdown Elements")]
	public TMP_Dropdown teamDropdown; // Reference to the existing team dropdown in the UI

	// --- Team Data --- //
	private string selectedTeamName; // Currently selected team

	// --- Start --- //
	private void Start()
		{
		// Set up button listeners
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamNameButtonClicked);
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeamButtonClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		clearTeamNameButton.onClick.AddListener(OnClearTeamNameButtonClicked);
		backButton.onClick.AddListener(OnBackButtonClicked);

		// Initialize the team dropdown
		InitializeTeamDropdown();
		}

	// --- Initialize Team Dropdown --- //
	private void InitializeTeamDropdown()
		{
		if (teamDropdown == null)
			{
			Debug.LogError("Team Dropdown is not assigned in TeamManagementPanel.");
			return;
			}

		// Populate team options
		teamDropdown.ClearOptions();
		var teamOptions = DataManager.Instance.GetTeamNames();
		teamOptions.Insert(0, "Select Team"); // Add default option
		teamDropdown.AddOptions(teamOptions);

		// Set default selection to "Select Team"
		teamDropdown.value = 0;

		// Add listener for dropdown value changes
		teamDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

		// Set initial selection to "Select Team"
		OnDropdownValueChanged(0);
		}

	// --- Dropdown Value Changed --- //
	private void OnDropdownValueChanged(int index)
		{
		if (index > 0) // Skip "Select Team" option
			{
			selectedTeamName = teamDropdown.options[index].text;
			teamNameText.text = "Selected Team: " + selectedTeamName;
			teamNameInputField.text = selectedTeamName; // Pre-fill input field for editing
			addUpdateTeamButton.GetComponentInChildren<TMP_Text>().text = "Update Team"; // Change button text
			}
		else
			{
			selectedTeamName = null;
			teamNameText.text = "No team selected.";
			teamNameInputField.text = "";
			addUpdateTeamButton.GetComponentInChildren<TMP_Text>().text = "Add Team"; // Change button text
			}
		}

	// --- Modify Team Name Button Clicked --- //
	private void OnModifyTeamNameButtonClicked()
		{
		if (string.IsNullOrEmpty(selectedTeamName))
			{
			OverlayFeedbackPanelManager.Instance.ShowPanel("No team selected for modification.");
			return;
			}

		// Populate input field with selected team name
		teamNameInputField.text = selectedTeamName;
		teamNameText.text = "Editing: " + selectedTeamName;
		OverlayFeedbackPanelManager.Instance.ShowPanel("Editing team name.");
		}

	// --- Add or Update Team Button Clicked --- //
	private void OnAddUpdateTeamButtonClicked()
		{
		string newTeamName = teamNameInputField.text.Trim();

		if (string.IsNullOrEmpty(newTeamName))
			{
			OverlayFeedbackPanelManager.Instance.ShowPanel("Team name cannot be empty.");
			return;
			}

		// If a team is selected, update it; otherwise, add a new team
		if (!string.IsNullOrEmpty(selectedTeamName))
			{
			if (DataManager.Instance.DoesTeamExist(newTeamName) && selectedTeamName != newTeamName)
				{
				OverlayFeedbackPanelManager.Instance.ShowPanel("A team with this name already exists.");
				return;
				}

			DataManager.Instance.UpdateTeamName(selectedTeamName, newTeamName);
			OverlayFeedbackPanelManager.Instance.ShowPanel("Team updated successfully.");
			}
		else
			{
			if (DataManager.Instance.DoesTeamExist(newTeamName))
				{
				OverlayFeedbackPanelManager.Instance.ShowPanel("Team already exists.");
				return;
				}

			DataManager.Instance.AddTeam(newTeamName);
			OverlayFeedbackPanelManager.Instance.ShowPanel("Team added successfully.");
			}

		// Save changes to PlayerPrefs
		DataManager.Instance.SaveTeamsToPlayerPrefs();

		// Refresh UI
		InitializeTeamDropdown();
		teamNameInputField.text = "";
		teamNameText.text = "No team selected.";
		selectedTeamName = null;
		addUpdateTeamButton.GetComponentInChildren<TMP_Text>().text = "Add Team"; // Reset button text
		}

	// --- Delete Button Clicked --- //
	private void OnDeleteButtonClicked()
		{
		if (string.IsNullOrEmpty(selectedTeamName))
			{
			OverlayFeedbackPanelManager.Instance.ShowPanel("No team selected for deletion.");
			return;
			}

		// Delete the team via DataManager
		DataManager.Instance.RemoveTeam(selectedTeamName);

		// Save changes to PlayerPrefs
		DataManager.Instance.SaveTeamsToPlayerPrefs();

		// Refresh UI
		InitializeTeamDropdown();
		selectedTeamName = null;
		teamNameInputField.text = "";
		teamNameText.text = "No team selected.";
		addUpdateTeamButton.GetComponentInChildren<TMP_Text>().text = "Add Team"; // Reset button text

		OverlayFeedbackPanelManager.Instance.ShowPanel("Team deleted successfully.");
		}

	// --- Clear Team Name Button Clicked --- //
	private void OnClearTeamNameButtonClicked()
		{
		teamNameInputField.text = "";
		selectedTeamName = null;
		teamNameText.text = "No team selected.";
		addUpdateTeamButton.GetComponentInChildren<TMP_Text>().text = "Add Team"; // Reset button text
		OverlayFeedbackPanelManager.Instance.ShowPanel("Input cleared.");
		}

	// --- Back Button Clicked --- //
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel("MainMenuPanel");
		}
	}