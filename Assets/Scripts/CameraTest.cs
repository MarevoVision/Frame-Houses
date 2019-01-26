using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnValueChange(float value)
	{
		Debug.Log(value);
		//transform.position.Set(transform.position.x,transform.position.y,transform.position.z+value); 
		transform.position = new Vector3(transform.position.x,transform.position.y,value); 
	}
	
}
