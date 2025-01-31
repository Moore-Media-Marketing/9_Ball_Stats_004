using TMPro;

using UnityEngine;

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

	#endregion UI References

	#region Methods

	// --- Compares two selected teams and generates matchup results --- //
	public void CompareTeams()
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
		}

	// --- Closes the matchup results panel --- //
	public void CloseMatchupPanel()
		{
		matchupPanel.SetActive(false);
		}

	#endregion Methods
	}