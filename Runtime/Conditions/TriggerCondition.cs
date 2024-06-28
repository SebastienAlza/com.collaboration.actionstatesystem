using UnityEngine;

namespace ActionStateSystem.Runtime
{
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
		[SerializeField] public string targetTag = "Enemy"; // Tag de la cible

		public TriggerEventType eventType = TriggerEventType.Enter; // Type d'événement par défaut

		[SerializeField] private bool useDynamicRadius = false; // Option pour utiliser le radius dynamique
		[SerializeField] private DataProperties dataProperties; // Référence au ScriptableObject générique
		[SerializeField] private string attributeName; // Nom de l'attribut pour le radius dynamique

		public override bool IsMet()
		{
			float effectiveRadius = radius; // Utiliser le radius par défaut

			if (useDynamicRadius && dataProperties != null && !string.IsNullOrEmpty(attributeName))
			{
				// Récupérer la valeur de la propriété dynamique
				effectiveRadius = dataProperties.GetCurrentValue(attributeName);
				Debug.Log("Radius : " + effectiveRadius);
			}

			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectiveRadius);
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
							conditionMet = false;
							break;
					}

					if (conditionMet)
					{
						break;
					}
				}
			}

			if (eventType == TriggerEventType.Exit)
			{
				conditionMet = !foundTarget;
			}

			return conditionMet;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, radius);
		}
	}
}
