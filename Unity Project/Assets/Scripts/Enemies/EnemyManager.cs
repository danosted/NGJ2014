using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class EnemyManager : MonoBehaviour {
	
	private List<Enemy> enemyPool;
	[SerializeField]
	private List<Enemy> enemyPrefabs;
	[SerializeField]
	private Vector2 enemySpawnLimits;
	private int enemySpawnMin;
	private int enemySpawnMax;
	private Dictionary<EnemyType, Transform[]> enemyPathsLeft;
	private Dictionary<EnemyType, Transform[]> enemyPathsRight;

	[SerializeField]
	private int rabbitSpecialBound;
	private int rabbitSpecialNumber;
	private int rabbitSpawnCount;
	
	void Awake()
	{
		this.Init();
	}
	
	void Init () 
	{
		enemySpawnLimits = new Vector2(1, 3);
		enemySpawnMin = 0;
		enemySpawnMax = 1;

		rabbitSpawnCount = 0;
		rabbitSpecialNumber = Random.Range (2, rabbitSpecialBound);
		
		enemyPathsLeft = new Dictionary<EnemyType, Transform[]>();
		enemyPathsRight = new Dictionary<EnemyType, Transform[]>();

		GenerateEnemyPaths();

		GameManager.Instance.OnStateChanged += this.OnStateChanged;

		Invoke("SpawnEnemyLoop", Random.Range (enemySpawnLimits.x, enemySpawnLimits.y));
	}
	
	private Enemy GetAvailableEnemy(EnemyType enemyType)
	{
		Enemy enemy = InstantiateEnemy(enemyType);
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
		if (enemy.GetEnemyType() == EnemyType.Rabbit && GameManager.Instance.GetCurrentState() == 0)
		{
			rabbitSpawnCount++;
			if (rabbitSpawnCount % rabbitSpecialNumber == 0)
			{
				enemy.SetIsSpecial(true);

			}
		}

		enemySpawnLimits.x = 0.25f + (enemySpawnLimits.x - 0.25f) * 0.95f;
		enemySpawnLimits.y = 0.5f + (enemySpawnLimits.y - 0.5f) * 0.95f;

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

	private void OnStateChanged(int currentState, float stateChangeTime)
	{
		if (currentState + 1 < enemyPrefabs.Count)
		{
			StartCoroutine("UpdateState", stateChangeTime);
		}
		if(currentState == 4)
		{
			Destroy(gameObject);
		}
	}
	private IEnumerator UpdateState(float stateChangetime)
	{
		yield return new WaitForSeconds(stateChangetime);
		enemySpawnMax++;
	}
}
