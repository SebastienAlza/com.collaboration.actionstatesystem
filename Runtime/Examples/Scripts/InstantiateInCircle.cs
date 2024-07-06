using UnityEngine;

public class InstantiateInCircle : MonoBehaviour
{
	public GameObject objectToInstantiate;
	public float radius = 5f; // Rayon de la zone circulaire
	public int numberOfObjects = 10; // Nombre d'objets à instancier

	void Start()
	{
		InstantiateObjects();
	}

	void InstantiateObjects()
	{
		for (int i = 0; i < numberOfObjects; i++)
		{
			// Calcul des coordonnées aléatoires à l'intérieur du cercle
			float angle = Random.Range(0f, Mathf.PI * 2); // Angle aléatoire en radians
			Vector3 randomPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * Random.Range(0f, radius);

			// Instanciation de l'objet à la position calculée
			Instantiate(objectToInstantiate, transform.position + randomPosition, Quaternion.identity);
		}
	}

	// Pour dessiner la zone circulaire dans l'éditeur Unity
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
