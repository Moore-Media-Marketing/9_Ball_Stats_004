// --- Region: Using Directives --- //
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
// --- End Region: Using Directives --- //

public class MatchupComparisonPanel:MonoBehaviour
	{
	#region UI Elements

	[Tooltip("Header text for the matchup comparison panel.")]
	public TMP_Text headerText;  // --- Displays the panel header ---

	[Tooltip("Instruction text to select Team A.")]
	public TMP_Text selectTeamAText;  // --- Text for selecting Team A ---

	[Tooltip("Dropdown for selecting Team A.")]
	public TMP_Dropdown teamADropdown;  // --- Dropdown for Team A selection ---

	[Tooltip("Scroll view that lists players for Team A.")]
	public ScrollRect teamAPlayerScrollView;  // --- Scroll view for Team A players ---

	[Tooltip("Text label for Team B.")]
	public TMP_Text teamBTeamText;  // --- Label for Team B ---

	[Tooltip("Dropdown for selecting Team B.")]
	public TMP_Dropdown teamBDropdown;  // --- Dropdown for Team B selection ---

	[Tooltip("Scroll view that lists players for Team B.")]
	public ScrollRect teamBPlayerScrollView;  // --- Scroll view for Team B players ---

	[Tooltip("Button to compare the matchups between Team A and Team B.")]
	public Button compareButton;  // --- Button to perform matchup comparison ---

	[Tooltip("Button to go back to the previous panel.")]
	public Button backButton;  // --- Back button for panel navigation ---

	#endregion UI Elements

	#region Unity Methods

	// --- Start method to initialize the panel --- //
	private void Start()
		{
		// --- Set up button listeners --- //
		backButton.onClick.AddListener(OnBackButtonClicked);
		compareButton.onClick.AddListener(OnCompareButtonClicked);

		// --- Refresh dropdowns using DropdownManager (if available) --- //
		if (DropdownManager.Instance != null)
			{
			// --- Populate both team dropdowns with team names from the DatabaseManager --- //
			List<string> teamNames = DatabaseManager.Instance.GetAllTeamNames();
			if (teamNames != null && teamNames.Count > 0)
				{
				DropdownManager.Instance.UpdateDropdown(teamADropdown, teamNames);
				DropdownManager.Instance.UpdateDropdown(teamBDropdown, teamNames);
				}
			else
				{
				Debug.LogWarning("No team names available to populate dropdowns.");
				}
			}
		else
			{
			Debug.LogWarning("DropdownManager instance not found. Ensure it is added to the scene.");
			}
		}

	#endregion Unity Methods

	#region Button Event Handlers

	// --- Handle the back button click --- //
	private void OnBackButtonClicked()
		{
		// --- Return to the home panel using UIManager --- //
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("MatchupComparisonPanel: Back button clicked, returning to home panel.");
		}

	// --- Handle the compare button click --- //
	private void OnCompareButtonClicked()
		{
		// --- Get selected teams and their players from dropdowns --- //
		Team selectedTeamA = GetTeamFromDropdown(teamADropdown);
		Team selectedTeamB = GetTeamFromDropdown(teamBDropdown);

		// --- Validate team compositions before proceeding --- //
		if (ValidateTeamComposition(selectedTeamA) && ValidateTeamComposition(selectedTeamB))
			{
			Debug.Log("Teams are valid, proceeding with comparison.");
			CompareMatchups(selectedTeamA, selectedTeamB);
			}
		else
			{
			Debug.LogWarning("One or both teams have invalid compositions based on the 23-Rule.");
			// Optionally, show an error message to the user.
			}
		}

	#endregion Button Event Handlers

	#region Matchup Comparison Logic

	// --- Get team data from the selected dropdown --- //
	private Team GetTeamFromDropdown(TMP_Dropdown dropdown)
		{
		string selectedTeamName = dropdown.options[dropdown.value].text;
		Team selectedTeam = DatabaseManager.Instance.GetTeamByName(selectedTeamName);
		return selectedTeam;
		}

	// --- Get points required to win based on Skill Level --- //
	private float GetPointsRequiredToWin(float skillLevel)
		{
		return (int) skillLevel switch
			{
				1 => 14,
				2 => 19,
				3 => 25,
				4 => 31,
				5 => 38,
				6 => 46,
				7 => 55,
				8 => 65,
				9 => 75,
				_ => (float) 0,
				};
		}

	// --- Compare matchups between players of Team A and Team B --- //
	private void CompareMatchups(Team teamA, Team teamB)
		{
		// --- Placeholder list to store results --- //
		List<string> results = new();

		// --- Compare each player from Team A with each player from Team B --- //
		foreach (var playerA in teamA.players)
			{
			foreach (var playerB in teamB.players)
				{
				float pointsRequiredA = GetPointsRequiredToWin(playerA.currentSeasonSkillLevel);
				float pointsRequiredB = GetPointsRequiredToWin(playerB.currentSeasonSkillLevel);

				// --- Compare the players based on the points required to win --- //
				if (pointsRequiredA > pointsRequiredB)
					{
					results.Add($"{playerA.name} wins vs {playerB.name}");
					}
				else if (pointsRequiredA < pointsRequiredB)
					{
					results.Add($"{playerB.name} wins vs {playerA.name}");
					}
				else
					{
					results.Add($"{playerA.name} vs {playerB.name} is a tie");
					}
				}
			}

		// --- Display the results in the UI --- //
		DisplayMatchupResults(results);
		}

	#endregion Matchup Comparison Logic

	#region Validation Logic

	// --- Validate team composition based on the 23-Rule --- //
	private bool ValidateTeamComposition(Team team)
		{
		int totalSkillLevel = 0;
		int seniorPlayersCount = 0;

		foreach (var player in team.players)
			{
			totalSkillLevel += (int) player.currentSeasonSkillLevel;
			if (player.currentSeasonSkillLevel >= 6) // Senior players (S/L 6, 7, 8, 9)
				{
				seniorPlayersCount++;
				}
			}

		return totalSkillLevel <= 23 && seniorPlayersCount <= 2;
		}

	#endregion Validation Logic

	#region Display Logic

	// --- Display the results of the matchup comparison --- //
	private void DisplayMatchupResults(List<string> results)
		{
		// --- Update the header --- //
		headerText.text = "Matchup Results";

		// --- Display the comparison results in the UI --- //
		string resultText = string.Join("\n", results);
		// Assuming you have a TextMeshPro text component to display results
		TMP_Text resultsText = teamAPlayerScrollView.content.GetComponentInChildren<TMP_Text>();
		resultsText.text = resultText;
		}

	#endregion Display Logic

	#region Additional Functions

	// --- Add any extra custom functions for processing matchup comparisons here --- //

	#endregion Additional Functions
	}
