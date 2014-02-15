using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField]
	private EnemyType type;
	private EnemyManager enemyManager;
	
	public void Init()
	{
		Transform[] enemyPath = this.enemyManager.GetEnemyPath();
		GetComponent<EnemyMovement>().SetEnemyPath(enemyPath);
		this.transform.position = enemyPath[0].position;
	}

	public EnemyType GetEnemyType()
	{
		return this.type;
	}
	
	public void SetEnemyManager(EnemyManager enemyManager)
	{
		this.enemyManager = enemyManager;
	}
}

public enum EnemyType
{
	Butterfly,
	Rabbit,
}
