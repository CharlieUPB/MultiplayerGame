using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door3 : MonoBehaviour {

	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		animator.SetBool("character_nearby",true);
	}
	void OnTriggerExit(Collider other) {
		animator.SetBool("character_nearby",false);
	}
}
