using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	private List<Enemy> enemyPool;

	[SerializeField]
	private GameObject enemyPathObject;
	[SerializeField]
	private List<Enemy> enemyPrefabs;

	void Awake()
	{
		this.Init();
	}

	void Init () 
	{
		enemyPool = new List<Enemy>();
	}
	 
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.B))
		{
			GetAvailableEnemy(EnemyType.Butterfly).Init();
		}
		else if(Input.GetKeyDown(KeyCode.R))
		{
			GetAvailableEnemy(EnemyType.Rabbit).Init();
		}
	}

	private Enemy GetAvailableEnemy(EnemyType enemyType)
	{
		Enemy enemy = enemyPool.Find(e => e.GetEnemyType() == enemyType && !e.gameObject.activeSelf);
		if (!enemy)
		{
			enemy = InstantiateEnemy(enemyType);
			enemyPool.Add(enemy);
		}
		return enemy;
	}

	private Enemy InstantiateEnemy(EnemyType enemyType)
	{
		Enemy enemy = ((GameObject)Instantiate(enemyPrefabs.Find(e => e.GetEnemyType() == enemyType).gameObject)).GetComponent<Enemy>();
		enemy.SetEnemyManager(this);
		enemy.Init();
		return enemy;
	}
}
