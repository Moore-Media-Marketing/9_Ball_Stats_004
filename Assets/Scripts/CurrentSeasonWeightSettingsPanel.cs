using TMPro;

using UnityEngine;

public class CurrentSeasonWeightSettingsPanel:MonoBehaviour
	{
	// --- Region: References --- //
	[Header("Player Weight Settings")]
	[Tooltip("Reference to the PlayerWeightSettings ScriptableObject.")]
	public PlayerWeightSettings weightSettings;

	// UI Input fields for current season weights
	[Header("Current Season Weight Input Fields")]
	[Tooltip("Input field for the number of points awarded in the current season.")]
	public TMP_InputField inputCurrentSeasonPointsAwarded;

	[Tooltip("Input field for the number of matches won in the current season.")]
	public TMP_InputField inputCurrentSeasonMatchesWon;

	[Tooltip("Input field for the current season's defensive shot average.")]
	public TMP_InputField inputCurrentSeasonDefensiveShotAverage;

	[Tooltip("Input field for the current season's skill level.")]
	public TMP_InputField inputCurrentSeasonSkillLevel;

	[Tooltip("Input field for the current season's PPM (Points per Match).")]
	public TMP_InputField inputCurrentSeasonPpm;

	[Tooltip("Input field for the number of shutouts in the current season.")]
	public TMP_InputField inputCurrentSeasonShutouts;

	[Tooltip("Input field for the number of mini slams in the current season.")]
	public TMP_InputField inputCurrentSeasonMiniSlams;

	[Tooltip("Input field for the number of 'Nine On The Snap' occurrences in the current season.")]
	public TMP_InputField inputCurrentSeasonNineOnTheSnap;

	[Tooltip("Input field for the current season's PA (Percentage Accuracy).")]
	public TMP_InputField inputCurrentSeasonPaPercentage;

	[Tooltip("Input field for the number of break and runs in the current season.")]
	public TMP_InputField inputCurrentSeasonBreakAndRun;

	// --- End Region: References --- //

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		// Check if the weightSettings reference is missing
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Current Season panel!");
			return;
			}

		// Load the current season settings into the UI
		LoadSettingsIntoUI();
		}
	// --- End Region: Initialize Panel --- //

	// --- Region: Load Settings into UI --- //
	// Populate the input fields with current season weight values from the PlayerWeightSettings
	private void LoadSettingsIntoUI()
		{
		inputCurrentSeasonPointsAwarded.text = weightSettings.weightCurrentSeasonPointsAwarded.ToString();
		inputCurrentSeasonMatchesWon.text = weightSettings.weightCurrentSeasonMatchesWon.ToString();
		inputCurrentSeasonDefensiveShotAverage.text = weightSettings.weightCurrentSeasonDefensiveShotAverage.ToString();
		inputCurrentSeasonSkillLevel.text = weightSettings.weightCurrentSeasonSkillLevel.ToString();
		inputCurrentSeasonPpm.text = weightSettings.weightCurrentSeasonPpm.ToString();
		inputCurrentSeasonShutouts.text = weightSettings.weightCurrentSeasonShutouts.ToString();
		inputCurrentSeasonMiniSlams.text = weightSettings.weightCurrentSeasonMiniSlams.ToString();
		inputCurrentSeasonNineOnTheSnap.text = weightSettings.weightCurrentSeasonNineOnTheSnap.ToString();
		inputCurrentSeasonPaPercentage.text = weightSettings.weightCurrentSeasonPaPercentage.ToString();
		inputCurrentSeasonBreakAndRun.text = weightSettings.weightCurrentSeasonBreakAndRun.ToString();
		}
	// --- End Region: Load Settings into UI --- //

	// --- Region: Save Settings --- //
	// Called from the Save button's OnClick event.
	public void SaveSettings()
		{
		// Check if the weightSettings reference is missing
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Current Season panel!");
			return;
			}

		// Parse the input fields and save the values to the weightSettings object
		if (float.TryParse(inputCurrentSeasonPointsAwarded.text, out float value))
			weightSettings.weightCurrentSeasonPointsAwarded = value;
		if (float.TryParse(inputCurrentSeasonMatchesWon.text, out value))
			weightSettings.weightCurrentSeasonMatchesWon = value;
		if (float.TryParse(inputCurrentSeasonDefensiveShotAverage.text, out value))
			weightSettings.weightCurrentSeasonDefensiveShotAverage = value;
		if (float.TryParse(inputCurrentSeasonSkillLevel.text, out value))
			weightSettings.weightCurrentSeasonSkillLevel = value;
		if (float.TryParse(inputCurrentSeasonPpm.text, out value))
			weightSettings.weightCurrentSeasonPpm = value;
		if (float.TryParse(inputCurrentSeasonShutouts.text, out value))
			weightSettings.weightCurrentSeasonShutouts = value;
		if (float.TryParse(inputCurrentSeasonMiniSlams.text, out value))
			weightSettings.weightCurrentSeasonMiniSlams = value;
		if (float.TryParse(inputCurrentSeasonNineOnTheSnap.text, out value))
			weightSettings.weightCurrentSeasonNineOnTheSnap = value;
		if (float.TryParse(inputCurrentSeasonPaPercentage.text, out value))
			weightSettings.weightCurrentSeasonPaPercentage = value;
		if (float.TryParse(inputCurrentSeasonBreakAndRun.text, out value))
			weightSettings.weightCurrentSeasonBreakAndRun = value;

		// Log that the settings have been saved
		Debug.Log("Current Season weight settings saved.");
		}
	// --- End Region: Save Settings --- //
	}
