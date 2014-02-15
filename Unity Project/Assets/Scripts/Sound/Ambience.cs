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
		if(themes.Length > state)
		{
			StartCoroutine(OldAudioToBridge(state));
		}
	}

	private IEnumerator OldAudioToBridge(int state)
	{
		audio.loop = false;
		startTime = Mathf.Abs(startTime - Time.time);
		yield return new WaitForSeconds(Mathf.Abs(audio.clip.length - startTime));
		audio.clip = bridges[state-1];
		audio.Play();
		Debug.Log("playin " + bridges[state-1].name);
		StartCoroutine(BridgeToAudio(state));
	}

	private IEnumerator BridgeToAudio(int state)
	{
		yield return new WaitForSeconds(audio.clip.length);
		audio.clip = themes[state];
		audio.loop = true;
		audio.Play();
		Debug.Log("playin " + themes[state].name);
	}
}
