using UnityEngine;

public abstract class BaseCondition : MonoBehaviour
{
	public int uniqueID;

	public bool IsActive { get; private set; }

	public void Activate()
	{
		IsActive = true;
		OnActivate();
	}

	public void Deactivate()
	{
		IsActive = false;
		OnDeactivate();
	}

	public abstract bool IsMet();

	protected virtual void OnActivate() { }
	protected virtual void OnDeactivate() { }
}
