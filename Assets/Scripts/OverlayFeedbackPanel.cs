using System.IO;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class OverlayFeedbackPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	[Header("UI Elements")]
	[Tooltip("Text field to display feedback messages.")]
	public TMP_Text feedbackText;

	[Tooltip("Panel that contains the feedback UI.")]
	public GameObject feedbackPanel;

	[Tooltip("Button to confirm the feedback action.")]
	public Button okButton;

	[Tooltip("Button to cancel the feedback action.")]
	public Button cancelButton;

	private string feedbackFilePath;
	// --- End Region: UI References --- //

	// --- Region: Initialize --- //
	private void Start()
		{
		// Define the feedback CSV file path
		feedbackFilePath = Path.Combine(Application.persistentDataPath, "feedback.csv");

		// Ensure the CSV file exists (create it if it doesn't)
		if (!File.Exists(feedbackFilePath))
			{
			File.WriteAllText(feedbackFilePath, "Id,Message\n");
			}

		// Set up button listeners
		cancelButton.onClick.AddListener(OnCancelButtonClicked);
		}
	// --- End Region: Initialize --- //

	// --- Region: Show Feedback with Options --- //
	// Displays the feedback panel with custom message and optional actions for OK and Cancel buttons
	public void ShowFeedbackWithOptions(string message, System.Action onOkAction)
		{
		feedbackPanel.SetActive(true);
		feedbackText.text = message;

		// Enable the buttons and set up their actions
		okButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(true);

		// Set the action for the OK button
		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(() => OnOkButtonClickedWithAction(onOkAction));

		// Set the action for the Cancel button
		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(OnCancelButtonClicked);
		}
	// --- End Region: Show Feedback with Options --- //

	// --- Region: OK Button Clicked --- //
	// Intermediate method to call OnOkButtonClicked with parameter
	private void OnOkButtonClickedWithAction(System.Action onOkAction)
		{
		OnOkButtonClicked(onOkAction);
		}

	// Called when the OK button is clicked
	private void OnOkButtonClicked(System.Action onOkAction)
		{
		// Disable buttons temporarily to prevent multiple clicks
		DisableButtons();

		// Execute the action passed when OK was clicked
		onOkAction.Invoke();

		// Provide feedback and close the panel after 2 seconds
		ShowFeedback("Action confirmed!", () =>
		{
			HideFeedback();
			EnableButtons();
		});
		}
	// --- End Region: OK Button Clicked --- //

	// --- Region: Cancel Button Clicked --- //
	// Called when the Cancel button is clicked
	private void OnCancelButtonClicked()
		{
		// Disable buttons temporarily to prevent multiple clicks
		DisableButtons();

		// Provide feedback and close the panel after 2 seconds
		ShowFeedback("Action cancelled.", () =>
		{
			HideFeedback();
			EnableButtons();
		});
		}
	// --- End Region: Cancel Button Clicked --- //

	// --- Region: Show Feedback --- //
	// Show the feedback panel with a message and optional callback after hiding
	private void ShowFeedback(string message, System.Action callback = null)
		{
		feedbackPanel.SetActive(true);
		feedbackText.text = message;

		// Save the feedback message to CSV
		SaveFeedbackToCsv(message);

		// Call the callback or hide feedback after 2 seconds
		if (callback != null)
			{
			Invoke(nameof(ClosePanel), 2f);
			}
		else
			{
			Invoke(nameof(HideFeedback), 2f);
			}
		}
	// --- End Region: Show Feedback --- //

	// --- Region: Hide Feedback --- //
	// Hide the feedback panel
	private void HideFeedback()
		{
		feedbackPanel.SetActive(false);
		}
	// --- End Region: Hide Feedback --- //

	// --- Region: Close Panel --- //
	// Close the feedback panel
	private void ClosePanel()
		{
		feedbackPanel.SetActive(false);
		}
	// --- End Region: Close Panel --- //

	// --- Region: Save Feedback to CSV --- //
	// Save feedback messages to a CSV file
	private void SaveFeedbackToCsv(string message)
		{
		try
			{
			int newId = GetNextFeedbackId();
			var line = $"{newId},{message}";
			File.AppendAllLines(feedbackFilePath, new[] { line });
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving feedback to CSV: {ex.Message}");
			}
		}
	// --- End Region: Save Feedback to CSV --- //

	// --- Region: Get Next Feedback ID --- //
	// Get the next available ID for the feedback
	private int GetNextFeedbackId()
		{
		int nextId = 1;

		if (File.Exists(feedbackFilePath))
			{
			var lines = File.ReadAllLines(feedbackFilePath);
			if (lines.Length > 1) // Ignore header row
				{
				var lastLine = lines[^1];
				var lastId = int.Parse(lastLine.Split(',')[0]);
				nextId = lastId + 1;
				}
			}

		return nextId;
		}
	// --- End Region: Get Next Feedback ID --- //

	// --- Region: Disable/Enable Buttons --- //
	// Disable the OK and Cancel buttons
	private void DisableButtons()
		{
		okButton.interactable = false;
		cancelButton.interactable = false;
		}

	// Enable the OK and Cancel buttons
	private void EnableButtons()
		{
		okButton.interactable = true;
		cancelButton.interactable = true;
		}
	// --- End Region: Disable/Enable Buttons --- //
	}
