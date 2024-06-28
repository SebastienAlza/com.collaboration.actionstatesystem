using UnityEngine;

namespace ActionStateSystem.Runtime
{
	public class AAttract : BaseAction
	{
		public float attractionSpeed = 3f; // Vitesse d'attraction par seconde
		[TagSelector]
		public string targetTag = "Collectible"; // Tag des objets à attirer
		public float attractionRadius = 5f; // Rayon d'attraction
		private bool isActionRunning; // Indique si l'action est en cours

		protected override void Awake()
		{
			actionName = "Attract Action";
		}

		public override void StartAction()
		{
			isActionRunning = true;
			Debug.Log("Starting AttractAction");
			base.StartAction();
		}

		public override void StopAction()
		{
			isActionRunning = false;
			Debug.Log("Stopping AttractAction");
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (!isActionRunning)
			{
				return;
			}

			AttractObjects();

			// Vérifier la condition de transition
			if (ShouldTransition())
			{
				StopAction();
			}
		}

		private void AttractObjects()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);

			foreach (Collider2D collider in colliders)
			{
				if (collider.CompareTag(targetTag))
				{
					Transform target = collider.transform;
					Vector3 direction = (transform.position - target.position).normalized;
					target.position += direction * attractionSpeed * Time.deltaTime;
				}
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, attractionRadius);
		}
	}
}
