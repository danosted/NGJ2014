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
	private bool isGameOver;

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
		isGameOver = false;
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
		currentState = 3;
		GoToNextState();
		whiteScreen.collider2D.enabled = true;
		isGameOver = true;
		StartCoroutine(FadeoutReset());
	}

	public int GetCurrentState()
	{
		return currentState;
	}
	public bool GetIsGameOver()
	{
		return this.isGameOver;
	}

	private IEnumerator FadeoutReset()
	{
		whiteScreen.GetComponent<AudioSource>().Play();
		whiteScreen.color = new Color(1f, 1f, 1f, 0f);
		while(whiteScreen.color.a < 0.85f)
		{
			whiteScreen.color = Color.Lerp(whiteScreen.color, Color.white, Time.deltaTime);
			yield return null;
		}
		while (true)
		{
			whiteScreen.color = Color.Lerp(whiteScreen.color, Color.white, Time.deltaTime);
			if (Input.GetMouseButtonDown(0))
			{
				Application.LoadLevel(0);
			}
			yield return null;
		}
	}
}
