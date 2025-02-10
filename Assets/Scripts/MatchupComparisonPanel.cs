using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupComparisonPanel:MonoBehaviour
	{
	public static MatchupComparisonPanel Instance { get; private set; }

	[Header("Team Selection UI")]
	public TMP_Dropdown team1Dropdown;
	public TMP_Dropdown team2Dropdown;
	public Button compareButton;
	public Button backButton;

	[Header("Player Selection Panels")]
	public GameObject team1Panel;
	public GameObject team2Panel;

	private Toggle[] team1PlayerToggles;
	private Text[] team1PlayerLabels;
	private Toggle[] team2PlayerToggles;
	private Text[] team2PlayerLabels;

	private List<Team> teams;
	private List<Player> team1Players;
	private List<Player> team2Players;

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

		FindTogglesAndLabels(); // Auto-find toggles & labels at runtime
		}

	private void Start()
		{
		LoadTeams();
		compareButton.onClick.AddListener(CompareTeams);
		backButton.onClick.AddListener(GoBack); // Link back button to UIManager
		}

	// --- Finds toggles and labels dynamically under Team1Panel and Team2Panel. --- //

	private void FindTogglesAndLabels()
		{
		// Find toggles and labels inside Team1Panel
		team1PlayerToggles = team1Panel.GetComponentsInChildren<Toggle>(true);
		team1PlayerLabels = new Text[team1PlayerToggles.Length];

		for (int i = 0; i < team1PlayerToggles.Length; i++)
			{
			team1PlayerLabels[i] = team1PlayerToggles[i].GetComponentInChildren<Text>();
			}

		// Find toggles and labels inside Team2Panel
		team2PlayerToggles = team2Panel.GetComponentsInChildren<Toggle>(true);
		team2PlayerLabels = new Text[team2PlayerToggles.Length];

		for (int i = 0; i < team2PlayerToggles.Length; i++)
			{
			team2PlayerLabels[i] = team2PlayerToggles[i].GetComponentInChildren<Text>();
			}

		Debug.Log($"Found {team1PlayerToggles.Length} toggles in Team1Panel and {team2PlayerToggles.Length} in Team2Panel.");
		}

	// --- Loads teams into TMP_Dropdowns from the DatabaseManager. --- //

	private void LoadTeams()
		{
		teams = DatabaseManager.Instance.GetAllTeams();
		team1Dropdown.ClearOptions();
		team2Dropdown.ClearOptions();

		List<string> teamNames = new List<string>();
		foreach (var team in teams)
			{
			teamNames.Add(team.TeamName);
			}

		team1Dropdown.AddOptions(teamNames);
		team2Dropdown.AddOptions(teamNames);

		team1Dropdown.onValueChanged.AddListener(delegate { PopulatePlayers(1); });
		team2Dropdown.onValueChanged.AddListener(delegate { PopulatePlayers(2); });

		// Populate players for default selections
		PopulatePlayers(1);
		PopulatePlayers(2);
		}

	// --- Populates the player toggles for the selected team. --- //

	private void PopulatePlayers(int teamNumber)
		{
		int selectedTeamIndex = (teamNumber == 1) ? team1Dropdown.value : team2Dropdown.value;
		int selectedTeamId = teams[selectedTeamIndex].TeamId;
		List<Player> selectedPlayers = DatabaseManager.Instance.GetAllPlayers().FindAll(p => p.TeamId == selectedTeamId);

		if (teamNumber == 1) team1Players = selectedPlayers;
		else team2Players = selectedPlayers;

		Toggle[] playerToggles = (teamNumber == 1) ? team1PlayerToggles : team2PlayerToggles;
		Text[] playerLabels = (teamNumber == 1) ? team1PlayerLabels : team2PlayerLabels;

		int maxToggles = playerToggles.Length;

		for (int i = 0; i < maxToggles; i++)
			{
			if (i < selectedPlayers.Count)
				{
				playerLabels[i].text = selectedPlayers[i].PlayerName;
				}
			else
				{
				playerLabels[i].text = "";
				}

			// Set all toggles to active
			playerToggles[i].gameObject.SetActive(true);
			playerToggles[i].isOn = true;  // You can set this to true if you want them to be pre-selected
			}
		}


	// --- Gathers selected players and sends them to MatchupResultsPanel. --- //

	private void CompareTeams()
		{
		List<Player> selectedTeam1Players = new List<Player>();
		List<Player> selectedTeam2Players = new List<Player>();

		// Debugging: Check if the players lists are null or empty
		if (team1Players == null || team2Players == null)
			{
			Debug.LogError("team1Players or team2Players is null.");
			return;
			}

		// Ensure that toggles are properly initialized and non-null
		if (team1PlayerToggles == null || team2PlayerToggles == null)
			{
			Debug.LogError("Player toggles are not initialized.");
			return;
			}

		// Debugging: Log the number of players
		Debug.Log($"Team 1 has {team1Players.Count} players, Team 2 has {team2Players.Count} players.");

		// Check if there are any toggles for the teams
		if (team1PlayerToggles.Length == 0 || team2PlayerToggles.Length == 0)
			{
			Debug.LogError("No player toggles found for one of the teams.");
			return;
			}

		// Selecting players for Team 1
		for (int i = 0; i < team1PlayerToggles.Length; i++)
			{
			if (team1PlayerToggles[i].gameObject.activeSelf && team1PlayerToggles[i].isOn)
				{
				if (i < team1Players.Count)
					{
					selectedTeam1Players.Add(team1Players[i]);
					}
				else
					{
					Debug.LogError($"Player toggle {i} is active, but no corresponding player in Team 1.");
					}
				}
			}

		// Selecting players for Team 2
		for (int i = 0; i < team2PlayerToggles.Length; i++)
			{
			if (team2PlayerToggles[i].gameObject.activeSelf && team2PlayerToggles[i].isOn)
				{
				if (i < team2Players.Count)
					{
					selectedTeam2Players.Add(team2Players[i]);
					}
				else
					{
					Debug.LogError($"Player toggle {i} is active, but no corresponding player in Team 2.");
					}
				}
			}

		// Debugging: Log selected players
		Debug.Log($"Selected {selectedTeam1Players.Count} players for Team 1 and {selectedTeam2Players.Count} players for Team 2.");

		if (selectedTeam1Players.Count == 0 || selectedTeam2Players.Count == 0)
			{
			Debug.LogWarning("Both teams must have at least one selected player.");
			return;
			}

		// Send selected players to MatchupResultsPanel
		MatchupResultsPanel.Instance.DisplayMatchupResults(selectedTeam1Players, selectedTeam2Players);

		// Switch UI to results panel using UIManager
		UIManager.Instance.ShowMatchupResultsPanel();
		}


	// --- Returns to the previous panel using UIManager. --- //

	private void GoBack()
		{
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}
