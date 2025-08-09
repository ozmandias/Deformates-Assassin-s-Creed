using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	UIManager gameuserinterfacemanager;
	bool playerpassout=false;
	int enemycount=30;

	// Use this for initialization
	void Start () {
		gameuserinterfacemanager = GameObject.Find("UICanvas").GetComponent<UIManager>();
		gameuserinterfacemanager.updateenemytext(enemycount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setplayerpassout(bool playerpassoutstatus){
		playerpassout=playerpassoutstatus;
	}
	public bool getplayerpassout(){
		return playerpassout;
	}
	public void reduceenemycount(){
		enemycount=enemycount-1;
		gameuserinterfacemanager.updateenemytext(enemycount);
	}
	public int getenemycount(){
		return enemycount;
	}
}
