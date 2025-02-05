using System.Collections.Generic;
using System.IO;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentSeasonDataInputPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	public TMP_Text headerText;

	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField gamesWonInputField;
	public TMP_InputField gamesPlayedInputField;
	public TMP_InputField totalPointsInputField;
	public TMP_InputField ppmInputField;
	public TMP_InputField paPercentInputField;
	public TMP_InputField breakAndRunInputField;
	public TMP_InputField miniSlamsInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField shutoutsInputField;
	public TMP_Dropdown skillLevelDropdown;
	public Button addPlayerButton;
	public Button removePlayerButton;
	public Button saveToCSVButton;
	public Button backButton;
	// --- End Region: UI References --- //

	// --- Region: CSV File Paths --- //
	private string teamCsvFilePath;

	private string playerCsvFilePath;
	private string seasonDataCsvFilePath;
	// --- End Region: CSV File Paths --- //

	// --- Region: Initialization --- //
	private void Start()
		{
		// Define CSV paths
		teamCsvFilePath = Path.Combine(Application.persistentDataPath, "teams.csv");
		playerCsvFilePath = Path.Combine(Application.persistentDataPath, "players.csv");
		seasonDataCsvFilePath = Path.Combine(Application.persistentDataPath, "season_data.csv");

		// Set up button listeners
		addPlayerButton.onClick.AddListener(OnAddPlayer);
		removePlayerButton.onClick.AddListener(OnRemovePlayer);
		saveToCSVButton.onClick.AddListener(OnSaveToCSV);
		backButton.onClick.AddListener(OnBackButton);

		// Initialize UI
		PopulateTeamDropdown();
		PopulateSkillLevelDropdown();
		PopulatePlayerDropdown();
		}

	// --- End Region: Initialization --- //

	// --- Region: Add or Remove Player --- //
	private void OnAddPlayer()
		{
		// Add player to season data CSV
		string selectedTeam = teamNameDropdown.options[teamNameDropdown.value].text;
		string selectedPlayer = playerNameDropdown.options[playerNameDropdown.value].text;
		int gamesWon = int.Parse(gamesWonInputField.text);
		int gamesPlayed = int.Parse(gamesPlayedInputField.text);
		int totalPoints = int.Parse(totalPointsInputField.text);
		float ppm = float.Parse(ppmInputField.text);
		float paPercent = float.Parse(paPercentInputField.text);
		int breakAndRun = int.Parse(breakAndRunInputField.text);
		int miniSlams = int.Parse(miniSlamsInputField.text);
		int nineOnTheSnap = int.Parse(nineOnTheSnapInputField.text);
		int shutouts = int.Parse(shutoutsInputField.text);
		int skillLevel = int.Parse(skillLevelDropdown.options[skillLevelDropdown.value].text);

		// Create new season data record
		PlayerSeasonData newPlayerSeasonData = new()
			{
			TeamName = selectedTeam,
			PlayerName = selectedPlayer,
			GamesWon = gamesWon,
			GamesPlayed = gamesPlayed,
			TotalPoints = totalPoints,
			PPM = ppm,
			PA_Percent = paPercent,
			BreakAndRun = breakAndRun,
			MiniSlams = miniSlams,
			NineOnTheSnap = nineOnTheSnap,
			Shutouts = shutouts,
			SkillLevel = skillLevel
			};

		// Write data to CSV
		List<PlayerSeasonData> seasonData = ReadSeasonDataFromCSV();
		seasonData.Add(newPlayerSeasonData);
		WriteSeasonDataToCSV(seasonData);

		Debug.Log($"Player {selectedPlayer} added to season data.");
		}

	private void OnRemovePlayer()
		{
		// Remove player from season data CSV
		string selectedPlayer = playerNameDropdown.options[playerNameDropdown.value].text;
		List<PlayerSeasonData> seasonData = ReadSeasonDataFromCSV();
		PlayerSeasonData playerToRemove = seasonData.FirstOrDefault(p => p.PlayerName == selectedPlayer);

		if (playerToRemove != null)
			{
			seasonData.Remove(playerToRemove);
			WriteSeasonDataToCSV(seasonData);
			Debug.Log($"Player {selectedPlayer} removed from season data.");
			}
		else
			{
			Debug.LogWarning("Player not found in season data.");
			}
		}

	// --- End Region: Add or Remove Player --- //

	// --- Region: Save to CSV --- //
	private void OnSaveToCSV()
		{
		// Save any changes made to the season data CSV
		List<PlayerSeasonData> seasonData = ReadSeasonDataFromCSV();
		WriteSeasonDataToCSV(seasonData);
		Debug.Log("Season data saved to CSV.");
		}

	// --- End Region: Save to CSV --- //

	// --- Region: Back Button --- //
	private void OnBackButton()
		{
		// Handle the back button (navigate to the previous panel)
		Debug.Log("Back button clicked.");
		}

	// --- End Region: Back Button --- //

	// --- Region: Populate UI Dropdowns --- //
	private void PopulateTeamDropdown()
		{
		// Populate team dropdown from CSV
		List<string> teamNames = ReadTeamsFromCSV();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teamNames);
		}

	private void PopulatePlayerDropdown()
		{
		// Populate player dropdown from CSV
		List<string> playerNames = ReadPlayersFromCSV();
		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(playerNames);
		}

	private void PopulateSkillLevelDropdown()
		{
		// Populate skill level dropdown
		List<string> skillLevels = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
		skillLevelDropdown.ClearOptions();
		skillLevelDropdown.AddOptions(skillLevels);
		}

	// --- End Region: Populate UI Dropdowns --- //

	// --- Region: CSV Helper Methods --- //
	private List<string> ReadTeamsFromCSV()
		{
		List<string> teamNames = new();

		if (File.Exists(teamCsvFilePath))
			{
			var lines = File.ReadAllLines(teamCsvFilePath);
			teamNames = lines.ToList();
			}

		return teamNames;
		}

	private List<string> ReadPlayersFromCSV()
		{
		List<string> playerNames = new();

		if (File.Exists(playerCsvFilePath))
			{
			var lines = File.ReadAllLines(playerCsvFilePath);
			playerNames = lines.ToList();
			}

		return playerNames;
		}

	private List<PlayerSeasonData> ReadSeasonDataFromCSV()
		{
		List<PlayerSeasonData> seasonData = new();

		if (File.Exists(seasonDataCsvFilePath))
			{
			var lines = File.ReadAllLines(seasonDataCsvFilePath);

			foreach (var line in lines)
				{
				var columns = line.Split(',');
				if (columns.Length >= 12)
					{
					seasonData.Add(new PlayerSeasonData
						{
						TeamName = columns[0],
						PlayerName = columns[1],
						GamesWon = int.Parse(columns[2]),
						GamesPlayed = int.Parse(columns[3]),
						TotalPoints = int.Parse(columns[4]),
						PPM = float.Parse(columns[5]),
						PA_Percent = float.Parse(columns[6]),
						BreakAndRun = int.Parse(columns[7]),
						MiniSlams = int.Parse(columns[8]),
						NineOnTheSnap = int.Parse(columns[9]),
						Shutouts = int.Parse(columns[10]),
						SkillLevel = int.Parse(columns[11])
						});
					}
				}
			}

		return seasonData;
		}

	private void WriteSeasonDataToCSV(List<PlayerSeasonData> seasonData)
		{
		List<string> lines = new();

		foreach (var data in seasonData)
			{
			lines.Add($"{data.TeamName},{data.PlayerName},{data.GamesWon},{data.GamesPlayed},{data.TotalPoints},{data.PPM},{data.PA_Percent},{data.BreakAndRun},{data.MiniSlams},{data.NineOnTheSnap},{data.Shutouts},{data.SkillLevel}");
			}

		File.WriteAllLines(seasonDataCsvFilePath, lines);
		}

	// --- End Region: CSV Helper Methods --- //

	// --- Region: Data Classes --- //
	[System.Serializable]
	public class PlayerSeasonData
		{
		public string TeamName;
		public string PlayerName;
		public int GamesWon;
		public int GamesPlayed;
		public int TotalPoints;
		public float PPM;
		public float PA_Percent;
		public int BreakAndRun;
		public int MiniSlams;
		public int NineOnTheSnap;
		public int Shutouts;
		public int SkillLevel;
		}

	// --- End Region: Data Classes --- //
	}