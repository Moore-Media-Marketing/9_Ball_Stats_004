using System.IO;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class OverlayFeedbackPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	public TMP_Text feedbackText;  // Reference to the TextMeshPro text element

	public GameObject feedbackPanel;  // Reference to the feedback panel
	public Button okButton;  // Reference to the OK button
	public Button cancelButton;  // Reference to the Cancel button

	private string feedbackFilePath;  // Path to the CSV file where feedback messages are stored

	// --- Region: Initialize --- //
	private void Start()
		{
		feedbackFilePath = Path.Combine(Application.persistentDataPath, "feedback.csv");

		// Ensure the CSV file exists (create it if it doesn't)
		if (!File.Exists(feedbackFilePath))
			{
			File.WriteAllText(feedbackFilePath, "Id,Message\n");  // Add headers if file is new
			}

		cancelButton.onClick.AddListener(OnCancelButtonClicked);
		}

	// --- Region: Show Feedback with Options --- //
	public void ShowFeedbackWithOptions(string message, System.Action onOkAction)
		{
		feedbackPanel.SetActive(true);
		feedbackText.text = message;

		// Enable the buttons and set up their actions
		okButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(true);

		// Store the action to be performed on OK
		okButton.onClick.RemoveAllListeners();

		// Add listener with an intermediate method
		okButton.onClick.AddListener(() => OnOkButtonClickedWithAction(onOkAction));

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(OnCancelButtonClicked);
		}

	// --- Region: OK Button Clicked --- //
	// Intermediate method to call OnOkButtonClicked with parameter
	private void OnOkButtonClickedWithAction(System.Action onOkAction)
		{
		OnOkButtonClicked(onOkAction);
		}

	// --- Region: OK Button Clicked --- //
	private void OnOkButtonClicked(System.Action onOkAction)
		{
		// Execute the action passed when OK was clicked
		onOkAction.Invoke();

		// Provide feedback and close the panel after 2 seconds
		ShowFeedback("Action confirmed!", () => HideFeedback());
		}

	// --- Region: Cancel Button Clicked --- //
	private void OnCancelButtonClicked()
		{
		// Provide feedback and close the panel after 2 seconds
		ShowFeedback("Action cancelled.", () => HideFeedback());
		}

	// --- Region: Show Feedback --- //
	private void ShowFeedback(string message, System.Action callback = null)
		{
		feedbackPanel.SetActive(true);
		feedbackText.text = message;

		// Save the feedback message to CSV
		SaveFeedbackToCsv(message);

		if (callback != null)
			{
			Invoke(nameof(ClosePanel), 2f);  // Wait for 2 seconds before closing the panel
			}
		else
			{
			Invoke(nameof(HideFeedback), 2f);  // Hide feedback after 2 seconds
			}
		}

	// --- Region: Hide Feedback --- //
	private void HideFeedback()
		{
		feedbackPanel.SetActive(false);  // Deactivate the panel
		}

	// --- Region: Close Panel --- //
	private void ClosePanel()
		{
		feedbackPanel.SetActive(false);  // Close the feedback panel
		}

	// --- Region: Save Feedback to CSV --- //
	private void SaveFeedbackToCsv(string message)
		{
		try
			{
			int newId = GetNextFeedbackId();
			var line = $"{newId},{message}";
			File.AppendAllLines(feedbackFilePath, new[] { line });  // Append new feedback to the CSV
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving feedback to CSV: {ex.Message}");
			}
		}

	// --- Region: Get Next Feedback ID --- //
	private int GetNextFeedbackId()
		{
		int nextId = 1;

		if (File.Exists(feedbackFilePath))
			{
			var lines = File.ReadAllLines(feedbackFilePath);
			if (lines.Length > 1)  // If there are already entries (skipping header)
				{
				var lastLine = lines[^1];
				var lastId = int.Parse(lastLine.Split(',')[0]);  // Extract the ID from the last line
				nextId = lastId + 1;
				}
			}

		return nextId;
		}
	}