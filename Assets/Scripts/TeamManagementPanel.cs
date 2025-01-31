using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using SQLite;
using UnityEngine;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	public TMP_Text headerText;
	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button clearTeamNameButton;
	public TMP_Dropdown teamDropdown;
	public Button modifyTeamNameButton;
	public Button deleteButton;
	public Button backButton;

	// --- Overlay Feedback Panel --- //
	public GameObject overlayFeedbackPanel;  // Reference to the overlay feedback panel
	public TMP_Text feedbackText;  // Feedback text component

	private List<Team> teamList = new();
	private RectTransform dropdownListRectTransform; // Cache dropdown list rect

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

		// Add a listener for dropdown selection
		teamDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		}

	// --- Handle dropdown selection change --- //
	private void OnTeamDropdownValueChanged(int value)
		{
		// Only populate input field if a team is selected (value > 0, because "Select Team" is at index 0)
		if (value > 0)
			{
			string selectedTeamName = teamDropdown.options[value].text;
			teamNameInputField.text = selectedTeamName;
			}
		else
			{
			// Clear input field if no valid team is selected
			teamNameInputField.text = "";
			}
		}

	private void UpdateTeamDropdown()
		{
		// Get the updated team list from the database
		teamList = DatabaseManager.Instance.GetAllTeams();
		Debug.Log("Number of teams retrieved: " + teamList.Count);

		// Remove duplicate team names
		List<string> uniqueTeamNames = teamList.Select(t => t.Name).Distinct().ToList();

		// Debug log for unique team names
		Debug.Log("Unique team names: " + string.Join(", ", uniqueTeamNames));

		// Clear previous dropdown options
		teamDropdown.ClearOptions();
		Debug.Log("Cleared previous dropdown options.");

		// Add "Select Team" option
		uniqueTeamNames.Insert(0, "Select Team");

		// Add unique team names to the dropdown
		teamDropdown.AddOptions(uniqueTeamNames);
		Debug.Log("Added team options to dropdown.");

		// Set header text
		headerText.text = uniqueTeamNames.Count > 1 ? uniqueTeamNames[1] : "No Teams Available";

		// Adjust dropdown height
		StartCoroutine(AdjustDropdownHeight());
		}

	private IEnumerator AdjustDropdownHeight()
		{
		yield return null; // Wait a frame to ensure dropdown list is generated

		// Log dropdown structure
		foreach (Transform child in teamDropdown.transform)
			{
			Debug.Log("Dropdown child: " + child.name);
			}

		// Find and cache the dropdown list RectTransform
		if (dropdownListRectTransform == null)
			{
			Transform dropdownListTransform = teamDropdown.transform.Find("Dropdown List");
			if (dropdownListTransform != null)
				{
				dropdownListRectTransform = dropdownListTransform.GetComponent<RectTransform>();
				Debug.Log("Dropdown List found!");
				}
			else
				{
				Debug.LogWarning("Dropdown List not found! Check if the dropdown structure is correct.");
				}
			}

		if (dropdownListRectTransform != null)
			{
			int itemCount = teamDropdown.options.Count;
			float itemHeight = 30f; // Approximate height per item
			float minHeight = 120f;
			float maxHeight = 300f;

			// Set new height
			dropdownListRectTransform.sizeDelta = new Vector2(dropdownListRectTransform.sizeDelta.x,
				Mathf.Clamp(itemCount * itemHeight, minHeight, maxHeight));
			Debug.Log("Adjusted dropdown height.");
			}
		else
			{
			Debug.LogWarning("Dropdown List RectTransform is missing!");
			}
		}

	public void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}

	public void OnAddUpdateTeamClicked()
		{
		string teamName = teamNameInputField.text;

		if (string.IsNullOrEmpty(teamName))
			{
			ShowFeedback("Team name cannot be empty.");
			return;
			}

		// Check if team exists
		Team existingTeam = teamList.FirstOrDefault(t => t.Name == teamName);

		if (existingTeam != null)
			{
			// Update existing team
			existingTeam.Name = teamName;
			DatabaseManager.Instance.UpdateTeam(existingTeam);
			ShowFeedback($"Team '{teamName}' updated.");
			}
		else
			{
			// Add a new team
			Team newTeam = new() { Name = teamName };
			DatabaseManager.Instance.AddTeam(newTeam);
			ShowFeedback($"Team '{teamName}' added.");
			}

		// Clear input and refresh dropdown
		teamNameInputField.text = "";
		UpdateTeamDropdown();
		}

	public void OnClearTeamNameClicked()
		{
		teamNameInputField.text = "";
		}

	public void OnModifyTeamNameClicked()
		{
		string teamName = teamNameInputField.text;

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
				selectedTeam.Name = teamName; // Modify the team name
				DatabaseManager.Instance.UpdateTeam(selectedTeam); // Update in the database
				ShowFeedback($"Team name updated to '{teamName}'");

				// Refresh the dropdown
				UpdateTeamDropdown();
				teamNameInputField.text = ""; // Clear the input field after modification
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
		yield return new WaitForSeconds(3f); // Wait before deleting

		DatabaseManager.Instance.RemoveTeam(selectedTeam);
		ShowFeedback($"Team '{selectedTeam.Name}' deleted.");

		// Refresh dropdown and clear input
		UpdateTeamDropdown();
		teamNameInputField.text = "";
		}

	private void ShowFeedback(string message)
		{
		feedbackText.text = message;
		overlayFeedbackPanel.SetActive(true);

		// Hide after 3 seconds
		StartCoroutine(HideFeedbackPanel());
		}

	private IEnumerator HideFeedbackPanel()
		{
		yield return new WaitForSeconds(3f);
		overlayFeedbackPanel.SetActive(false);
		}
	}
