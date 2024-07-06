using System.Collections;
using System.Collections.Generic;
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

	void Update()
	{
		transform.Translate(direction * speed * Time.deltaTime, Space.World);

		// Check if the projectile is out of the camera bounds
		Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
		if (position.x < 0 || position.x > 1 || position.y < 0 || position.y > 1)
		{
			Destroy(gameObject);
		}
	}
}
