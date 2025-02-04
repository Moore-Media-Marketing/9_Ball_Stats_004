using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class UIManager:MonoBehaviour
	{
	public static UIManager Instance;

	[Header("Panels")]
	public GameObject homePanel;
	public GameObject teamManagementPanel;
	public GameObject playerManagementPanel;
	public GameObject playerLifetimeDataInputPanel;
	public GameObject playerCurrentSeasonDataInputPanel;
	public GameObject matchupComparisonPanel;
	public GameObject matchupResultsPanel;
	public GameObject settingsPanel;
	public GameObject overlayFeedbackPanel;

	[Header("Team Management UI Elements")]
	public TMP_InputField teamNameInputField;
	public TMP_Dropdown teamDropdown;

	[Header("Player Management UI Elements")]
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;

	[Header("Feedback UI Elements")]
	public TMP_Text feedbackText;

	// Example list of teams and players. Replace with actual data from your manager.
	private List<Team> teamList = new();
	private List<Player> playerList = new();

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			}
		}

	// --- Updates the UI dropdowns with teams and players --- //
	public void UpdateDropdowns()
		{
		// --- Clear existing options --- //
		teamDropdown.ClearOptions();
		teamNameDropdown.ClearOptions();
		playerNameDropdown.ClearOptions();

		// --- Update Team Dropdown --- //
		// Fetch teams from DatabaseManager
		teamList = DatabaseManager.Instance.GetAllTeams();  // Correct method call
		List<string> teamNames = new();

		foreach (var team in teamList)
			{
			teamNames.Add(team.Name);  // Add the team names to the list
			}

		teamDropdown.AddOptions(teamNames);  // Add teams to the dropdown
		teamNameDropdown.AddOptions(teamNames); // Update the player management dropdown too

		// --- Update Player Dropdown --- //
		// Assume player data is tied to the selected team
		if (teamList.Count > 0)  // Only populate player dropdown if there are teams
			{
			// Fetch players for the first team (you can modify this to support the selected team)
			playerList = DatabaseManager.Instance.GetPlayersByTeam(teamList[0].Id);  // Correct method call
			List<string> playerNames = new();

			foreach (var player in playerList)
				{
				playerNames.Add(player.Name);  // Add player names to the list
				}

			playerNameDropdown.AddOptions(playerNames);  // Populate player dropdown
			}

		// Optionally, select the first team and player by default if they exist
		if (teamList.Count > 0)
			{
			teamDropdown.value = 0;  // Select first team by default
			teamDropdown.RefreshShownValue();  // Refresh the dropdown UI

			// Update player dropdown based on the selected team
			if (playerList.Count > 0)
				{
				playerNameDropdown.value = 0;  // Select first player by default if players exist
				playerNameDropdown.RefreshShownValue();  // Refresh the dropdown UI
				}
			}
		}

	// --- Toggles visibility of panels --- //
	public void ShowPanel(GameObject panel)
		{
		homePanel.SetActive(false);
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		playerLifetimeDataInputPanel.SetActive(false);
		playerCurrentSeasonDataInputPanel.SetActive(false);
		matchupComparisonPanel.SetActive(false);
		matchupResultsPanel.SetActive(false);
		settingsPanel.SetActive(false);
		overlayFeedbackPanel.SetActive(false);

		panel.SetActive(true);
		}
	}
