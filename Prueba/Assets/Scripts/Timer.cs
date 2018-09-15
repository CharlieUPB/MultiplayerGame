using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour {

	public Text timerText;
	public float startTime = 0.0f;
	
	private int minutes;
	private int seconds;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		startTime -= Time.deltaTime;
		minutes = (int)(startTime / 60);
		seconds = (int)(startTime - (60*minutes));
		timerText.text = "Time left: " + minutes + ":" + seconds;
	}
}
