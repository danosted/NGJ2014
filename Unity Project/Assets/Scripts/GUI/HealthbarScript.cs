﻿using UnityEngine;
using System.Collections;

public class HealthbarScript : MonoBehaviour {

	[SerializeField]
	private GameObject greenPart;

	private float maxHealth;
	private float currentHealth;
	private Material myMaterial;

	void Start () {

		if(transform.parent.GetComponent<Enemy>())
		{
//			maxHealth = transform.parent.GetComponent<Enemy>().GetHealth();
		}
		else
		{
			maxHealth = transform.parent.GetComponent<Character>().GetHealth();
		}
		currentHealth = maxHealth;
		myMaterial = greenPart.renderer.material;
		
	}

	public void DamageTaken(float damage)
	{
		currentHealth = (currentHealth-damage >= 0) ? currentHealth-damage : 0f;
		float cutOffValue = (maxHealth-currentHealth)/maxHealth;
		myMaterial.SetFloat("_Cutoff", cutOffValue);
	}

}
