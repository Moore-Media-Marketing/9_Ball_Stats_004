using UnityEngine;
using TMPro; // For TextMeshPro
using System.Collections;

namespace NickWasHere
	{
	public class OverlayFeedbackPanelManager:MonoBehaviour
		{
		public static OverlayFeedbackPanelManager Instance; // Singleton Instance

		// --- UI References --- //
		[Header("UI Elements")]
		public GameObject panel;            // The panel that holds both overlay and feedback content
		public TextMeshProUGUI overlayText; // The text component for overlay messages
		public TextMeshProUGUI feedbackText;// The text component for feedback messages
		public float autoCloseTime = 2f;    // Time to auto-close the panel for feedback messages

		// --- State Control --- //
		private bool isTouched = false;  // Tracks if the panel is interacted with
		private bool isPanelActive = false; // Tracks panel visibility
		private Coroutine autoCloseCoroutine; // Reference for auto-close coroutine

		// --- Awake: Ensure Singleton Instance --- //
		private void Awake()
			{
			if (Instance == null)
				{
				Instance = this;
				}
			else
				{
				Destroy(gameObject); // Prevent duplicate instances
				}
			}

		// --- Start: Initialize Panel Visibility --- //
		private void Start()
			{
			if (panel == null || overlayText == null || feedbackText == null)
				{
				Debug.LogError("Panel or Text elements are not assigned.");
				return;
				}

			panel.SetActive(false); // Hide panel at start
			}

		// --- Show Panel --- //
		public void ShowPanel(string message, string feedback = "", bool isOverlay = true)
			{
			if (panel == null) return;

			panel.SetActive(true); // Show panel

			if (isOverlay)
				{
				overlayText.text = message;
				feedbackText.text = "";
				}
			else
				{
				overlayText.text = "";
				feedbackText.text = message;

				// Start auto-close coroutine if it's a feedback message
				if (autoCloseCoroutine != null)
					{
					StopCoroutine(autoCloseCoroutine);
					}
				autoCloseCoroutine = StartCoroutine(AutoClosePanel());
				}

			isTouched = false; // Reset touch detection
			isPanelActive = true;
			}

		// --- Hide Panel --- //
		public void HidePanel()
			{
			if (panel == null) return;

			panel.SetActive(false);
			isPanelActive = false;
			}

		// --- Auto-Close Coroutine --- //
		private IEnumerator AutoClosePanel()
			{
			yield return new WaitForSeconds(autoCloseTime);

			if (!isTouched)
				{
				HidePanel();
				}
			}

		// --- Handle User Interaction to Cancel Auto-Close --- //
		public void OnPanelTouched()
			{
			if (isPanelActive)
				{
				isTouched = true;
				HidePanel(); // Optionally hide the panel immediately
				}
			}

		// --- Convenience Methods --- //
		public void DisplayErrorMessage(string message)
			{
			ShowPanel(message, "Please try again.", false);
			}

		public void ShowOverlayMessage(string message)
			{
			ShowPanel(message, "", true);
			}

		public void OnPanelTapped()
			{
			OnPanelTouched();
			}
		}
	}
