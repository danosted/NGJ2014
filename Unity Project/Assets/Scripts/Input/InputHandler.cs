using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public delegate void OnPressDelegate();
	public event OnPressDelegate OnPress;

	public delegate void OnReleaseDelegate();
	public event OnReleaseDelegate OnRelease;

	public delegate void OnFaceLeftDelegate();
	public event OnFaceLeftDelegate OnFaceLeft;

	public delegate void OnFaceRightDelegate();
	public event OnFaceRightDelegate OnFaceRight;

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
#if UNITY_ANDROID
		if(Input.GetMouseButton(0))
		{
			if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
			{
				if(OnFaceLeft != null)
					OnFaceLeft();
			}
			else
			{
				if(OnFaceRight != null)
					OnFaceRight();
			}
		}
#endif
	}
}
