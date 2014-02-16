using UnityEngine;
using System.Collections;

public class CthulhuScript : MonoBehaviour {

	private GameObject currentCthulhu;
	private Vector3 targetPosition;
	private float movementSpeed;

	public void Init()
	{
		movementSpeed = 3f;
		GameManager.Instance.OnStateChanged += this.OnStateChanged;
	}

	void Update()
	{
		if (currentCthulhu != null && targetPosition != null)
		{
			currentCthulhu.transform.position = Vector3.MoveTowards(currentCthulhu.transform.position, targetPosition, Time.deltaTime * movementSpeed);
		}
	}

	private void OnStateChanged(int currentState, float changeTime)
	{
		if (currentState >= 3)
		{
			StartCoroutine ("ShowCthulhu");
		}
	}

	private IEnumerator ShowCthulhu()
	{
		currentCthulhu = transform.GetChild(Random.Range (0, 4)).gameObject;
		Vector3 movementVector = Vector3.zero;
		switch (currentCthulhu.name)
		{
		case "Animator_Top":
			movementVector = new Vector3(0, -6, 0);
			break;
		case "Animator_Left":
			movementVector = new Vector3(6, 0, 0);
			break;
		case "Animator_Right":
			movementVector = new Vector3(-6, 0, 0);
			break;
		}
		targetPosition = currentCthulhu.transform.position + movementVector;
		yield return new WaitForSeconds(5f);
		targetPosition -= movementVector;
		yield return new WaitForSeconds(5f);
		StartCoroutine("ShowCthulhu");
	}

}
