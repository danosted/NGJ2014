using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private Transform[] enemyPath;
	private int enemyPathIndex;
	private Enemy enemyReference;
	private float pathProgress;
	private EnemyManager enemyManager;
	[SerializeField]
	private float movementSpeed;
	[SerializeField]
	private GameObject enemyPathObject;
	private Orientation orientation;

	public void Init(Enemy enemyReference)
	{
		this.enemyReference = enemyReference;
		this.orientation = (Random.value > 0.5f ? Orientation.Right : Orientation.Right);
		this.pathProgress = 0;
		this.GetComponent<Rigidbody2D>().Sleep();
	}

	public void Deactivate()
	{
		this.enabled = false;
		rigidbody2D.gravityScale = 0.3f;
	}

	void Update () 
	{
		if (pathProgress >= 1)
		{
			this.Deactivate();
		}
		else if(!enemyReference.GetIsSpecialAnimating())
		{
			if (enemyPath == null)
			{   
				// find enemyPath!
				if (!enemyManager)
				{
					// Find enemyManager!
					if (!enemyReference)
					{
						enemyReference = GetComponent<Enemy>();
					}
					enemyManager = enemyReference.GetEnemyManager();
				}
				enemyPath = enemyManager.GetEnemyPath(enemyReference.GetEnemyType(), orientation);
				transform.position = enemyPath[0].position;
				enemyPathIndex++;
			}
			transform.position = Vector3.MoveTowards(transform.position, enemyPath[enemyPathIndex].position, Time.deltaTime * movementSpeed);
			if (transform.position == enemyPath[enemyPathIndex].position)
			{
				if (++enemyPathIndex == enemyPath.Length)
				{
					this.Deactivate();
				}
			}
		}
	}

	public void GotShot(Projectile projectile)
	{
		bool isDead = enemyReference.GetHealth() <= 0;
		if (isDead)
		{
			collider2D.isTrigger = false;
			rigidbody2D.AddForce(new Vector2(projectile.GetDirection().y, -projectile.GetDirection().x * ((-projectile.GetDirection().x < 0) ? -1 : 1)) * 80);
			this.Deactivate();
		}
	}
	public GameObject GetEnemyPathObject()
	{
		return this.enemyPathObject;
	}
}
