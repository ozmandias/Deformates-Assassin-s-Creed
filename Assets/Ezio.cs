using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezio : MonoBehaviour {

	Animator ezioanimator;
	CharacterController eziocontroller;
	// Rigidbody eziobody;
	GameObject leftattackcolliderobject;
	GameObject rightattackcolliderobject;
	Collider leftattackcollider;
	Collider rightattackcollider;
	GameObject eziocamera;
	float horizontal;
	float vertical;
	Vector3 direction;
	float rotatedirection;
	float smoothrotatedirection;
	float smoothrotatevelocity;
	float smoothrotatetime=0.2f;
	float smoothrotatespeed=Mathf.Infinity;
	int health;
	float speed=28f;
	float jump=4f;
	float airdirection;
	float gravity=0.2f;
	bool doublejump=false;
	bool ezioattack=false;
	int ezioattackcount=0;
	AnimatorClipInfo[] ezioanimatorclipinfo;
	bool movekeypress=false;
	bool passout=false;
	int hitcount;
	Manager villagegamemanager;
	UIManager userinterfacemanager;

	// Use this for initialization
	void Start () {
		ezioanimator = gameObject.GetComponent<Animator>();
		eziocontroller = gameObject.GetComponent<CharacterController>();
		// eziobody = gameObject.GetComponent<Rigidbody>();
		// leftattackcolliderobject = GameObject.Find("AttackColliderL");
		// rightattackcolliderobject = GameObject.Find("AttackColliderR");
		leftattackcolliderobject = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
		rightattackcolliderobject = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
		leftattackcollider = leftattackcolliderobject.GetComponent<Collider>();
		rightattackcollider = rightattackcolliderobject.GetComponent<Collider>();
		eziocamera = GameObject.Find("Main Camera");
		ezioanimatorclipinfo = ezioanimator.GetCurrentAnimatorClipInfo(0);
		villagegamemanager = GameObject.Find("manager").GetComponent<Manager>();
		userinterfacemanager = GameObject.Find("UICanvas").GetComponent<UIManager>();
		health=3;
		leftattackcollider.enabled=false;
		rightattackcollider.enabled=false;
		villagegamemanager.setplayerpassout(passout);
		userinterfacemanager.updatehealthtext(health);
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		move();
		attack();
		healthcheck();
	}

	void move(){
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		direction = new Vector3(horizontal,0,vertical);

		if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
			rotatedirection = Mathf.Atan2(horizontal,vertical)*Mathf.Rad2Deg + eziocamera.gameObject.transform.eulerAngles.y;
			smoothrotatedirection = Mathf.SmoothDampAngle(rotatedirection,smoothrotatedirection,ref smoothrotatevelocity,smoothrotatetime,smoothrotatespeed,Time.deltaTime);
			if(passout==false){
				gameObject.transform.eulerAngles = Vector3.up * smoothrotatedirection;
			}
		}

		if(Input.GetKeyDown(KeyCode.W)){
			playrunanimation();
			movekeypress=true;
		}
		if(Input.GetKeyDown(KeyCode.A)){
			playrunanimation();
			movekeypress=true;
		}
		if(Input.GetKeyDown(KeyCode.S)){
			playrunanimation();
			movekeypress=true;
		}
		if(Input.GetKeyDown(KeyCode.D)){
			playrunanimation();
			movekeypress=true;
		}
		if(Input.GetKeyUp(KeyCode.W)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				playrunanimation();
				movekeypress=true;
			}else{
				stoprunanimation();
				movekeypress=false;
			}
		}
		if(Input.GetKeyUp(KeyCode.A)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				playrunanimation();
				movekeypress=true;
			}else{
				stoprunanimation();
				movekeypress=false;
			}
		}
		if(Input.GetKeyUp(KeyCode.S)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				playrunanimation();
				movekeypress=true;
			}else{
				stoprunanimation();
				movekeypress=false;
			}
		}
		if(Input.GetKeyUp(KeyCode.D)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				playrunanimation();
				movekeypress=true;
			}else{
				stoprunanimation();
				movekeypress=false;
			}
		}

		if(eziocontroller.isGrounded==true&&passout==false){
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

		direction = transform.forward;
		
		// direction.y = direction.y-gravity;
		airdirection = airdirection-gravity;
		direction.y = airdirection;
		if(movekeypress==false||ezioattack==true||passout==true){
			direction = new Vector3(0,direction.y,0);
		}
		eziocontroller.Move(direction*speed*Time.deltaTime);
		// gameObject.transform.Translate(direction*speed*Time.deltaTime);
		// gameObject.transform.Translate(direction*speed*Time.deltaTime,Space.World);
	}

	void attack(){
		// if(ezioattack==true){
		// 	playattackanimation();
		// }else{
		// 	stopattackanimation();
		// }
		if(Input.GetKeyDown(KeyCode.Mouse0)&&passout==false){
			ezioattack=true;
			// ezioattackcount=ezioattackcount+1;
			ezioattackcount=Random.Range(1,3);
			if(ezioattackcount==1){
				// playattackanimation();
				rightattackcollider.enabled=true;
				ezioanimator.Play("attack");
			}else if(ezioattackcount==2){
				// playattack2animation();
				leftattackcollider.enabled=true;
				ezioanimator.Play("attack2");
			}else if(ezioattackcount==3){
				// playattack3animation();
				ezioanimator.Play("attack3");
			}
			// StartCoroutine(resetezioattack());
			// resetezioattack();
			Invoke("resetezioattack",0.5f);
		}
		// if(Input.GetKeyUp(KeyCode.Mouse0)){
		// 	ezioattack=false;
		// }
	}

	void healthcheck(){
		if(health<0){
			if(hitcount==1){
				passout=true;
				villagegamemanager.setplayerpassout(passout);
				playpassoutanimation();
			}
		}
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
	void playhitanimation(){
		ezioanimator.SetTrigger("hit");
	}
	void playpassoutanimation(){
		ezioanimator.SetTrigger("passout");
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
			if(leftattackcollider.enabled==true){
				leftattackcollider.enabled=false;
			}
			if(rightattackcollider.enabled==true){
				rightattackcollider.enabled=false;
			}
		}
	}

	IEnumerator resethitcount(){
		yield return new WaitForSeconds(0.5f);
		hitcount=0;
	}

	void OnTriggerEnter(Collider objectcollider){
		if(objectcollider.gameObject.CompareTag("EnemyAttackCollider")){
			hitcount=hitcount+1;
			if(hitcount==1){
				health=health-1;
				userinterfacemanager.updatehealthtext(health);
				playhitanimation();
				StartCoroutine(resethitcount());
			}else{
				hitcount=0;
			}
		}
	}
}
