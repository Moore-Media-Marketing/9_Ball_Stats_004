using System.Collections.Generic;

using TMPro;

using UnityEngine;

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

	private List<Team> teamList = new();
	private List<Player> playerList = new();
	private GameObject previousPanel; // Track the previous panel for back functionality

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

		// Show the home panel by default when the app starts
		ShowHomePanel();
		}

	// --- Updates the UI dropdowns with teams and players --- //
	public void UpdateDropdowns()
		{
		// --- Clear existing options --- //
		teamDropdown.ClearOptions();
		teamNameDropdown.ClearOptions();
		playerNameDropdown.ClearOptions();

		// --- Update Team Dropdown --- //
		teamList = DatabaseManager.Instance.GetAllTeams();
		List<string> teamNames = new();

		foreach (var team in teamList)
			{
			teamNames.Add(team.Name);  // Add the team names to the list
			}

		teamDropdown.AddOptions(teamNames);  // Add teams to the dropdown
		teamNameDropdown.AddOptions(teamNames); // Update the player management dropdown too

		// --- Update Player Dropdown --- //
		if (teamList.Count > 0)
			{
			playerList = DatabaseManager.Instance.GetPlayersByTeam(teamList[0].Id);
			List<string> playerNames = new();

			foreach (var player in playerList)
				{
				playerNames.Add(player.Name);
				}

			playerNameDropdown.AddOptions(playerNames);
			}

		if (teamList.Count > 0)
			{
			teamDropdown.value = 0;
			teamDropdown.RefreshShownValue();

			if (playerList.Count > 0)
				{
				playerNameDropdown.value = 0;
				playerNameDropdown.RefreshShownValue();
				}
			}
		}

	// --- Show Home Panel --- //
	public void ShowHomePanel()
		{
		ShowPanel(homePanel);  // Show the home panel specifically
		}

	// --- Show Team Management Panel --- //
	public void ShowTeamManagementPanel()
		{
		ShowPanel(teamManagementPanel);  // Show the team management panel
		}

	// --- Show Player Management Panel --- //
	public void ShowPlayerManagementPanel()
		{
		ShowPanel(playerManagementPanel);  // Show the player management panel
		}

	// --- Show Matchup Comparison Panel --- //
	public void ShowMatchupComparisonPanel()
		{
		ShowPanel(matchupComparisonPanel);  // Show the matchup comparison panel
		}

	// --- Show Settings Panel --- //
	public void ShowSettingsPanel()
		{
		ShowPanel(settingsPanel);  // Show the settings panel
		}

	// --- Toggles visibility of panels --- //
	public void ShowPanel(GameObject panel)
		{
		// Hide all panels first
		homePanel.SetActive(false);
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		playerLifetimeDataInputPanel.SetActive(false);
		playerCurrentSeasonDataInputPanel.SetActive(false);
		matchupComparisonPanel.SetActive(false);
		matchupResultsPanel.SetActive(false);
		settingsPanel.SetActive(false);
		overlayFeedbackPanel.SetActive(false);

		// Show the selected panel
		panel.SetActive(true);

		// Track the current panel to handle "back" functionality
		previousPanel = panel;
		}

	// --- Back Button Handler --- //
	public void OnBackButtonClicked()
		{
		if (previousPanel != null)
			{
			ShowPanel(previousPanel);  // Show the previous panel
			}
		else
			{
			// If there's no previous panel, go back to the home panel
			ShowHomePanel();
			}
		}
	}