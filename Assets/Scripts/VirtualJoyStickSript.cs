using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoyStickSript : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

//	public RectTransform VJSRect;
	private Image BGImg;
	private Image StickImg;
	private Vector3 InputV;

	private void Start(){
//		VJSRect = GetComponent<RectTransform> ();
		BGImg = transform.GetChild (0).GetComponent<Image> ();
		StickImg = transform.GetChild (0).GetChild (0).GetComponent<Image> ();
	}

	public virtual void OnDrag(PointerEventData ped){
		Vector2 posi;
		// We get the local position of the joystick
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(BGImg.rectTransform,ped.position, ped.pressEventCamera, out posi)){
			posi.x = posi.x / BGImg.rectTransform.sizeDelta.x;
			posi.y = posi.y / BGImg.rectTransform.sizeDelta.y;
			InputV = new Vector3 (2 * (posi.x + 0.5f), 0, 2 * (posi.y - 0.5f));
			InputV = (InputV.magnitude > 1.0f) ? InputV.normalized : InputV;
//			Debug.Log ("InputV:" + InputV.ToString ());
			// Move joystick img around the pad
			StickImg.rectTransform.anchoredPosition = new Vector3(InputV.x * (StickImg.rectTransform.sizeDelta.x/2), InputV.z * (StickImg.rectTransform.sizeDelta.y/2));
		}
	}

	public virtual void OnPointerDown(PointerEventData ped){
		OnDrag (ped);
	}

	public virtual void OnPointerUp(PointerEventData ped){
		InputV = Vector3.zero;
		StickImg.rectTransform.anchoredPosition = Vector3.zero;
	}

	// Two choices: first, if input is a touch on the virtual joystick, it will return the value (-1,1) horizontal and vetical.
	// else if input is by input "Horizontal" or "Vertical" (e.g. mac keyboard), it will return that value.
	public float Horizontal(){
		if (InputV.x != 0)
			return InputV.x;
		else
			return Input.GetAxis ("Horizontal");
	}

	public float Vertical(){
		if (InputV.z != 0)
			return InputV.z;
		else
			return Input.GetAxis ("Vertical");
	}
		
}
