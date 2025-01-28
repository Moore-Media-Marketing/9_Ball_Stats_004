using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIManager:MonoBehaviour
	{
	public static UIManager Instance { get; private set; } // Singleton instance

	[Header("Panels")]
	public GameObject[] panels; // Array of all panels

	[Header("Back Button")]
	public Button backButton; // Back button to navigate to the previous panel

	private int currentPanelIndex = -1; // Tracks the currently active panel
	private Stack<int> panelHistory = new(); // History of panel navigation
	private Dictionary<string, int> panelLookup; // Dictionary to map panel names to indices

	// --- Awake --- //
	private void Awake()
		{
		// Ensure only one instance of UIManager exists
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			return;
			}

		// Optionally, make the UIManager persistent across scenes
		DontDestroyOnLoad(gameObject);

		// Initialize panel lookup dictionary
		panelLookup = new Dictionary<string, int>();
		for (int i = 0; i < panels.Length; i++)
			{
			if (panels[i] != null)
				{
				panelLookup[panels[i].name] = i;
				}
			else
				{
				Debug.LogError($"Panel at index {i} is null in the UIManager.");
				}
			}

		// Set back button functionality
		if (backButton != null)
			{
			backButton.onClick.AddListener(GoBackToPreviousPanel);
			}
		else
			{
			Debug.LogError("Back button is not assigned in the UIManager.");
			}
		}

	// --- Show Panel by Index --- //
	public void ShowPanel(int panelIndex)
		{
		if (panelIndex < 0 || panelIndex >= panels.Length)
			{
			Debug.LogError($"Invalid panel index: {panelIndex}");
			return;
			}

		// Disable the currently active panel (if any)
		if (currentPanelIndex >= 0 && currentPanelIndex < panels.Length)
			{
			panels[currentPanelIndex].SetActive(false);
			}

		// Push the current panel index to history if it's valid
		if (currentPanelIndex != -1)
			{
			panelHistory.Push(currentPanelIndex);
			}

		// Enable the requested panel
		panels[panelIndex].SetActive(true);
		currentPanelIndex = panelIndex;

		Debug.Log($"Switched to panel {panelIndex}");
		}

	// --- Show Panel by Name --- //
	public void ShowPanel(string panelName)
		{
		if (panelLookup.TryGetValue(panelName, out int panelIndex))
			{
			ShowPanel(panelIndex);
			}
		else
			{
			Debug.LogError($"Panel with name '{panelName}' not found in UIManager.");
			}
		}

	// --- Go Back Functionality --- //
	public void GoBackToPreviousPanel()
		{
		if (panelHistory.Count > 0)
			{
			// Pop the last panel index from the history
			int previousPanelIndex = panelHistory.Pop();

			// Switch to the previous panel
			ShowPanel(previousPanelIndex);
			}
		else
			{
			Debug.Log("No previous panels in history.");
			}
		}
	}
