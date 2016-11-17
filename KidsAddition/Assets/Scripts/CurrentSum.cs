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

		sumObject.TriggerSumReached();

		List<CountedEntity> countedEntities = new List<CountedEntity>();
		countedEntities.AddRange(FindObjectsOfType<CountedEntity>());
		for (int i = 0; i < countedEntities.Count; i++)
		{
			countedEntities[i].TriggerSumReached();
		}

		FindObjectOfType<CountedEntitySpawner>().TriggerSumReached();
	}
}
