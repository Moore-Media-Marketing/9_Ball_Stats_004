using TMPro;

using UnityEngine;

public class LifetimeWeightSettingsPanel:MonoBehaviour
	{
	// --- Region: References --- //
	[Header("Player Weight Settings")]
	[Tooltip("Reference to the PlayerWeightSettings ScriptableObject.")]
	public PlayerWeightSettings weightSettings;

	// UI Input fields for lifetime weights
	[Header("Lifetime Weight Input Fields")]
	[Tooltip("Input field for the number of lifetime games won.")]
	public TMP_InputField inputLifetimeGamesWon;

	[Tooltip("Input field for the number of lifetime mini slams.")]
	public TMP_InputField inputLifetimeMiniSlams;

	[Tooltip("Input field for the number of lifetime 'Nine On The Snap' occurrences.")]
	public TMP_InputField inputLifetimeNineOnTheSnap;

	[Tooltip("Input field for the number of lifetime shutouts.")]
	public TMP_InputField inputLifetimeShutouts;

	[Tooltip("Input field for the number of lifetime break and runs.")]
	public TMP_InputField inputLifetimeBreakAndRun;

	[Tooltip("Input field for the average lifetime defensive shot.")]
	public TMP_InputField inputLifetimeDefensiveShotAverage;

	[Tooltip("Input field for the total lifetime matches played.")]
	public TMP_InputField inputLifetimeMatchesPlayed;

	[Tooltip("Input field for the total lifetime matches won.")]
	public TMP_InputField inputLifetimeMatchesWon;

	// --- End Region: References --- //

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		// Check if the weightSettings reference is missing
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Lifetime panel!");
			return;
			}

		// Load the settings into the UI
		LoadSettingsIntoUI();
		}
	// --- End Region: Initialize Panel --- //

	// --- Region: Load Settings into UI --- //
	// Populate the input fields with lifetime weight values from the PlayerWeightSettings
	private void LoadSettingsIntoUI()
		{
		inputLifetimeGamesWon.text = weightSettings.weightLifetimeGamesWon.ToString();
		inputLifetimeMiniSlams.text = weightSettings.weightLifetimeMiniSlams.ToString();
		inputLifetimeNineOnTheSnap.text = weightSettings.weightLifetimeNineOnTheSnap.ToString();
		inputLifetimeShutouts.text = weightSettings.weightLifetimeShutouts.ToString();
		inputLifetimeBreakAndRun.text = weightSettings.weightLifetimeBreakAndRun.ToString();
		inputLifetimeDefensiveShotAverage.text = weightSettings.weightLifetimeDefensiveShotAverage.ToString();
		inputLifetimeMatchesPlayed.text = weightSettings.weightLifetimeMatchesPlayed.ToString();
		inputLifetimeMatchesWon.text = weightSettings.weightLifetimeMatchesWon.ToString();
		}
	// --- End Region: Load Settings into UI --- //

	// --- Region: Save Settings --- //
	// Called from the Save button's OnClick event.
	public void SaveSettings()
		{
		// Check if the weightSettings reference is missing
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Lifetime panel!");
			return;
			}

		// Parse the input fields and save values to the weightSettings object
		if (float.TryParse(inputLifetimeGamesWon.text, out float value))
			weightSettings.weightLifetimeGamesWon = value;
		if (float.TryParse(inputLifetimeMiniSlams.text, out value))
			weightSettings.weightLifetimeMiniSlams = value;
		if (float.TryParse(inputLifetimeNineOnTheSnap.text, out value))
			weightSettings.weightLifetimeNineOnTheSnap = value;
		if (float.TryParse(inputLifetimeShutouts.text, out value))
			weightSettings.weightLifetimeShutouts = value;
		if (float.TryParse(inputLifetimeBreakAndRun.text, out value))
			weightSettings.weightLifetimeBreakAndRun = value;
		if (float.TryParse(inputLifetimeDefensiveShotAverage.text, out value))
			weightSettings.weightLifetimeDefensiveShotAverage = value;
		if (float.TryParse(inputLifetimeMatchesPlayed.text, out value))
			weightSettings.weightLifetimeMatchesPlayed = value;
		if (float.TryParse(inputLifetimeMatchesWon.text, out value))
			weightSettings.weightLifetimeMatchesWon = value;

		// Log that the settings have been saved
		Debug.Log("Lifetime weight settings saved.");
		}
	// --- End Region: Save Settings --- //
	}
