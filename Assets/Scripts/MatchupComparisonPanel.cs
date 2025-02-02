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

	private void Start()
		{
		// --- Set up button listeners ---
		backButton.onClick.AddListener(OnBackButtonClicked);
		compareButton.onClick.AddListener(OnCompareButtonClicked);

		// --- Refresh dropdowns using DropdownManager (if available) ---
		if (DropdownManager.Instance != null)
			{
			// --- Populate both team dropdowns with team names from the DatabaseManager ---
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

	// --- Handle the back button click ---
	private void OnBackButtonClicked()
		{
		// --- Return to the home panel using UIManager ---
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("MatchupComparisonPanel: Back button clicked, returning to home panel.");
		}

	// --- Handle the compare button click ---
	private void OnCompareButtonClicked()
		{
		// --- Insert your matchup comparison logic here ---
		Debug.Log("MatchupComparisonPanel: Compare button clicked. Implement matchup comparison logic here.");
		}

	#endregion Button Event Handlers

	#region Additional Functions

	// --- Add any extra custom functions for processing matchup comparisons here ---

	#endregion Additional Functions
	}
