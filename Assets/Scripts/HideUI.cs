using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUI : MonoBehaviour {

	public bool isVisible = false;

	// Method called when clicking the button Hide/View
	// It hides or displays the UI according to the isVisible boolean
	// Its only purpose is comfort of view
	public void hideUI()
	{
		// Loop to set the objects in the UI in the Canvas as Active or Inactive
		foreach(Transform compo in GameObject.Find("/Canvas/UI/").transform)
		{
			compo.gameObject.SetActive(isVisible);
		}
		// Invert the isVisible variable
		isVisible = !isVisible;
		// Change the button text to "View" or "Hide" according to the isVisible value
		if (isVisible)
		{
			GameObject.Find("/Canvas/Fixed UI/ViewButton/Text").GetComponent<Text>().text = "View";
		}
		else
		{
			GameObject.Find("/Canvas/Fixed UI/ViewButton/Text").GetComponent<Text>().text = "Hide";
		}
	}	
}
