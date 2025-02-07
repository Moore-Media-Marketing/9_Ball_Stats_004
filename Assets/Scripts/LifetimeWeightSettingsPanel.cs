using UnityEngine;
using TMPro;

public class LifetimeWeightSettingsPanel:MonoBehaviour
	{
	// Reference to the PlayerWeightSettings ScriptableObject.
	public PlayerWeightSettings weightSettings;

	// UI Input fields for lifetime weights.
	public TMP_InputField inputLifetimeGamesWon;
	public TMP_InputField inputLifetimeMiniSlams;
	public TMP_InputField inputLifetimeNineOnTheSnap;
	public TMP_InputField inputLifetimeShutouts;
	public TMP_InputField inputLifetimeBreakAndRun;
	public TMP_InputField inputLifetimeDefensiveShotAverage;
	public TMP_InputField inputLifetimeMatchesPlayed;
	public TMP_InputField inputLifetimeMatchesWon;

	private void Start()
		{
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Lifetime panel!");
			return;
			}
		LoadSettingsIntoUI();
		}

	// Populate the input fields with lifetime weight values.
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

	// Called from the Save button's OnClick event.
	public void SaveSettings()
		{
		if (weightSettings == null)
			{
			Debug.LogError("PlayerWeightSettings reference is missing on the Lifetime panel!");
			return;
			}

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

		Debug.Log("Lifetime weight settings saved.");
		}
	}
