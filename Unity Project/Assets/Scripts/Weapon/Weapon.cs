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

	private bool isFiring;
	private bool isCooling;

	public void StartFiring()
	{
		isFiring = true;
		StartCoroutine(PointGun());
		StartCoroutine(StartFiringWeapon());
	}

	public void StopFiring()
	{
		isFiring = false;
	}

//	private IEnumerator PointGun()
//	{
//		Vector3 curMousePos;
//		Vector3 lastMousePos = Vector3.zero;
//		while(isFiring)
//		{
//			curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			curMousePos = new Vector3(curMousePos.x, curMousePos.y, 0f);
//			Vector3 dir = (curMousePos - lastMousePos) - barrel.position;
//			dir.Normalize();
//			float angle = Mathf.Atan(dir.y/dir.x);
//			Debug.Log(angle);
//			transform.RotateAround(pivot.position, Vector3.up, angle);
//			yield return null;
//			lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			lastMousePos = new Vector3(lastMousePos.x, lastMousePos.y, 0f);
//		}
//	}

	private IEnumerator StartFiringWeapon()
	{
		while(isFiring && !isCooling)
		{
			GameObject pGO = Instantiate(projectile.gameObject, barrel.position, projectile.transform.rotation) as GameObject;
			Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 dir = new Vector3(mousepos.x, mousepos.y, 0f) - barrel.position;
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
