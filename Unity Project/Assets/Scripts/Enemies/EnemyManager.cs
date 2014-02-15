using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour {

	private List<Enemy> enemyPool;

	[SerializeField]
	private GameObject enemyPathObject;
	[SerializeField]
	private List<Enemy> enemyPrefabs;
	private Vector2 enemySpawnLimits;
	private int enemySpawnCount;
	private List<int> enemySpawnIntervals;
	private int enemySpawnTypes;

	void Awake()
	{
		this.Init();
	}

	void Init () 
	{
		enemyPool = new List<Enemy>();
		enemySpawnLimits = new Vector2(2, 5);
		enemySpawnCount = 0;
		enemySpawnIntervals = new List<int>();
		enemySpawnIntervals.AddRange(new int[3] {3, 10, 100});
		enemySpawnTypes = 0;

		Invoke("SpawnEnemyLoop", Random.Range (enemySpawnLimits.x, enemySpawnLimits.y));
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

	private void SpawnEnemyLoop()
	{
		enemySpawnLimits *= 0.95f;
		Debug.Log ("EnemySpawnCount: " + enemySpawnCount);
		GetAvailableEnemy((EnemyType)Random.Range(0, enemySpawnTypes + 1)).Init();
		if (++enemySpawnCount >= enemySpawnIntervals[enemySpawnTypes])
		{
			enemySpawnTypes++;
			Debug.Log ("EnemySpawnTypes updated");
		}

		Invoke("SpawnEnemyLoop", Random.Range (enemySpawnLimits.x, enemySpawnLimits.y));
	}
}
