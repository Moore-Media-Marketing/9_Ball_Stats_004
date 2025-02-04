using UnityEngine;
using TMPro;
using SQLite;
using System.Collections.Generic;

public class MatchupManager:MonoBehaviour
	{
	#region UI References

	[Header("Matchup UI Elements")]
	[Tooltip("Panel that displays matchup results.")]
	public GameObject matchupPanel;

	[Tooltip("Template for matchup entries in the scroll view.")]
	public GameObject matchupEntryTemplate;

	[Tooltip("Parent object for matchup entries.")]
	public Transform matchupListContent;

	[Tooltip("Parent object for best matchup entries.")]
	public Transform bestMatchupListContent;

	[Tooltip("Text element for the best matchup header.")]
	public TMP_Text bestMatchupHeader;

	#endregion

	#region Database Fields

	// SQLite connection for storing matchup data
	private SQLiteConnection dbConnection;

	#endregion

	#region Methods

	// --- Initialize SQLite connection --- //
	private void Start()
		{
		dbConnection = new SQLiteConnection("YourDatabasePath");
		dbConnection.CreateTable<Matchup>();  // Create table for Matchup
		}

	// --- Compares two selected teams and generates matchup results --- //
	public void CompareTeams(int teamId1, int teamId2)
		{
		// --- Ensure panel is active --- //
		matchupPanel.SetActive(true);

		// --- Clear previous matchup results --- //
		foreach (Transform child in matchupListContent)
			{
			Destroy(child.gameObject);
			}

		foreach (Transform child in bestMatchupListContent)
			{
			Destroy(child.gameObject);
			}

		// --- Generate matchups (Placeholder for actual logic) --- //
		Debug.Log("Comparing teams and generating matchups...");

		// Example: Retrieve teams and create matchups (replace with actual logic)
		List<Player> team1Players = GetPlayersByTeam(teamId1);
		List<Player> team2Players = GetPlayersByTeam(teamId2);

		foreach (var player1 in team1Players)
			{
			foreach (var player2 in team2Players)
				{
				// Add matchup entry in the list (for display)
				CreateMatchupEntry(player1, player2);

				// Save the matchup to the database
				SaveMatchup(player1.Id, player2.Id);
				}
			}

		// --- Update best matchup content if necessary --- //
		bestMatchupHeader.text = "Best Matchup"; // Placeholder
		}

	// --- Create a matchup entry in the UI --- //
	private void CreateMatchupEntry(Player player1, Player player2)
		{
		GameObject entry = Instantiate(matchupEntryTemplate, matchupListContent);
		TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();

		texts[0].text = $"{player1.Name} vs {player2.Name}";
		texts[1].text = $"{player1.SkillLevel} vs {player2.SkillLevel}";
		}

	// --- Saves matchup to the SQLite database --- //
	private void SaveMatchup(int player1Id, int player2Id)
		{
		Matchup matchup = new Matchup
			{
			Player1Id = player1Id,
			Player2Id = player2Id,
			DateTime = System.DateTime.Now
			};

		dbConnection.Insert(matchup);
		}

	// --- Retrieves players from a team (teamId) --- //
	private List<Player> GetPlayersByTeam(int teamId)
		{
		return dbConnection.Table<Player>().Where(p => p.TeamId == teamId).ToList();
		}

	// --- Closes the matchup results panel --- //
	public void CloseMatchupPanel()
		{
		matchupPanel.SetActive(false);
		}

	#endregion
	}

#region Database Model

// --- Matchup class to store matchup data in SQLite --- //
public class Matchup
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public int Player1Id { get; set; }
	public int Player2Id { get; set; }
	public System.DateTime DateTime { get; set; }
	}

#endregion
