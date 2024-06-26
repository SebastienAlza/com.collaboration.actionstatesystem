using UnityEngine;
using System.Collections.Generic;
namespace ActionStateSystem.Runtime
{
	public enum ActivationType
	{
		Activate,
		Deactivate
	}

	public class AActivateComponent : BaseAction
	{
		public List<MonoBehaviour> targetComponents = new List<MonoBehaviour>();
		public ActivationType activationType = ActivationType.Activate;

		protected override void Awake()
		{
			actionName = "## > Action Activate Components";
		}

		public override void StartAction()
		{

			foreach (var component in targetComponents)
			{
				if (component != null)
				{
					if (activationType == ActivationType.Activate)
					{
						component.enabled = true; // Active le composant MonoBehaviour
						Debug.Log("Component activated: " + component.GetType().Name);
					}
					else if (activationType == ActivationType.Deactivate)
					{
						component.enabled = false; // Désactive le composant MonoBehaviour
						Debug.Log("Component deactivated: " + component.GetType().Name);
					}
				}
				else
				{
					Debug.LogWarning("Null component found in the list!");
				}
			}

			base.StartAction(); // Active la condition si nécessaire
		}

		public override void StopAction()
		{
			// Il n'est pas nécessaire d'arrêter l'action spécifiquement pour activer ou désactiver les composants
			// car cela est généralement instantané.
			base.StopAction(); // Désactive la condition si nécessaire
		}

		public override void UpdateAction()
		{
			if (ShouldTransition())
			{
				// Il n'y a pas de mise à jour continue nécessaire ici car l'activation ou la désactivation
				// des composants est instantanée.
				StopAction();
			}
		}
	}
}