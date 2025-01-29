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

		// Constructor to initialize the player with name, skill level, games played, and games won
		public Player(string name, int skillLevel, int gamesPlayed, int gamesWon)
			{
			this.name = name;
			this.skillLevel = skillLevel;
			this.gamesPlayed = gamesPlayed;
			this.gamesWon = gamesWon;
			this.ppm = 0;
			this.pa = 0;
			}

		// Method to calculate win percentage
		private float CalculateWinPercentage()
			{
			if (this.gamesPlayed == 0) return 0f;  // Use 'this' for clarity
			return (float) this.gamesWon / this.gamesPlayed * 100f;  // Use 'this' for clarity
			}

		// Method to update match stats
		public void UpdateStats(bool won, float pointsPerMatch, float pointsAwarded)
			{
			this.gamesPlayed++;  // Use 'this' for clarity
			if (won) this.gamesWon++;  // Use 'this' for clarity
			this.ppm = pointsPerMatch;
			this.pa = pointsAwarded;
			}
		}
	}
