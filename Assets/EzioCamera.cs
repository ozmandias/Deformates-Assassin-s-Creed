using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzioCamera : MonoBehaviour {
	
	GameObject ezioobject;
	float mousehorizontal;
	float mousevertical;
	Vector3 mousedirection;
	Vector3 smoothmousedirection;
	Vector3 smoothmousevelocity;
	float smoothmousetime=0.2f;
	float smoothmousespeed=Mathf.Infinity;
	float minimummousehorizontal=-20f;
	float maximummousehorizontal=20f;
	float mousespeed=2f;
	float reversemouse=-1f;
	float ezioandcameradistance=40f;

	// Use this for initialization
	void Start () {
		ezioobject = GameObject.Find("Ezio");
		// ezioandcameradistance = gameObject.transform.position - ezioobject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		movecamera();
	}

	void movecamera(){
		mousehorizontal = mousehorizontal + Input.GetAxis("Mouse X");
		Mathf.Clamp(mousehorizontal,minimummousehorizontal,maximummousehorizontal);
		mousevertical = mousevertical + Input.GetAxis("Mouse Y");
		mousedirection = new Vector3((mousevertical*reversemouse)*mousespeed,mousehorizontal*mousespeed,0);
		smoothmousedirection = Vector3.SmoothDamp(mousedirection,smoothmousedirection,ref smoothmousevelocity,smoothmousetime,smoothmousespeed,Time.deltaTime);
		// gameObject.transform.eulerAngles = smoothmousedirection*Time.deltaTime;
		gameObject.transform.eulerAngles = smoothmousedirection;
		
		// gameObject.transform.position = ezioobject.transform.position + ezioandcameradistance;
		// gameObject.transform.position = ezioobject.transform.position - (Vector3.forward*ezioandcameradistance);
		gameObject.transform.position = ezioobject.transform.position - (gameObject.transform.forward*ezioandcameradistance);
	}
}
