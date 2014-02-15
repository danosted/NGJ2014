using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public delegate void OnStateChangedDelegate(int state, float stateChangeTime);
	public event OnStateChangedDelegate OnStateChanged;

	[SerializeField]
	private float stateChangeTime;
	[SerializeField]
	private float comboKillInterval;
	[SerializeField]
	private SpriteRenderer whiteScreen;

	public static GameManager Instance { get; private set;}

	private int currentState;
	private float score;
	private int combo;
	private int frameKillCount;
	private float lastKillTime;

	void Awake()
	{
		Instance = this;
		currentState = 0;
		score = 0;
		combo = 0;
		frameKillCount = 0;
		lastKillTime = 0;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.IncrementFrameKillCount();
		}
		if (frameKillCount != 0)
		{
			combo += frameKillCount;
			lastKillTime = Time.time;
		}
		else if(lastKillTime + comboKillInterval < Time.time)
		{
			combo = 0;
		}
		frameKillCount = 0;

		score += combo * Time.deltaTime;
	}

	public void GoToNextState()
	{
		currentState++;
		if(OnStateChanged != null)
			OnStateChanged(currentState, stateChangeTime);
	}

	public void IncrementFrameKillCount()
	{
		frameKillCount++;
	}

	public void GameOver()
	{
		whiteScreen.collider2D.enabled = true;
		StartCoroutine(FadeoutReset());
	}

	private IEnumerator FadeoutReset()
	{
		while(whiteScreen.color.a < 1f)
		{
			whiteScreen.color = Color.Lerp(whiteScreen.color, Color.white, Time.deltaTime);
			yield return null;
		}
		Application.LoadLevel(0);
	}

}
