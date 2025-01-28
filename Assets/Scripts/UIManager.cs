using UnityEngine;
using UnityEngine.UI;

public class UIManager:MonoBehaviour
	{
	public GameObject[] panels; // Array of all panels
	public Button backButton; // Back button to navigate to the previous panel

	// --- Awake ---
	private void Awake()
		{
		// Set back button functionality
		backButton.onClick.AddListener(GoBackToPreviousPanel);
		}

	// --- Switch Panel ---
	public void SwitchPanel(int panelIndex)
		{
		// Disable all panels
		foreach (var panel in panels)
			{
			panel.SetActive(false);
			}

		// Enable the requested panel
		panels[panelIndex].SetActive(true);
		}

	// --- Go Back Functionality ---
	public void GoBackToPreviousPanel()
		{
		// Implement logic to go back to the previous panel (this could involve a stack if multiple panels are involved)
		Debug.Log("Going back to the previous panel.");
		}
	}
