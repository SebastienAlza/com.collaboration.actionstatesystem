using UnityEngine;

namespace ActionStateSystem.Runtime
{
	public class AColorChange : BaseAction
	{
		public float colorChangeSpeed = 1.0f; // Vitesse de changement de couleur par seconde
		private SpriteRenderer spriteRenderer;
		private bool isActionRunning;
		private Color originalColor;
		public Color targetColor = Color.red; // Couleur cible
		private float timer;

		protected override void Awake()
		{
			actionName = "Color Change Action";
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			originalColor = spriteRenderer.color;
		}

		public override void StartAction()
		{
			isActionRunning = true;
			base.StartAction();
		}

		public override void StopAction()
		{
			isActionRunning = false;
			spriteRenderer.color = originalColor; // Réinitialiser la couleur
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (!isActionRunning)
			{
				return;
			}

			// Changement de couleur
			timer += Time.deltaTime * colorChangeSpeed;
			spriteRenderer.color = Color.Lerp(originalColor, targetColor, Mathf.PingPong(timer, 1));

			// Vérifier la condition de transition
			if (ShouldTransition())
			{
				StopAction();
			}
		}
	}
}
