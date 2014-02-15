using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField]
	private EnemyType type;
	private EnemyManager enemyManager;
	[SerializeField]
	private int health;
	private bool isSpecial;
	private bool isSpecialAnimating;
	
	public void Init()
	{
		this.enabled = true;
		this.isSpecialAnimating = false;
		this.gameObject.SetActive(true);
		GetComponent<EnemyMovement>().Init(this);
		GameManager.Instance.OnStateChanged += this.OnStateChanged;
	}

	public void DeactivateObject()
	{
		this.gameObject.SetActive(false);
		GameManager.Instance.OnStateChanged -= this.OnStateChanged;
	}

    void OnTriggerEnter2D(Collider2D collider)
	{
		Projectile projectile = collider.GetComponent<Projectile>();
		if (projectile)
		{
			this.GotShot(projectile);
		}
	}

	private void GotShot(Projectile projectile)
	{
		if(!isSpecial || transform.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Rabbit_Monster_Move") && !rigidbody2D.IsSleeping())
		{
			this.health--;
			GetComponent<EnemyMovement>().GotShot(projectile);
		}
		else
		{	
			StartCoroutine("WaitForSpecialAnimation");
		}
	}

	private IEnumerator WaitForSpecialAnimation()
	{ 
		this.isSpecialAnimating = true;
		transform.GetComponentInChildren<Animator>().SetBool("SpecialAnimating", true);
		rigidbody2D.Sleep();
		GameManager.Instance.GoToNextState();
		yield return new WaitForSeconds(0.5f);
		transform.GetComponentInChildren<Animator>().SetBool("SpecialAnimating", false);
		rigidbody2D.WakeUp ();
		this.isSpecialAnimating = false;
	}

	private void OnStateChanged(int currentState, float changeTime)
	{
		if (currentState == 1 && !isSpecial)
		{
			GetComponent<EnemyMovement>().Deactivate();
		}
	}

	#region Public getters and setters
	public void SetEnemyManager(EnemyManager enemyManager)
	{
		this.enemyManager = enemyManager;
	}
	
	public EnemyType GetEnemyType()
	{
		return this.type;
	}

	public int GetHealth()
	{
		return this.health;
	}

	public bool GetIsSpecialAnimating()
	{
		return this.isSpecialAnimating;
	}

	public void SetIsSpecial(bool isSpecial)
	{
		this.isSpecial = isSpecial;
	}
	#endregion
}

public enum EnemyType
{
	Butterfly,
	Rabbit,
}

public enum Orientation
{
	Left,
	Right,
}
