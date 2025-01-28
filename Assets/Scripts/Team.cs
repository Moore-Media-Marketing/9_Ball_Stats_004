using UnityEngine;

namespace NickWasHere
	{
	[System.Serializable]
	public class Team
		{
		public string teamName;        // Team's name
		public Player[] players;       // Array of players in the team

		// Constructor to initialize the team
		public Team(string teamName, Player[] players)
			{
			this.teamName = teamName;
			this.players = players;
			}

		// Property for team name
		public string Name
			{
			get { return teamName; }
			set { teamName = value; }
			}

		// Method to get the total skill level of all players on the team
		public int GetTotalSkillLevel()
			{
			int totalSkillLevel = 0;
			foreach (var player in players)
				{
				totalSkillLevel += player.skillLevel;
				}
			return totalSkillLevel;
			}

		// Method to get the total games played by all players in the team
		public int GetTotalGamesPlayed()
			{
			int totalGamesPlayed = 0;
			foreach (var player in players)
				{
				totalGamesPlayed += player.gamesPlayed;
				}
			return totalGamesPlayed;
			}

		// Method to get the total games won by all players in the team
		public int GetTotalGamesWon()
			{
			int totalGamesWon = 0;
			foreach (var player in players)
				{
				totalGamesWon += player.gamesWon;
				}
			return totalGamesWon;
			}

		// Method to calculate the average win percentage of the team
		public float GetWinPercentage()
			{
			float totalWinPercentage = 0f;
			foreach (var player in players)
				{
				totalWinPercentage += player.WinPercentage;
				}
			return players.Length > 0 ? totalWinPercentage / players.Length : 0f;
			}
		}
	}
