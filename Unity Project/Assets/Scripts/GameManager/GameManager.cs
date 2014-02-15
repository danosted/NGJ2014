using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public delegate void OnStateChangedDelegate(int state, float stateChangeTime);
	public event OnStateChangedDelegate OnStateChanged;

	[SerializeField]
	private float stateChangeTime;

	public static GameManager Instance { get; private set;}

	private int currentState;
	private int score;
	private int combo;

	void Awake()
	{
		Instance = this;
		currentState = 0;
		score = 0;
		combo = 0;
	}

	public void GoToNextState()
	{
		currentState++;
		OnStateChanged(currentState, stateChangeTime);
	}

}
