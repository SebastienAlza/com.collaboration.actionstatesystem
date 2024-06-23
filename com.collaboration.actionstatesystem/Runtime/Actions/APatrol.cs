using UnityEngine;

public class APatrol : BaseAction
{
	public float speedMin = 2f;
	public float speedMax = 4f;
	public float patrolRadius = 10f; // Rayon de la patrouille
	public int numberOfPatrolPoints = 5; // Nombre de points de patrouille
	public float reachThreshold = 0.5f; // Distance à laquelle on considère que le point est atteint
	public float weightTransitionDistance = 2f; // Distance à partir de laquelle on commence à se diriger vers le prochain point

	private Vector2[] patrolPoints; // Points de patrouille
	private int currentPointIndex = 0; // Index du point de patrouille actuel
	private int nextPointIndex = 1; // Index du prochain point de patrouille
	private float patrolSpeed; // Vitesse de patrouille
	private bool isActionRunning; // Indique si l'action est en cours

	protected override void Awake()
	{
		actionName = "## > Patrol Action";
	}

	public override void Execute()
	{
		// Code à exécuter immédiatement lorsque l'action est déclenchée
	}

	public override void StartAction()
	{
		isActionRunning = true;
		patrolSpeed = UnityEngine.Random.Range(speedMin, speedMax);

		GeneratePatrolPoints();

		if (patrolPoints.Length > 0)
		{
			currentPointIndex = 0;
			nextPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
		}
		else
		{
			Debug.LogWarning("No patrol points generated.");
			isActionRunning = false;
		}

		Debug.Log("Entering PatrolAction");
		base.StartAction();
	}

	public override void StopAction()
	{
		isActionRunning = false;
		Debug.Log("Exiting PatrolAction");
		base.StopAction();
	}

	public override void UpdateAction()
	{
		if (!isActionRunning)
		{
			return;
		}

		if (patrolPoints.Length > 0)
		{
			Vector2 currentTarget = patrolPoints[currentPointIndex];
			Vector2 nextTarget = patrolPoints[nextPointIndex];

			// Calcule la direction vers le point actuel
			Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;

			// Calcule la distance restante vers le point actuel
			float distanceToCurrent = Vector2.Distance(transform.position, currentTarget);

			// Si l'on est proche du point actuel, commence à se diriger vers le prochain point
			if (distanceToCurrent <= weightTransitionDistance)
			{
				float weight = 1 - (distanceToCurrent / weightTransitionDistance);
				Vector2 nextDirection = (nextTarget - (Vector2)transform.position).normalized;
				direction = Vector2.Lerp(direction, nextDirection, weight);
			}

			// Déplace l'objet en fonction de la direction calculée
			transform.position += (Vector3)(direction * patrolSpeed * Time.deltaTime);

			// Si l'on atteint le point actuel, passe au point suivant
			if (distanceToCurrent <= reachThreshold)
			{
				currentPointIndex = nextPointIndex;
				nextPointIndex = (nextPointIndex + 1) % patrolPoints.Length;
			}
		}

		// Vérifier la condition de transition
		if (ShouldTransition())
		{
			StopAction();
		}
	}

	private void GeneratePatrolPoints()
	{
		patrolPoints = new Vector2[numberOfPatrolPoints];
		for (int i = 0; i < numberOfPatrolPoints; i++)
		{
			Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
			patrolPoints[i] = (Vector2)transform.position + randomPoint;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, patrolRadius);

		if (patrolPoints != null)
		{
			foreach (Vector2 point in patrolPoints)
			{
				Gizmos.DrawSphere(point, 0.2f);
			}
		}
	}
}
