using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private float speed;
	private float damage;
	private float aoe;
	private float maxDist;
	private Vector3 direction;
	private Transform target;
	private bool hit;

	public void Shoot(float speed, float damage, float aoe, float maxDist, Vector3 direction)
	{
		this.speed = speed;
		this.damage = damage;
		this.aoe = aoe;
		this.maxDist = maxDist;
		this.direction = direction;
		this.hit = false;
		StartCoroutine(Shooting());
	}

	private IEnumerator Shooting()
	{
		while(!hit)
		{
			Vector3 distToTravel = direction * speed * Time.deltaTime;
			if(maxDist < distToTravel.magnitude)
			{
				hit = true;
				KillProjectile();
			}
			maxDist -= distToTravel.magnitude;
			transform.Translate(distToTravel);
			yield return null;
		}
	}

//	public IEnumerator ShootHoming(float speed, float damage, float aoe, Transform target)
//	{
//
//	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		hit = true;
		KillProjectile();
	}

	private void KillProjectile()
	{
		StopCoroutine("Shooting");
		Destroy(gameObject);
	}

}
