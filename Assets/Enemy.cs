using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	GameObject target;
	[SerializeField]Animator enemyanimator;
	GameObject enemyattackcolliderobject;
	Collider enemyattackcollider;
	float enemyattackdistance=28f;
	float targetandenemydistance;
	int enemyhealth;
	float enemyspeed=14f;
	Vector3 targetdirection;
	// Vector3 lookdirection;
	Quaternion lookrotate;
	Quaternion spherelookrotate;
	float rotatespeed=4f;
	bool enemynearenemy=false;
	Vector3 enemyandenemydistance;
	GameObject nearbyenemy=null;
	bool targetpassout=false;
	bool enemypassout=false;
	int enemyhitcount;
	Manager enemyvillagegamemanager;

	// Use this for initialization
	void Start () {
		// enemyanimator = GameObject.Find("AssassinEnemyArt").GetComponent<Animator>();
		enemyattackcolliderobject = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
		enemyattackcollider = enemyattackcolliderobject.GetComponent<Collider>();
		enemyvillagegamemanager = GameObject.Find("manager").GetComponent<Manager>();
		settarget(GameObject.Find("Ezio"));
		targetpassout = enemyvillagegamemanager.getplayerpassout();
		enemyhealth=3;
		enemyattackcollider.enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		checktargetandenemydistance();
		followtarget();
		enemyhealthcheck();
	}

	void checktargetandenemydistance(){
		targetandenemydistance = Vector3.Distance(target.transform.position,gameObject.transform.position);
	}
	void followtarget(){
		if(targetandenemydistance<=enemyattackdistance){
			if(enemypassout==false){
				targetpassout=enemyvillagegamemanager.getplayerpassout();
				looktarget();
				// Debug.Log(targetandenemydistance);
				if(targetandenemydistance<=12f){
					if(targetpassout==false){
						gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
						stopenemyrunanimation();
						attacktarget();
						// Debug.Log("EnemyStopFollow");
					}
				}else{
					if(enemynearenemy==true){
						// gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
						gameObject.transform.position = nearbyenemy.transform.position + enemyandenemydistance;
					}
					gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,target.transform.position,enemyspeed*Time.deltaTime);
					playenemyrunanimation();
				}
			}
		}else{
			if(enemypassout==false){
				stopenemyrunanimation();
			}
		}
	}
	void looktarget(){
		targetdirection = target.transform.position - gameObject.transform.position;
		// lookdirection = Vector3.RotateTowards(transform.forward,targetdirection,Time.deltaTime,0);
		// lookrotate = Quaternion.LookRotation(lookdirection,Vector3.up);
		
		lookrotate = Quaternion.LookRotation(targetdirection,Vector3.up);
		// gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,lookrotate,Time.deltaTime);
		
		spherelookrotate = Quaternion.Slerp(gameObject.transform.rotation,lookrotate,rotatespeed*Time.deltaTime);
		if(target.transform.position.y>=8f){spherelookrotate.eulerAngles=new Vector3(0,spherelookrotate.eulerAngles.y,0);}
		gameObject.transform.eulerAngles = spherelookrotate.eulerAngles;
	}
	void attacktarget(){
		enemyattackcollider.enabled=true;
		playenemyattackanimation();
		Invoke("resetenemyattack",1.5f);
	}

	void settarget(GameObject followtarget){
		target = followtarget;
	}

	void enemyhealthcheck(){
		if(enemypassout==false){
			if(enemyhealth<1){
				enemypassout=true;
				if(enemyhitcount==1){
					enemyvillagegamemanager.reduceenemycount();
					playenemypassoutanimation();
					// enemyanimator.Play("passout");
				}
			}
		}
	}

	void playenemyrunanimation(){
		enemyanimator.SetBool("run",true);
	}
	void stopenemyrunanimation(){
		enemyanimator.SetBool("run",false);
	}
	void playenemyattackanimation(){
		enemyanimator.SetTrigger("attack");
	}
	void playenemyhitanimation(){
		enemyanimator.SetTrigger("hit");
	}
	void playenemypassoutanimation(){
		enemyanimator.SetTrigger("passout");
	}

	void resetenemyattack(){
		if(enemyattackcollider.enabled==true){
			enemyattackcollider.enabled=false;
		}
	}

	IEnumerator resetenemyhitcount(){
		yield return new WaitForSeconds(0.5f);
		enemyhitcount=0;
	}

	void OnTriggerEnter(Collider objectcollider){
		if(objectcollider.gameObject.CompareTag("AttackCollider")){
			if(enemypassout==false){
				enemyhitcount=enemyhitcount+1;
				if(enemyhitcount==1){
					// playenemyhitanimation();
					enemyhealth=enemyhealth-1;
					enemyanimator.Play("hit");
					StartCoroutine(resetenemyhitcount());
				}else{
					enemyhitcount=0;
				}
			}
		}
		if(objectcollider.gameObject.CompareTag("Enemy")){
			enemynearenemy=true;
			nearbyenemy=objectcollider.gameObject;
			// enemyandenemydistance = gameObject.transform.position - objectcollider.gameObject.transform.position;
			enemyandenemydistance = gameObject.transform.position - nearbyenemy.transform.position;
		}
	}
	void OnTriggerExit(Collider objectcollider){
		if(objectcollider.gameObject.CompareTag("Enemy")){
			enemynearenemy=false;
			nearbyenemy=null;
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(gameObject.transform.position,enemyattackdistance);
	}
}
