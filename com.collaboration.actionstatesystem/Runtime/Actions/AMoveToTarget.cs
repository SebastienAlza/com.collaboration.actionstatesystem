using UnityEngine;

public class AMoveToTarget : BaseAction
{
	public string message;
	public Transform target;
	public float speed = 1.0f;

	protected override void Awake()
	{
		//runInParallel = true;  // Indiquer que cette action peut s'ex�cuter en parall�le
	}

	public override void Execute()
	{
		Debug.Log("Action A ex�cut�e : " + message);
	}

	public override void StartAction()
	{
		Debug.Log("Move To Target"+ target.name);
	}

	public override void StopAction()
	{
		Debug.Log("Action A arr�t�e");
	}

	public override bool ShouldTransition()
	{
		// Condition de transition pour ActionA
		return Vector3.Distance(transform.position, target.position) < 0.1f;
	}

	public override void UpdateAction()
	{
		// D�placement vers la cible
		if (target != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		}
	}
}