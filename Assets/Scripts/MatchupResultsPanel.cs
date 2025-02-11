using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchupResultsPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TextMeshProUGUI matchupHeaderText;
	public TextMeshProUGUI team1NameText;
	public TextMeshProUGUI team2NameText;
	public TextMeshProUGUI team1WinProbabilityText;
	public TextMeshProUGUI team2WinProbabilityText;
	public Button backButton;
	public Button confirmButton;

	[Header("Panels")]
	public GameObject winOddsPanel;
	public GameObject bestMatchupPanel;

	[Header("Scroll View Components")]
	public Transform matchupListContent;  // The Content object of MatchupListScrollView
	public Transform bestMatchupListContent; // The Content object of BestMatchupListScrollView

	private List<Player> team1Players;
	private List<Player> team2Players;

	public static MatchupResultsPanel Instance;

	private void Awake()
		{
		if (Instance == null) { Instance = this; }
		else { Destroy(gameObject); }
		}

	private void Start()
		{
		backButton.onClick.AddListener(BackToComparison);
		confirmButton.onClick.AddListener(ConfirmMatchup);
		}

	public void DisplayMatchupResults(List<Player> team1, List<Player> team2)
		{
		if (team1 == null || team2 == null || team1.Count == 0 || team2.Count == 0)
			{
			Debug.LogError("Invalid teams. Cannot display matchup results.");
			return;
			}

		team1Players = team1;
		team2Players = team2;

		team1NameText.text = "Team 1: " + team1[0].TeamName;
		team2NameText.text = "Team 2: " + team2[0].TeamName;

		float team1WinProbability = HandicapSystem.CalculateAdjustedWinProbability(team1, team2);
		float team2WinProbability = 1f - team1WinProbability;

		team1WinProbabilityText.text = $"Win Probability: {team1WinProbability * 100f:F2}%";
		team2WinProbabilityText.text = $"Win Probability: {team2WinProbability * 100f:F2}%";

		matchupHeaderText.text = "Matchup Results";

		DisplayWinOddsPanel(team1, team2);
		DisplayBestMatchupPanel(team1, team2);
		}

	private void DisplayWinOddsPanel(List<Player> team1, List<Player> team2)
		{
		foreach (Transform child in matchupListContent)
			{
			Destroy(child.gameObject);
			}

		foreach (var player1 in team1)
			{
			foreach (var player2 in team2)
				{
				GameObject winOddsEntry = new("WinOddsEntry", typeof(RectTransform), typeof(TextMeshProUGUI));
				winOddsEntry.transform.SetParent(matchupListContent, false);

				RectTransform entryRect = winOddsEntry.GetComponent<RectTransform>();
				entryRect.anchorMin = new Vector2(0f, 1f);
				entryRect.anchorMax = new Vector2(1f, 1f);
				entryRect.pivot = new Vector2(0.5f, 1f);
				entryRect.sizeDelta = new Vector2(0, 50);

				float winProbability = HandicapSystem.CalculateWinProbability(player1, player2);

				TextMeshProUGUI text = winOddsEntry.GetComponent<TextMeshProUGUI>();
				text.text = $"{player1.PlayerName} vs {player2.PlayerName}: {winProbability * 100f:F2}%";
				text.fontSize = Mathf.Lerp(20, 30, Screen.width / 1080f);
				text.alignment = TextAlignmentOptions.Center;
				text.textWrappingMode = TextWrappingModes.NoWrap;
				}
			}
		}

	private void DisplayBestMatchupPanel(List<Player> team1, List<Player> team2)
		{
		foreach (Transform child in bestMatchupListContent)
			{
			Destroy(child.gameObject);
			}

		var bestTeam1Players = HandicapSystem.FindOptimalTeamSelection(team1);
		var bestTeam2Players = HandicapSystem.FindOptimalTeamSelection(team2);

		for (int i = 0; i < bestTeam1Players.Count; i++)
			{
			GameObject bestMatchupEntry = new("BestMatchupEntry", typeof(RectTransform), typeof(TextMeshProUGUI));
			bestMatchupEntry.transform.SetParent(bestMatchupListContent, false);

			float winProbability = HandicapSystem.CalculateWinProbability(bestTeam1Players[i], bestTeam2Players[i]);

			var text = bestMatchupEntry.GetComponent<TextMeshProUGUI>();
			text.text = $"Best Matchup: {bestTeam1Players[i].PlayerName} vs {bestTeam2Players[i].PlayerName} - {winProbability * 100f:F2}%";
			text.fontSize = Mathf.Lerp(18, 28, Screen.width / 1080f);
			text.textWrappingMode = TextWrappingModes.NoWrap;
			}
		}

	private void BackToComparison()
		{
		Debug.Log("Returning to Matchup Comparison panel.");
		UIManager.Instance.GoBackToPreviousPanel();
		}

	private void ConfirmMatchup()
		{
		Debug.Log("Matchup confirmed between Team 1 and Team 2.");
		}
	}
