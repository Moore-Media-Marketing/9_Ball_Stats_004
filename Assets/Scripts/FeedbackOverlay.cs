using UnityEngine;
using TMPro;
using System.Collections;
using SQLite;

public class FeedbackOverlay:MonoBehaviour
	{
	#region UI References

	[Header("Feedback UI Elements")]
	[Tooltip("Text element for displaying feedback messages.")]
	public TMP_Text feedbackText;

	[Tooltip("Panel that displays the feedback message.")]
	public GameObject feedbackPanel;

	#endregion

	#region Methods

	// --- Displays a feedback message for a set duration --- //
	public void ShowFeedback(string message, float duration = 2f)
		{
		StopAllCoroutines(); // Ensure previous messages don't overlap
		feedbackText.text = message;
		feedbackPanel.SetActive(true);
		StartCoroutine(HideFeedbackAfterTime(duration));

		// Log the feedback message to the database
		LogFeedbackMessage(message);
		}

	// --- Hides the feedback panel after a delay --- //
	private IEnumerator HideFeedbackAfterTime(float delay)
		{
		yield return new WaitForSeconds(delay);
		feedbackPanel.SetActive(false);
		}

	#endregion

	#region SQLite Integration

	// Logs the feedback message to the SQLite database
	private void LogFeedbackMessage(string message)
		{
		using (var dbConnection = new SQLiteConnection(DatabaseManager.Instance.GetDatabasePath()))
			{
			dbConnection.CreateTable<FeedbackMessage>();
			var feedback = new FeedbackMessage
				{
				Message = message,
				Timestamp = System.DateTime.Now
				};
			dbConnection.Insert(feedback);
			}
		}

	#endregion
	}

public class FeedbackMessage
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	public string Message { get; set; }

	public System.DateTime Timestamp { get; set; }
	}
