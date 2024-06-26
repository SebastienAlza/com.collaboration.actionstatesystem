using UnityEngine;
namespace ActionStateSystem.Runtime
{
	public class ASpawn : BaseAction
	{
		public GameObject prefab; // Le prefab � instancier
		public Vector2 areaSize = new Vector2(10f, 10f); // La taille de la zone o� les objets seront instanci�s
		public int numberOfInstances = 5; // Le nombre d'instances � cr�er
		public Transform spawnCenter; // Le centre de la zone de spawn

		private bool isActionRunning; // Indique si l'action est en cours

		protected override void Awake()
		{
			actionName = "## > Spawn Action";
		}

		public override void StartAction()
		{
			isActionRunning = true;

			// Assurer que le centre de la zone de spawn est d�fini
			if (spawnCenter == null)
			{
				spawnCenter = transform;
			}

			SpawnPrefabs();
			Debug.Log("Entering SpawnAction");
			base.StartAction();
		}

		public override void StopAction()
		{
			isActionRunning = false;
			Debug.Log("Exiting SpawnAction");
			base.StopAction();
		}

		public override void UpdateAction()
		{
			// Cette action peut �tre instantan�e, donc pas de mise � jour continue n�cessaire
			if (!isActionRunning)
			{
				return;
			}

			// V�rifier la condition de transition
			if (ShouldTransition())
			{
				StopAction();
			}
		}

		private void SpawnPrefabs()
		{
			for (int i = 0; i < numberOfInstances; i++)
			{
				Vector3 spawnPosition = GetRandomPosition();
				Instantiate(prefab, spawnPosition, Quaternion.identity);
			}
		}

		private Vector3 GetRandomPosition()
		{
			float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
			float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
			return new Vector3(spawnCenter.position.x + randomX, spawnCenter.position.y + randomY, spawnCenter.position.z);
		}

		private void OnDrawGizmosSelected()
		{
			if (spawnCenter == null)
			{
				spawnCenter = transform;
			}

			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(spawnCenter.position, new Vector3(areaSize.x, areaSize.y, 1));
		}
	}
}