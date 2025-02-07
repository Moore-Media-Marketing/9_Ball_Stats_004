using UnityEngine;
using TMPro;

public class CurrentSeasonWeightSettingsPanel:MonoBehaviour
	{
	// Reference to the PlayerWeightSettings ScriptableObject.
	public PlayerWeightSettings weightSettings;

	// UI Input fields for current season weights.
	public TMP_InputField inputCurrentSeasonPointsAwarded;
	public TMP_InputField inputCurrentSeasonMatchesWon;
	public TMP_InputField inputCurrentSeasonDefensiveShotAverage;
	public TMP_InputField inputCurrentSeasonSkillLevel;
	public TMP_InputField inputCurrentSeasonPpm;
	public TMP_InputField inputCurrentSeasonShutouts;
	public TMP_InputField inputCurrentSeasonMiniSlams;
	public TMP_InputField inputCurrentSeasonNineOnTheSnap;
	public TMP_InputField inputCurrentSeasonPaPercentage;
	public TMP_InputField inputCurrentSeasonBreakAndRun;

	private void Start()
		{
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Current Season panel!");
			return;
			}
		LoadSettingsIntoUI();
		}

	// Populate the input fields with current season weight values.
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

	// Called from the Save button's OnClick event.
	public void SaveSettings()
		{
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Current Season panel!");
			return;
			}

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

		Debug.Log("Current Season weight settings saved.");
		}
	}
