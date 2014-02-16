using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	[SerializeField]
	private float firingDelay;
	[SerializeField]
	private float firingRange;
	[SerializeField]
	private float projectileSpeed;
	[SerializeField]
	private float projectileDamage;
	[SerializeField]
	private float projectileAoe;
	[SerializeField]
	private Transform barrel;
	[SerializeField]
	private Projectile projectile;
	[SerializeField]
	private Transform pivot;
	[SerializeField]
	private Transform gunModel;
	[SerializeField]
	private AudioClip[] clips;
	private AudioSource audio;
	private float timeStart;

	private bool isFiring;
	private bool isCooling;

	void Start()
	{
		audio = GetComponent<AudioSource>();
	}

	public void StartFiring()
	{
		isFiring = true;
		StartCoroutine(StartFiringWeapon());
	}

	public void StopFiring()
	{
		isFiring = false;
		if(audio.isPlaying && audio.clip.length > 10f)
		{
			audio.Stop();
		}
	}

	private IEnumerator StartFiringWeapon()
	{
		while(isFiring && !isCooling)
		{
			PlayRandomSound();
			GameObject pGO = Instantiate(projectile.gameObject, barrel.position, projectile.transform.rotation) as GameObject;
			Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 dir = barrel.position - pivot.position;
			dir.Normalize();
			dir *= firingRange;
			pGO.GetComponent<Projectile>().Shoot(projectileSpeed, projectileDamage, projectileAoe, firingRange, dir);
			yield return StartCoroutine(WaitForCooldown());
		}
    }

	private IEnumerator WaitForCooldown()
	{
		isCooling = true;
		yield return new WaitForSeconds(firingDelay);
		isCooling = false;
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
}
