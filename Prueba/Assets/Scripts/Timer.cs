using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : NetworkBehaviour {

	public Text timerText;
	[SyncVar] public float startTime = 0.0f;
	[SyncVar] public int minutes;
	[SyncVar] public int seconds;
	[SyncVar] public bool masterTimer = false;

	[SyncVar] public bool gameOver = false;


	Timer serverTimer;
	// Use this for initialization
	void Start () {

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

		if(SceneManager.GetActiveScene().name.Equals("gameScene")) 
		{
			timerText.enabled = true;
			if(masterTimer) 
			{
				startTime -= Time.deltaTime;
				minutes = (int)(startTime / 60);
				seconds = (int)(startTime - (60*minutes));
				if(startTime < 0) {
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
