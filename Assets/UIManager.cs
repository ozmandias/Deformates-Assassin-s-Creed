using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	GameObject pausemenuobject;
	[SerializeField]Text healthtext;
	[SerializeField]Text enemytext;
	bool pause=false;

	// Use this for initialization
	void Start () {
		pausemenuobject = gameObject.transform.GetChild(0).gameObject;
		// healthtext = gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
		// enemytext = gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();
		pausemenuobject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		pausecheck();
	}

	void pausecheck(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(pause==false){
				pause=true;
				pausegame();
			}else{
				pause=false;
				resumegame();
			}
		}
	}

	public void pausegame(){
		Time.timeScale=0;
		pause=true;
		pausemenuobject.SetActive(true);
	}
	public void resumegame(){
		Time.timeScale=1;
		pause=false;
		pausemenuobject.SetActive(false);
	}
	public void resetgame(){
		Time.timeScale=1;
		pause=false;
		SceneManager.LoadScene("game");
	}
	public void mainmenu(){
		SceneManager.LoadScene("mainmenu");
	}

	public void updatehealthtext(int uihealthcount){
		healthtext.text="Health-"+uihealthcount;
	}

	public void updateenemytext(int uienemycount){
		enemytext.text="Enemy-"+uienemycount;
	}
}
