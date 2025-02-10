using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the settings panel UI, including toggles and buttons.
/// </summary>
public class SettingsPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TextMeshProUGUI headerText; // Header text
	public Toggle sampleDataGeneratorToggle; // Toggle for sample data generator
	public Text sampleDataLabel; // Label for the sample data toggle
	public Button currentSeasonWeightSettingsButton; // Button for current season weight settings
	public Button lifetimeWeightSettingsButton; // Button for lifetime weight settings
	public Button backButton; // Back button

	private void Start()
		{
		// Initialize UI elements
		InitializeSettingsPanel();

		// Add listeners for buttons
		currentSeasonWeightSettingsButton.onClick.AddListener(OpenCurrentSeasonWeightSettings);
		lifetimeWeightSettingsButton.onClick.AddListener(OpenLifetimeWeightSettings);
		backButton.onClick.AddListener(BackToPreviousMenu);

		// Add listener for the sample data generator toggle
		sampleDataGeneratorToggle.onValueChanged.AddListener(ToggleSampleDataGenerator);
		}

	/// <summary>
	/// Initializes the settings panel UI elements.
	/// </summary>
	private void InitializeSettingsPanel()
		{
		headerText.text = "Settings"; // Set header text
		sampleDataGeneratorToggle.isOn = false; // Default state of the toggle
		sampleDataLabel.text = "Enable Sample Data Generation"; // Set toggle label text
		}

	/// <summary>
	/// Opens the current season weight settings menu.
	/// </summary>
	private void OpenCurrentSeasonWeightSettings()
		{
		Debug.Log("Current Season Weight Settings Opened");
		UIManager.Instance.ShowCurrentSeasonWeightSettingsPanel();
		}

	/// <summary>
	/// Opens the lifetime weight settings menu.
	/// </summary>
	private void OpenLifetimeWeightSettings()
		{
		Debug.Log("Lifetime Weight Settings Opened");
		UIManager.Instance.ShowLifetimeWeightSettingsPanel();
		}

	/// <summary>
	/// Toggles the sample data generator on and off.
	/// </summary>
	/// <param name="isOn">The toggle state.</param>
	private void ToggleSampleDataGenerator(bool isOn)
		{
		if (isOn)
			{
			Debug.Log("Sample Data Generator Enabled");
			// Logic to enable the sample data generator
			SampleDataGenerator.Instance.EnableSampleDataGeneration();
			}
		else
			{
			Debug.Log("Sample Data Generator Disabled");
			// Logic to disable the sample data generator
			SampleDataGenerator.Instance.DisableSampleDataGeneration();
			}
		}

	/// <summary>
	/// Handles the back button click event.
	/// </summary>
	private void BackToPreviousMenu()
		{
		Debug.Log("Back button clicked. Returning to the previous menu.");
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}