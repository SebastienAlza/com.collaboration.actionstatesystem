using Codice.CM.Common;
using UnityEngine;

public class AFollow : BaseAction
{
	public float speedMin = 2f;
	public float speedMax = 4f;
	[TagSelector]
	public string targetTag = "Enemy"; // Tag de la cible

	private float followSpeed = 3f; // Vitesse de suivi par seconde
	private bool isActionRunning; // Indique si l'action est en cours
	private Transform target; // Cible du suivi

	protected override void Awake()
	{
		actionName = "## > Follow Action";
	}

	public override void Execute()
	{
		// Code à exécuter immédiatement lorsque l'action est déclenchée
	}

	public override void StartAction()
	{
		isActionRunning = true;

		followSpeed = UnityEngine.Random.Range(speedMin, speedMax);
		// Trouver la cible
		target = FindTargetWithTag(targetTag);

		Debug.Log("Entering FollowAction");
		base.StartAction();
	}

	public override void StopAction()
	{
		isActionRunning = false;
		Debug.Log("Exiting FollowAction");
		base.StopAction();
	}

	public override void UpdateAction()
	{
		if (!isActionRunning)
		{
			return;
		}

		if (target != null)
		{
			Vector3 direction = (target.position - transform.position).normalized;
			transform.position += direction * followSpeed * Time.deltaTime;
		}

		// Vérifier la condition de transition
		if (ShouldTransition())
		{
			StopAction();
		}
	}

	private Transform FindTargetWithTag(string tag)
	{
		GameObject targetObject = GameObject.FindWithTag(tag);
		if (targetObject != null)
		{
			return targetObject.transform;
		}
		return null;
	}
}
