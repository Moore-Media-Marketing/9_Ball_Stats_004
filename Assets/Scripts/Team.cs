using UnityEngine;

// Updated the namespace to NickWasHere
namespace NickWasHere
	{
	[System.Serializable]  // System.Serializable attribute should only be applied once
	public class Team
		{
		public string teamName;
		public int skillLevel;
		public Player[] players;

		// Constructor to initialize the team
		public Team(string teamName, int skillLevel, Player[] players)
			{
			this.teamName = teamName;
			this.skillLevel = skillLevel;
			this.players = players;
			}

		// Example method to get the total skill level of all players on the team
		public int GetTotalSkillLevel()
			{
			int totalSkillLevel = 0;
			foreach (var player in players)
				{
				totalSkillLevel += player.skillLevel;
				}
			return totalSkillLevel;
			}
		}
	}
