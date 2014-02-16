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
	private bool hasSpecialAnimated;
	[SerializeField]
	private AudioClip hitClip;
	[SerializeField]
	private AudioClip specialHitClip;
	
	public void Init()
	{
		this.enabled = true;
		this.isSpecialAnimating = false;
		this.hasSpecialAnimated = false;
		this.gameObject.SetActive(true);
		GetComponent<EnemyMovement>().Init(this);
		GameManager.Instance.OnStateChanged += this.OnStateChanged;
	}

	void Update()
	{
		if (transform.position.magnitude > 30)
		{
			this.DestroySelf();
		}
	}

	public void DeactivateObject()
	{
		this.gameObject.SetActive(false);
		GameManager.Instance.OnStateChanged -= this.OnStateChanged;
	}
	private void DestroySelf()
	{
		DestroyObject(this.gameObject);
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
		if (isSpecial && !isSpecialAnimating && !hasSpecialAnimated)
		{	
			this.isSpecialAnimating = true;
			GetComponent<AudioSource>().clip = specialHitClip;
			GetComponent<AudioSource>().volume = 1f;
			GetComponent<AudioSource>().Play();
			StartCoroutine("WaitForSpecialAnimation");
		}
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().clip = hitClip;
			GetComponent<AudioSource>().Play();
		}
		this.health--;
		GetComponentInChildren<HealthbarScript>().DamageTaken(1);
		GetComponent<EnemyMovement>().GotShot(projectile);

	}

	private IEnumerator WaitForSpecialAnimation()
	{ 
		transform.GetComponentInChildren<Animator>().SetBool("SpecialAnimating", true);
		rigidbody2D.Sleep();
		GameManager.Instance.GoToNextState();
		yield return new WaitForSeconds(0.5f);
		transform.GetComponentInChildren<Animator>().SetBool("SpecialAnimating", false);
		rigidbody2D.WakeUp ();
		this.isSpecialAnimating = false;
		this.hasSpecialAnimated = true;
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

	public EnemyManager GetEnemyManager()
	{
		return this.enemyManager;
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
	public bool GetIsSpecial()
	{
		return this.isSpecial;
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
	Gnome,
	UFO,
	Plane,
	Cthulhu,
}

public enum Orientation
{
	Left,
	Right,
}
