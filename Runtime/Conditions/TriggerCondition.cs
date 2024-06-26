using UnityEngine;

public enum TriggerEventType
{
	Enter,
	Exit,
	Stay
}

public class TriggerCondition : BaseCondition
{
	[SerializeField] private float radius = 1.0f; // Radius of the overlap circle
	[TagSelector]
	[SerializeField]
	public string targetTag = "Enemy"; // Tag de la cible

	public TriggerEventType eventType = TriggerEventType.Enter; // Type d'�v�nement par d�faut

	public override bool IsMet()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
		bool conditionMet = false;
		bool foundTarget = false;

		foreach (var hit in hits)
		{
			if (hit.CompareTag(targetTag))
			{
				foundTarget = true;

				switch (eventType)
				{
					case TriggerEventType.Enter:
						conditionMet = true;
						break;

					case TriggerEventType.Exit:
						conditionMet = false;
						break;

					case TriggerEventType.Stay:
						// Vous pouvez ajouter une logique sp�cifique pour le "stay" si n�cessaire
						conditionMet = false;
						break;
				}

				// Sortir de la boucle si la condition est remplie pour au moins un collider
				if (conditionMet)
				{
					break;
				}
			}
		}

		// Si l'�v�nement est "Exit", v�rifiez si aucun collider avec le tag sp�cifi� n'est encore dans la zone
		if (eventType == TriggerEventType.Exit)
		{
			conditionMet = !foundTarget;
		}

		return conditionMet;
	}


	private bool IsAnyColliderStillInArea(Collider2D[] colliders)
	{
		foreach (var collider in colliders)
		{
			if (collider.CompareTag(targetTag))
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
