﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NamePlayerController : NetworkBehaviour {

	public InputField PlayerName;
    public GameObject btnReady;
    public GameObject ButtonStart;
    public Text MessaggeWait;

    public Text playerNickName;

	// Use this for initialization
	void Start () 
	{
		MessaggeWait.enabled = false;
        ButtonStart.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BtnIAmReady()
    {
        if(PlayerName.text != "")
        {
            btnReady.SetActive(false);
            
            playerNickName.text = PlayerName.text;
            MessaggeWait.text = "Waiting for host...";
            MessaggeWait.enabled = true;
            
            if(isServer)
            {
                ButtonStart.SetActive(true);
            }
        }
        else
        {
            MessaggeWait.text = "Enter a nickname!";
            MessaggeWait.enabled = true;
        }
    }

    public void changeScene()
    {
        if(isServer) 
        {
            NetworkManager.singleton.ServerChangeScene("gameScene");
        }
        
    }
}
