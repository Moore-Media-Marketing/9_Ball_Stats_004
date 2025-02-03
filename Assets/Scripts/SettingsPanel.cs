using TMPro;

using UnityEngine;
using UnityEngine.UI;

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
			// Load the toggle state from PlayerPrefs
			sampleDataToggle.isOn = PlayerPrefs.GetInt("SampleDataEnabled", 0) == 1;

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

		// Save toggle state in PlayerPrefs for persistence
		PlayerPrefs.SetInt("SampleDataEnabled", isOn ? 1 : 0);
		PlayerPrefs.Save();
		}
	// --- End Region ---

	// --- Region: Enable Sample Data ---
	private void EnableSampleData()
		{
		if (sampleDataGenerator != null)
			{
			sampleDataGenerator.GenerateSampleData();
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
			Debug.Log("Sample Data cleared from PlayerPrefs.");
			}
		else
			{
			Debug.LogError("SampleDataGenerator is not assigned!");
			}
		}
	// --- End Region ---
	}
