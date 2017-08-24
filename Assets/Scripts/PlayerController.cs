using UnityEngine;

using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {
	
	[HideInInspector] public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		winText.text = "";
		speed = 10;
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up")){
			other.gameObject.SetActive (false);
		}
		if (other.gameObject.CompareTag ("Finish")) {
			other.gameObject.SetActive (false);
		}
	}

	public void SetSpeed(float SpeedINputs){
		speed = SpeedINputs;
	}
}