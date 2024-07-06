using UnityEngine;

namespace ActionStateSystem.Runtime
{
	public enum InstantiationMode
	{
		Single,
		Multiple
	}

	public class AInstanciate : BaseAction
	{
		public GameObject instancePrefab;
		public float fireRate = 1.0f; // Bullets per second
		public InstantiationMode instantiationMode = InstantiationMode.Single; // Mode d'instanciation

		private float timeSinceLastShot;
		private bool isActionRunning;
		private bool hasShotOnce;

		protected override void Awake()
		{
			actionName = "Action Instanciate";
		}

		public override void StartAction()
		{
			isActionRunning = true;
			timeSinceLastShot = 0f;
			hasShotOnce = false;
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

			timeSinceLastShot += Time.deltaTime;

			switch (instantiationMode)
			{
				case InstantiationMode.Single:
					if (!hasShotOnce)
					{
						Instanciation();
						hasShotOnce = true;
						StopAction(); // Stop action after spawning once
					}
					break;
				case InstantiationMode.Multiple:
					if (timeSinceLastShot >= 1.0f / fireRate)
					{
						timeSinceLastShot = 0f;
						Instanciation();
					}
					break;
			}

			if (ShouldTransition())
			{
				//Debug.Log("Transition condition met, stopping action.");
				StopAction();
			}
		}

		private void Instanciation()
		{
			Instantiate(instancePrefab, transform.position, Quaternion.identity);
		}
	}
}
