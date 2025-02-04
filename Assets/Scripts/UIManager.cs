using UnityEngine;
using UnityEngine.UI;  // Added for Toggle
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

	[Header("Settings Panel")]
	public GameObject settingsPanel;
	public TMP_Text settingsHeaderText;
	public Toggle settingsToggle;  // Fixed Toggle component
	public TMP_Text settingsBackButtonText;
	public Button settingsBackButton;

	[Header("Overlay Feedback Panel")]
	public GameObject overlayFeedbackPanel;
	public TMP_Text overlayFeedbackText;

	// --- CSV File Paths --- //
	private string teamsCsvPath;
	private string playersCsvPath;

	// --- Data Lists --- //
	private List<Team> teams = new();
	private List<Player> players = new();

	// --- Initialization --- //
	private void Start()
		{
		InitializePanels();
		InitializeButtonListeners();

		// Define file paths for CSVs
		teamsCsvPath = Path.Combine(Application.persistentDataPath, "teams.csv");
		playersCsvPath = Path.Combine(Application.persistentDataPath, "players.csv");

		// Load data from CSV files
		teams = LoadTeamsFromCSV();
		players = LoadPlayersFromCSV();

		// Populate dropdowns and UI elements
		PopulateTeamDropdown();
		PopulatePlayerDropdown();
		}

	// --- Panel Initialization --- //
	private void InitializePanels()
		{
		// Initialize visibility of panels
		homePanel.SetActive(true);
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		matchupComparisonPanel.SetActive(false);
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

		compareButton.onClick.AddListener(OnCompareMatchups);
		backButtonMatchupComparison.onClick.AddListener(ShowHomePanel);

		settingsBackButton.onClick.AddListener(ShowHomePanel);
		}

	// --- Button Actions --- //
	private void OnAddUpdateTeam() { /* Add or Update Team logic here */ }
	private void ClearTeamNameInput() { teamNameInputField.text = ""; }
	private void OnModifyTeamName() { /* Modify Team Name logic here */ }
	private void OnDeleteTeam() { /* Delete Team logic here */ }
	private void OnAddPlayer() { /* Add Player logic here */ }
	private void OnDeletePlayer() { /* Delete Player logic here */ }
	private void OnAddPlayerDetails() { /* Add Player Details logic here */ }
	private void OnCompareMatchups() { /* Compare Matchups logic here */ }

	// --- Panel Control Functions --- //
	public void ShowHomePanel()
		{
		homePanel.SetActive(true);
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		matchupComparisonPanel.SetActive(false);
		settingsPanel.SetActive(false);
		overlayFeedbackPanel.SetActive(false);
		}

	public void ShowTeamManagementPanel() { homePanel.SetActive(false); teamManagementPanel.SetActive(true); }
	public void ShowPlayerManagementPanel() { homePanel.SetActive(false); playerManagementPanel.SetActive(true); }
	public void ShowMatchupComparisonPanel() { homePanel.SetActive(false); matchupComparisonPanel.SetActive(true); }
	public void ShowSettingsPanel() { homePanel.SetActive(false); settingsPanel.SetActive(true); }

	// --- CSV Loading Functions --- //
	private List<Team> LoadTeamsFromCSV()
		{
		List<Team> loadedTeams = new();
		if (File.Exists(teamsCsvPath))
			{
			string[] lines = File.ReadAllLines(teamsCsvPath);
			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length == 2) // Assuming 2 columns: Id, Name
					{
					loadedTeams.Add(new Team { Id = int.Parse(columns[0]), Name = columns[1] });
					}
				}
			}
		return loadedTeams;
		}

	private List<Player> LoadPlayersFromCSV()
		{
		List<Player> loadedPlayers = new();
		if (File.Exists(playersCsvPath))
			{
			string[] lines = File.ReadAllLines(playersCsvPath);
			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length == 4) // Assuming 4 columns: Id, Name, TeamId, SkillLevel
					{
					loadedPlayers.Add(new Player
						{
						Id = int.Parse(columns[0]),
						Name = columns[1],
						TeamId = int.Parse(columns[2]),
						SkillLevel = int.Parse(columns[3])
						});
					}
				}
			}
		return loadedPlayers;
		}

	// --- UI Population Functions --- //
	private void PopulateTeamDropdown()
		{
		teamDropdown.ClearOptions();
		teamDropdown.AddOptions(teams.Select(t => t.Name).ToList());
		}

	private void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(players.Select(p => p.Name).ToList());
		}

	// --- Additional Functions --- //
	// You can add more utility methods here as needed.
	}
