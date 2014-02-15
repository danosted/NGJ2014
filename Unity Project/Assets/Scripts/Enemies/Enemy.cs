using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField]
	private EnemyType type;
	private EnemyManager enemyManager;
	private bool isShot;
	
	public void Init()
	{
		isShot = false;
		this.enabled = true;
		this.gameObject.SetActive(true);
		GetComponent<EnemyMovement>().Init();
	}

	public void DeactivateObject()
	{
		this.gameObject.SetActive(false);
	}

    void OnTriggerEnter2D(Collider2D collider)
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
