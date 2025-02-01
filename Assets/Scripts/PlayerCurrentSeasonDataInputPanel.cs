using TMPro;

using UnityEngine;

public class PlayerCurrentSeasonDataInputPanel:MonoBehaviour
	{
	#region Fields and UI Elements

	[Header("Player Data")]
	[Tooltip("Reference to the currently selected player.")]
	public Player selectedPlayer;  // --- The player whose data is displayed ---

	[Header("UI Elements")]
	[Tooltip("Text element displaying current season total points.")]
	public TextMeshProUGUI currentSeasonTotalPointsText;  // --- UI for total points ---

	[Tooltip("Text element displaying current season points per match.")]
	public TextMeshProUGUI currentSeasonPpmText;  // --- UI for points per match ---

	[Tooltip("Text element displaying current season accuracy percentage.")]
	public TextMeshProUGUI currentSeasonPaPercentageText;  // --- UI for accuracy percentage ---

	[Tooltip("Text element displaying current season break and run stats.")]
	public TextMeshProUGUI currentSeasonBreakAndRunText;  // --- UI for break and run stats ---

	[Tooltip("Text element displaying current season skill level.")]
	public TextMeshProUGUI currentSeasonSkillLevelText;  // --- UI for skill level ---

	#endregion Fields and UI Elements

	#region Unity Methods

	private void Start()
		{
		// --- Update the UI with the player's current season data on start ---
		UpdatePlayerData();
		}

	#endregion Unity Methods

	#region Data Update Methods

	// --- Update the UI with the selected player's current season data --- //
	private void UpdatePlayerData()
		{
		if (selectedPlayer != null)
			{
			currentSeasonTotalPointsText.text = selectedPlayer.CurrentSeasonTotalPoints.ToString();
			currentSeasonPpmText.text = selectedPlayer.CurrentSeasonPpm.ToString();
			currentSeasonPaPercentageText.text = selectedPlayer.CurrentSeasonPaPercentage.ToString();
			currentSeasonBreakAndRunText.text = selectedPlayer.CurrentSeasonBreakAndRun.ToString();
			currentSeasonSkillLevelText.text = selectedPlayer.CurrentSeasonSkillLevel.ToString(); // Fixed line
			}
		else
			{
			Debug.LogWarning("No player selected! Please select a player to display their data.");
			}
		}

	#endregion Data Update Methods

	#region Additional Functions

	// --- Additional custom functions for data input can be added here --- //

	#endregion Additional Functions
	}
