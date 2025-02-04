using UnityEngine;
using TMPro;
using SQLite; // Required for SQLite functionality
using UnityEngine.UI; // Required for buttons

public class OverlayFeedbackPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	public TMP_Text feedbackText;  // Reference to the TextMeshPro text element
	public GameObject feedbackPanel;  // Reference to the feedback panel
	public Button okButton;  // Reference to the OK button
	public Button cancelButton;  // Reference to the Cancel button

	private SQLiteConnection db;
	private string dbPath;  // SQLite database path

	// --- Region: Initialize SQLite --- //
	private void Start()
		{
		dbPath = System.IO.Path.Combine(Application.persistentDataPath, "feedback.db");
		db = new SQLiteConnection(dbPath);
		db.CreateTable<Feedback>();  // Ensure the Feedback table exists

		// Attach button click listeners
		okButton.onClick.AddListener(OnOkButtonClicked);
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
		okButton.onClick.AddListener(() => OnOkButtonClicked(onOkAction));

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(OnCancelButtonClicked);
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

		if (callback != null)
			{
			Invoke("ClosePanel", 2f);  // Wait for 2 seconds before closing the panel
			}
		else
			{
			Invoke("HideFeedback", 2f);  // Hide feedback after 2 seconds
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

	// --- Region: Feedback Model --- //
	public class Feedback
		{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }  // Unique ID for feedback record
		public string Message { get; set; }  // Store the feedback message
		}
	}
