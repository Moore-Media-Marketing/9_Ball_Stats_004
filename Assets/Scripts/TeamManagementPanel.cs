using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- References to UI elements ---
	public TextMeshProUGUI headerText;              // Header text for the panel
	public TextMeshProUGUI teamNameText;            // Text to display selected team name
	public TMP_InputField teamNameInputField;      // Input field for entering a team name
	public TMP_Dropdown teamDropdown;              // Dropdown to select an existing team
	public Button clearTeamNameButton;             // Button to clear the team name input field
	public Button modifyTeamNameButton;            // Button to modify the team name
	public Button addUpdateTeamButton;             // Button to add or update a team
	public Button deleteButton;                    // Button to delete the selected team
	public Button backButton;                      // Button to go back to the previous panel
	public TextMeshProUGUI backButtonText;          // Text for the back button (modifiable)

	// --- Team data ---
	private List<Team> Teams => DataManager.Instance.teams;  // Access the teams from DataManager
	private Team selectedTeam;                      // Currently selected team

	// --- Start method ---
	private void Start()
		{
		// --- Check if references are set ---
		if (headerText == null || teamNameText == null || teamNameInputField == null ||
			teamDropdown == null || clearTeamNameButton == null || modifyTeamNameButton == null ||
			addUpdateTeamButton == null || deleteButton == null || backButton == null ||
			backButtonText == null)
			{
			Debug.LogError("Missing references in TeamManagementPanel.");
			return;
			}

		// --- Hook up buttons to methods ---
		clearTeamNameButton.onClick.AddListener(OnClearTeamNameButtonClicked);
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamNameButtonClicked);
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeamButtonClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		backButton.onClick.AddListener(OnBackButtonClicked);

		// --- Set up the dropdown ---
		teamDropdown.onValueChanged.AddListener(OnTeamDropdownChanged);

		// --- Display teams in the dropdown ---
		DisplayTeamList();
		}

	// --- Display Team List Method ---
	private void DisplayTeamList()
		{
		Debug.Log("Displaying team list...");
		teamDropdown.ClearOptions();
		List<string> options = new() { "Select Team" };

		// Fetch teams from DataManager
		foreach (var team in Teams)
			{
			options.Add(team.Name);
			Debug.Log("Adding team: " + team.Name);
			}

		teamDropdown.AddOptions(options);
		teamDropdown.value = 0;
		teamDropdown.RefreshShownValue();
		}

	// --- Handle Clear Team Name Button Click ---
	private void OnClearTeamNameButtonClicked()
		{
		teamNameInputField.text = string.Empty; // Clear the input field
		}

	// --- Handle Modify Team Name Button Click ---
	private void OnModifyTeamNameButtonClicked()
		{
		if (selectedTeam != null)
			{
			selectedTeam.Name = teamNameInputField.text; // Update the team name
			teamNameText.text = selectedTeam.Name;      // Update the displayed team name
			Debug.Log($"Team name updated to: {selectedTeam.Name}");

			// Save the updated team name in DataManager
			DataManager.Instance.SaveDataToPlayerPrefs();
			}
		else
			{
			Debug.LogWarning("No team selected for modification.");
			}
		}

	// --- Handle Add or Update Team Button Click ---
	private void OnAddUpdateTeamButtonClicked()
		{
		string teamName = teamNameInputField.text;

		if (string.IsNullOrEmpty(teamName))
			{
			Debug.LogWarning("Please enter a team name.");
			return;
			}

		// Check if the team already exists in the DataManager's teams list
		Team existingTeam = Teams.Find(t => t.Name == teamName);
		if (existingTeam != null)
			{
			// Update the existing team
			existingTeam.Name = teamName;
			Debug.Log($"Updated existing team: {existingTeam.Name}");

			// Save the updated team list
			DataManager.Instance.SaveDataToPlayerPrefs();
			}
		else
			{
			// Create a new team
			Team newTeam = new(teamName);
			Teams.Add(newTeam);
			Debug.Log($"Created new team: {newTeam.Name}");

			// Save the new team list
			DataManager.Instance.SaveDataToPlayerPrefs();
			}

		// Refresh the team dropdown list and display updated team info
		DisplayTeamList();
		}

	// --- Handle Delete Team Button Click ---
	private void OnDeleteButtonClicked()
		{
		if (selectedTeam != null)
			{
			Teams.Remove(selectedTeam);
			Debug.Log($"Deleted team: {selectedTeam.Name}");

			// Save the updated team list
			DataManager.Instance.SaveDataToPlayerPrefs();

			// Refresh the team list in the dropdown and clear displayed team info
			DisplayTeamList();
			teamNameInputField.text = string.Empty; // Clear the input field
			teamNameText.text = "No team selected"; // Reset the displayed team name
			}
		else
			{
			Debug.LogWarning("No team selected for deletion.");
			}
		}

	// --- Handle Team Dropdown Selection Change ---
	private void OnTeamDropdownChanged(int index)
		{
		if (index > 0) // Exclude the "Select Team" option
			{
			selectedTeam = Teams[index - 1]; // Get the selected team based on dropdown index
			teamNameInputField.text = selectedTeam.Name; // Set the input field to the selected team name
			teamNameText.text = selectedTeam.Name; // Display the selected team's name
			}
		else
			{
			selectedTeam = null; // No team selected
			teamNameInputField.text = string.Empty; // Clear the input field
			teamNameText.text = "No team selected"; // Show default message
			}
		}

	// --- Handle Back Button Click ---
	private void OnBackButtonClicked()
		{
		// Example: Change the text of the back button dynamically
		backButtonText.text = "Back to Main Menu"; // Adjust button text as needed

		// Hide this panel and go back to the previous screen (or panel)
		gameObject.SetActive(false);
		Debug.Log("Going back to previous panel.");

		// Make sure the panel is deactivated first, then use UIManager to go back
		UIManager.Instance.GoBackToPreviousPanel();
		}

	// --- Remove any Blocker buttons or objects --- 
	private void RemoveBlocker()
		{
		// Look for any UI elements named 'Blocker' and remove them
		Transform blockerTransform = transform.Find("Blocker");
		if (blockerTransform != null)
			{
			Destroy(blockerTransform.gameObject); // Destroy the blocker element
			Debug.Log("Removed Blocker element.");
			}
		}
	}
