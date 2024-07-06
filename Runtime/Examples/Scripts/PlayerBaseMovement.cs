using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovement : MonoBehaviour
{
	// Variables de vitesse
	public float speed = 5f;
	public float dashSpeed = 15f;
	public float dashDuration = 0.2f;
	public float dashCooldown = 1f;
	public float accelerationTime = 0.1f;
	public float decelerationTime = 0.1f;

	private bool isDashing = false;
	private float lastDashTime;

	void Update()
	{
		// Récupérer l'entrée du joueur
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		// Calculer le vecteur de mouvement
		Vector2 movement = new Vector2(moveHorizontal, moveVertical);

		// Appliquer le mouvement au GameObject
		if (!isDashing)
		{
			transform.Translate(movement * speed * Time.deltaTime);
		}

		// Vérifier si la touche de dash (espace) est pressée et si le cooldown est terminé
		if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDashTime + dashCooldown)
		{
			StartCoroutine(Dash(movement));
		}
	}

	IEnumerator Dash(Vector2 direction)
	{
		isDashing = true;
		lastDashTime = Time.time;

		// Accélération
		float currentSpeed = speed;
		float accelerationRate = (dashSpeed - speed) / accelerationTime;
		float time = 0f;

		while (time < accelerationTime)
		{
			currentSpeed += accelerationRate * Time.deltaTime;
			transform.Translate(direction * currentSpeed * Time.deltaTime);
			time += Time.deltaTime;
			yield return null;
		}

		// Maintenir la vitesse de dash
		time = 0f;
		while (time < dashDuration)
		{
			transform.Translate(direction * dashSpeed * Time.deltaTime);
			time += Time.deltaTime;
			yield return null;
		}

		// Décélération
		float decelerationRate = (dashSpeed - speed) / decelerationTime;
		time = 0f;

		while (time < decelerationTime)
		{
			currentSpeed -= decelerationRate * Time.deltaTime;
			transform.Translate(direction * currentSpeed * Time.deltaTime);
			time += Time.deltaTime;
			yield return null;
		}

		isDashing = false;
	}
}
