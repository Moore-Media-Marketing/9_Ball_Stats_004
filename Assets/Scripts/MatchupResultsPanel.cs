using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchupResultsPanel:MonoBehaviour
	{
	public static MatchupResultsPanel Instance;

	[Header("UI Elements")]
	public TMP_Text teamAWinProbabilityText;
	public TMP_Text teamBWinProbabilityText;
	public Transform matchupListContainer;
	public Transform bestMatchupListContainer;
	public GameObject matchupEntryPrefab;
	public Button backButton;

	private void Awake()
		{
		Instance = this;
		}

	/// <summary>
	/// Displays matchup results based on selected teams.
	/// </summary>
	public void DisplayMatchupResults(List<Player> teamAPlayers, List<Player> teamBPlayers)
		{
		float winChanceA = HandicapSystem.CalculateWinProbability(teamAPlayers, teamBPlayers);
		float winChanceB = 1 - winChanceA;

		teamAWinProbabilityText.text = $"Win Probability: {winChanceA * 100:F1}%";
		teamBWinProbabilityText.text = $"Win Probability: {winChanceB * 100:F1}%";

		PopulateMatchupList(teamAPlayers, teamBPlayers);
		gameObject.SetActive(true);
		}

	/// <summary>
	/// Populates the list of matchups and identifies the best matchup.
	/// </summary>
	private void PopulateMatchupList(List<Player> teamAPlayers, List<Player> teamBPlayers)
		{
		foreach (Transform child in matchupListContainer) Destroy(child.gameObject);
		foreach (Transform child in bestMatchupListContainer) Destroy(child.gameObject);

		foreach (var playerA in teamAPlayers)
			{
			foreach (var playerB in teamBPlayers)
				{
				float matchupOdds = HandicapSystem.CalculateWinProbability(new List<Player> { playerA }, new List<Player> { playerB });
				GameObject matchupEntry = Instantiate(matchupEntryPrefab, matchupListContainer);
				matchupEntry.GetComponent<TMP_Text>().text = $"{playerA.PlayerName} vs {playerB.PlayerName}: {matchupOdds * 100:F1}%";
				}
			}
		}
	}
