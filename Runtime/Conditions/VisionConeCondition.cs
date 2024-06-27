using UnityEditor;
using UnityEngine;

public enum VisionConeEventType
{
	Enter,
	Exit,
	Stay
}

public class VisionConeCondition : BaseCondition
{
	[SerializeField] public float radius = 1.0f; // Radius of the vision cone
	[SerializeField] public float viewAngle = 90f; // Angle of the vision cone
	[TagSelector]
	[SerializeField] private string targetTag = "Enemy"; // Tag of the target

	public VisionConeEventType eventType = VisionConeEventType.Enter; // Default event type

	private bool conditionMetPreviously = false; // Keeps track of the previous state

	public override bool IsMet()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
		bool foundTarget = false;

		foreach (var hit in hits)
		{
			if (hit.CompareTag(targetTag) && IsInFieldOfView(hit.transform))
			{
				foundTarget = true;

				switch (eventType)
				{
					case VisionConeEventType.Enter:
						if (!conditionMetPreviously) // Condition is met if it was not met previously
						{
							conditionMetPreviously = true;
							return true;
						}
						break;

					case VisionConeEventType.Stay:
						if (conditionMetPreviously) // Condition is met if it was already met
						{
							return true;
						}
						break;
				}
			}
		}

		// If the event is "Exit", check if no collider with the specified tag is still in the area
		if (eventType == VisionConeEventType.Exit && conditionMetPreviously && !foundTarget)
		{
			conditionMetPreviously = false;
			return true;
		}

		// Reset the previous state if the target is not found
		if (!foundTarget)
		{
			conditionMetPreviously = false;
		}

		return false;
	}

	private bool IsInFieldOfView(Transform target)
	{
		Vector2 directionToTarget = (target.position - transform.position).normalized;
		float angle = Vector2.Angle(transform.right, directionToTarget); // Assuming the viewing direction is 'right'
		return angle < viewAngle / 2f;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);

		// Draw the vision cone
		Vector3 forward = transform.right; // Assuming the viewing direction is 'right'
		Vector3 leftBoundary = Quaternion.Euler(0, 0, viewAngle / 2f) * forward * radius;
		Vector3 rightBoundary = Quaternion.Euler(0, 0, -viewAngle / 2f) * forward * radius;

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
		Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(VisionConeCondition))]
public class VisionConeConditionEditor : Editor
{
	private void OnSceneGUI()
	{
		VisionConeCondition visionCone = (VisionConeCondition)target;

		Handles.color = Color.blue;
		Vector3 forward = visionCone.transform.right; // Assuming the viewing direction is 'right'
		Vector3 leftBoundary = Quaternion.Euler(0, 0, visionCone.viewAngle / 2f) * forward * visionCone.radius;
		Vector3 rightBoundary = Quaternion.Euler(0, 0, -visionCone.viewAngle / 2f) * forward * visionCone.radius;

		Handles.DrawLine(visionCone.transform.position, visionCone.transform.position + leftBoundary);
		Handles.DrawLine(visionCone.transform.position, visionCone.transform.position + rightBoundary);
		Handles.DrawWireArc(visionCone.transform.position, Vector3.forward, leftBoundary, visionCone.viewAngle, visionCone.radius);
	}
}
#endif
