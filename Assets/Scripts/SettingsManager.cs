using System.IO;

using UnityEngine;

public class SettingsManager:MonoBehaviour
	{
	// --- Region: CSV File Path --- //
	private string settingsFilePath;  // Path to the settings CSV file

	// --- Region: Sample Data Toggle --- //
	[SerializeField]
	private bool sampleDataEnabled;  // Boolean to check if sample data is enabled (toggle)

	// --- Region: Sample Data Generation --- //
	public void SetSampleDataGeneration(bool enabled)
		{
		sampleDataEnabled = enabled;

		if (enabled)
			{
			Debug.Log("Sample Data Generation Enabled");
			// Initialize sample data generation logic here
			GenerateSampleData();
			}
		else
			{
			Debug.Log("Sample Data Generation Disabled");
			// Disable or clear any generated data here
			ClearSampleData();
			}

		SaveSettings();  // Save the updated settings
		}

	// --- Region: Sample Data Generation --- //
	private void GenerateSampleData()
		{
		// Your sample data generation logic here
		// For example, adding players or teams to the CSV
		Debug.Log("Generating sample data...");
		}

	// --- Region: Clear Sample Data --- //
	private void ClearSampleData()
		{
		// Your logic for clearing the sample data
		Debug.Log("Clearing sample data...");
		}

	// --- Region: Save Settings --- //
	public void SaveSettings()
		{
		try
			{
			settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.csv");

			// Create or append to the CSV file
			var settings = new Settings { SampleDataEnabled = sampleDataEnabled };

			// Write to CSV (or overwrite existing settings)
			SaveSettingsToCsv(settings);

			Debug.Log("Settings saved.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving settings: {ex.Message}");
			}
		}

	// --- Region: Load Settings --- //
	public void LoadSettings()
		{
		try
			{
			settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.csv");

			// Load settings from CSV
			var settings = LoadSettingsFromCsv();
			if (settings != null)
				{
				sampleDataEnabled = settings.SampleDataEnabled;  // Set the state of sample data toggle
				Debug.Log("Settings loaded.");
				}
			else
				{
				Debug.Log("No saved settings found.");
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading settings: {ex.Message}");
			}
		}

	// --- Region: UIManager for Back Button --- //
	public void HandleBackButton()
		{
		// Handle back button press (you can use Unity's built-in back button handling here)
		// Example: Go back to the previous screen or close the settings panel
		Debug.Log("Back button pressed. Returning to the previous screen.");
		}

	// --- Region: Additional Functions --- //
	// You can add any extra utility functions here if needed in the future

	// --- CSV Helper Methods ---
	// Save settings to CSV file
	private void SaveSettingsToCsv(Settings settings)
		{
		var lines = new string[] { settings.ToCsv() };
		File.WriteAllLines(settingsFilePath, lines);  // Overwrite existing settings or create new file
		}

	// Load settings from the CSV file
	private Settings LoadSettingsFromCsv()
		{
		if (File.Exists(settingsFilePath))
			{
			string[] lines = File.ReadAllLines(settingsFilePath);
			if (lines.Length > 0)
				{
				return Settings.FromCsv(lines[0]);  // Read the first line and convert to Settings
				}
			}

		return null;
		}
	}

// --- Region: Settings Data Model --- //
public class Settings
	{
	public bool SampleDataEnabled { get; set; }  // Store the state of sample data toggle

	// Convert the Settings object to a CSV string
	public string ToCsv()
		{
		return $"{SampleDataEnabled}";
		}

	// Convert a CSV string to a Settings object
	public static Settings FromCsv(string csvLine)
		{
		string[] values = csvLine.Split(',');
		return new Settings
			{
			SampleDataEnabled = bool.Parse(values[0])  // Parse the SampleDataEnabled field from CSV
			};
		}
	}