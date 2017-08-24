using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GameMangerScript : MonoBehaviour {

	// publics
	public int WinCount = 3;
	public float FinishingLine = 20;
	public float m_StartDelay = 1;         
	public float m_EndDelay = 100000000;
	public GameObject DeadPlane;
	public GameObject Player;
	public PlayerController PlayerControllerScript;
	public GameObject Pickup;
	public Text Count; 
	public Text Win;
	public Text SetSpeedText; 
	public Text SpeedShower; 
	public Text TimeShower; 
	// !!!!!!UI involved! need to add public, UI related
	//	public Text CurrentSpeed;
	//	public Text WinWithTime;
	public Button RestartButton;
	public Button MainMenuButton;
	public Button StartButton;
	public InputField inputSpeedField;
	public Button SetSpeedButton;

	//privates
	//	private Regex rgx1 = new Regex(@"[0-9]*");
	//	private Regex rgx2 = new Regex(@"[0-9]*\.[0-9]*");
	private WaitForSeconds m_StartWait;     
	private WaitForSeconds m_EndWait;
	private Vector3 NewPickupPosition;
	private int counts;
	private bool Playing;
	private double timetaken;
	// private, UI related

	// Use this for initialization
	void Start () {
		m_StartWait = new WaitForSeconds(m_StartDelay);
		m_EndWait = new WaitForSeconds(m_EndDelay);
		PlayerControllerScript = Player.GetComponent<PlayerController> ();
		Playing = false;
		ResetPlayer ();
		timetaken = 0;
		Playing = false;
		DisableControl ();
		timetaken = 0;
		counts = 0;
		Count.text = "Collected: " + counts.ToString();

		SetButtonOnMenu ();

//		StartCoroutine(GameLoop());
	}

	//the game loop state machine
	private IEnumerator GameLoop()
	{
		yield return StartCoroutine(RoundStarting());
		yield return StartCoroutine(RoundPlaying());
		yield return StartCoroutine(RoundEnding());
	}


	private IEnumerator RoundStarting()
	{
		SetButtonWhenPlaying ();

		ResetPlayer ();
		SetPickupPosition ();
		timetaken = 0;
		Playing = false;
		DisableControl ();
		timetaken = 0;
		counts = 0;
		Count.text = "Collected: " + counts.ToString();
		Win.text = "Ready?GO!";
		yield return m_StartWait;
	}


	private IEnumerator RoundPlaying()
	{
		SetButtonWhenPlaying ();

		Win.text = "";
		EnableControl ();
		Playing = true;
		while (counts < WinCount) {
			if (Pickup.activeSelf == false) {
				counts++;
				SetPickupPosition ();
				Count.text = "Collected: " + counts.ToString();
			}

			if (DeadPlane.activeSelf == false) {
				break;
			}
			yield return null;
		}
	}


	private IEnumerator RoundEnding()
	{
		DisableControl ();
		ResetPlayer ();
		Playing = false;

		if (DeadPlane.activeSelf == false) {
			Win.text = "You fall out of the edge, please try again!";
			DeadPlane.SetActive(true);
		}
		else if (timetaken < FinishingLine) {
			Win.text = "WIN with time: " + timetaken.ToString ();
		}
		else{
			Win.text = "Try again! time: "+ timetaken.ToString ();
		}


		SetButtonOnMenu ();

		yield return m_EndWait;
	}

	//spawn a player incase fall down
	private void ResetPlayer () {
		Player.transform.position = new Vector3 (0, 0.5f, 0);
		Player.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		Player.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 0, 0);
	}

	//reset Pickup to a random position
	public void SetPickupPosition () {

		Vector3 newPosition = new Vector3(Random.Range(-9.5f, 9.5f), 0.5f, Random.Range(-9.5f, 9.5f));

		while (Vector3.Distance (newPosition, Player.transform.position) <= 2) {
			newPosition = new Vector3 (Random.Range (-9.5f, 9.5f), 0.5f, Random.Range (-9.5f, 9.5f));
		}

		Pickup.transform.position = newPosition;

		Pickup.SetActive (true);
	}

	private void DisableControl () {
		PlayerControllerScript.enabled = false;
	}

	private void EnableControl () {
		PlayerControllerScript.enabled = true;
	}

	// for UI button show and disable
	private void SetButtonOnMenu(){
		StartButton.gameObject.SetActive (true);
		inputSpeedField.gameObject.SetActive (true);
		SetSpeedButton.gameObject.SetActive (true);
		RestartButton.gameObject.SetActive (false);
		MainMenuButton.gameObject.SetActive (false);
	}

	private void SetButtonWhenPlaying(){
		StartButton.gameObject.SetActive (false);
		inputSpeedField.gameObject.SetActive (false);
		SetSpeedButton.gameObject.SetActive (false);
		RestartButton.gameObject.SetActive (true);
		MainMenuButton.gameObject.SetActive (true);
	}

	void Update() {
		if (Playing){
			timetaken += Time.deltaTime;
			TimeShower.text = "Time used: " + timetaken.ToString ();
		}
	}

	// Start button and restart button shares the same functionality.
	public void Restart() {
		StartButton.gameObject.SetActive (false);
		RestartButton.gameObject.SetActive (true);
		MainMenuButton.gameObject.SetActive (true);

		StopCoroutine (GameLoop ());
		StartCoroutine (GameLoop ());
	}

	// MainMenuButton uses this
	public void MainMenu() {
		Start ();
	}

	// SetButton uses this
	public void SetSpeedFromButton() {
		try{
			float.Parse (SetSpeedText.text);
		}
		catch(System.Exception e){
			return;
		}
		float SpeedINputs = float.Parse (SetSpeedText.text);
		PlayerControllerScript.SetSpeed (SpeedINputs);
		SpeedShower.text = "Speed: " + SpeedINputs.ToString ();
	}
}
