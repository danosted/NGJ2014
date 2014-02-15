using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField]
	private EnemyType type;
	private EnemyManager enemyManager;
	private bool isShot;
	
	public void Init()
	{
		Transform[] enemyPath = this.enemyManager.GetEnemyPath();
		GetComponent<EnemyMovement>().SetEnemyPath(enemyPath);
		this.transform.position = enemyPath[0].position;
		isShot = false;
	}

	public void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.GetComponent<Projectile>())
		{
			this.GetComponent<Rigidbody2D>().WakeUp();
			transform.collider2D.isTrigger = false;
			isShot = true;
		}
	}

	public void SetEnemyManager(EnemyManager enemyManager)
	{
		this.enemyManager = enemyManager;
	}
	public bool GetIsShot()
	{
		return this.isShot;
	}
	
	public EnemyType GetEnemyType()
	{
		return this.type;
	}
}

public enum EnemyType
{
	Butterfly,
	Rabbit,
}
