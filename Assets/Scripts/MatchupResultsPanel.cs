using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	public TMP_Text headerText;
	public TMP_Text teamAHeaderText;
	public TMP_Text teamBHeaderText;
	public ScrollRect matchupsScrollView;
	public TMP_Text bestMatchupHeaderText;
	public ScrollRect bestMatchupsScrollView;
	public Button backButton;

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Use UIManager to handle panel switching
		}
	}