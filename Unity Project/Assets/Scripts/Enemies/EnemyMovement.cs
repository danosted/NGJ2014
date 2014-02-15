using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private Transform[] enemyPath;
	private float pathProgress;
	[SerializeField]
	private float MovementSpeed;
	[SerializeField]
	private GameObject enemyPathObject;

	public void Init()
	{
		if (enemyPath == null)
		{
			enemyPath = this.GetEnemyPath(enemyPathObject);
		}
		this.transform.position = enemyPath[0].position;
		this.pathProgress = 0;
		this.GetComponent<Rigidbody2D>().Sleep();
	}

	private void Deactivate()
	{
		this.enabled = false;
	}

	void Update () 
	{
		if (GetComponent<Enemy>().GetIsShot() || pathProgress >= 1)
		{
			this.Deactivate();
		}
		iTween.PutOnPath(gameObject, enemyPath, pathProgress);
		pathProgress += MovementSpeed * 0.01f * Time.deltaTime;
	}
	private Transform[] GetEnemyPath(GameObject enemyPathObject)
	{
		int enemyPathSize = 1;
		for(int i = 0; i < enemyPathObject.transform.childCount - 1; i++)
		{
			enemyPathSize += Mathf.FloorToInt((enemyPathObject.transform.GetChild(i + 1).position - enemyPathObject.transform.GetChild(i).position).magnitude);
		}
		Transform[] enemyPath = new Transform[enemyPathSize];
		int index = 0;
		
		for(int i = 0; i < enemyPathObject.transform.childCount; i++)
		{
			if (i + 1 < enemyPathObject.transform.childCount)
			{
				Transform currentNode = enemyPathObject.transform.GetChild(i);
				Transform nextNode = enemyPathObject.transform.GetChild(i + 1);
				int nodeAmount = Mathf.FloorToInt((nextNode.position - currentNode.position).magnitude);
				for(int j = 0; j < nodeAmount; j++)
				{
					GameObject newNodeObject = (GameObject)(Instantiate(currentNode.gameObject, currentNode.position + (nextNode.position - currentNode.position) / nodeAmount * j, Quaternion.identity));
//					newNodeObject.transform.parent = enemyManager.gameObject.transform;
					Transform newNode = newNodeObject.transform;
					enemyPath[index++] = newNode;
				}
			}
			else
			{
				enemyPath[index] = enemyPathObject.transform.GetChild(i);
			}
		}
		return enemyPath;
	}
}
