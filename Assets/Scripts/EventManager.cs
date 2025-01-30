using System;

using UnityEngine;
using UnityEngine.Events;

public class EventManager:MonoBehaviour
	{
	// Singleton instance
	public static EventManager Instance { get; private set; }

	// Declare events for different actions
	public UnityEvent OnTeamSelected = new();
	public UnityEvent OnPlayerDataUpdated = new();

	private void Awake()
		{
		// Ensure only one instance of EventManager exists
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			}
		}

	// You can add additional methods to trigger specific events
	public void TriggerTeamSelectedEvent()
		{
		OnTeamSelected.Invoke();
		}

	public void TriggerPlayerDataUpdatedEvent()
		{
		OnPlayerDataUpdated.Invoke();
		}
	}
