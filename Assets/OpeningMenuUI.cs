using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpeningMenuUI : MonoBehaviour {
	public Button newGame;
	//public Scene game;
	public GameObject loreText;
	public Button lore;
	public GameObject controlsText;
	public Button controls;
	public Button quit;
	public Button close;
	public GameObject textbox;
	//public AssetBundle a
	//private var myLoadedAssetBundle: AssetBundle;

	void Start () {
		Screen.SetResolution(1148, 536, true);
		newGame.GetComponent<Button>().onClick.AddListener(newGameButton);
		lore.GetComponent<Button>().onClick.AddListener(loreButton);
		controls.GetComponent<Button>().onClick.AddListener(controlsButton);
		quit.GetComponent<Button>().onClick.AddListener(quitButton);
		close.GetComponent<Button>().onClick.AddListener(closeButton);
	}

	void newGameButton(){
		SceneManager.LoadScene("testScene", LoadSceneMode.Single);
	}

	void loreButton(){
		close.gameObject.SetActive(true);
		textbox.SetActive(true);
		loreText.SetActive(true);
	}

	void controlsButton(){
		close.gameObject.SetActive(true);
		textbox.SetActive(true);
		controlsText.SetActive(true);
	}

	void quitButton(){
		Application.Quit();
	}

	void closeButton(){
		close.gameObject.SetActive(false);
		textbox.SetActive(false);
		controlsText.SetActive(false);
		loreText.SetActive(false);
	}
}
