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
	#endregion

	#region Singleton
	public static FeedbackOverlay Instance { get; private set; }

	private void Awake()
		{
		Debug.Log("FeedbackOverlay Awake called.");
		if (Instance != null && Instance != this)
			{
			Debug.LogWarning("Multiple FeedbackOverlay instances detected. Destroying duplicate.");
			Destroy(gameObject);
			}
		else
			{
			Instance = this;
			Debug.Log("FeedbackOverlay instance set.");
			}
		}
	#endregion

	#region Methods
	public void ShowFeedback(string message, float duration = 2f)
		{
		StopAllCoroutines(); // Stop previous messages from overlapping
		feedbackText.text = message;
		feedbackPanel.SetActive(true);
		StartCoroutine(HideFeedbackAfterTime(duration));
		}

	private IEnumerator HideFeedbackAfterTime(float delay)
		{
		yield return new WaitForSeconds(delay);
		feedbackPanel.SetActive(false);
		}
	#endregion
	}
