using TMPro;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the overlay feedback panel, displaying messages and handling user interaction.
/// </summary>
public class OverlayFeedbackPanel:MonoBehaviour
	{
	// --- Singleton Instance --- //
	public static OverlayFeedbackPanel Instance { get; private set; }

	// --- UI Elements --- //
	[Tooltip("Text element displaying feedback messages.")]
	public TMP_Text feedbackText;

	[Tooltip("OK button for confirming actions.")]
	public Button okButton;

	[Tooltip("Cancel button for dismissing the panel.")]
	public Button cancelButton;

	// Action callbacks for button interactions
	private System.Action onOkPressed;

	private System.Action onCancelPressed;

	/// <summary>
	/// Initializes the singleton instance and assigns button listeners.
	/// </summary>
	private void Awake()
		{
		// Ensure there is only one instance
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject); // Destroy duplicate instances
			return;
			}

		// Ensure the panel starts hidden
		gameObject.SetActive(false);

		// Log in case the required components are not assigned in the inspector
		if (feedbackText == null)
			{
			Debug.LogError("Feedback Text is not assigned in the inspector.");
			}
		if (okButton == null)
			{
			Debug.LogError("OK Button is not assigned in the inspector.");
			}
		if (cancelButton == null)
			{
			Debug.LogError("Cancel Button is not assigned in the inspector.");
			}

		// Assign button click handlers
		if (okButton != null)
			{
			okButton.onClick.AddListener(HandleOkClick);
			}
		if (cancelButton != null)
			{
			cancelButton.onClick.AddListener(HandleCancelClick);
			}
		}

	/// <summary>
	/// Displays the feedback panel with a custom message and optional button callbacks.
	/// </summary>
	/// <param name="message">The feedback message to display.</param>
	/// <param name="okCallback">Action to execute when OK is pressed (optional).</param>
	/// <param name="cancelCallback">Action to execute when Cancel is pressed (optional).</param>
	public void ShowFeedback(string message, System.Action okCallback = null, System.Action cancelCallback = null)
		{
		if (feedbackText != null)
			{
			feedbackText.text = message;
			}
		else
			{
			Debug.LogError("Feedback Text is not assigned or has been destroyed.");
			}

		onOkPressed = okCallback;
		onCancelPressed = cancelCallback;

		// Show the panel
		gameObject.SetActive(true);

		// Hide the Cancel button if no cancel callback is provided
		if (cancelButton != null)
			{
			cancelButton.gameObject.SetActive(cancelCallback != null);
			}
		}

	/// <summary>
	/// Hides the feedback panel and resets callbacks.
	/// </summary>
	public void HideFeedback()
		{
		gameObject.SetActive(false);
		onOkPressed = null;
		onCancelPressed = null;
		}

	/// <summary>
	/// Handles OK button click.
	/// </summary>
	private void HandleOkClick()
		{
		onOkPressed?.Invoke();
		HideFeedback();
		}

	/// <summary>
	/// Handles Cancel button click.
	/// </summary>
	private void HandleCancelClick()
		{
		onCancelPressed?.Invoke();
		HideFeedback();
		}
	}