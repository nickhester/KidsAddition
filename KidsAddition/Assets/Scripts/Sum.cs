using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sum : MonoBehaviour, IEventSubscriber
{
	private int myValue;
	[SerializeField] private int rangeMin = 1;
	[SerializeField] private int rangeMax = 10;
	
	void Start ()
	{
		myValue = Random.Range(rangeMin, rangeMax);
		GetComponent<Text>().text = myValue.ToString();

		FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.SUM_REACHED, this);
	}
	
	void Update ()
	{
		
	}

	public int GetMyValue()
	{
		return myValue;
	}

	public void TriggerSumReached()
	{
		GetComponent<Text>().color = Color.white;
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.SUM_REACHED)
		{
			TriggerSumReached();
		}
	}
}
