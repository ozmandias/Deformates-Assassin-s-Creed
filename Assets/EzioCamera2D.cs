using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzioCamera2D : MonoBehaviour {
	
	GameObject ezioobject;
	Vector3 ezioandcameradistance;

	// Use this for initialization
	void Start () {
		ezioobject = GameObject.Find("Ezio2d");
		ezioandcameradistance = gameObject.transform.position - ezioobject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		movecamera();
	}

	void movecamera(){
		gameObject.transform.position = ezioobject.transform.position + ezioandcameradistance;
	}
}
