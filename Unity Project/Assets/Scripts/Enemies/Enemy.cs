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
	private float volume;
	[SerializeField]
	private AudioClip[] clips;

	void Awake()
	{
		volume = audio.volume;
	}
	
	public void Init()
	{
		this.enabled = true;
		this.isSpecialAnimating = false;
		this.hasSpecialAnimated = false;
		this.gameObject.SetActive(true);
		GetComponent<EnemyMovement>().Init(this);
		GameManager.Instance.OnStateChanged += this.OnStateChanged;
		audio.clip = clips[0];
		audio.volume = volume;
		audio.Play();
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
			audio.clip = clips[2];
			audio.volume = 1f;
			audio.Play();
			StartCoroutine("WaitForSpecialAnimation");
		}
		else if(isSpecial)
		{
			audio.clip = clips[3];
			audio.volume = 1f;
			audio.Play();
		}
		else
		{
			audio.clip = clips[1];
			audio.volume = 0.02f;
			audio.Play();
		}
		this.health -= Mathf.RoundToInt(projectile.GetDamage());
		GetComponentInChildren<HealthbarScript>().DamageTaken(projectile.GetDamage());
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
		if(currentState == 4)
		{
			Destroy(gameObject);
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
	public bool GetHasSpecialAnimated()
	{
		return this.hasSpecialAnimated;
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
