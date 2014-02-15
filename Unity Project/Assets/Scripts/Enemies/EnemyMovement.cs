using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private Transform[] enemyPath;
	private float pathProgress;
	[SerializeField]
	private float MovementSpeed;
	private bool isDead;

	public void Init()
	{
		this.pathProgress = 0;
		this.isDead = false;
		this.GetComponent<Rigidbody2D>().Sleep();
	}

	private void Deactivate()
	{
		gameObject.SetActive(false);
	}
	
	public void OnTriggerEnter2D(Collider2D collider)
	{
//		if (!isDead && collider.GetComponent<Projectile>())
//		{
//			isDead = true;
//			this.GetComponent<Rigidbody2D>().IsAwake = true;
//		}
//		else if(isDead && collider.GetComponent<Enemy>())                         // PUSH BACK OTHER ENEMIES
//		{
//
//		}
	}

	void Update () 
	{
		if (!isDead)
		{
			if (pathProgress >= 1)
			{
				this.GetComponent<Rigidbody2D>().WakeUp();
				transform.collider2D.isTrigger = false;
				this.isDead = true;
			}
			iTween.PutOnPath(gameObject, enemyPath, pathProgress);
			pathProgress += MovementSpeed * 0.01f * Time.deltaTime;
		}
	}

	public void SetEnemyPath(Transform[] enemyPath)
	{
		this.enemyPath = enemyPath;
	}
}
