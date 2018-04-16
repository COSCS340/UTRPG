using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour {
    public GameObject player;
    public Button exitMenu;
    public GameObject talents;
    public Image currentHealthBar;
    public Text healthBarText;
    public int numSouls;
    public Text numberSouls;
    Player playerScript;
    int storedHP = -1;
    int storedMaxHP = -1;
    float ratio;

	void Start () {
        //Screen.height = 535;
        //Screen.width = 1100;
        Screen.SetResolution(800, 535, true, 60);
        playerScript = player.GetComponent<Player>();
        exitMenu.GetComponent<Button>().onClick.AddListener(exit);
	}

    void exit(){
        talents.SetActive(false);
        Time.timeScale = 1.0f;
    }
	
	void Update () {
        //Debug.Log("screen resolution: " + Screen.currentResolution);
        //Debug.Log("screen dimensions" + Screen.width + " " + Screen.height);
        if(Input.GetKeyDown(KeyCode.T)){
            Time.timeScale = 0.0f;
            talents.SetActive(true);
        }
		if(playerScript.health != storedHP || playerScript.maxHealth != storedMaxHP)
        {
            storedHP = playerScript.health;
            storedMaxHP = playerScript.maxHealth;
            ratio = ((float) storedHP) / ((float) storedMaxHP);
            currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
            healthBarText.text = "Health: " + (ratio*100).ToString("0") + '%';
        }
        numberSouls.text = "Number of Souls Collected: " + numSouls;
	}
}
