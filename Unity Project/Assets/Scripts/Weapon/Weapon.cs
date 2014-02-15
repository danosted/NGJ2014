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

	private bool isFiring;
	private bool isCooling;

	public void StartFiring()
	{
		isFiring = true;
		StartCoroutine(StartFiringWeapon());
	}

	public void StopFiring()
	{
		isFiring = false;
	}

	private IEnumerator StartFiringWeapon()
	{
		while(isFiring && !isCooling)
		{
			GameObject pGO = Instantiate(projectile.gameObject, barrel.transform.position, projectile.transform.rotation) as GameObject;
			pGO.transform.parent = transform;
			Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 dir = new Vector3(mousepos.x, mousepos.y, 0f) - barrel.transform.position;
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
}
