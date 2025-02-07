using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// --- Region: MatchupComparisonPanel --- //
public class MatchupComparisonPanel:MonoBehaviour
	{
	// --- Panel References --- //
	public TMP_Text headerText;
	public TMP_Text selectTeamAText;
	public TMP_Dropdown teamADropdown;
	public GameObject teamAPlayerScrollView;
	public TMP_Text teamBTeamText;
	public TMP_Dropdown teamBDropdown;
	public GameObject teamBPlayerScrollView;
	public TMP_Text compareButtonText;
	public TMP_Text backButtonText;
	// --- End Region: Panel References --- //

	// --- Player Toggle Prefab Reference --- //
	public GameObject playerTogglePrefab;

	// --- Matchup Result Panel --- //
	public GameObject matchupResultPanel;
	public TMP_Text matchupResultText;

	// --- Initialization --- //
	private void Start()
		{
		PopulateTeamDropdowns();
		compareButtonText.text = "Compare";
		backButtonText.text = "Back";
		}

	// --- Populate Team Dropdowns --- //
	private void PopulateTeamDropdowns()
		{
		List<Team> teams = DatabaseManager.Instance.LoadTeams();
		List<string> teamNames = new List<string>();

		if (teams == null || teams.Count == 0)
			{
			Debug.LogError("No teams found!");
			return;
			}

		foreach (var team in teams)
			{
			Debug.Log($"Loaded Team: {team.TeamName}");
			teamNames.Add(team.TeamName);
			}

		teamADropdown.ClearOptions();
		teamADropdown.AddOptions(teamNames);

		teamBDropdown.ClearOptions();
		teamBDropdown.AddOptions(teamNames);
		}

	// --- Select Team A --- //
	public void OnSelectTeamA()
		{
		int selectedTeamId = teamADropdown.value + 1; // Assuming 1-based ID for teams
		List<Player> teamAPlayers = DatabaseManager.Instance.LoadPlayersFromCsv(selectedTeamId);
		CreatePlayerToggles(teamAPlayerScrollView, teamAPlayers);
		}

	// --- Select Team B --- //
	public void OnSelectTeamB()
		{
		int selectedTeamId = teamBDropdown.value + 1; // Assuming 1-based ID for teams
		List<Player> teamBPlayers = DatabaseManager.Instance.LoadPlayersFromCsv(selectedTeamId);
		CreatePlayerToggles(teamBPlayerScrollView, teamBPlayers);
		}

	// --- Create Player Toggles --- //
	private void CreatePlayerToggles(GameObject playerScrollView, List<Player> players)
		{
		foreach (Transform child in playerScrollView.transform)
			{
			Destroy(child.gameObject); // Remove any existing toggles
			}

		foreach (Player player in players)
			{
			GameObject toggleObject = Instantiate(playerTogglePrefab, playerScrollView.transform);
			TMP_Text playerNameText = toggleObject.GetComponentInChildren<TMP_Text>();
			playerNameText.text = player.PlayerName;
			// Add any additional toggle logic here
			}
		}

	// --- Compare Teams --- //
	public void CompareTeams()
		{
		List<Player> teamAPlayers = DatabaseManager.Instance.LoadPlayersFromCsv(teamADropdown.value + 1);
		List<Player> teamBPlayers = DatabaseManager.Instance.LoadPlayersFromCsv(teamBDropdown.value + 1);
		matchupResultPanel.SetActive(true);

		int teamAScore = CalculateTotalScore(teamAPlayers);
		int teamBScore = CalculateTotalScore(teamBPlayers);

		matchupResultText.text = $"Team A Score: {teamAScore}\nTeam B Score: {teamBScore}";
		}

	// --- Calculate Total Score --- //
	private int CalculateTotalScore(List<Player> players)
		{
		int totalScore = 0;
		foreach (Player player in players)
			{
			totalScore += player.CurrentSeasonPointsAwarded;
			}
		return totalScore;
		}
	}
// --- End Region: MatchupComparisonPanel --- //
