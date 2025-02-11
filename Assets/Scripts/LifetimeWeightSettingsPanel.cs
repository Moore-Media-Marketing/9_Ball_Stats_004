using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

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

	private void Start()
		{
		// Load existing values if available
		LoadLifetimeWeightSettings();

		// Add button listeners
		saveButton.onClick.AddListener(SaveLifetimeWeightSettings);
		backButton.onClick.AddListener(GoBack);

		// Update back button text dynamically if needed
		UpdateBackButtonText("Back");
		}

	// Load saved settings from CSV or other storage
	private void LoadLifetimeWeightSettings()
		{
		// Example of loading from a CSV file (adjust if you store it differently)
		string filePath = "LifetimeWeightSettings.csv"; // Your CSV path
		if (File.Exists(filePath))
			{
			string[] lines = File.ReadAllLines(filePath);
			foreach (string line in lines)
				{
				string[] values = line.Split(',');
				if (values.Length == 8) // Assuming 8 fields
					{
					lifetimeGamesWonInputField.text = values[0];
					lifetimeMiniSlamsInputField.text = values[1];
					lifetimeNineOnTheSnapInputField.text = values[2];
					lifetimeShutoutsInputField.text = values[3];
					lifetimeBreakAndRunInputField.text = values[4];
					lifetimeDefensiveShotAverageInputField.text = values[5];
					lifetimeMatchesPlayedInputField.text = values[6];
					lifetimeMatchesWonInputField.text = values[7];
					}
				}
			}
		}

	// Save the entered values to a CSV file
	private void SaveLifetimeWeightSettings()
		{
		// Get the values from the input fields
		string gamesWon = lifetimeGamesWonInputField.text;
		string miniSlams = lifetimeMiniSlamsInputField.text;
		string nineOnTheSnap = lifetimeNineOnTheSnapInputField.text;
		string shutouts = lifetimeShutoutsInputField.text;
		string breakAndRun = lifetimeBreakAndRunInputField.text;
		string defensiveShotAverage = lifetimeDefensiveShotAverageInputField.text;
		string matchesPlayed = lifetimeMatchesPlayedInputField.text;
		string matchesWon = lifetimeMatchesWonInputField.text;

		// Write to CSV file
		string filePath = "LifetimeWeightSettings.csv";
		List<string> lines = new()
			{
			$"{gamesWon},{miniSlams},{nineOnTheSnap},{shutouts},{breakAndRun},{defensiveShotAverage},{matchesPlayed},{matchesWon}"
		};

		File.AppendAllLines(filePath, lines); // Append data to CSV

		Debug.Log("Lifetime weight settings saved to CSV.");
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
