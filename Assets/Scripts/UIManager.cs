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
	private Stack<GameObject> panelHistory = new(); // Stack to track panel history
	private GameObject currentPanel; // Tracks the currently active panel

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
		teamDropdown.ClearOptions();
		teamNameDropdown.ClearOptions();
		playerNameDropdown.ClearOptions();

		// --- Update Team Dropdown --- //
		teamList = DatabaseManager.Instance.GetAllTeams();
		List<string> teamNames = new();

		foreach (var team in teamList)
			{
			teamNames.Add(team.Name);
			}

		teamDropdown.AddOptions(teamNames);
		teamNameDropdown.AddOptions(teamNames);

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

	// --- Show Panels --- //
	public void ShowHomePanel() => ShowPanel(homePanel);
	public void ShowTeamManagementPanel() => ShowPanel(teamManagementPanel);
	public void ShowPlayerManagementPanel() => ShowPanel(playerManagementPanel);
	public void ShowMatchupComparisonPanel() => ShowPanel(matchupComparisonPanel);
	public void ShowSettingsPanel() => ShowPanel(settingsPanel);

	// --- Toggles visibility of panels --- //
	public void ShowPanel(GameObject panel)
		{
		if (currentPanel != null && currentPanel != panel)
			{
			panelHistory.Push(currentPanel); // Store the previous panel before switching
			}

		// Hide all panels
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
		currentPanel = panel; // Update the currently active panel
		}

	// --- Back Button Handler --- //
	public void OnBackButtonClicked()
		{
		if (panelHistory.Count > 0)
			{
			ShowPanel(panelHistory.Pop()); // Show the last panel from history
			}
		else
			{
			ShowHomePanel(); // Default back to home if no history exists
			}
		}
	}
