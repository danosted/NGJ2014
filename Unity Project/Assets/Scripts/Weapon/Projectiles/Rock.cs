using UnityEngine;
using System.Collections;

public class Rock : Projectile {

	public void Shoot(float speed, float damage, float aoe, float maxDist, Vector3 direction)
	{
		this.speed = speed;
		this.damage = damage;
		this.aoe = aoe;
		this.maxDist = maxDist;
		this.direction = direction;
		this.hit = false;
		rigidbody2D.AddForce(new Vector2(this.direction.x, this.direction.y)*this.speed);
	}

}
