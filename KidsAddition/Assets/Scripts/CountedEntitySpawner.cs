using UnityEngine;
using System.Collections;
using System;

public class CountedEntitySpawner : MonoBehaviour, IEventSubscriber
{
	public GameObject CountedEntityPrefab;
	private CountedEntity mostRecentSpawnedEntity;
	[SerializeField] private float spawnDelay = 1.0f;
	private float spawnDelayCounter = 0.0f;

	private bool isSpawning = true;
	private int maxSpawn = 50;
	private int currentSpawned = 0;

	void Start()
	{
		FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.SUM_REACHED, this);
	}

	void Update ()
	{
		if (isSpawning)
		{
			if (!mostRecentSpawnedEntity)
			{
				SpawnCountedEntity();
			}

			if (currentSpawned < maxSpawn)
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
	}

	void SpawnCountedEntity()
	{
		GameObject go = Instantiate(CountedEntityPrefab) as GameObject;
		go.transform.position = transform.position;
		mostRecentSpawnedEntity = go.GetComponent<CountedEntity>();
		currentSpawned++;
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.SUM_REACHED)
		{
			isSpawning = false;
		}
	}
}
