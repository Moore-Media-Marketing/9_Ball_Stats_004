using System.Collections;

using TMPro;

using UnityEngine;

public class FeedbackOverlay:MonoBehaviour
	{
	[Header("Feedback UI Elements")]
	[Tooltip("Text element for displaying feedback messages.")]
	public TMP_Text feedbackText;

	[Tooltip("Panel that displays the feedback message.")]
	public GameObject feedbackPanel;

	public static FeedbackOverlay Instance { get; private set; }

	private void Awake()
		{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
			Instance = this;
		}

	public void ShowFeedback(string message, float duration = 2f)
		{
		StopAllCoroutines();
		feedbackText.text = message;
		feedbackPanel.SetActive(true);
		StartCoroutine(HideFeedbackAfterTime(duration));
		}

	private IEnumerator HideFeedbackAfterTime(float delay)
		{
		yield return new WaitForSeconds(delay);
		feedbackPanel.SetActive(false);
		}
	}
