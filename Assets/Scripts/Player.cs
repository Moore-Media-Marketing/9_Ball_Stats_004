using UnityEngine;

namespace NickWasHere
	{
	[System.Serializable]
	public class Player
		{
		public string name;           // Player's name
		public int skillLevel;        // Player's skill level
		public int gamesPlayed;       // Total games played by the player
		public int gamesWon;          // Total games won by the player
		public float ppm;             // Points per match
		public float pa;              // Points awarded
		public float WinPercentage => CalculateWinPercentage();  // Win percentage

		// Constructor to initialize the player
		public Player(string name, int skillLevel)
			{
			this.name = name;
			this.skillLevel = skillLevel;
			this.gamesPlayed = 0;
			this.gamesWon = 0;
			this.ppm = 0;
			this.pa = 0;
			}

		// Method to calculate win percentage
		private float CalculateWinPercentage()
			{
			if (gamesPlayed == 0) return 0f;
			return (float) gamesWon / gamesPlayed * 100f;
			}

		// Method to update match stats
		public void UpdateStats(bool won, float pointsPerMatch, float pointsAwarded)
			{
			gamesPlayed++;
			if (won) gamesWon++;
			ppm = pointsPerMatch;
			pa = pointsAwarded;
			}
		}
	}
