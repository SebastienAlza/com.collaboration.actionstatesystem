using UnityEngine;

public class EnemyColliderManager : MonoBehaviour
{
	public float activationDistance = 50f; // Distance à laquelle le collider sera activé
	private Transform playerTransform;
	private Collider2D enemyCollider;
	private bool isColliderActive;

	void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform; // Assurez-vous que le joueur a le tag "Player"
		enemyCollider = GetComponent<Collider2D>();
		isColliderActive = enemyCollider.enabled;
	}

	void Update()
	{
		if (playerTransform == null) return;
		float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
		if (distanceToPlayer <= activationDistance && !isColliderActive)
		{
			ActivateCollider();
		}
		else if (distanceToPlayer > activationDistance && isColliderActive)
		{
			DeactivateCollider();
		}
	}

	void ActivateCollider()
	{
		enemyCollider.enabled = true;
		isColliderActive = true;
	}

	void DeactivateCollider()
	{
		enemyCollider.enabled = false;
		isColliderActive = false;
	}
}
