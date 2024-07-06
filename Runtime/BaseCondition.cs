using UnityEngine;

public abstract class BaseCondition : MonoBehaviour
{
	[HideInInspector]
	public int uniqueID;

	public bool useDynamicData = false; // Option pour utiliser le radius dynamique
	public DataProperties dataProperties; // Référence au ScriptableObject générique
	public string attributeName; // Nom de l'attribut pour le radius dynamique
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
