using UnityEngine;

public class PlayerStats:MonoBehaviour
	{
	[Header("Player Stats")]
	[Tooltip("The player's current skill level (1-9)")]
	public int skillLevel;

	[Tooltip("The number of games the player has played")]
	public int gamesPlayed;

	[Tooltip("The number of games the player has won")]
	public int gamesWon;

	[Tooltip("The player's win percentage")]
	public float winPercentage;

	// --- Update Win Percentage Method ---
	public void UpdateWinPercentage()
		{
		if (gamesPlayed > 0)
			{
			winPercentage = (float) gamesWon / gamesPlayed * 100;
			}
		else
			{
			winPercentage = 0;
			}
		}

	// --- Record Win Method ---
	public void RecordWin()
		{
		gamesWon++;
		gamesPlayed++;
		UpdateWinPercentage();
		}

	// --- Record Loss Method ---
	public void RecordLoss()
		{
		gamesPlayed++;
		UpdateWinPercentage();
		}
	}