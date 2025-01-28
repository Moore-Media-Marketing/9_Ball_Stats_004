using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include TMPro for text elements

public class TeamManagementPanel:MonoBehaviour
	{
	// --- UI Elements ---
	[Header("UI Elements")]
	public TMP_InputField teamNameInputField;  // TMP Input field for entering team name
	public TMP_Text teamNameText;  // TMP Text to display the team name
	public TMP_Dropdown teamDropdown;  // TMP Dropdown to select a team for deletion
	public Button updateTeamButton;  // Button to update the selected team
	public Button deleteButton;  // Button to delete the selected team
	public Button backButton;  // Button to go back to the previous panel

	// --- Team Data ---
	private string selectedTeamName;  // Currently selected team for updating or deletion

	// Start is called before the first frame update
	private void Start()
		{
		// Set up button listeners
		updateTeamButton.onClick.AddListener(OnUpdateTeamButtonClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		backButton.onClick.AddListener(OnBackButtonClicked);

		// Initialize the dropdown and populate with team names
		InitializeTeamDropdown();
		}

	// --- Initialize Team Dropdown ---
	private void InitializeTeamDropdown()
		{
		// Clear any previous options
		teamDropdown.ClearOptions();

		// Retrieve all teams (replace with actual data retrieval)
		var teamNames = DataManager.Instance.GetTeamNames();  // Assuming this method exists

		// Add a default "Select Team" option
		teamNames.Insert(0, "Select Team");

		// Add the options to the dropdown
		teamDropdown.AddOptions(teamNames);

		// Add listener for dropdown value change
		teamDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
		}

	// --- Dropdown Value Changed ---
	private void OnDropdownValueChanged(int index)
		{
		if (index > 0)  // Skip the "Select Team" option
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

	// --- Update Team Button Clicked ---
	private void OnUpdateTeamButtonClicked()
		{
		if (string.IsNullOrEmpty(selectedTeamName))
			{
			Debug.LogError("No team selected for updating.");
			return;
			}

		string newTeamName = teamNameInputField.text.Trim();

		if (string.IsNullOrEmpty(newTeamName))
			{
			Debug.LogError("Team name cannot be empty.");
			return;
			}

		// Update the team name via DataManager (replace with actual method)
		DataManager.Instance.UpdateTeamName(selectedTeamName, newTeamName);

		// Update the dropdown and reset the input field
		InitializeTeamDropdown();
		teamNameInputField.text = "";
		selectedTeamName = null;
		teamNameText.text = "No team selected.";

		Debug.Log("Team updated to: " + newTeamName);
		}

	// --- Delete Button Clicked ---
	private void OnDeleteButtonClicked()
		{
		if (string.IsNullOrEmpty(selectedTeamName))
			{
			Debug.LogError("No team selected for deletion.");
			return;
			}

		// Delete the team via DataManager (replace with actual method)
		DataManager.Instance.RemoveTeam(selectedTeamName);

		// Update the dropdown
		InitializeTeamDropdown();

		// Clear selection and input field
		selectedTeamName = null;
		teamNameInputField.text = "";
		teamNameText.text = "No team selected.";

		Debug.Log("Team deleted: " + selectedTeamName);
		}

	// --- Back Button Clicked ---
	private void OnBackButtonClicked()
		{
		// Navigate back to the previous panel (MainMenuPanel in this case)
		UIManager.Instance.ShowPanel("MainMenuPanel");
		}
	}
