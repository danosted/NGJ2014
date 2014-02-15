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
	private int enemySpawnMin;
	private int enemySpawnMax;
	private Dictionary<EnemyType, Transform[]> enemyPathsLeft;
	private Dictionary<EnemyType, Transform[]> enemyPathsRight;

	[SerializeField]
	private int rabbitSpecialNumber;
	private int rabbitSpawnCount;
	
	void Awake()
	{
		this.Init();
	}
	
	void Init () 
	{
		enemyPool = new List<Enemy>();
		enemySpawnLimits = new Vector2(1, 3);
		enemySpawnMin = 0;
		enemySpawnMax = 1;

		rabbitSpawnCount = 0;
		
		enemyPathsLeft = new Dictionary<EnemyType, Transform[]>();
		enemyPathsRight = new Dictionary<EnemyType, Transform[]>();

		GenerateEnemyPaths();

		GameManager.Instance.OnStateChanged += this.OnStateChanged;

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
		return enemy;
	}
	
	private void SpawnEnemyLoop()
	{
		Enemy enemy = GetAvailableEnemy((EnemyType)Random.Range(enemySpawnMin, enemySpawnMax + 1));
		enemy.Init();
		enemy.SetEnemyManager(this);
		if (enemy.GetEnemyType() == EnemyType.Rabbit && (rabbitSpawnCount < rabbitSpecialNumber))
		{
			rabbitSpawnCount++;
			if (rabbitSpawnCount >= rabbitSpecialNumber)
			{
				enemy.SetIsSpecial(true);
			}
		}
		
		Invoke("SpawnEnemyLoop", Random.Range (enemySpawnLimits.x, enemySpawnLimits.y));
	}
	private void GenerateEnemyPaths()
	{
		foreach(Enemy enemy in enemyPrefabs)
		{
			EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
			Transform[] pathNodesRight = new Transform[enemyMovement.GetEnemyPathObject().transform.childCount];
			Transform[] pathNodesLeft  = new Transform[pathNodesRight.Length];
			for(int i = 0; i < pathNodesLeft.Length; i++)
			{
				pathNodesRight[i] = ((GameObject)Instantiate(enemyMovement.GetEnemyPathObject().transform.GetChild(i).gameObject)).transform;
				pathNodesLeft[i]  = ((GameObject)Instantiate(pathNodesRight[i].gameObject)).transform;
				pathNodesLeft[i].position = new Vector3(-pathNodesLeft[i].position.x, pathNodesLeft[i].position.y, pathNodesLeft[i].position.z);
			}
			enemyPathsRight.Add(enemy.GetEnemyType(), pathNodesRight);
			enemyPathsLeft.Add(enemy.GetEnemyType(), pathNodesLeft);
		}
	}
	public Transform[] GetEnemyPath(EnemyType enemyType, Orientation orientation)
	{
		
		Transform[] enemyPath;
		if (orientation == Orientation.Left)
		{
			enemyPath = enemyPathsLeft[enemyType];
		}
		else
		{
			enemyPath = enemyPathsRight[enemyType];
		}
		return enemyPath;
	}

	private void OnStateChanged(int currentState, float changeTime)
	{
		if (currentState + 1 < enemyPrefabs.Count)
		{
			enemySpawnMin++;
			enemySpawnMax++;
		}
	}
}
