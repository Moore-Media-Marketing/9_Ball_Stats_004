using System.Collections;

using TMPro;

using UnityEngine;

public class FeedbackOverlay:MonoBehaviour
	{
	#region UI References

	[Header("Feedback UI Elements")]
	[Tooltip("Text element for displaying feedback messages.")]
	public TMP_Text feedbackText;

	[Tooltip("Panel that displays the feedback message.")]
	public GameObject feedbackPanel;

	#endregion UI References

	#region Singleton

	// Static reference to the instance
	public static FeedbackOverlay Instance { get; private set; }

	private void Awake()
		{
		// Ensure that there is only one instance of FeedbackOverlay
		if (Instance != null && Instance != this)
			{
			Destroy(gameObject);
			}
		else
			{
			Instance = this;
			}
		}

	#endregion Singleton

	#region Methods

	// --- Displays a feedback message for a set duration --- //
	public void ShowFeedback(string message, float duration = 2f)
		{
		StopAllCoroutines(); // Ensure previous messages don't overlap
		feedbackText.text = message;
		feedbackPanel.SetActive(true);
		StartCoroutine(HideFeedbackAfterTime(duration));
		}

	// --- Hides the feedback panel after a delay --- //
	private IEnumerator HideFeedbackAfterTime(float delay)
		{
		yield return new WaitForSeconds(delay);
		feedbackPanel.SetActive(false);
		}

	#endregion Methods
	}
