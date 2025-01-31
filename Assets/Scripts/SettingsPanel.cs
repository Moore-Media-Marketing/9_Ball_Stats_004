using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel:MonoBehaviour
	{
	public TMP_Text headerText;
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