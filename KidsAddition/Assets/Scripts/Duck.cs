using UnityEngine;
using System.Collections;

public class Duck : CountedEntity
{
	protected float localCreationTime;
	[SerializeField] private float winFlySpeed = 0.5f;
	private Vector2 currentFlyDirection;

	protected override void Start()
	{
		base.Start();

		localCreationTime = Time.time;

		currentFlyDirection = GetNewFlyDirection();
	}

	Vector2 GetNewFlyDirection()
	{
		Vector2 returnVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
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
			// fly around
			transform.Translate(currentFlyDirection * winFlySpeed * Time.deltaTime);

			Vector2 currentPosition = Camera.main.WorldToScreenPoint(transform.position);
			if (currentPosition.x < 0
				|| currentPosition.x > Camera.main.pixelWidth
				|| currentPosition.y < 0
				|| currentPosition.y > Camera.main.pixelHeight)
			{
				currentFlyDirection = GetNewFlyDirection();
			}
		}
	}

	protected override Vector2 FindDropPosition()
	{
		return transform.position;
	}

	public override void TriggerSumReached()
	{
		ChangeState(EntityState.EXITING);
	}
}
