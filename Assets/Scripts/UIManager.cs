using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Added for Toggle

public class UIManager:MonoBehaviour
	{
	// --- Panel References --- //
	[Header("Home Panel")]
	public GameObject homePanel;
	public TMP_Text homeHeaderText;
	public TMP_Text manageTeamsButtonText;
	public TMP_Text createPlayerButtonText;
	public TMP_Text comparisonButtonText;
	public TMP_Text settingsButtonText;
	public TMP_Text exitButtonText;
	public Button manageTeamsButton;
	public Button createPlayerButton;
	public Button comparisonButton;
	public Button settingsButton;
	public Button exitButton;

	[Header("Team Management Panel")]
	public GameObject teamManagementPanel;
	public TMP_Text teamManagementHeaderText;
	public TMP_Text teamNameText;
	public TMP_InputField teamNameInputField;
	public TMP_Text addUpdateTeamButtonText;
	public TMP_Text clearTeamNameButtonText;
	public TMP_Dropdown teamDropdown;
	public TMP_Text modifyTeamNameButtonText;
	public TMP_Text deleteButtonText;
	public TMP_Text backButtonText;
	public Button addUpdateTeamButton;
	public Button clearTeamNameButton;
	public Button modifyTeamNameButton;
	public Button deleteButton;
	public Button backButton;

	[Header("Player Management Panel")]
	public GameObject playerManagementPanel;
	public TMP_Text playerManagementHeaderText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public TMP_Text addPlayerButtonText;
	public TMP_Text deletePlayerButtonText;
	public TMP_Text addPlayerDetailsButtonText;
	public TMP_Text backButtonPlayerManagementText;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button backButtonPlayerManagement;

	[Header("Player Lifetime Data Input Panel")]
	public GameObject playerLifetimeDataInputPanel;
	public TMP_Text lifetimeDataInputHeaderText;
	public TMP_Dropdown lifetimeTeamNameDropdown;
	public TMP_Dropdown lifetimePlayerNameDropdown;
	public TMP_InputField lifetimeGamesWonInputField;
	public TMP_InputField lifetimeGamesPlayedInputField;
	public TMP_InputField lifetimeDefensiveShotAvgInputField;
	public TMP_InputField matchesPlayedInLast2YearsInputField;
	public TMP_InputField lifetimeBreakAndRunInputField;
	public TMP_InputField lifetimeMiniSlamsInputFieldLifetime;  // Renamed to avoid conflict
	public TMP_InputField lifetimeShutoutsInputField;
	public TMP_InputField nineOnTheSnapLifetimeInputField;
	public TMP_Text updateLifetimeButtonText;
	public TMP_Text backButtonLifetimeDataText;
	public Button updateLifetimeButton;
	public Button backButtonLifetimeData;

	[Header("Player Current Season Data Input Panel")]
	public GameObject playerCurrentSeasonDataInputPanel;
	public TMP_Text currentSeasonDataInputHeaderText;
	public TMP_Dropdown currentSeasonTeamNameDropdown;
	public TMP_Dropdown currentSeasonPlayerNameDropdown;
	public TMP_InputField gamesWonInputField;
	public TMP_InputField gamesPlayedInputField;
	public TMP_InputField totalPointsInputField;
	public TMP_InputField ppmInputField;
	public TMP_InputField paPercentageInputField;
	public TMP_InputField breakAndRunInputField;
	public TMP_InputField miniSlamsInputField;
	public TMP_InputField nineOnTheSnapCurrentSeasonInputField;
	public TMP_InputField shutoutsInputField;
	public TMP_Dropdown skillLevelDropdown;
	public TMP_Text addPlayerButtonTextCurrentSeason;
	public TMP_Text removePlayerButtonTextCurrentSeason;
	public TMP_Text backButtonCurrentSeasonDataText;
	public Button addPlayerButtonCurrentSeason;
	public Button removePlayerButtonCurrentSeason;
	public Button backButtonCurrentSeasonData;

	[Header("Matchup Comparison Panel")]
	public GameObject matchupComparisonPanel;
	public TMP_Text matchupComparisonHeaderText;
	public TMP_Text selectTeamAText;
	public TMP_Dropdown teamADropdown;
	public GameObject teamAPlayerScrollView;
	public TMP_Text teamBHeaderText;
	public TMP_Dropdown teamBDropdown;
	public GameObject teamBPlayerScrollView;
	public TMP_Text compareButtonText;
	public TMP_Text backButtonMatchupComparisonText;
	public Button compareButton;
	public Button backButtonMatchupComparison;

	[Header("Matchup Results Panel")]
	public GameObject matchupResultsPanel;
	public TMP_Text matchupResultsHeaderText;
	public TMP_Text teamAHeaderTextResult;
	public TMP_Text teamBHeaderTextResult;
	public GameObject matchupsScrollView;
	public TMP_Text bestMatchupHeaderText;
	public GameObject bestMatchupsScrollView;
	public TMP_Text backButtonMatchupResultsText;
	public Button backButtonMatchupResults;

	[Header("Settings Panel")]
	public GameObject settingsPanel;
	public TMP_Text settingsHeaderText;
	public Toggle settingsToggle;  // Fixed Toggle component
	public TMP_Text settingsBackButtonText;
	public Button settingsBackButton;

	[Header("Overlay Feedback Panel")]
	public GameObject overlayFeedbackPanel;
	public TMP_Text overlayFeedbackText;

	// --- Initialization --- //
	private void Start()
		{
		InitializePanels();
		InitializeButtonListeners();
		}

	// --- Panel Initialization --- //
	private void InitializePanels()
		{
		// Initialize visibility of panels
		homePanel.SetActive(true);
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		playerLifetimeDataInputPanel.SetActive(false);
		playerCurrentSeasonDataInputPanel.SetActive(false);
		matchupComparisonPanel.SetActive(false);
		matchupResultsPanel.SetActive(false);
		settingsPanel.SetActive(false);
		overlayFeedbackPanel.SetActive(false);
		}

	// --- Button Listeners --- //
	private void InitializeButtonListeners()
		{
		manageTeamsButton.onClick.AddListener(ShowTeamManagementPanel);
		createPlayerButton.onClick.AddListener(ShowPlayerManagementPanel);
		comparisonButton.onClick.AddListener(ShowMatchupComparisonPanel);
		settingsButton.onClick.AddListener(ShowSettingsPanel);
		exitButton.onClick.AddListener(Application.Quit);

		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeam);
		clearTeamNameButton.onClick.AddListener(ClearTeamNameInput);
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamName);
		deleteButton.onClick.AddListener(OnDeleteTeam);
		backButton.onClick.AddListener(ShowHomePanel);

		addPlayerButton.onClick.AddListener(OnAddPlayer);
		deletePlayerButton.onClick.AddListener(OnDeletePlayer);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetails);
		backButtonPlayerManagement.onClick.AddListener(ShowHomePanel);

		updateLifetimeButton.onClick.AddListener(OnUpdateLifetimeData);
		backButtonLifetimeData.onClick.AddListener(ShowPlayerManagementPanel);

		addPlayerButtonCurrentSeason.onClick.AddListener(OnAddPlayerCurrentSeason);
		removePlayerButtonCurrentSeason.onClick.AddListener(OnRemovePlayerCurrentSeason);
		backButtonCurrentSeasonData.onClick.AddListener(ShowPlayerManagementPanel);

		compareButton.onClick.AddListener(OnCompareMatchups);
		backButtonMatchupComparison.onClick.AddListener(ShowHomePanel);

		backButtonMatchupResults.onClick.AddListener(ShowHomePanel);

		settingsBackButton.onClick.AddListener(ShowHomePanel);
		}

	// --- Button Actions --- //
	private void OnAddUpdateTeam() { /* Add or Update Team */ }
	private void ClearTeamNameInput() { teamNameInputField.text = ""; }
	private void OnModifyTeamName() { /* Modify Team Name */ }
	private void OnDeleteTeam() { /* Delete Team */ }
	private void OnAddPlayer() { /* Add Player */ }
	private void OnDeletePlayer() { /* Delete Player */ }
	private void OnAddPlayerDetails() { /* Add Player Details */ }
	private void OnUpdateLifetimeData() { /* Update Lifetime Data */ }
	private void OnAddPlayerCurrentSeason() { /* Add Player to Current Season */ }
	private void OnRemovePlayerCurrentSeason() { /* Remove Player from Current Season */ }
	private void OnCompareMatchups() { /* Compare Matchups */ }

	// --- Panel Control Functions --- //
	public void ShowHomePanel()
		{
		homePanel.SetActive(true);
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		playerLifetimeDataInputPanel.SetActive(false);
		playerCurrentSeasonDataInputPanel.SetActive(false);
		matchupComparisonPanel.SetActive(false);
		matchupResultsPanel.SetActive(false);
		settingsPanel.SetActive(false);
		overlayFeedbackPanel.SetActive(false);
		}

	public void ShowTeamManagementPanel()
		{
		homePanel.SetActive(false);
		teamManagementPanel.SetActive(true);
		}

	public void ShowPlayerManagementPanel()
		{
		homePanel.SetActive(false);
		playerManagementPanel.SetActive(true);
		}

	public void ShowPlayerLifetimeDataInputPanel()
		{
		homePanel.SetActive(false);
		playerLifetimeDataInputPanel.SetActive(true);
		}

	public void ShowPlayerCurrentSeasonDataInputPanel()
		{
		homePanel.SetActive(false);
		playerCurrentSeasonDataInputPanel.SetActive(true);
		}

	public void ShowMatchupComparisonPanel()
		{
		homePanel.SetActive(false);
		matchupComparisonPanel.SetActive(true);
		}

	public void ShowMatchupResultsPanel()
		{
		homePanel.SetActive(false);
		matchupResultsPanel.SetActive(true);
		}

	public void ShowSettingsPanel()
		{
		homePanel.SetActive(false);
		settingsPanel.SetActive(true);
		}

	public void ShowOverlayFeedbackPanel(string feedback)
		{
		overlayFeedbackPanel.SetActive(true);
		overlayFeedbackText.text = feedback;
		}

	// --- Additional Functions --- //
	// You can add more utility methods here as needed.
	}
