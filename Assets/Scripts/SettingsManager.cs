using UnityEngine;
using SQLite; // Required for SQLite functionality
using System.IO; // For file handling

public class SettingsManager:MonoBehaviour
	{
	// --- Region: Database Path --- //
	private string dbPath;  // Path to the SQLite database

	private SQLiteConnection db;  // SQLite connection

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
		// For example, adding players or teams to the database
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
			dbPath = Path.Combine(Application.persistentDataPath, "settings.db");
			db = new SQLiteConnection(dbPath);

			db.CreateTable<Settings>(); // Create the Settings table if it doesn't exist

			// Save the sample data toggle state to the database
			var settings = new Settings { SampleDataEnabled = sampleDataEnabled };
			db.InsertOrReplace(settings);

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
			dbPath = Path.Combine(Application.persistentDataPath, "settings.db");
			db = new SQLiteConnection(dbPath);

			db.CreateTable<Settings>();  // Ensure Settings table exists

			// Load settings from the database
			var settings = db.Table<Settings>().FirstOrDefault();
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
	}

// --- Region: Settings Data Model --- //
public class Settings
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }  // Unique ID for each settings record
	public bool SampleDataEnabled { get; set; }  // Store the state of sample data toggle
	}
