using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public delegate void OnPressDelegate();
	public event OnPressDelegate OnPress;

	public delegate void OnReleaseDelegate();
	public event OnReleaseDelegate OnRelease;

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(OnPress != null)
			{
				OnPress();
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			if(OnRelease != null)
			{
				OnRelease();
			}
		}
	}
}
