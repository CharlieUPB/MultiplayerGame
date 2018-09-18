using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NamePlayerController : NetworkBehaviour {

	public InputField PlayerName;
    public GameObject btnReady;
    public GameObject ButtonStart;
    public Text MessaggeWait;
    public Text playerNickName;
    public string name;


	// Use this for initialization
	void Start () 
	{
		MessaggeWait.enabled = false;
        ButtonStart.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        name = PlayerName.text;
        playerNickName.text = name;
        
        if(SceneManager.GetActiveScene().name.Equals("gameScene")) 
        {
            PlayerName.DeactivateInputField();
            
            PlayerName.enabled = false;
            btnReady.SetActive(false);
            MessaggeWait.enabled = false;
    
            if(isServer)
            {
                ButtonStart.SetActive(false);
            }
	    }
	}

	public void BtnIAmReady()
    {
        if(PlayerName.text != "")
        {
            btnReady.SetActive(false);
            name = PlayerName.text;
            playerNickName.text = name;
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
