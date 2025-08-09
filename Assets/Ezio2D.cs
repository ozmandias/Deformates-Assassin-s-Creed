using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezio2D : MonoBehaviour {

	Animator ezioanimator;
	CharacterController eziocontroller;
	float horizontal;
	Vector3 direction;
	float speed=8f;
	float jump=4f;
	float airdirection;
	float gravity=0.2f;
	bool doublejump=false;
	bool ezioattack=false;
	int ezioattackcount=0;
	AnimatorClipInfo[] ezioanimatorclipinfo;

	// Use this for initialization
	void Start () {
		ezioanimator = gameObject.GetComponent<Animator>();
		eziocontroller = gameObject.GetComponent<CharacterController>();
		ezioanimatorclipinfo = ezioanimator.GetCurrentAnimatorClipInfo(0);
	}
	
	// Update is called once per frame
	void Update () {
		move();
		attack();
	}

	void move(){
		horizontal = Input.GetAxis("Horizontal");
		direction = new Vector3(0,0,horizontal);

		if(Input.GetKeyDown(KeyCode.A)){
			playrunanimation();
			gameObject.transform.eulerAngles = new Vector3(0,-180f,0);
			direction = new Vector3(0,0,-1);
		}
		if(Input.GetKeyDown(KeyCode.D)){
			playrunanimation();
			gameObject.transform.eulerAngles = new Vector3(0,0,0);
			direction = new Vector3(0,0,1);
		}
		if(Input.GetKeyUp(KeyCode.A)){
			if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)){
				playrunanimation();
			}else{
				stoprunanimation();
			}
		}
		if(Input.GetKeyUp(KeyCode.D)){
			if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)){
				playrunanimation();
			}else{
				stoprunanimation();
			}
		}

		if(eziocontroller.isGrounded==true){
			if(Input.GetKeyDown(KeyCode.Space)){
				// direction.y = jump;
				airdirection = jump;
				doublejump = true;
				playjumpanimation();
			}
		}else{
			if(doublejump==true){
				if(Input.GetKeyDown(KeyCode.Space)){
					airdirection = airdirection+jump;
					doublejump = false;
				}
			}
		}
		
		// direction.y = direction.y-gravity;
		airdirection = airdirection-gravity;
		direction.y = airdirection;
		if(ezioattack==true){
			direction = new Vector3(0,direction.y,0);
		}
		eziocontroller.Move(direction*speed*Time.deltaTime);
		// gameObject.transform.Translate(direction*speed*Time.deltaTime);
	}

	void attack(){
		// if(ezioattack==true){
		// 	playattackanimation();
		// }else{
		// 	stopattackanimation();
		// }
		if(Input.GetKeyDown(KeyCode.Mouse0)){
			ezioattack=true;
			ezioattackcount=ezioattackcount+1;
			if(ezioattackcount==1){
				playattackanimation();
			}else if(ezioattackcount==2){
				playattack2animation();
			}else if(ezioattackcount==3){
				playattack3animation();
			}
			// StartCoroutine(resetezioattack());
			resetezioattack();
		}
		// if(Input.GetKeyUp(KeyCode.Mouse0)){
		// 	ezioattack=false;
		// }
	}
	void playrunanimation(){
		ezioanimator.SetBool("run",true);
	}
	void playattackanimation(){
		// ezioanimator.SetBool("attack",true);
		ezioanimator.SetTrigger("attack");
	}
	void playattack2animation(){
		ezioanimator.SetTrigger("attack2");
	}
	void playattack3animation(){
		ezioanimator.SetTrigger("attack3");
	}
	void playjumpanimation(){
		ezioanimator.SetTrigger("jump");
	}
	void stoprunanimation(){
		ezioanimator.SetBool("run",false);
	}
	// void stopattackanimation(){
	// 	ezioanimator.SetBool("attack",false);
	// }

	void resetezioattack(){
		// yield return new WaitForSeconds(0.5f);
		// if(Input.GetKeyUp(KeyCode.Mouse0)==false && ezioattackcount>0){
		// 	ezioattack=false;
		// 	ezioattackcount=0;
		// }
		if(ezioanimatorclipinfo[0].clip.name=="Idle" || ezioanimatorclipinfo[0].clip.name=="Run" || ezioanimatorclipinfo[0].clip.name=="Jump"){
			ezioattack=false;
			ezioattackcount=0;
		}
	}
}
