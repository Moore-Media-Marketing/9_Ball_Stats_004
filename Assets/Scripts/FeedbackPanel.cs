using TMPro;

using UnityEngine;

public class FeedbackPanel:MonoBehaviour
	{
	[Header("Overlay Panel")]
	[Tooltip("The overlay panel that will show feedback")]
	public GameObject overlayPanel;

	[Tooltip("Text component that will display feedback messages")]
	public TMP_Text feedbackText;

	// --- Show Feedback Method --- //
	public void ShowFeedback(string message)
		{
		overlayPanel.SetActive(true);
		feedbackText.text = message;
		}

	// --- Hide Feedback Method --- //
	public void HideFeedback()
		{
		overlayPanel.SetActive(false);
		}
	}