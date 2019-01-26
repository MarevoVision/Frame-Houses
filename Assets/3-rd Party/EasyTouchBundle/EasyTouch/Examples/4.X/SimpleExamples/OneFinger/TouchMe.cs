using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HedgehogTeam.EasyTouch;

public class TouchMe : MonoBehaviour 
{
	// Subscribe to events
	void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
	}

	void OnDisable()
	{
		UnsubscribeEvent();
	}
	
	void OnDestroy()
	{
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	// At the touch beginning 
	private void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject)
		{
			gameObject.SetActive(false);
		}
	}
}
