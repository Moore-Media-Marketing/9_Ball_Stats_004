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

	private Stack<GameObject> panelHistory = new Stack<GameObject>();
	private GameObject currentPanel;

	private void Awake()
		{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		ShowHomePanel();
		}

	public void UpdateDropdowns()
		{
		teamDropdown.ClearOptions();
		teamNameDropdown.ClearOptions();
		playerNameDropdown.ClearOptions();

		List<Team> teamList = DatabaseManager.Instance.GetAllTeams();
		List<string> teamNames = new List<string>();
		foreach (var team in teamList)
			teamNames.Add(team.Name);

		teamDropdown.AddOptions(teamNames);
		teamNameDropdown.AddOptions(teamNames);

		if (teamList.Count > 0)
			{
			List<Player> playerList = DatabaseManager.Instance.GetPlayersByTeam(teamList[0].Id);
			List<string> playerNames = new List<string>();
			foreach (var player in playerList)
				playerNames.Add(player.Name);

			playerNameDropdown.AddOptions(playerNames);
			}
		}

	public void ShowHomePanel() => ShowPanel(homePanel);
	public void ShowTeamManagementPanel() => ShowPanel(teamManagementPanel);
	public void ShowPlayerManagementPanel() => ShowPanel(playerManagementPanel);
	public void ShowMatchupComparisonPanel() => ShowPanel(matchupComparisonPanel);
	public void ShowSettingsPanel() => ShowPanel(settingsPanel);

	public void ShowPanel(GameObject panel)
		{
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

		// Show selected panel and update history
		panel.SetActive(true);
		if (currentPanel != panel)
			{
			if (currentPanel != null)
				panelHistory.Push(currentPanel);
			currentPanel = panel;
			}
		}

	public void OnBackButtonClicked()
		{
		if (panelHistory.Count > 0)
			ShowPanel(panelHistory.Pop());
		else
			ShowHomePanel();
		}
	}
