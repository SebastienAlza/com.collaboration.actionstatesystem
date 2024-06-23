using UnityEngine;

public class TimeCondition : BaseCondition
{
	[SerializeField] private float duration = 5.0f; // Duration to wait in seconds
	[SerializeField] public float currentTime; 
	private float startTime;
	private bool isStarted;

	protected override void OnActivate()
	{
		ResetCondition();
	}

	public override bool IsMet()
	{
		if (!isStarted)
		{
			isStarted = true;
			startTime = Time.time;
		}
		currentTime = Time.time - startTime;
		return currentTime >= duration;
	}

	public void ResetCondition()
	{
		isStarted = false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
