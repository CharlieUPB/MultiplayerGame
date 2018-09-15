﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	public GameObject PlayerTransform;
	private Vector3 camera_Offset;

	[Range(0.01f, 1.0f)]
	public float SmoothFactor = 0.5f;
	public bool lookAtPlayer = false;

	// Use this for initialization
	void Start () 
	{
		camera_Offset = transform.position - PlayerTransform.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		//transform.position = PlayerTransform.position + camera_Offset;
		Vector3 newPos = PlayerTransform.transform.position + camera_Offset;   
		transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

		if(lookAtPlayer)
		{
			transform.LookAt(PlayerTransform.transform);
		}
	}
}
