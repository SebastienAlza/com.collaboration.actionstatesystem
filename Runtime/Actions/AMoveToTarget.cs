using UnityEngine;

public class AMoveToTarget : BaseAction
{
	public string message;
	public Transform target;
	public float speed = 1.0f;

	protected override void Awake()
	{
		//runInParallel = true;  // Indiquer que cette action peut s'exécuter en parallèle
	}

	public override void Execute()
	{
		Debug.Log("Action A exécutée : " + message);
	}

	public override void StartAction()
	{
		Debug.Log("Move To Target"+ target.name);
	}

	public override void StopAction()
	{
		Debug.Log("Action A arrêtée");
	}

	public override bool ShouldTransition()
	{
		// Condition de transition pour ActionA
		return Vector3.Distance(transform.position, target.position) < 0.1f;
	}

	public override void UpdateAction()
	{
		// Déplacement vers la cible
		if (target != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		}
	}
}