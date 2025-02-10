using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifetimeWeightSettingsPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TMP_Text headerText;
	public TMP_InputField lifetimeGamesWonInputField;
	public TMP_InputField lifetimeMiniSlamsInputField;
	public TMP_InputField lifetimeNineOnTheSnapInputField;
	public TMP_InputField lifetimeShutoutsInputField;
	public TMP_InputField lifetimeBreakAndRunInputField;
	public TMP_InputField lifetimeDefensiveShotAverageInputField;
	public TMP_InputField lifetimeMatchesPlayedInputField;
	public TMP_InputField lifetimeMatchesWonInputField;
	public Button saveButton;
	public Button backButton;
	public TMP_Text backButtonText;

	void Start()
		{
		// Load existing values if available
		LoadLifetimeWeightSettings();

		// Add button listeners
		saveButton.onClick.AddListener(SaveLifetimeWeightSettings);
		backButton.onClick.AddListener(GoBack);

		// Update back button text dynamically if needed
		UpdateBackButtonText("Back");
		}

	// Load saved settings (placeholder function)
	private void LoadLifetimeWeightSettings()
		{
		// Example of loading values from a manager or PlayerPrefs (adjust as needed)
		lifetimeGamesWonInputField.text = PlayerPrefs.GetString("LifetimeGamesWon", "");
		lifetimeMiniSlamsInputField.text = PlayerPrefs.GetString("LifetimeMiniSlams", "");
		lifetimeNineOnTheSnapInputField.text = PlayerPrefs.GetString("LifetimeNineOnTheSnap", "");
		lifetimeShutoutsInputField.text = PlayerPrefs.GetString("LifetimeShutouts", "");
		lifetimeBreakAndRunInputField.text = PlayerPrefs.GetString("LifetimeBreakAndRun", "");
		lifetimeDefensiveShotAverageInputField.text = PlayerPrefs.GetString("LifetimeDefensiveShotAverage", "");
		lifetimeMatchesPlayedInputField.text = PlayerPrefs.GetString("LifetimeMatchesPlayed", "");
		lifetimeMatchesWonInputField.text = PlayerPrefs.GetString("LifetimeMatchesWon", "");
		}

	// Save the entered values (placeholder function)
	private void SaveLifetimeWeightSettings()
		{
		// Example of saving to PlayerPrefs (replace with actual database logic if needed)
		PlayerPrefs.SetString("LifetimeGamesWon", lifetimeGamesWonInputField.text);
		PlayerPrefs.SetString("LifetimeMiniSlams", lifetimeMiniSlamsInputField.text);
		PlayerPrefs.SetString("LifetimeNineOnTheSnap", lifetimeNineOnTheSnapInputField.text);
		PlayerPrefs.SetString("LifetimeShutouts", lifetimeShutoutsInputField.text);
		PlayerPrefs.SetString("LifetimeBreakAndRun", lifetimeBreakAndRunInputField.text);
		PlayerPrefs.SetString("LifetimeDefensiveShotAverage", lifetimeDefensiveShotAverageInputField.text);
		PlayerPrefs.SetString("LifetimeMatchesPlayed", lifetimeMatchesPlayedInputField.text);
		PlayerPrefs.SetString("LifetimeMatchesWon", lifetimeMatchesWonInputField.text);
		PlayerPrefs.Save();

		Debug.Log("Lifetime weight settings saved.");
		}

	// Go back to the previous panel
	private void GoBack()
		{
		UIManager.Instance.GoBackToPreviousPanel();
		}

	// Update the back button text dynamically
	public void UpdateBackButtonText(string text)
		{
		if (backButtonText != null)
			{
			backButtonText.text = text;
			}
		}
	}
