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
	private Weapon weapon;

	private bool isFiring;

	void Awake()
	{
		GetComponent<InputHandler>().OnPress += OnPressed;
		GetComponent<InputHandler>().OnRelease += OnReleased;
		GameObject weaponGO = Instantiate(weapon.gameObject, gunPosition.position, Quaternion.identity) as GameObject;
		weaponGO.transform.parent = transform;
		weapon = weaponGO.GetComponent<Weapon>();
	}

	public void Initialize()
	{

	}

	private void OnPressed()
	{
		this.weapon.StartFiring();
	}

	private void OnReleased()
	{
		this.weapon.StopFiring();
	}

	private void SwitchWeapon(Weapon weapon)
	{

	}

}
