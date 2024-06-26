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
						component.enabled = false; // D�sactive le composant MonoBehaviour
						Debug.Log("Component deactivated: " + component.GetType().Name);
					}
				}
				else
				{
					Debug.LogWarning("Null component found in the list!");
				}
			}

			base.StartAction(); // Active la condition si n�cessaire
		}

		public override void StopAction()
		{
			// Il n'est pas n�cessaire d'arr�ter l'action sp�cifiquement pour activer ou d�sactiver les composants
			// car cela est g�n�ralement instantan�.
			base.StopAction(); // D�sactive la condition si n�cessaire
		}

		public override void UpdateAction()
		{
			if (ShouldTransition())
			{
				// Il n'y a pas de mise � jour continue n�cessaire ici car l'activation ou la d�sactivation
				// des composants est instantan�e.
				StopAction();
			}
		}
	}
}