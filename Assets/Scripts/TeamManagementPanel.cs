using UnityEngine;
using UnityEngine.UI;
using TMPro; // --- Make sure to include TMPro for text elements --- //

using NickWasHere;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	[Header("UI Elements")]
	public TMP_InputField teamNameInputField; // TMP Input field for entering team name //
	public TMP_Text teamNameText; // TMP Text to display the team name //
	public TMP_Dropdown teamDropdown; // TMP Dropdown to select a team for deletion //
	public Button updateTeamButton; // Button to update the selected team //
	public Button deleteButton; // Button to delete the selected team //
	public Button addTeamButton; // Button to add a new team //
	public Button backButton; // Button to go back to the previous panel //

	// --- Team Data --- //
	private string selectedTeamName; // Currently selected team for updating or deletion //

	// --- Start is called before the first frame update --- //
	private void Start()
		{
		// Set up button listeners //
		updateTeamButton.onClick.AddListener(OnUpdateTeamButtonClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		addTeamButton.onClick.AddListener(OnAddTeamButtonClicked); // Add listener for adding team
		backButton.onClick.AddListener(OnBackButtonClicked);

		// Initialize the dropdown and populate with team names //
		InitializeTeamDropdown();
		}

	// --- Initialize Team Dropdown --- //
	private void InitializeTeamDropdown()
		{
		// Clear any previous options //
		teamDropdown.ClearOptions();

		// Retrieve all teams //
		var teamNames = DataManager.Instance.GetTeamNames();

		// Add a default "Select Team" option //
		teamNames.Insert(0, "Select Team");

		// Add the options to the dropdown //
		teamDropdown.AddOptions(teamNames);

		// Add listener for dropdown value change //
		teamDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

		// Reset selection //
		teamDropdown.value = 0;
		OnDropdownValueChanged(0);
		}

	// --- Dropdown Value Changed --- //
	private void OnDropdownValueChanged(int index)
		{
		if (index > 0) // Skip the "Select Team" option //
			{
			selectedTeamName = teamDropdown.options[index].text;
			teamNameText.text = "Selected Team: " + selectedTeamName;
			}
		else
			{
			selectedTeamName = null;
			teamNameText.text = "No team selected.";
			}
		}

	// --- Update Team Button Clicked --- //
	private void OnUpdateTeamButtonClicked()
		{
		if (string.IsNullOrEmpty(selectedTeamName))
			{
			OverlayFeedbackPanelManager.Instance.ShowPanel("No team selected for updating.", "", false);
			return;
			}

		string newTeamName = teamNameInputField.text.Trim();

		if (string.IsNullOrEmpty(newTeamName))
			{
			OverlayFeedbackPanelManager.Instance.ShowPanel("Team name cannot be empty.");
			return;
			}

		// Update the team name via DataManager //
		DataManager.Instance.UpdateTeamName(selectedTeamName, newTeamName);

		// Refresh UI //
		InitializeTeamDropdown();
		teamNameInputField.text = "";
		selectedTeamName = null;
		teamNameText.text = "No team selected.";

		OverlayFeedbackPanelManager.Instance.ShowPanel("Team updated successfully.");
		}

	// --- Delete Button Clicked --- //
	private void OnDeleteButtonClicked()
		{
		if (string.IsNullOrEmpty(selectedTeamName))
			{
			OverlayFeedbackPanelManager.Instance.ShowPanel("No team selected for deletion.");
			return;
			}

		// Delete the team via DataManager //
		DataManager.Instance.RemoveTeam(selectedTeamName);

		// Refresh UI //
		InitializeTeamDropdown();
		selectedTeamName = null;
		teamNameInputField.text = "";
		teamNameText.text = "No team selected.";

		OverlayFeedbackPanelManager.Instance.ShowPanel("Team deleted successfully.");
		}

	// --- Add Team Button Clicked --- //
	private void OnAddTeamButtonClicked()
		{
		string newTeamName = teamNameInputField.text.Trim();

		if (string.IsNullOrEmpty(newTeamName))
			{
			OverlayFeedbackPanelManager.Instance.ShowPanel("Team name cannot be empty.");
			return;
			}

		// Add new team via DataManager //
		DataManager.Instance.AddTeam(newTeamName);

		// Refresh UI //
		InitializeTeamDropdown();
		teamNameInputField.text = "";
		teamNameText.text = "No team selected.";

		OverlayFeedbackPanelManager.Instance.ShowPanel("Team added successfully.");
		}

	// --- Back Button Clicked --- //
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel("MainMenuPanel");
		}
	}
