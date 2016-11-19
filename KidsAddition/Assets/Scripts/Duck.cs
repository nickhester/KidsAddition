using UnityEngine;
using System.Collections;

public class Duck : CountedEntity
{
	protected float localCreationTime;
	[SerializeField] private float winFlySpeed = 0.5f;
	[SerializeField] private Vector2 gradualDirectionTendency;
	[SerializeField] private int edgeOfScreenPad = 50;
	private Vector2 currentFlyDirection;
	private float edgeOfScreenDetectionMinInterval = 0.5f;
	private float edgeOfScreenDetectionCounter = 0.0f;
	private SpriteRenderer[] allSpriteRenderers;

	protected override void Start()
	{
		base.Start();
		allSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		localCreationTime = Time.time;
		currentFlyDirection = GetNewFlyDirection(Vector2.up);
	}

	Vector2 GetNewFlyDirection(Vector2 _generalDirection)
	{
		Vector2 returnVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
		returnVector += _generalDirection;
		return returnVector.normalized;
	}

	protected override void Loiter()
	{
		if (myState == EntityState.STANDING_INSIDE_STAGE)
		{
			transform.position = assignedPosition + new Vector2(0.0f, Mathf.Sin((Time.time + localCreationTime) * sinFrequency) * sinMagnitude);
		}
		else if (myState == EntityState.EXITING)
		{
			edgeOfScreenDetectionCounter += Time.deltaTime;
			if (edgeOfScreenDetectionCounter > edgeOfScreenDetectionMinInterval)
			{
				edgeOfScreenDetectionCounter = 0.0f;

				Vector2 currentPosition = Camera.main.WorldToScreenPoint(transform.position);
				if (currentPosition.x < edgeOfScreenPad)
				{
					currentFlyDirection = GetNewFlyDirection(Vector2.right);
				}
				else if (currentPosition.x > (Camera.main.pixelWidth - edgeOfScreenPad))
				{
					currentFlyDirection = GetNewFlyDirection(Vector2.left);
				}
				else if (currentPosition.y < edgeOfScreenPad)
				{
					currentFlyDirection = GetNewFlyDirection(Vector2.up);
				}
				else if (currentPosition.y > (Camera.main.pixelHeight - edgeOfScreenPad))
				{
					currentFlyDirection = GetNewFlyDirection(Vector2.down);
				}

				bool isMovingOppositeDirection = currentFlyDirection.x > 0.0f;
				for (int i = 0; i < allSpriteRenderers.Length; i++)
				{
					allSpriteRenderers[i].flipX = isMovingOppositeDirection;
				}
			}

			// affect flight
			currentFlyDirection = currentFlyDirection + gradualDirectionTendency;

			// fly around
			transform.Translate(currentFlyDirection * winFlySpeed * Time.deltaTime);
		}
	}

	protected override Vector2 FindDropPosition()
	{
		return transform.position;
	}

	public override void TriggerSumReached()
	{
		if (myState == EntityState.STANDING_INSIDE_STAGE)
		{
			ChangeState(EntityState.EXITING);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
