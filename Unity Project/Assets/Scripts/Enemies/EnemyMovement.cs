using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private Transform[] enemyPath;
	private Enemy enemyReference;
	private float pathProgress;
	[SerializeField]
	private float MovementSpeed;
	[SerializeField]
	private GameObject enemyPathObject;

	public void Init(Enemy enemyReference)
	{
		if (enemyPath == null)
		{
			enemyPath = this.GetEnemyPath(enemyPathObject);
		}
		this.enemyReference = enemyReference;
		this.transform.position = enemyPath[0].position;
		this.pathProgress = 0;
		this.GetComponent<Rigidbody2D>().Sleep();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Hill")
		{
			// Start movement along path again.
		}
	}

	private void Deactivate()
	{
		this.enabled = false;
	}

	void Update () 
	{
		if (pathProgress >= 1)
		{
			this.Deactivate();
		}
		else if(!enemyReference.GetIsSpecialAnimating())
		{
			// Don't move if flying from knockback.
			iTween.PutOnPath(gameObject, enemyPath, pathProgress);
			pathProgress += MovementSpeed * 0.01f * Time.deltaTime;
		}
	}

	public void GotShot(Projectile projectile)
	{
		bool isDead = enemyReference.GetHealth() <= 0;
		int factor = isDead ? 10 : 3;
//		rigidbody2D.AddForce(new Vector2(projectile.GetDirection().x, projectile.GetDirection().y) * factor);
//		rigidbody2D.AddForce(new Vector2(projectile.GetDirection().y, -projectile.GetDirection().x * factor) * 10);
		// Disable movement along path.
		if (isDead)
		{
			collider2D.isTrigger = false;
			this.Deactivate();
		}
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
				enemyPath[index] = ((GameObject)Instantiate(enemyPathObject.transform.GetChild(i).gameObject)).transform;
			}
		}
		if (Random.value >= 0.5f)
		{
			foreach(Transform pathNode in enemyPath)
			{
				pathNode.position = new Vector3(-pathNode.position.x, pathNode.position.y, pathNode.position.z);
			}
		}
		return enemyPath;
	}
}
