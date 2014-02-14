using UnityEngine;
using System.Collections;

[System.Serializable]
public class MultidimensionalGameObject {

	[SerializeField]
	private GameObject dependentGameObject;
	[SerializeField]
	private GameObject[] dependencies;
	
	public int Length {
		get {
			return dependencies.Length;
		}
	}

	public GameObject GetDependent()
	{
		return dependentGameObject;
	}

	public GameObject[] GetDependencies()
	{
		return dependencies;
	}
}
