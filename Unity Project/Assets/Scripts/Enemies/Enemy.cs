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
		Projectile projectile = collider.GetComponent<Projectile>();
		if (projectile)
		{
			this.GetComponent<Rigidbody2D>().WakeUp();
			transform.collider2D.isTrigger = false;
			rigidbody2D.AddForce(new Vector2(projectile.GetDirection().x, projectile.GetDirection().y) * 10);
			rigidbody2D.AddForce(new Vector2(projectile.GetDirection().y, -projectile.GetDirection().x * 10) * 10);
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
