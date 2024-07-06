using UnityEngine;

namespace ActionStateSystem.Runtime
{
	public class AAttract : BaseAction
	{
		public float attractionSpeed = 3f; // Vitesse d'attraction par seconde
		public LayerMask targetLayer; // Layer des objets à attirer
		public float attractionRadius = 5f; // Rayon d'attraction
		private bool isActionRunning; // Indique si l'action est en cours

		protected override void Awake()
		{
			actionName = "Attract Action";
		}

		public override void StartAction()
		{
			isActionRunning = true;
			base.StartAction();
		}

		public override void StopAction()
		{
			isActionRunning = false;
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (!isActionRunning) return;

			AttractObjects();

			if (ShouldTransition()) StopAction();
		}

		private void AttractObjects()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius, targetLayer);

			foreach (Collider2D collider in colliders)
			{	
				Transform target = collider.transform;
				Vector3 direction = (transform.position - target.position).normalized;
				if (useDynamicData && dataProperties != null && !string.IsNullOrEmpty(attributeName))
				{
					attractionSpeed = dataProperties.GetCurrentRandomFloatValue(attributeName);
				}
				target.position += direction * (attractionSpeed * Time.deltaTime);
			}
		}


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, attractionRadius);
		}
	}
}
