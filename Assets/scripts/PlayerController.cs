using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	int MAX_KARMA = 100;
	float MAX_VELOCITY = 5.4f;

	//Física
	Rigidbody2D rb2d;
	Vector2 input;
	Vector2 tempVelocity;


	//Stats
	int karma = 50;
	bool isShooting = false;
	bool isStunning = false;
	bool isArmed = false;
	bool isDead = false;

	GameObject CurrentItem = null;

	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		input = new Vector2 ();
		tempVelocity = new Vector2 ();
	}

	void Update ()
	{
		HandleInput ();
	}

	void HandleInput ()
	{
		input.Set(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		//Action
		if (Input.GetKeyDown (KeyCode.J)) {
			if (isArmed == true) {
				isShooting = true;
			} else {
				isStunning = true;
			}
		} else {
			isShooting = false;
			isStunning = false;
		}

		if (Input.GetKeyDown (KeyCode.L)) {
			if (CurrentItem != null) {
				CurrentItem.transform.SetParent (null);
				CurrentItem.SetActive (true);
				CurrentItem = null;
			}
		}
	}

	void FixedUpdate ()
	{
		tempVelocity.x = rb2d.velocity.x;

		tempVelocity.x += input.x;
		tempVelocity.x = Mathf.Clamp (tempVelocity.x, -MAX_VELOCITY, MAX_VELOCITY); 
		tempVelocity.y += input.y;
		tempVelocity.y = Mathf.Clamp (tempVelocity.y, -MAX_VELOCITY, MAX_VELOCITY);

		if (input.x == 0)
		{
			//Diminui a velocidade para zero
			tempVelocity.x = Mathf.Lerp (tempVelocity.x, 0f, .14f);
		}

		if (input.y == 0) 
		{
			//diminui a velocidade para zero
			tempVelocity.y = Mathf.Lerp (tempVelocity.y, 0f, .14f);
		}

		rb2d.velocity = new Vector2 (tempVelocity.x, tempVelocity.y);
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		//Debug.Log("collisao");
		if (Input.GetKeyDown (KeyCode.K)) 
		{
			//Debug.Log("clique");
			if (CurrentItem == null && collision.tag == "item")
			{
				GameObject item = collision.transform.gameObject;
				item.transform.SetParent(this.transform);
				CurrentItem = item;
				item.SetActive(false);
			}
		}
	}
}
