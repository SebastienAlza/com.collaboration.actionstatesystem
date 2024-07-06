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
		[TagSelector]
		public string targetTag; // Tag de la cible

		private float followSpeed = 3f; // Vitesse de suivi par seconde
		private bool isActionRunning; // Indique si l'action est en cours
		private Transform target; // Cible du suivi
		public FollowEffect followEffect = FollowEffect.Normal; // Effet de suivi

		// Variables spécifiques à chaque effet
		public float rippleAmplitude = 0.5f; // Amplitude de l'ondulation

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

			FindTarget(); // Trouver la cible au début

			base.StartAction();
		}

		public override void StopAction()
		{
			isActionRunning = false;
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (!isActionRunning || target == null)
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

		private void FindTarget()
		{
			GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
			if (targetObject != null)
			{
				target = targetObject.transform;
			}
		}
	}
}
