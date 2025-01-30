using System.Collections;

using TMPro; // For TextMeshPro

using UnityEngine;

public class OverlayFeedbackPanelManager:MonoBehaviour
	{
	public static OverlayFeedbackPanelManager Instance; // Singleton Instance

	// --- UI References --- //
	[Header("UI Elements")]
	public GameObject panel;            // The panel that holds both overlay and feedback content

	public TextMeshProUGUI overlayText; // The text component for overlay messages
	public TextMeshProUGUI feedbackText; // The text component for feedback messages
	public float autoCloseTime = 2f;    // Time to auto-close the panel for feedback messages

	// --- State Control --- //
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
	public void ShowPanel(string message, bool isOverlay = true)
		{
		if (panel == null || isPanelActive) return; // Prevent multiple panel activations

		panel.SetActive(true); // Show panel
		isPanelActive = true;

		// Handle text based on overlay or feedback
		if (isOverlay)
			{
			overlayText.text = message;
			feedbackText.text = "";
			}
		else
			{
			overlayText.text = "";
			feedbackText.text = message;
			}

		// Start auto-close coroutine
		if (autoCloseCoroutine != null)
			{
			StopCoroutine(autoCloseCoroutine); // Stop previous coroutine if any
			}
		autoCloseCoroutine = StartCoroutine(AutoClosePanel()); // Start new auto-close coroutine
		}

	// --- Auto-Close Coroutine --- //
	private IEnumerator AutoClosePanel()
		{
		yield return new WaitForSeconds(autoCloseTime); // Wait for the specified time
		HidePanel(); // Hide the panel after the wait time
		}

	// --- Hide Panel --- //
	public void HidePanel()
		{
		if (panel == null)
			{
			Debug.LogError("Panel is not assigned.");
			return;
			}

		panel.SetActive(false); // Hide the parent panel
		isPanelActive = false; // Reset panel state
		}

	// --- Convenience Methods --- //
	public void ShowOverlayMessage(string message)
		{
		ShowPanel(message, true); // Show overlay message
		}

	public void ShowFeedbackMessage(string message)
		{
		ShowPanel(message, false); // Show feedback message
		}
	}