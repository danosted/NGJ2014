using UnityEngine;
using System.Collections;

public class HealthbarScript : MonoBehaviour {
	
	[SerializeField]
	private GameObject greenPart;
	[SerializeField]
	private GameObject bubbleObject;


	private float maxHealth;
	private float currentHealth;
	private Material myMaterial;

	void Start () {

		if(transform.parent)
		{
			maxHealth = transform.parent.GetComponent<Enemy>().GetHealth();
			currentHealth = maxHealth;
			myMaterial = greenPart.renderer.material;
		}
		
	}

	public void Init(float health)
	{
		maxHealth = health;
		currentHealth = maxHealth;
		myMaterial = greenPart.renderer.material;
		if (bubbleObject)
		{
			bubbleObject.transform.FindChild("Bubble").transform.localScale = Vector3.one * 1.25f;
		}
	}

	public void DamageTaken(float damage)
	{
		currentHealth = (currentHealth-damage >= 0) ? currentHealth-damage : 0f;
		float cutOffValue = (maxHealth-currentHealth)/maxHealth;
		myMaterial.SetFloat("_Cutoff", cutOffValue);
		if (bubbleObject)
		{
			bubbleObject.transform.FindChild("Bubble").transform.localScale = Vector3.one * (1 - 0.4f * (maxHealth - currentHealth)/maxHealth) * 1.25f;
			iTween.PunchScale(bubbleObject.transform.FindChild("Bubble").gameObject, Vector3.one * -0.5f, 0.5f);
		}

	}

}
