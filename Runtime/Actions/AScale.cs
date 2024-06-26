using UnityEngine;
namespace ActionStateSystem.Runtime
{
	public class AScale : BaseAction
	{
		public float scaleSpeed = 1.0f; // Vitesse de mise à l'échelle par seconde
		public float minScale = 0.5f; // Échelle minimale
		public float maxScale = 2.0f; // Échelle maximale
		private bool isScalingUp = true; // Indique si l'objet est en train de grandir
		private bool isActionRunning;

		protected override void Awake()
		{
			actionName = "## > Action Scale";
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
			if (!isActionRunning)
			{
				return;
			}

			// Mise à l'échelle
			float scaleChange = scaleSpeed * Time.deltaTime;
			if (isScalingUp)
			{
				transform.localScale += new Vector3(scaleChange, scaleChange, scaleChange);
				if (transform.localScale.x >= maxScale)
				{
					isScalingUp = false;
				}
			}
			else
			{
				transform.localScale -= new Vector3(scaleChange, scaleChange, scaleChange);
				if (transform.localScale.x <= minScale)
				{
					isScalingUp = true;
				}
			}
			// Vérifier la condition de transition
			if (ShouldTransition())
			{
				StopAction();
			}
		}
	}
}