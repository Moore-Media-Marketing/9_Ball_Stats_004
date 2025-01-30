using TMPro; // Added to support TextMeshPro

using UnityEngine;

public class ComparisonResult:MonoBehaviour
	{
	[Header("Result Panel")]
	[Tooltip("Text component to display the result")]
	public TMP_Text resultText; // Changed to TMP_Text for TextMeshPro support

	[Tooltip("Text component to display home team details")]
	public TMP_Text homeTeamText; // Changed to TMP_Text for TextMeshPro support

	[Tooltip("Text component to display away team details")]
	public TMP_Text awayTeamText; // Changed to TMP_Text for TextMeshPro support

	// --- Display Comparison Results ---
	public void DisplayResult(Team homeTeam, Team awayTeam, Team winner)
		{
		homeTeamText.text = "Home Team: " + homeTeam.TeamName;
		awayTeamText.text = "Away Team: " + awayTeam.TeamName;

		if (winner != null)
			{
			resultText.text = winner.TeamName + " Wins!";
			}
		else
			{
			resultText.text = "It's a Draw!";
			}
		}
	}