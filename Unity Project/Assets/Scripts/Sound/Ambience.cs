using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Ambience : MonoBehaviour {

	[SerializeField]
	private AudioClip[] themes;
	[SerializeField]
	private AudioClip[] bridges;

	private AudioSource audio;
	private float startTime = 0f;
	private bool isChanging;
	private int state;
	private float stateChangeTime;

	void Start () 
	{
		GameManager.Instance.OnStateChanged += OnStateChange;
		audio = GetComponent<AudioSource>();
		audio.clip = themes[0];
		audio.Play();
		startTime = Time.time;
	}

	private void OnStateChange(int state, float stateChangeTime)
	{
		if (state == 4)
		{
			gameObject.SetActive(false);
			return;
		}
		this.stateChangeTime = stateChangeTime;
		if(this.state < state)
		{
			this.state = state;
		}
		if(isChanging)
		{
			StartCoroutine(ChangingAfter());
		} 
		else if(themes.Length > state)
		{
			StartCoroutine(OldAudioToBridge());
		}
	}

	private IEnumerator ChangingAfter()
	{
		while(isChanging)
		{
			yield return null;
		}
		OnStateChange(state, stateChangeTime);
	}

	private IEnumerator OldAudioToBridge()
	{
		audio.loop = false;
		startTime = Mathf.Abs(startTime - Time.time);
		isChanging = true;
		while(audio.isPlaying)
		{
			yield return null;
		}
		audio.clip = bridges[state-1];
		audio.loop = true;
		audio.Play();
		Debug.Log("playin " + bridges[state-1].name);
		StartCoroutine(BridgeToAudio());
	}

	private IEnumerator BridgeToAudio()
	{
		while(audio.isPlaying)
		{
			yield return null;
		}
		audio.clip = themes[state];
		audio.loop = true;
		audio.Play();
		isChanging = false;
		Debug.Log("playin " + themes[state].name);
	}
}
