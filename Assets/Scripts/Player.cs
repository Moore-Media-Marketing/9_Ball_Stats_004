using UnityEngine;

// Updated the namespace to NickWasHere
namespace NickWasHere
	{
	[System.Serializable]  // System.Serializable attribute should only be applied once
	public class Player
		{
		public string name;
		public int skillLevel;
		public int gamesPlayed;
		public int gamesWon;
		public float ppm;  // Points per match
		public float pa;   // Points awarded
		public float winPercentage => CalculateWinPercentage();

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

		// Example method to update match stats
		public void UpdateStats(bool won, float pointsPerMatch, float pointsAwarded)
			{
			gamesPlayed++;
			if (won) gamesWon++;
			ppm = pointsPerMatch;
			pa = pointsAwarded;
			}
		}
	}
