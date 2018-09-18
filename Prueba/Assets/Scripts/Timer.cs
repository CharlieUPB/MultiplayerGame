using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : NetworkBehaviour {

	public Text timerText;
	public AudioClip gameStartedSound;

	public AudioClip gameLoadingSound;
    private AudioSource StartedGame;
	private AudioSource LoadingGame;

	private bool onLoadScene = false;
	private bool onGameScene = false;
	[SyncVar] public float startTime = 0.0f;
	[SyncVar] public int minutes;
	[SyncVar] public int seconds;
	[SyncVar] public bool masterTimer = false;

	[SyncVar] public bool gameOver = false;


	Timer serverTimer;
	// Use this for initialization
	void Start () {

		AudioSource[] audios = GetComponents<AudioSource>();
		LoadingGame = audios[1];
		StartedGame = audios[2];

		timerText.enabled = false;
		if(isServer) 
		{
			if(isLocalPlayer)
			{
				serverTimer = this;
				masterTimer = true;
			}
		}
		else if (isLocalPlayer) 
		{
			Timer[] timers = FindObjectsOfType<Timer>();
			for(int i =0; i<timers.Length; i++)
			{
				if(timers[i].masterTimer)
				{
					serverTimer = timers[i];
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(!onLoadScene) {
			onLoadScene = true;
			if(SceneManager.GetActiveScene().name.Equals("LoadScene")) {
				LoadingGame.PlayOneShot(this.gameLoadingSound,1F);
			}
		}
		if(!onGameScene) {
			onGameScene = true;
			if(SceneManager.GetActiveScene().name.Equals("gameScene")) {
				this.StartedGame.PlayOneShot(this.gameStartedSound,1F);
			} 
		}
		

		if(SceneManager.GetActiveScene().name.Equals("gameScene")) 
		{
			timerText.enabled = true;
			if(masterTimer) 
			{
				startTime -= Time.deltaTime;
				minutes = (int)(startTime / 60);
				seconds = (int)(startTime - (60*minutes));
				if(startTime < 0) {
					NetworkServer.DisconnectAll();
					gameOver = true;
				}
			}
			if(isLocalPlayer) {
				if(serverTimer)
				{
					startTime = serverTimer.startTime;
					minutes = serverTimer.minutes;
					seconds = serverTimer.seconds;
					gameOver = serverTimer.gameOver;
				}
				else 
				{
					Timer[] timers = FindObjectsOfType<Timer>();
					for(int i =0; i<timers.Length; i++)
					{
						if(timers[i].masterTimer)
						{
							serverTimer = timers[i];
						}
					}
				}
				timerText.text =  "Time Left: " + minutes + ":" + seconds;
			}
		}		
	}
}
