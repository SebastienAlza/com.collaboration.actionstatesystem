using System.Collections.Generic;
using UnityEngine;

namespace ActionStateSystem.Runtime
{
	public enum FollowEffect
	{
		Normal,
		Ripple
	}

	public class AFollow : BaseAction
	{
		public float speedMin = 2f;
		public float speedMax = 4f;
		public bool followHeaded = false;
		public LayerMask targetLayer; // LayerMask de la cible
		public float searchRadius = 20f; // Rayon de recherche pour les cibles
		public float updateInterval = 0.5f; // Intervalle de mise à jour des cibles potentielles

		private float followSpeed = 3f; // Vitesse de suivi par seconde
		private bool isActionRunning; // Indique si l'action est en cours
		private Transform target; // Cible du suivi
		private List<Transform> potentialTargets = new List<Transform>(); // Liste des cibles potentielles
		public FollowEffect followEffect = FollowEffect.Normal; // Effet de suivi

		// Variables spécifiques à chaque effet
		public float rippleAmplitude = 0.5f; // Amplitude de l'ondulation
		private float nextUpdateTime = 0.1f; // Temps pour la prochaine mise à jour des cibles

		protected override void Awake()
		{
			actionName = "Follow Action";
		}

		public override void StartAction()
		{
			isActionRunning = true;

			if (useDynamicData && dataProperties != null && !string.IsNullOrEmpty(attributeName))
			{
				followSpeed = dataProperties.GetCurrentRandomFloatValue(attributeName);
			}
			else
			{
				followSpeed = Random.Range(speedMin, speedMax);
			}

			rippleAmplitude = Random.Range(rippleAmplitude * 0.5f, rippleAmplitude);
			base.StartAction();
		}

		public override void StopAction()
		{
			isActionRunning = false;
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (!isActionRunning)
			{
				return;
			}

			if (Time.time >= nextUpdateTime)
			{
				FindPotentialTargets();
				nextUpdateTime = Time.time + updateInterval;
			}

			if (target == null || !potentialTargets.Contains(target))
			{
				SetTarget();
			}

			if (target == null)
			{
				return;
			}

			// Calculer la direction vers la cible
			Vector3 direction = (target.position - transform.position).normalized;

			// Orienter l'objet pour regarder vers la cible
			if (followHeaded)
			{
				transform.right = direction; // En 2D, utilisez transform.right pour orienter l'objet vers la direction
			}

			// Appliquer l'effet de suivi selon le type choisi
			switch (followEffect)
			{
				case FollowEffect.Normal:
					transform.position += direction * followSpeed * Time.deltaTime;
					break;

				case FollowEffect.Ripple:
					// Exemple d'effet d'ondulation perpendiculaire
					Vector3 perpendicularDirection = new Vector3(direction.y, -direction.x, 0f); // Calculer la direction perpendiculaire
					float rippleFactor = Mathf.Sin(Time.time * 5f) * rippleAmplitude; // Amplitude d'ondulation
					transform.position += (direction + perpendicularDirection * rippleFactor) * followSpeed * Time.deltaTime;
					break;
			}

			// Vérifier la condition de transition
			if (ShouldTransition())
			{
				StopAction();
			}
		}

		private void FindPotentialTargets()
		{
			potentialTargets.Clear(); // Clear the list to avoid duplications
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, targetLayer);
			foreach (Collider2D collider in colliders)
			{
				potentialTargets.Add(collider.transform);
			}
		}

		private void SetTarget()
		{
			target = FindClosestTarget();
		}

		private Transform FindClosestTarget()
		{
			Transform closestTarget = null;
			float closestDistance = Mathf.Infinity;

			foreach (Transform potentialTarget in potentialTargets)
			{
				if (potentialTarget == null)
				{
					continue; // Skip destroyed targets
				}

				float distance = Vector3.Distance(transform.position, potentialTarget.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestTarget = potentialTarget;
				}
			}

			return closestTarget;
		}
	}
}
