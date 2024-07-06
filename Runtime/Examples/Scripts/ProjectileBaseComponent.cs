using System.Collections;
using UnityEngine;

public class ProjectileBaseComponent : MonoBehaviour
{
	public float speed = 10f;
	public float damage = 20f;
	private Vector2 direction;

	public void SetDirection(Vector2 dir)
	{
		direction = dir.normalized;
	}

	private void OnEnable()
	{
		StartCoroutine(MoveProjectile());
	}

	private void OnDisable()
	{
		StopCoroutine(MoveProjectile());
	}

	private IEnumerator MoveProjectile()
	{
		while (true)
		{
			// Déplacement du projectile
			transform.Translate(direction * speed * Time.deltaTime, Space.World);

			// Vérification si le projectile est en dehors des limites de la caméra
			Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
			if (position.x < 0 || position.x > 1 || position.y < 0 || position.y > 1)
			{
				Destroy(gameObject);
				yield break; // Quitter la coroutine
			}

			// Attendre la fin de la frame avant de continuer la boucle
			yield return null;
		}
	}
}
