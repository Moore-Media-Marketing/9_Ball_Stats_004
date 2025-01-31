using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupComparisonPanel:MonoBehaviour
	{
	public TMP_Text headerText;
	public TMP_Text selectTeamAText;
	public TMP_Dropdown teamADropdown;
	public ScrollRect teamAPlayerScrollView;
	public TMP_Text teamBTeamText;
	public TMP_Dropdown teamBDropdown;
	public ScrollRect teamBPlayerScrollView;
	public Button compareButton;
	public Button backButton;

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		compareButton.onClick.AddListener(OnCompareButtonClicked);
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Use UIManager to handle panel switching
		}

	private void OnCompareButtonClicked()
		{
		// Logic to compare matchups
		}
	}