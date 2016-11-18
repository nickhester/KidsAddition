using UnityEngine;
using System.Collections;
using System;

public abstract class CountedEntity : MonoBehaviour, IEventSubscriber
{
	//private Vector2 dragMouseOffset;
	protected Collider2D colliderHoveringOver = null;
	protected bool hasBeenMovedByPlayer = false;
	[SerializeField] protected float distanceToWalkFromStart = 1.0f;
	[SerializeField] protected float speedToWalkFromStart = 2.0f;

	// animations
	[SerializeField] protected Animator animator;

	// loiter
	protected Vector2 assignedPosition;
	[SerializeField] protected float sinFrequency = 1.0f;
	[SerializeField] protected float sinMagnitude = 1.0f;

	[SerializeField] protected Sprite LoiterOutOfStage;
	[SerializeField] protected Sprite LoiterInStage;

	protected enum EntityState
	{
		WALKING_TO_START,
		STANDING_OUTSIDE_STAGE,
		STANDING_INSIDE_STAGE,
		PICKED_UP,
		EXITING
	}
	protected EntityState myState = EntityState.WALKING_TO_START;

	protected virtual void Start ()
	{
		FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.SUM_REACHED, this);

		assignedPosition = transform.position;

		ChangeState(EntityState.WALKING_TO_START);
		StartCoroutine(WalkToStart());
	}
	
	void Update ()
	{
		if (myState == EntityState.PICKED_UP)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = mousePosition;

			if (!Input.GetMouseButton(0))
			{
				BeDropped();
			}
		}
		else
		{
			Loiter();

			// set Z position so they cascade
			transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.y / 50.0f));
		}
	}

	private IEnumerator WalkToStart()
	{
		float lerpProgress = 0.0f;
		Vector2 startPosition = transform.position;
		Vector2 targetPosition = new Vector2(startPosition.x, startPosition.y + distanceToWalkFromStart);
		while (true)
		{
			lerpProgress += Time.deltaTime * speedToWalkFromStart;
			if (lerpProgress >= 1.0f)
			{
				transform.position = targetPosition;
				ChangeState(EntityState.STANDING_OUTSIDE_STAGE);
				break;
			}

			transform.position = Vector2.Lerp(startPosition, targetPosition, lerpProgress);

			yield return null;
		}
	}

	public bool GetHasBeenMovedByPlayer()
	{
		return hasBeenMovedByPlayer;
	}

	void OnMouseDown()
	{
		if (
			myState == EntityState.STANDING_INSIDE_STAGE
			|| myState == EntityState.STANDING_OUTSIDE_STAGE)
		{
			BePickedUp();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<AddendStage>())
		{
			colliderHoveringOver = other;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.GetComponent<AddendStage>())
		{
			colliderHoveringOver = null;
		}
	}

	void BeDropped()
	{
		AddendStage stage = null;
		if (colliderHoveringOver)
		{
			stage = colliderHoveringOver.GetComponent<AddendStage>();
		}

		if (stage)
		{
			assignedPosition = FindDropPosition();

			ChangeState(EntityState.STANDING_INSIDE_STAGE);

			stage.UpdateNumInhabitants(1);
		}
		else
		{
			ChangeState(EntityState.STANDING_OUTSIDE_STAGE);
		}
	}

	protected abstract Vector2 FindDropPosition();

	void BePickedUp()
	{
		ChangeState(EntityState.PICKED_UP);
		hasBeenMovedByPlayer = true;

		if (colliderHoveringOver)
		{
			AddendStage stage = colliderHoveringOver.GetComponent<AddendStage>();
			if (stage)
			{
				stage.UpdateNumInhabitants(-1);
			}
		}
	}

	protected void ChangeState(EntityState _state)
	{
		// if already exiting, ignore any other state changes
		if (myState == EntityState.EXITING)
		{
			return;
		}

		myState = _state;

		switch (_state)
		{
			case EntityState.WALKING_TO_START:
				break;
			case EntityState.STANDING_OUTSIDE_STAGE:
				{
					animator.SetTrigger("DroppedOffStage");
					break;
				}
			case EntityState.STANDING_INSIDE_STAGE:
				{
					animator.SetTrigger("DroppedOnStage");
					break;
				}
			case EntityState.PICKED_UP:
				{
					animator.SetTrigger("PickedUp");
					break;
				}
			case EntityState.EXITING:
				{
					animator.SetTrigger("LevelWin");
					break;
				}
			default:
				break;
		}

		
	}

	protected abstract void Loiter();

	public abstract void TriggerSumReached();

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.SUM_REACHED)
		{
			TriggerSumReached();
		}
	}
}
