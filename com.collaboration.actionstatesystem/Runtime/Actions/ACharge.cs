using UnityEngine;

public class ACharge : BaseAction
{
	public float chargeSpeed = 5f; // Vitesse de charge par seconde
	public float chargeDuration = 1f; // Dur�e de la charge en secondes
	private float chargeStartTime; // Temps de d�but de la charge
	private Vector3 chargeDirection; // Direction de la charge

	public LayerMask targetLayerMask; // LayerMask pour trouver la cible
	public float detectionRadius = 5f; // Rayon de d�tection pour trouver la cible

	private bool isActionRunning; // Indique si l'action est en cours
	private Transform target; // Cible de la charge

	protected override void Awake()
	{
		actionName = "## > Charge Action";
	}

	public override void Execute()
	{
		// Code � ex�cuter imm�diatement lorsque l'action est d�clench�e
	}

	public override void StartAction()
	{
		isActionRunning = true;
		chargeStartTime = Time.time;

		// Trouver la cible
		target = FindTarget();

		if (target != null)
		{
			chargeDirection = (target.position - transform.position).normalized;
		}

		Debug.Log("Entering ChargeAction");
		base.StartAction();
	}

	public override void StopAction()
	{
		isActionRunning = false;
		Debug.Log("Exiting ChargeAction");
		base.StopAction();
	}

	public override void UpdateAction()
	{
		if (!isActionRunning)
		{
			return;
		}

		if (target != null)
		{
			transform.position += chargeDirection * chargeSpeed * Time.deltaTime;

			if (Time.time - chargeStartTime >= chargeDuration)
			{
				StopAction();
			}
		}

		// V�rifier la condition de transition
		if (ShouldTransition())
		{
			StopAction();
		}
	}

	private Transform FindTarget()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayerMask);
		if (hits.Length > 0)
		{
			// Retourne la position du premier collider trouv�
			return hits[0].transform;
		}
		return null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}
}
