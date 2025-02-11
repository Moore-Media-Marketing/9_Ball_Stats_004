using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Manages the Current Season Weight Settings Panel.
/// </summary>
public class CurrentSeasonWeightSettingsPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TextMeshProUGUI headerText; // Header text

	public TMP_InputField currentSeasonPointsAwardedInputField; // Points Awarded Input Field
	public TMP_InputField currentSeasonMatchesWonInputField; // Matches Won Input Field
	public TMP_InputField currentSeasonDefensiveShotAverageInputField; // Defensive Shot Average Input Field
	public TMP_InputField currentSeasonSkillLevelInputField; // Skill Level Input Field
	public TMP_InputField currentSeasonPpmInputField; // Points Per Match Input Field
	public TMP_InputField currentSeasonShutoutsInputField; // Shutouts Input Field
	public TMP_InputField currentSeasonMiniSlamsInputField; // Mini Slams Input Field
	public TMP_InputField currentSeasonNineOnTheSnapInputField; // Nine On The Snap Input Field
	public TMP_InputField currentSeasonPaPercentageInputField; // Percentage Input Field
	public TMP_InputField currentSeasonBreakAndRunInputField; // Break and Run Input Field
	public Button saveButton; // Save button
	public TextMeshProUGUI saveButtonText; // Save button text
	public Button backButton; // Back button

	private void Start()
		{
		// Initialize UI elements
		InitializeInputFields();

		// Add listeners for buttons
		saveButton.onClick.AddListener(SaveSettings);
		backButton.onClick.AddListener(BackToSettings);
		}

	/// <summary>
	/// Initializes the input fields with placeholders.
	/// </summary>
	private void InitializeInputFields()
		{
		headerText.text = "Current Season Weight Settings";

		currentSeasonPointsAwardedInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Points Awarded";
		currentSeasonMatchesWonInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Matches Won";
		currentSeasonDefensiveShotAverageInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Defensive Shot Average";
		currentSeasonSkillLevelInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Skill Level (1-9)";
		currentSeasonPpmInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Points Per Match";
		currentSeasonShutoutsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Shutouts";
		currentSeasonMiniSlamsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Mini Slams";
		currentSeasonNineOnTheSnapInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Nine On The Snap";
		currentSeasonPaPercentageInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Percentage (0-1)";
		currentSeasonBreakAndRunInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Break and Run";

		saveButtonText.text = "Save";
		}

	/// <summary>
	/// Saves the current settings to a CSV file.
	/// </summary>
	private void SaveSettings()
		{
		// Get the values from the input fields
		string pointsAwarded = currentSeasonPointsAwardedInputField.text;
		string matchesWon = currentSeasonMatchesWonInputField.text;
		string defensiveShotAverage = currentSeasonDefensiveShotAverageInputField.text;
		string skillLevel = currentSeasonSkillLevelInputField.text;
		string pointsPerMatch = currentSeasonPpmInputField.text;
		string shutouts = currentSeasonShutoutsInputField.text;
		string miniSlams = currentSeasonMiniSlamsInputField.text;
		string nineOnTheSnap = currentSeasonNineOnTheSnapInputField.text;
		string percentage = currentSeasonPaPercentageInputField.text;
		string breakAndRun = currentSeasonBreakAndRunInputField.text;

		// Log the data to the console (optional)
		Debug.Log("Settings Saved:");
		Debug.Log($"Points Awarded: {pointsAwarded}");
		Debug.Log($"Matches Won: {matchesWon}");
		Debug.Log($"Defensive Shot Average: {defensiveShotAverage}");
		Debug.Log($"Skill Level: {skillLevel}");
		Debug.Log($"Points Per Match: {pointsPerMatch}");
		Debug.Log($"Shutouts: {shutouts}");
		Debug.Log($"Mini Slams: {miniSlams}");
		Debug.Log($"Nine On The Snap: {nineOnTheSnap}");
		Debug.Log($"Percentage: {percentage}");
		Debug.Log($"Break and Run: {breakAndRun}");

		// Path to the CSV file
		string filePath = "CurrentSeasonWeightSettings.csv";

		// Prepare the data to be saved
		List<string> lines = new()
			{
			$"{pointsAwarded},{matchesWon},{defensiveShotAverage},{skillLevel},{pointsPerMatch},{shutouts},{miniSlams},{nineOnTheSnap},{percentage},{breakAndRun}"
		};

		// Append the data to the CSV file
		File.AppendAllLines(filePath, lines);

		Debug.Log("Current season weight settings saved to CSV.");
		}

	/// <summary>
	/// Handles the back button click event.
	/// </summary>
	private void BackToSettings()
		{
		Debug.Log("Back button clicked. Returning to settings.");
		UIManager.Instance.GoBackToPreviousPanel(); // Go back using UIManager
		}
	}
