using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurrentSum : MonoBehaviour
{
	private int currentSum = 0;
	private List<AddendStage> stages = new List<AddendStage>();
	private Sum sumObject;
	private bool hasTriggeredSumReactions = false;
	
	void Start ()
	{
		stages.AddRange(FindObjectsOfType<AddendStage>());
		sumObject = FindObjectOfType<Sum>();
	}
	
	void Update ()
	{
		int sum = 0;
		for (int i = 0; i < stages.Count; i++)
		{
			sum += stages[i].GetNumInhabitants();
		}
		currentSum = sum;

		if (currentSum >= sumObject.GetMyValue())
		{
			if (!hasTriggeredSumReactions)
			{
				TriggerSumReachedReactions();
			}
		}
	}

	void TriggerSumReachedReactions()
	{
		hasTriggeredSumReactions = true;

		FindObjectOfType<EventBroadcast>().TriggerEvent(EventBroadcast.Event.SUM_REACHED);
		
		FindObjectOfType<CountedEntitySpawner>().TriggerSumReached();
	}
}
