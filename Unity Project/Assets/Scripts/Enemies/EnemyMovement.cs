using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private Transform[] enemyPath;
	private float pathProgress;
	[SerializeField]
	private float MovementSpeed;

	public void Init()
	{
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
	public void SetEnemyPath(Transform[] enemyPath)
	{
		this.enemyPath = enemyPath;
	}
}
