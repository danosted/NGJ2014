using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	
	[SerializeField]
	private float health = 100f;
	[SerializeField]
	private float zen = 0f;
	[SerializeField]
	private Transform gunPosition;
	[SerializeField]
	private Weapon[] weapons;
	[SerializeField]
	private Transform crossHairs;
	[SerializeField]
	private HealthbarScript healthBar;
	[SerializeField]
	private AudioClip[] clips;


	private Weapon weapon;
	private bool facingLeft;
	private bool isFiring;
	private Transform model;
	private float timeStart;
	private bool flashing;

	private GameManager gameManInstance;

	void Awake()
	{
		InputHandler input = GetComponent<InputHandler>();
		input.OnPress += OnPressed;
		input.OnRelease += OnReleased;
#if UNITY_ANDROID
		input.OnFaceLeft += FaceLeft;
		input.OnFaceRight += FaceRight;
#endif
		model = transform.FindChild("Model");
		weapon = weapons[0];
		GameObject weaponGO = Instantiate(weapon.gameObject, gunPosition.position, weapon.transform.rotation) as GameObject;
		weaponGO.transform.parent = transform;
		weapon = weaponGO.GetComponent<Weapon>();
		gameManInstance = GameManager.Instance;
		gameManInstance.OnStateChanged += OnStateChange;
		GetComponent<GameInitializer2Object>().OnInitializeWithDependencies += Initialize;
//		GameObject ch = Instantiate(crossHairs.gameObject) as GameObject;
//		this.crossHairs = ch.transform;
//		Screen.showCursor = false;
		StartCoroutine(PointGun());
	}

	public void Initialize(GameObject[] dependencies)
	{
		healthBar = dependencies[0].GetComponent<HealthbarScript>();
		healthBar.Init(health);
	}
#if UNITY_ANDROID

#else
	void Update()
	{
		if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
		{
			FaceLeft();
		}
		else
		{
			FaceRight();
		}
	}
#endif

	private void FaceLeft()
	{
		if(!facingLeft)
		{
			facingLeft = true;
			float scale_x = transform.localScale.x;
			transform.localScale = new Vector3(-1f * scale_x, transform.localScale.y, transform.localScale.z);
		}
	}

	private void FaceRight()
	{
		if(facingLeft)
		{
			facingLeft = false;
			float scale_x = transform.localScale.x;
			transform.localScale = new Vector3(-1f * scale_x, transform.localScale.y, transform.localScale.z);
		}
	}

	private IEnumerator PointGun()
	{
		while(weapon)
		{
			Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			crossHairs.position = new Vector3(mousepos.x, mousepos.y, 0f);
			Vector3 weaponPos = weapon.transform.position;
			Vector3 weaponToMouse = (mousepos - weaponPos).normalized;
			float angle = Mathf.Atan(weaponToMouse.y/weaponToMouse.x);
			if(facingLeft)
			{
				weapon.transform.rotation = Quaternion.AngleAxis(angle * 180f/Mathf.PI, Vector3.back);
			}
			else
			{
				weapon.transform.rotation = Quaternion.AngleAxis(angle * 180f/Mathf.PI, Vector3.forward);
			}
			yield return null;
		}
	}

	private IEnumerator FireGun()
	{
		while(isFiring)
		{
			this.weapon.StartFiring();
			yield return null;
		}
	}

	private void OnPressed()
	{
		isFiring = true;
		StartCoroutine(FireGun());
	}

	private void OnReleased()
	{
		this.weapon.StopFiring();
		isFiring = false;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.GetComponent<Enemy>() && !gameManInstance.GetIsGameOver())
		{
			iTween.PunchScale(transform.FindChild("Model").gameObject, Vector3.one, 0.5f);
			float damageTaken = (gameManInstance.GetCurrentState() == 3) ? 1/3f : 1f;
			if(health > damageTaken)
			{
				health -= damageTaken;
				if(!flashing)
					StartCoroutine(FlashOnDamage());
				PlayRandomSound();
			} 
			else
			{
				healthBar.DamageTaken(int.MaxValue);
				PlayRandomSound();
				gameManInstance.GameOver();
				return;
			}
			healthBar.DamageTaken(damageTaken);
			if (!(collider.GetComponent<Enemy>().GetIsSpecial()))
				collider.enabled = false;
		}
	}

	private IEnumerator FlashOnDamage()
	{
		flashing = true;
		SpriteRenderer r = model.GetComponent<SpriteRenderer>();
		float waitTime = 0.1f;
		float totalTime = 0.5f;
		while(totalTime > 0)
		{
			r.color = Color.red;

			yield return new WaitForSeconds(waitTime);
			r.color = Color.white;
			yield return new WaitForSeconds(waitTime);
			totalTime -= (Time.deltaTime + 2f*waitTime);
		}
		flashing = false;
	}

	private void OnStateChange(int state, float stateChangeTime)
	{

		StartCoroutine(ChangeToState(state, stateChangeTime));

	}

	private IEnumerator ChangeToState(int state, float stateChangeTime)
	{
		if(state == 4)
		{
			//TODO: On Death Sequence
			this.enabled = false;
		}
		yield return new WaitForSeconds(stateChangeTime);
		if(weapons.Length > state)
		{
			Destroy(weapon.gameObject);
			GameObject wGO = Instantiate(weapons[state].gameObject, gunPosition.position, weapons[state].transform.rotation) as GameObject;
			wGO.transform.parent = transform;
			if(facingLeft)
			{
				wGO.transform.localScale = new Vector3(-1f * wGO.transform.localScale.x,wGO.transform.localScale.y, wGO.transform.localScale.z);
			}
			weapon = wGO.GetComponent<Weapon>();
			StartCoroutine(PointGun());
		}
	}

	private void PlayRandomSound()
	{
		if(clips.Length == 0)
		{
			return;
		}
		if(clips[0].name == "GunShot")
		{
			audio.Play();
		}
		if(clips.Length == 1)
		{
			audio.clip = clips[0];
			audio.Play();
		}
		else if(!audio.isPlaying)
		{
			int randIndex = Random.Range(0, clips.Length-1);
			timeStart = Time.time;
			audio.clip = clips[randIndex];
			audio.Play();
		}
		else
		{
			StartCoroutine(PlaySoundAfter());
		}
	}
	
	private IEnumerator PlaySoundAfter()
	{
		int randIndex = Random.Range(0, clips.Length-1);
		float timeSinceStart = Time.time - timeStart;
		yield return new WaitForSeconds(audio.clip.length - timeSinceStart);
		audio.clip = clips[randIndex];
		audio.Play();
	}

	public float GetHealth()
	{
		return this.health;
	}

}