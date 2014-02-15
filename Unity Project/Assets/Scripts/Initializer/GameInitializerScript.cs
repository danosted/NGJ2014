using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializerScript : MonoBehaviour {

	[SerializeField]
	private GameObject startAnim;
	[SerializeField]
	private TextMesh startText;
	private bool isPlaying = true;

	[SerializeField]
	public GameObject[] nonDependents;

	[SerializeField]
	private MultidimensionalGameObject[] dependents;

	private List<GameObject> instantiatedDependencies = new List<GameObject>();
	private List<GameObject> instantiatedDependents = new List<GameObject>();

	private Dictionary<GameObject, GameObject[]> depdendent2dependencies_non = new Dictionary<GameObject, GameObject[]>();
	private Dictionary<GameObject, GameObject[]> depdendent2dependencies_inst = new Dictionary<GameObject, GameObject[]>();

	void Awake () 
	{
		StartCoroutine(StartAnimation());
	}

	private IEnumerator StartAnimation()
	{
		while(isPlaying)
		{
			if(Input.GetMouseButton(0))
			{
				isPlaying = false;
				Destroy(startAnim);
				Destroy(startText.gameObject);
			}
			yield return null;
		}
		InstantiateObjects();
	}

	private void InstantiateObjects()
	{
		InstantiateNonDependents();
		InstantiateDependents();
	}

	private void InstantiateNonDependents()
	{
		for(int i = 0; i < nonDependents.Length; i++)
		{
			GameObject GO = nonDependents[i];
			GO = Instantiate(GO, GO.transform.position, GO.transform.rotation) as GameObject;
			if(GO.GetComponent<GameInitializer2Object>())
			{
				GO.GetComponent<GameInitializer2Object>().Initialize();
			}
			else
			{
				Debug.Log ("missing script: GameInitializer2Object", GO);
			}
		}
	}

	private void InstantiateDependents()
	{
		for(int i = 0; i < dependents.GetLength(0); i++)
		{
			GameObject dependent = dependents[i].GetDependent();
			GameObject[] dependencies = dependents[i].GetDependencies();
			depdendent2dependencies_non.Add(dependent, dependencies);
			
			if(!instantiatedDependencies.Exists(x => (x.name).Equals(dependent.name+"(Clone)")) && 
			   !instantiatedDependents.Exists(x => (x.name).Equals(dependent.name+"(Clone)")))
			{
				dependent = Instantiate(dependent, dependent.transform.position, dependent.transform.rotation) as GameObject;
				instantiatedDependents.Add(dependent);
			}
			else if(instantiatedDependencies.Exists(x => (x.name).Equals(dependent.name+"(Clone)")))
			{
				dependent = instantiatedDependencies.Find(x => (x.name).Equals(dependent.name+"(Clone)"));
				instantiatedDependents.Add(dependent);
			}
			
			for(int j = 0; j < dependents[i].GetDependencies().Length; j++)
			{
				if(!instantiatedDependents.Exists(x => x.name.Equals(dependencies[j].name+"(Clone)")) &&
				   !instantiatedDependencies.Exists(x => x.name.Equals(dependencies[j].name+"(Clone)")))
				{
					GameObject dependency = Instantiate(dependencies[j], dependencies[j].transform.position, dependencies[j].transform.rotation) as GameObject;
					instantiatedDependencies.Add(dependency);
				}
				else if(instantiatedDependents.Exists(x => (x.name).Equals(dependencies[j].name+"(Clone)")))
				{
					GameObject dependency = instantiatedDependents.Find(x => (x.name).Equals(dependencies[j].name+"(Clone)"));
					instantiatedDependencies.Add (dependency);
				}
			}
		}
		OrderDependencies();
		SetUpDependencies();
	}

	private void OrderDependencies()
	{
		foreach(KeyValuePair<GameObject, GameObject[]> go in depdendent2dependencies_non)
		{
			GameObject dependent = instantiatedDependents.Find(x => x.name.Equals(go.Key.name+"(Clone)"));
			GameObject[] dependencies = new GameObject[go.Value.Length];
			for(int i = 0; i < go.Value.Length; i++)
			{
				dependencies[i] = instantiatedDependencies.Find(x => x.name.Equals(go.Value[i].name+"(Clone)"));
			}
			depdendent2dependencies_inst.Add(dependent, dependencies);
		}
	}

	private void SetUpDependencies()
	{
		foreach(KeyValuePair<GameObject, GameObject[]> go in depdendent2dependencies_inst)
		{
			if(go.Key.GetComponent<GameInitializer2Object>())
			{
				go.Key.GetComponent<GameInitializer2Object>().Initialize(go.Value);
			}
			else
			{
				Debug.Log ("missing script: GameInitializer2Object", go.Key);
			}
		}
	}
}
