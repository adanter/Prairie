using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	private bool open;
	private bool showInventoryUI;
	private FirstPersonInteractor playerScript;
	private bool isTextHide = true;
	private const string HIDEUITAG = "HideUI";
	private const string PAUSEMENU = "Pause Menu";
	private const string OPTIONSMENU = "Options Menu";
	private const string KEYBINDINGS = "Keybindings";
	private const string PLAYER = "Player";
	private const string INVENTORYPANEL = "InventoryPanel";
	private const string MENUBUTTON = "escape";

	// Use this for initialization
	void Awake () {
		open = false;
		showInventoryUI = true;
		gameObject.transform.Find(PAUSEMENU).gameObject.SetActive (false);
		gameObject.transform.Find(OPTIONSMENU).gameObject.SetActive (false);
		gameObject.transform.Find(KEYBINDINGS).gameObject.SetActive (false);
		playerScript = (FirstPersonInteractor) GameObject.FindWithTag(PLAYER).GetComponent(typeof(FirstPersonInteractor));
	}

	//Looks for the menu button 
	void Update(){
		if (Input.GetKeyDown (MENUBUTTON)) {
			if (!open) {
				gameObject.transform.Find(PAUSEMENU).gameObject.SetActive (true);
				open = true;
				playerScript.setWorldActive(PAUSEMENU);
				GameObject.FindGameObjectsWithTag (PLAYER)[0].GetComponent<FirstPersonInteractor> ().enabled = false;
			} else {
				resume ();

			}
		}

	}

	//Resumes the game, closes all pause menu UI elements
	public void resume(){
		open = false;
		gameObject.transform.Find(PAUSEMENU).gameObject.SetActive (false);
		gameObject.transform.Find(OPTIONSMENU).gameObject.SetActive (false);
		gameObject.transform.Find(KEYBINDINGS).gameObject.SetActive (false);
		playerScript.setWorldActive(PAUSEMENU);
		GameObject.FindGameObjectsWithTag (PLAYER)[0].GetComponent<FirstPersonInteractor> ().enabled = true;

	}
		
	public void showKeybindings(){
		gameObject.transform.Find(PAUSEMENU).gameObject.SetActive (false);
		gameObject.transform.Find(KEYBINDINGS).gameObject.SetActive (true);
	}
		
	//Turns the inventory opacity to 0 if button is pressed once, and to 106 if pressed agai (alpha value is a percentage)
	public void hideShowInventoryUI(){
		GameObject inventory = GameObject.Find (INVENTORYPANEL);
		if (showInventoryUI) {
			showInventoryUI = false;
			var noColor = inventory.GetComponent<Image>().color;
			noColor.a = 0; 
			inventory.GetComponent<Image>().color= noColor;
		} else {
			showInventoryUI = true;
			var originalColor = inventory.GetComponent<Image>().color;
			originalColor.a = .4156f;
			inventory.GetComponent<Image>().color = originalColor;
		}

	}

	//Changes the text of the Show/hide inventory UI buttom
	public void changeShowHideText(){
		GameObject UIelement = GameObject.FindGameObjectWithTag (HIDEUITAG);
		Text text = UIelement.GetComponentInChildren<Text> ();
		string show = "Show Inventory UI";
		string hide = "Hide Inventory UI";
		if (isTextHide) {
			text.text = show;
			isTextHide = false;
		} else {
			isTextHide = true;
			text.text = hide;
		}
	}

	public void options(){
		gameObject.transform.Find(PAUSEMENU).gameObject.SetActive (false);
		gameObject.transform.Find(OPTIONSMENU).gameObject.SetActive (true);
	}

	public void quit(){
		Application.Quit();
	}

	//Used for all the back buttons on the menu navigation
	public void backToMainMenu(){
		gameObject.transform.Find(PAUSEMENU).gameObject.SetActive (true);
		gameObject.transform.Find(OPTIONSMENU).gameObject.SetActive (false);
		gameObject.transform.Find(KEYBINDINGS).gameObject.SetActive (false);
	}



}
