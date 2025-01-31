using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField lifetimeGamesWonInputField;
	public TMP_InputField lifetimeGamesPlayedInputField;
	public TMP_InputField lifetimeDefensiveShotAvgInputField;
	public TMP_InputField matchesPlayedInLast2YearsInputField;
	public TMP_InputField lifetimeBreakAndRunInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField lifetimeMiniSlamsInputField;
	public TMP_InputField lifetimeShutoutsInputField;
	public Button updateLifetimeButton;
	public Button backButton;

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		updateLifetimeButton.onClick.AddListener(OnUpdateLifetimeButtonClicked);
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Use UIManager to handle panel switching
		}

	private void OnUpdateLifetimeButtonClicked()
		{
		// Logic to update lifetime data
		}
	}
