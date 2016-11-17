using UnityEngine;
using System.Collections;

public class Duck : CountedEntity
{
	protected float localCreationTime;

	protected override void Start()
	{
		base.Start();

		localCreationTime = Time.time;
	}

	protected override void Loiter()
	{
		if (myState == EntityState.STANDING_INSIDE_STAGE)
		{
			transform.position = assignedPosition + new Vector2(0.0f, Mathf.Sin((Time.time + localCreationTime) * sinFrequency) * sinMagnitude);
		}
	}

	protected override Vector2 FindDropPosition()
	{
		return transform.position;
	}

	public override void TriggerSumReached()
	{
		print("I'm an excited duck");
	}
}
