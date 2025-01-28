using UnityEngine;
using TMPro; // For TextMeshPro
using UnityEngine.Events;
using System.Collections;

namespace NickWasHere
	{
	public class OverlayFeedbackPanelManager:MonoBehaviour
		{
		// References to the UI elements
		public GameObject panel;            // The panel that holds both overlay and feedback content
		public TextMeshProUGUI overlayText; // The text component for overlay messages
		public TextMeshProUGUI feedbackText;// The text component for feedback messages
		public float autoCloseTime = 2f;    // Time to auto-close the panel for feedback messages
		private bool isTouched = false;     // To track if the panel is touched or interacted with

		// For controlling if the panel is showing
		private bool isPanelActive = false;

		// Coroutine reference for auto-close functionality
		private Coroutine autoCloseCoroutine;

		// Start is called before the first frame update
		void Start()
			{
			if (panel == null || overlayText == null || feedbackText == null)
				{
				Debug.LogError("Panel or Text elements are not assigned.");
				return;
				}

			// Initially hide the panel
			panel.SetActive(false);
			}

		// Method to show the overlay/feedback panel
		public void ShowPanel(string message, string feedback = "", bool isOverlay = true)
			{
			// Activate the panel and set appropriate text
			panel.SetActive(true);

			if (isOverlay)
				{
				// Show the overlay message and hide feedback
				overlayText.text = message;
				feedbackText.text = "";
				}
			else
				{
				// Show the feedback message and hide overlay
				overlayText.text = "";
				feedbackText.text = message;
				}

			// Reset the touched flag for auto-close logic
			isTouched = false;

			// Start the auto-close coroutine for feedback messages
			if (autoCloseCoroutine != null)
				{
				StopCoroutine(autoCloseCoroutine);
				}

			if (!isOverlay)
				{
				autoCloseCoroutine = StartCoroutine(AutoClosePanel());
				}

			// Mark panel as active
			isPanelActive = true;
			}

		// Method to hide the panel manually
		public void HidePanel()
			{
			// Deactivate the panel
			panel.SetActive(false);
			isPanelActive = false;
			}

		// Coroutine to auto-close the panel after the specified time
		private IEnumerator AutoClosePanel()
			{
			yield return new WaitForSeconds(autoCloseTime);

			if (!isTouched)
				{
				HidePanel(); // Close the panel if it wasn't touched
				}
			}

		// Method to handle user touch (or any interaction) to cancel the auto-close
		public void OnPanelTouched()
			{
			if (isPanelActive)
				{
				isTouched = true;
				HidePanel(); // Optionally hide the panel immediately when touched
				}
			}

		// Method for displaying error messages (just as an example usage)
		public void DisplayErrorMessage(string message)
			{
			ShowPanel(message, "Please try again.", false);
			}

		// Method to show the overlay (for example usage)
		public void ShowOverlayMessage(string message)
			{
			ShowPanel(message, "", true);
			}

		// If the user taps anywhere on the panel, this method can be called
		public void OnPanelTapped()
			{
			OnPanelTouched();
			}
		}
	}
