using TMPro; // --- Importing TextMeshPro for UI elements ---
using UnityEngine; // --- Importing UnityEngine for Unity-related functionality ---
using UnityEngine.UI; // --- Importing UI components for handling buttons and toggles ---
using System.Data; // --- Importing for working with SQLite ---
using Mono.Data.Sqlite; // --- Importing for SQLite functionality ---

public class SettingsPanel:MonoBehaviour
	{
	// --- Region: UI Elements ---
	[Header("UI Elements")]
	[Tooltip("Header text at the top of the Settings Panel")]
	public TMP_Text headerText;

	[Tooltip("Button to navigate back to the Home Panel")]
	public Button backButton;

	[Tooltip("Toggle to enable or disable Sample Data Generator")]
	public Toggle sampleDataToggle;

	// --- End Region ---

	// --- Region: References ---
	[Header("References")]
	[Tooltip("Reference to the SampleDataGenerator script")]
	public SampleDataGenerator sampleDataGenerator;

	// --- End Region ---

	// --- Region: Initialization ---
	private void Start()
		{
		// Ensure the back button works
		if (backButton != null)
			{
			backButton.onClick.AddListener(OnBackButtonClicked);
			}
		else
			{
			Debug.LogError("Back button reference is missing!");
			}

		// Ensure toggle exists before setting its initial state
		if (sampleDataToggle != null)
			{
			// Load the toggle state from SQLite
			sampleDataToggle.isOn = GetSampleDataEnabledFromDatabase();

			// Add a listener to detect toggle changes
			sampleDataToggle.onValueChanged.AddListener(OnSampleDataToggleChanged);
			}
		else
			{
			Debug.LogError("SampleDataToggle is not assigned in the Inspector!");
			}

		// Ensure the SampleDataGenerator reference is assigned
		if (sampleDataGenerator == null)
			{
			Debug.LogError("SampleDataGenerator reference is missing!");
			}
		}
	// --- End Region ---

	// --- Region: Button Logic ---
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Use UIManager to switch panels
		}
	// --- End Region ---

	// --- Region: Toggle Handling ---
	private void OnSampleDataToggleChanged(bool isOn)
		{
		if (isOn)
			{
			EnableSampleData();
			}
		else
			{
			ClearSampleData();
			}

		// Save toggle state to SQLite for persistence
		SetSampleDataEnabledInDatabase(isOn);
		}
	// --- End Region ---

	// --- Region: Enable Sample Data ---
	private void EnableSampleData()
		{
		if (sampleDataGenerator != null)
			{
			sampleDataGenerator.GenerateSampleTeamsAndPlayers(); // Correct method call
			Debug.Log("Sample Data Generator activated.");
			}
		else
			{
			Debug.LogError("SampleDataGenerator is not assigned!");
			}
		}
	// --- End Region ---

	// --- Region: Clear Sample Data ---
	private void ClearSampleData()
		{
		if (sampleDataGenerator != null)
			{
			sampleDataGenerator.ClearSampleData();
			Debug.Log("Sample Data cleared.");
			}
		else
			{
			Debug.LogError("SampleDataGenerator is not assigned!");
			}
		}
	// --- End Region ---

	// --- Region: SQLite Database Handling ---

	private string dbConnectionString = "URI=file:" + Application.persistentDataPath + "/settings.db"; // Database path

	private void SetSampleDataEnabledInDatabase(bool isEnabled)
		{
		using (var connection = new SqliteConnection(dbConnectionString))
			{
			connection.Open();

			string query = "INSERT OR REPLACE INTO Settings (id, sample_data_enabled) VALUES (1, @isEnabled)";
			using (var command = new SqliteCommand(query, connection))
				{
				command.Parameters.AddWithValue("@isEnabled", isEnabled ? 1 : 0);
				command.ExecuteNonQuery();
				}
			}
		}

	private bool GetSampleDataEnabledFromDatabase()
		{
		using (var connection = new SqliteConnection(dbConnectionString))
			{
			connection.Open();

			string query = "SELECT sample_data_enabled FROM Settings WHERE id = 1";
			using (var command = new SqliteCommand(query, connection))
				{
				var result = command.ExecuteScalar();
				return result != null && Convert.ToInt32(result) == 1;
				}
			}
		}

	// Ensure the Settings table is created if it doesn't exist
	private void InitializeDatabase()
		{
		using (var connection = new SqliteConnection(dbConnectionString))
			{
			connection.Open();
			string createTableQuery = "CREATE TABLE IF NOT EXISTS Settings (id INTEGER PRIMARY KEY, sample_data_enabled INTEGER)";
			using (var command = new SqliteCommand(createTableQuery, connection))
				{
				command.ExecuteNonQuery();
				}
			}
		}

	// Call this method to initialize the database when the app starts
	private void Awake()
		{
		InitializeDatabase();
		}

	// --- End Region ---
	}
