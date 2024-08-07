using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace ActionStateSystem.Runtime
{
	public class APatrol : BaseAction
	{
		public float speedMin = 2f;
		public float speedMax = 4f;
		public bool followHeaded = false;
		public float patrolRadius = 10f; // Rayon de la patrouille
		public int numberOfPatrolPoints = 5; // Nombre de points de patrouille
		public float reachThreshold = 0.5f; // Distance � laquelle on consid�re que le point est atteint
		public int samplesPerSegment = 10; // Nombre d'�chantillons par segment de spline
		public float elasticity = 0.1f; // Facteur d'�lasticit� pour les transitions douces

		private Vector2[] patrolPoints; // Points de patrouille
		private NativeArray<Vector2> sampledPath; // Chemin �chantillonn� pour le mouvement � vitesse constante
		private int currentSampleIndex = 0; // Index de l'�chantillon actuel
		private float patrolSpeed; // Vitesse de patrouille
		private bool isActionRunning; // Indique si l'action est en cours
		private Vector2 currentVelocity; // Vitesse actuelle pour l'interpolation douce

		protected override void Awake()
		{
			actionName = "Patrol Action";
		}

		public override void StartAction()
		{
			isActionRunning = true;
			patrolSpeed = UnityEngine.Random.Range(speedMin, speedMax);

			GeneratePatrolPoints();
			GenerateSampledPath();

			if (sampledPath.Length > 0)
			{
				currentSampleIndex = 0;
			}
			else
			{
				isActionRunning = false;
			}

			base.StartAction();
		}

		public override void StopAction()
		{
			isActionRunning = false;
			CleanupNativeArrays(); // Cleanup NativeArrays when stopping the action
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (!isActionRunning || sampledPath.Length == 0)
			{
				return;
			}

			Vector2 targetPosition = sampledPath[currentSampleIndex];
			Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
			currentVelocity = Vector2.Lerp(currentVelocity, direction * patrolSpeed, Time.deltaTime / elasticity);
			transform.position += (Vector3)(currentVelocity * Time.deltaTime);

			if (Vector2.Distance(transform.position, targetPosition) <= reachThreshold)
			{
				currentSampleIndex = (currentSampleIndex + 1) % sampledPath.Length;
			}

			// Orienter l'objet pour regarder vers la cible
			if (followHeaded)
			{
				transform.right = Vector3.Lerp(transform.right, currentVelocity.normalized, Time.deltaTime / elasticity); // Utilisez Lerp pour une transition plus fluide
			}

			// V�rifier la condition de transition
			if (ShouldTransition())
			{
				StopAction();
			}
		}

		private void GeneratePatrolPoints()
		{
			patrolPoints = new Vector2[numberOfPatrolPoints];
			Vector2 initialPosition = transform.position; // Cache initial position to avoid multiple calls to transform.position
			for (int i = 0; i < numberOfPatrolPoints; i++)
			{
				Vector2 randomPoint;
				bool pointIsValid;

				do
				{
					pointIsValid = true;
					randomPoint = Random.insideUnitCircle * patrolRadius;
					patrolPoints[i] = initialPosition + randomPoint;

					// Check the distance to previous points
					for (int j = 0; j < i; j++)
					{
						if (Vector2.Distance(patrolPoints[i], patrolPoints[j]) < reachThreshold * 2) // Adjust this threshold as needed
						{
							pointIsValid = false;
							break;
						}
					}
				}
				while (!pointIsValid);
			}
		}

		private void GenerateSampledPath()
		{
			int totalSamples = numberOfPatrolPoints * samplesPerSegment;
			if (sampledPath.IsCreated)
			{
				sampledPath.Dispose();
			}
			sampledPath = new NativeArray<Vector2>(totalSamples, Allocator.Persistent);

			var job = new GenerateSampledPathJob
			{
				patrolPoints = new NativeArray<Vector2>(patrolPoints, Allocator.TempJob),
				samplesPerSegment = samplesPerSegment,
				sampledPath = sampledPath
			};

			JobHandle jobHandle = job.Schedule(totalSamples, 1);
			jobHandle.Complete();

			job.patrolPoints.Dispose();
		}

		[BurstCompile]
		private struct GenerateSampledPathJob : IJobParallelFor
		{
			[ReadOnly] public NativeArray<Vector2> patrolPoints;
			public int samplesPerSegment;
			public NativeArray<Vector2> sampledPath;

			public void Execute(int index)
			{
				int n = patrolPoints.Length;
				int segmentIndex = index / samplesPerSegment;
				float t = (index % samplesPerSegment) / (float)samplesPerSegment;

				Vector2 p0 = patrolPoints[(segmentIndex - 1 + n) % n];
				Vector2 p1 = patrolPoints[segmentIndex];
				Vector2 p2 = patrolPoints[(segmentIndex + 1) % n];
				Vector2 p3 = patrolPoints[(segmentIndex + 2) % n];

				sampledPath[index] = CatmullRomSpline(t, p0, p1, p2, p3);
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
		}

		private void OnDestroy()
		{
			CleanupNativeArrays();
		}

		private void CleanupNativeArrays()
		{
			if (sampledPath.IsCreated)
			{
				sampledPath.Dispose();
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, patrolRadius);
		}
	}
}
