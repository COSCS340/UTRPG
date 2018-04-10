using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour {
    public GameObject player;
    public Image currentHealthBar;
    public Text healthBarText;
    Player playerScript;
    int storedHP = -1;
    int storedMaxHP = -1;
    float ratio;

	void Start () {
        playerScript = player.GetComponent<Player>();
	}
	
	void Update () {
		if(playerScript.health != storedHP || playerScript.maxHealth != storedMaxHP)
        {
            storedHP = playerScript.health;
            storedMaxHP = playerScript.maxHealth;
            ratio = ((float) storedHP) / ((float) storedMaxHP);
            Debug.Log("stored hp: " + storedHP + " storedMaxHP is: " + storedMaxHP);
            Debug.Log("ratio is: " + ratio);
            currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
            healthBarText.text = "Health: " + (ratio*100).ToString("0") + '%';
        }
	}
}
