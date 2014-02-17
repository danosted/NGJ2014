using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private Transform[] enemyPath;
	private int enemyPathIndex;
	private Enemy enemyReference;
	private float pathProgress;
	private EnemyManager enemyManager;
	[SerializeField]
	private float movementSpeed;
	[SerializeField]
	private GameObject enemyPathObject;
	private Orientation orientation;
	private static float punchAmount = 0.8f;

	public void Init(Enemy enemyReference)
	{
		this.enemyReference = enemyReference;
		this.orientation = (Random.value > 0.5f ? Orientation.Left : Orientation.Right);
		if (orientation == Orientation.Left)
		{
			this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
		}
		this.pathProgress = 0;
		this.GetComponent<Rigidbody2D>().Sleep();
	}

	public void Deactivate()
	{
		this.enabled = false;
		rigidbody2D.gravityScale = 0.3f;
	}

	void Update () 
	{
		if(!enemyReference.GetIsSpecialAnimating())
		{
			if (enemyPath == null)
			{   
				// find enemyPath!
				if (!enemyManager)
				{
					// Find enemyManager!
					if (!enemyReference)
					{
						enemyReference = GetComponent<Enemy>();
					}
					enemyManager = enemyReference.GetEnemyManager();
				}
				enemyPath = enemyManager.GetEnemyPath(enemyReference.GetEnemyType(), orientation);
				transform.position = enemyPath[0].position;
				enemyPathIndex++;
			}
			transform.position = Vector3.MoveTowards(transform.position, enemyPath[enemyPathIndex].position, Time.deltaTime * movementSpeed);
			if (transform.position == enemyPath[enemyPathIndex].position)
			{
				if (++enemyPathIndex == enemyPath.Length)
				{
					if (enemyReference.GetIsSpecial() && enemyReference.GetHasSpecialAnimated() && GameManager.Instance.GetCurrentState() == 1)
					{
						GameManager.Instance.GoToNextState();
					}
					this.Deactivate();
				}
			}
		}
	}

	public void GotShot(Projectile projectile)
	{
		iTween.PunchScale(transform.FindChild("Animator").gameObject, Vector3.one * punchAmount, 0.5f);
		iTween.PunchRotation(transform.FindChild("Animator").gameObject, Vector3.one * punchAmount, 0.5f);
		bool isDead = enemyReference.GetHealth() <= 0;
		if (isDead)
		{
			collider2D.isTrigger = false;
			rigidbody2D.AddForce(new Vector2(projectile.GetDirection().y, -projectile.GetDirection().x * ((-projectile.GetDirection().x < 0) ? -1 : 1)) * 80);
			if (enemyReference.GetIsSpecial() && GameManager.Instance.GetCurrentState() == 1)
				GameManager.Instance.GoToNextState();
			GameManager.Instance.IncrementFrameKillCount();
			if (GameManager.Instance.GetCurrentState() >= 3)
			{
				punchAmount += 0.1f;
			}
			GetComponentInChildren<HealthbarScript>().DamageTaken(int.MaxValue);
			collider2D.enabled = false;
			this.Deactivate();
		}
	}
	public GameObject GetEnemyPathObject()
	{
		return this.enemyPathObject;
	}
	void OnLevelWasLoaded()
	{
		punchAmount = 0f;
	}
}
