using UnityEngine;
namespace ActionStateSystem.Runtime
{
	public class ARotate : BaseAction
	{
		public float rotationSpeed = 10.0f; // Vitesse de rotation en degr�s par seconde
		private bool isActionRunning;

		protected override void Awake()
		{
			actionName = "## > Action Rotate";
		}

		public override void StartAction()
		{
			isActionRunning = true;
			//Debug.Log("Action Rotation d�marr�e");
			base.StartAction(); // Active la condition
		}

		public override void StopAction()
		{
			//Debug.Log("Action Rotation arr�t�e");
			isActionRunning = false;
			base.StopAction(); // D�sactive la condition et notifie l'ActionManager
		}

		public override void UpdateAction()
		{
			if (!isActionRunning)
			{
				return;
			}

			transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
			if (ShouldTransition())
			{
				//Debug.Log("Transition condition met, stopping action.");
				StopAction();
			}
		}
	}
}