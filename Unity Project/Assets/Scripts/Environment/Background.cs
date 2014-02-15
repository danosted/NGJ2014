using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	[SerializeField]
	private Transform[] clouds;
	[SerializeField]
	private Transform start;
	[SerializeField]
	private Transform end;

	void Start()
	{
		StartCoroutine(UpdatePositions());
	}

	private IEnumerator UpdatePositions () 
	{
		while(true)
		{
			float speed = Random.Range(1f, 4f);
			for(int i = 0; i < clouds.Length; i++)
			{
				if(clouds[i].position.x == end.position.x)
				{
					clouds[i].position = new Vector3(start.position.x, clouds[i].position.y, clouds[i].position.z);
				}
				clouds[i].position = Vector3.MoveTowards(clouds[i].position, new Vector3(end.position.x, clouds[i].position.y, clouds[i].position.z), Time.deltaTime * speed);
			}
			yield return null;
		}
	}
}
