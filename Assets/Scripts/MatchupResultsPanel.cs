using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	public static MatchupResultsPanel Instance { get; private set; }

	[Header("Win Odds ScrollView")]
	public Transform winOddsContent;  // Parent container for Win Odds entries
	public GameObject winOddsEntryPrefab;  // Prefab for displaying win odds

	[Header("Best Matchups ScrollView")]
	public Transform bestMatchupContent;  // Parent container for Best Matchup entries
	public GameObject bestMatchupEntryPrefab;  // Prefab for displaying best matchups

	[Header("Navigation Buttons")]
	public Button backButton;

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

		backButton.onClick.AddListener(GoBack);
		}

	// --- Displays Matchup Results with Win Odds and Best Matchups --- //
	public void DisplayMatchupResults(List<Player> team1Players, List<Player> team2Players)
		{
		if (team1Players == null || team2Players == null)
			{
			Debug.LogError("MatchupResultsPanel: Null team players received.");
			return;
			}

		ClearExistingEntries(); // Clears previous matchup data

		float winOdds = CalculateWinOdds(team1Players, team2Players);
		DisplayWinOdds(winOdds);
		DisplayBestMatchups(team1Players, team2Players);
		}

	// --- Clears previous matchup entries from both scroll views --- //
	private void ClearExistingEntries()
		{
		foreach (Transform child in winOddsContent)
			{
			Destroy(child.gameObject);
			}

		foreach (Transform child in bestMatchupContent)
			{
			Destroy(child.gameObject);
			}
		}

	// --- Calculates the win probability of Team 1 based on players --- //
	private float CalculateWinOdds(List<Player> team1Players, List<Player> team2Players)
		{
		float team1Skill = 0;
		float team2Skill = 0;

		foreach (var player in team1Players)
			{
			team1Skill += player.SkillLevel;
			}

		foreach (var player in team2Players)
			{
			team2Skill += player.SkillLevel;
			}

		float totalSkill = team1Skill + team2Skill;
		return (totalSkill > 0) ? (team1Skill / totalSkill) * 100f : 50f;
		}

	// --- Instantiates a UI entry for Win Odds --- //
	private void DisplayWinOdds(float winOdds)
		{
		GameObject entry = Instantiate(winOddsEntryPrefab, winOddsContent);
		TMP_Text winOddsText = entry.GetComponent<TMP_Text>();

		if (winOddsText != null)
			{
			winOddsText.text = $"Win Probability: {winOdds:F2}%";
			}
		else
			{
			Debug.LogError("WinOddsEntry prefab missing TMP_Text component!");
			}
		}

	// --- Generates and displays best possible matchups for Team 1 to win --- //
	private void DisplayBestMatchups(List<Player> team1Players, List<Player> team2Players)
		{
		List<Player> bestMatchupPlayers = CalculateBestMatchup(team1Players, team2Players);
		GameObject entry = Instantiate(bestMatchupEntryPrefab, bestMatchupContent);
		TMP_Text matchupText = entry.GetComponent<TMP_Text>();

		if (matchupText != null)
			{
			matchupText.text = "Best Matchup: " + string.Join(", ", bestMatchupPlayers.ConvertAll(p => p.PlayerName));
			}
		else
			{
			Debug.LogError("BestMatchupEntry prefab missing TMP_Text component!");
			}
		}

	// --- Determines the best player matchups for Team 1 to maximize win odds --- //
	private List<Player> CalculateBestMatchup(List<Player> team1Players, List<Player> team2Players)
		{
		team1Players.Sort((a, b) => b.SkillLevel.CompareTo(a.SkillLevel)); // Sort by highest skill
		return team1Players.GetRange(0, Mathf.Min(5, team1Players.Count)); // Select top 5 players
		}

	// --- Returns to the previous panel using UIManager --- //
	private void GoBack()
		{
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}
