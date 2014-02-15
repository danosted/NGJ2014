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

	private bool facingLeft;
	private bool isFiring;
	private Transform model;

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
		GameObject weaponGO = Instantiate(weapon.gameObject, gunPosition.position, weapon.transform.rotation) as GameObject;
		weaponGO.transform.parent = transform;
		weapon = weaponGO.GetComponent<Weapon>();

	}

	public void Initialize()
	{

	}
#if UNITY_EDITOR
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
		while(isFiring)
		{
			Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousepos.Normalize();
			float angle = Mathf.Atan(mousepos.y/mousepos.x);
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

	private void OnPressed()
	{
		this.weapon.StartFiring();
		isFiring = true;
		StartCoroutine(PointGun());
	}

	private void OnReleased()
	{
		this.weapon.StopFiring();
		isFiring = false;
	}

	private void SwitchWeapon(Weapon weapon)
	{

	}

}
