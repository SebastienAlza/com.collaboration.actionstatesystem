using UnityEngine;

public class TriggerCondition : BaseCondition
{
	[SerializeField] private float radius = 1.0f; // Radius of the overlap circle
	[TagSelector]
	[SerializeField]
	public string targetTag = "Enemy"; // Tag de la cible

	private void Awake()
	{
		// Optional: Initialize anything needed on awake
	}

	public override bool IsMet()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
		foreach (var hit in hits)
		{
			if (hit.CompareTag(targetTag))
			{
				return true;
			}
		}
		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
