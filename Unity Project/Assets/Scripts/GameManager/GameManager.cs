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
		if (Instance.frameKillCount != 0)
		{
			Instance.combo += Instance.frameKillCount;
			Instance.lastKillTime = Time.time;
		}
		else if(Instance.lastKillTime + Instance.comboKillInterval < Time.time)
		{
			Instance.combo = 0;
		}
		if (Instance.combo >= 30 && Instance.currentState == 2)
		{	
			GoToNextState();
		}
		else if (Instance.combo >= 100 && Instance.currentState == 3)
		{	
			GameOver();
		}
		Instance.frameKillCount = 0;

		Instance.score += Instance.combo * Time.deltaTime;
	}

	public void GoToNextState()
	{
		currentState++;
		if(OnStateChanged != null)
			OnStateChanged(currentState, stateChangeTime + ((currentState == 2) ? 15f : 0f));
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

	public int GetCurrentState()
	{
		return currentState;
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
