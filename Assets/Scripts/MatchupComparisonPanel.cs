using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

// --- MatchupComparisonPanel Class --- //
public class MatchupComparisonPanel:MonoBehaviour
	{
	// --- Panel References --- //
	public TMP_Text headerText;
	public TMP_Text selectTeamAText;
	public TMP_Dropdown teamADropdown;
	public GameObject teamAPlayerScrollView;
	public TMP_Text selectTeamBText;
	public TMP_Dropdown teamBDropdown;
	public GameObject teamBPlayerScrollView;
	public TMP_Text compareButtonText;
	public TMP_Text backButtonText;

	// --- Player Toggle Prefab Reference --- //
	public GameObject playerTogglePrefab;

	// --- Matchup Result Panel --- //
	public GameObject matchupResultPanel;
	public TMP_Text matchupResultText;

	// --- Cached Team Data --- //
	private List<Player> players;

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
		players = DatabaseManager.Instance.LoadPlayers(); // Load all players from the CSV
		if (players == null || players.Count == 0)
			{
			Debug.LogError("No players found!");
			return;
			}

		// Get a list of unique team names from the players list
		List<string> teamNames = players.Select(p => p.TeamName).Distinct().ToList();

		teamADropdown.ClearOptions();
		teamADropdown.AddOptions(teamNames);

		teamBDropdown.ClearOptions();
		teamBDropdown.AddOptions(teamNames);
		}

	// --- Select Team A --- //
	public void OnSelectTeamA()
		{
		int selectedTeamId = GetSelectedTeamId(teamADropdown.value);
		List<Player> teamAPlayers = LoadPlayersByTeamId(selectedTeamId);
		CreatePlayerToggles(teamAPlayerScrollView, teamAPlayers);
		}

	// --- Select Team B --- //
	public void OnSelectTeamB()
		{
		int selectedTeamId = GetSelectedTeamId(teamBDropdown.value);
		List<Player> teamBPlayers = LoadPlayersByTeamId(selectedTeamId);
		CreatePlayerToggles(teamBPlayerScrollView, teamBPlayers);
		}

	// --- Load Players by Team ID --- //
	private List<Player> LoadPlayersByTeamId(int teamId)
		{
		// Filter players by team ID from the loaded data
		return players.Where(p => p.TeamId == teamId).ToList();
		}

	// --- Create Player Toggles --- //
	private void CreatePlayerToggles(GameObject playerScrollView, List<Player> players)
		{
		if (players == null || players.Count == 0)
			{
			Debug.LogWarning("No players found for the selected team.");
			return;
			}

		// Clear existing player toggles
		foreach (Transform child in playerScrollView.transform)
			{
			Destroy(child.gameObject);
			}

		// Create new toggles
		foreach (Player player in players)
			{
			GameObject toggleObject = Instantiate(playerTogglePrefab, playerScrollView.transform);
			TMP_Text playerNameText = toggleObject.GetComponentInChildren<TMP_Text>();

			if (playerNameText != null)
				{
				playerNameText.text = player.PlayerName;
				}
			else
				{
				Debug.LogError("Player toggle prefab is missing a TMP_Text component.");
				}
			}
		}

	// --- Compare Teams --- //
	public void CompareTeams()
		{
		List<Player> teamAPlayers = LoadPlayersByTeamId(GetSelectedTeamId(teamADropdown.value));
		List<Player> teamBPlayers = LoadPlayersByTeamId(GetSelectedTeamId(teamBDropdown.value));

		matchupResultPanel.SetActive(true);

		int teamAScore = CalculateTotalScore(teamAPlayers);
		int teamBScore = CalculateTotalScore(teamBPlayers);

		matchupResultText.text = $"Team A Score: {teamAScore}\nTeam B Score: {teamBScore}";
		}

	// --- Calculate Total Score --- //
	private int CalculateTotalScore(List<Player> players)
		{
		if (players == null || players.Count == 0)
			{
			return 0;
			}

		int totalScore = 0;
		foreach (Player player in players)
			{
			totalScore += player.CurrentSeasonPointsAwarded;
			}
		return totalScore;
		}

	// --- Get Selected Team ID --- //
	private int GetSelectedTeamId(int dropdownIndex)
		{
		// Retrieve the team ID based on the dropdown index
		string selectedTeamName = teamADropdown.options[dropdownIndex].text;
		Player selectedPlayer = players.FirstOrDefault(p => p.TeamName == selectedTeamName);

		if (selectedPlayer != null)
			{
			return selectedPlayer.TeamId;
			}

		Debug.LogError("Invalid team selection.");
		return -1;
		}
	}
