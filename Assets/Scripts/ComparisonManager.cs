using UnityEngine;

// --- Region: ComparisonManager --- //
public class ComparisonManager:MonoBehaviour
	{
	// Reference to the PlayerWeightSettings scriptable object
	public PlayerWeightSettings weightSettings;

	// --- Region: Compare Players --- //
	// Method to compare two players
	public void ComparePlayers(Player player1, Player player2)
		{
		if (player1 == null || player2 == null)
			{
			Debug.LogError("One or both players are null.");
			return;
			}

		// Calculate overall scores for both players
		float player1Score = player1.CalculateOverallScore(weightSettings);
		float player2Score = player2.CalculateOverallScore(weightSettings);

		// Compare the scores and display the result
		if (player1Score > player2Score)
			{
			Debug.Log($"{player1.PlayerName} is the better player with a score of {player1Score} compared to {player2.PlayerName} with a score of {player2Score}.");
			}
		else if (player2Score > player1Score)
			{
			Debug.Log($"{player2.PlayerName} is the better player with a score of {player2Score} compared to {player1.PlayerName} with a score of {player1Score}.");
			}
		else
			{
			Debug.Log($"Both players, {player1.PlayerName} and {player2.PlayerName}, have identical scores of {player1Score}.");
			}
		}

	// --- End Region: Compare Players --- //

	// --- Region: Simulate Matchup --- //
	// Method to simulate a matchup between two players and return the winner
	public Player SimulateMatchup(Player player1, Player player2)
		{
		if (player1 == null || player2 == null)
			{
			Debug.LogError("One or both players are null.");
			return null;
			}

		// Calculate overall scores for both players
		float player1Score = player1.CalculateOverallScore(weightSettings);
		float player2Score = player2.CalculateOverallScore(weightSettings);

		// Return the player with the higher score as the winner
		if (player1Score > player2Score)
			{
			return player1;
			}
		else if (player2Score > player1Score)
			{
			return player2;
			}
		else
			{
			Debug.Log($"It's a draw! Both players have identical scores of {player1Score}.");
			return null;  // It's a draw if scores are the same
			}
		}

	// --- End Region: Simulate Matchup --- //

	// --- Region: Display Player Score --- //
	// Helper method to display a player's overall score
	public void DisplayPlayerScore(Player player)
		{
		if (player == null)
			{
			Debug.LogError("Player is null.");
			return;
			}

		float score = player.CalculateOverallScore(weightSettings);
		Debug.Log($"{player.PlayerName} has an overall score of {score}.");
		}

	// --- End Region: Display Player Score --- //
	}

// --- End Region: ComparisonManager --- //