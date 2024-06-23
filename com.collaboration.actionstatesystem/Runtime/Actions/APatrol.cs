using UnityEngine;
using System.Collections.Generic;

public class APatrol : BaseAction
{
	public float speedMin = 2f;
	public float speedMax = 4f;
	public float patrolRadius = 10f; // Rayon de la patrouille
	public int numberOfPatrolPoints = 5; // Nombre de points de patrouille
	public float reachThreshold = 0.5f; // Distance à laquelle on considère que le point est atteint
	public int samplesPerSegment = 10; // Nombre d'échantillons par segment de spline

	private Vector2[] patrolPoints; // Points de patrouille
	private List<Vector2> sampledPath; // Chemin échantillonné pour le mouvement à vitesse constante
	private int currentSampleIndex = 0; // Index de l'échantillon actuel
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
		GenerateSampledPath();

		if (sampledPath.Count > 0)
		{
			currentSampleIndex = 0;
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
		if (!isActionRunning || sampledPath.Count == 0)
		{
			return;
		}

		Vector2 targetPosition = sampledPath[currentSampleIndex];
		Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
		transform.position += (Vector3)(direction * patrolSpeed * Time.deltaTime);

		if (Vector2.Distance(transform.position, targetPosition) <= reachThreshold)
		{
			currentSampleIndex = (currentSampleIndex + 1) % sampledPath.Count;
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
			Debug.Log($"Generated Patrol Point {i}: {patrolPoints[i]}");
		}
	}

	private void GenerateSampledPath()
	{
		sampledPath = new List<Vector2>();
		int n = patrolPoints.Length;

		for (int i = 0; i < n; i++)
		{
			Vector2 p0 = patrolPoints[(i - 1 + n) % n];
			Vector2 p1 = patrolPoints[i];
			Vector2 p2 = patrolPoints[(i + 1) % n];
			Vector2 p3 = patrolPoints[(i + 2) % n];

			for (int j = 0; j <= samplesPerSegment; j++)
			{
				float t = j / (float)samplesPerSegment;
				Vector2 pointOnSpline = CatmullRomSpline(t, p0, p1, p2, p3);
				sampledPath.Add(pointOnSpline);
			}
		}
	}

	private Vector2 CatmullRomSpline(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float tt = t * t;
		float ttt = tt * t;

		float q0 = -ttt + 2.0f * tt - t;
		float q1 = 3.0f * ttt - 5.0f * tt + 2.0f;
		float q2 = -3.0f * ttt + 4.0f * tt + t;
		float q3 = ttt - tt;

		float x = 0.5f * (p0.x * q0 + p1.x * q1 + p2.x * q2 + p3.x * q3);
		float y = 0.5f * (p0.y * q0 + p1.y * q1 + p2.y * q2 + p3.y * q3);

		return new Vector2(x, y);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, patrolRadius);

		if (patrolPoints != null)
		{
			for (int i = 0; i < patrolPoints.Length; i++)
			{
				Gizmos.DrawSphere(patrolPoints[i], 0.2f);
			}

			// Dessiner les courbes de Catmull-Rom
			Gizmos.color = Color.yellow;
			for (int i = 0; i < patrolPoints.Length; i++)
			{
				Vector2 p0 = patrolPoints[(i - 1 + patrolPoints.Length) % patrolPoints.Length];
				Vector2 p1 = patrolPoints[i];
				Vector2 p2 = patrolPoints[(i + 1) % patrolPoints.Length];
				Vector2 p3 = patrolPoints[(i + 2) % patrolPoints.Length];
				Vector2 previousPoint = p1;

				for (float t = 0; t <= 1; t += 1.0f / samplesPerSegment)
				{
					Vector2 pointOnSpline = CatmullRomSpline(t, p0, p1, p2, p3);
					Gizmos.DrawLine(previousPoint, pointOnSpline);
					previousPoint = pointOnSpline;
				}
			}

			// Dessiner le chemin échantillonné
			if (sampledPath != null)
			{
				Gizmos.color = Color.red;
				for (int i = 0; i < sampledPath.Count - 1; i++)
				{
					Gizmos.DrawLine(sampledPath[i], sampledPath[i + 1]);
				}
			}
		}
	}
}
