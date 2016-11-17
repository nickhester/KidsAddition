using UnityEngine;
using System.Collections;
using System;

public class CountedEntitySpawner : MonoBehaviour
{
	public GameObject CountedEntityPrefab;
	private CountedEntity mostRecentSpawnedEntity;
	[SerializeField] private float spawnDelay = 1.0f;
	private float spawnDelayCounter = 0.0f;

	private bool isSpawning = true;
	private int maxSpawn = 50;
	private int currentSpawned = 0;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		if (!mostRecentSpawnedEntity)
		{
			SpawnCountedEntity();
		}

		if (isSpawning && currentSpawned < maxSpawn)
		{
			if (mostRecentSpawnedEntity && mostRecentSpawnedEntity.GetHasBeenMovedByPlayer())
			{
				spawnDelayCounter += Time.deltaTime;

				if (spawnDelayCounter >= spawnDelay)
				{
					spawnDelayCounter = 0.0f;
					SpawnCountedEntity();
				}
			}
		}
	}

	void SpawnCountedEntity()
	{
		GameObject go = Instantiate(CountedEntityPrefab) as GameObject;
		go.transform.position = transform.position;
		mostRecentSpawnedEntity = go.GetComponent<CountedEntity>();
		currentSpawned++;
	}

	public void TriggerSumReached()
	{
		isSpawning = false;
	}
}
