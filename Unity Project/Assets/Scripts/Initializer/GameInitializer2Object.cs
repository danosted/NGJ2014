using UnityEngine;
using System.Collections;

public class GameInitializer2Object : MonoBehaviour {

	public delegate void OnInitializedDelegate();
	public event OnInitializedDelegate OnInitialize;

	public delegate void OnInitializedWithDependenciesDelegate(GameObject[] dependencies);
	public event OnInitializedWithDependenciesDelegate OnInitializeWithDependencies;

	public void Initialize()
	{
		if(OnInitialize != null)
		{
			OnInitialize();
		}
		else
		{
			Debug.Log ("No event connected", gameObject);
		}
	}

	public void Initialize(GameObject[] dependencies)
	{
		if(OnInitializeWithDependencies != null)
		{
			OnInitializeWithDependencies(dependencies);
		}
		else
		{
			Debug.Log ("No event connected", gameObject);
		}
	}

}
