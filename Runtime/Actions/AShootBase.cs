using System.Collections.Generic;
using UnityEngine;

namespace ActionStateSystem.Runtime
{
	public class AShootBase : BaseAction
	{
		public GameObject bulletPrefab;
		public float detectionRadius = 100f;
		public float fireRate = 1.0f; // Bullets per second
		public LayerMask targetLayer; // LayerMask de la cible
		public float updateInterval = 0.5f; // Intervalle de mise à jour des cibles potentielles

		private Transform target;
		private List<Transform> potentialTargets = new List<Transform>();
		private float timeSinceLastShot;
		private bool isActionRunning;
		private float nextUpdateTime;

		protected override void Awake()
		{
			actionName = "Action Shoot Base";
		}

		public override void StartAction()
		{
			isActionRunning = true;
			timeSinceLastShot = 0f;
			nextUpdateTime = 0f;
			base.StartAction(); // Active la condition
		}

		public override void StopAction()
		{
			isActionRunning = false;
			base.StopAction(); // Désactive la condition et notifie l'ActionManager
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

			timeSinceLastShot += Time.deltaTime;

			if (timeSinceLastShot >= 1.0f / fireRate)
			{
				timeSinceLastShot = 0f;
				if (target != null)
				{
					Shoot();
				}
			}

			if (ShouldTransition())
			{
				StopAction();
			}
		}

		private void Shoot()
		{
			if (target == null) return;

			Vector3 direction = (target.position - transform.position).normalized;
			GameObject projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			projectile.GetComponent<ProjectileBaseComponent>().SetDirection(direction);
		}

		private void FindPotentialTargets()
		{
			potentialTargets.Clear();
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);
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
				if (potentialTarget == null) continue;

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
