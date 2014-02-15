﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	protected float speed;
	protected float damage;
	protected float aoe;
	protected float maxDist;
	protected Vector3 direction;
	protected Transform target;
	protected bool hit;

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

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.GetComponent<Enemy>())
		{
			//TODO: Do damage
			hit = true;
			KillProjectile();
		}
	}

	public Vector3 GetDirection()
	{
		return this.direction;
	}

	private void KillProjectile()
	{
		StopCoroutine("Shooting");
		Destroy(gameObject);
	}

}
