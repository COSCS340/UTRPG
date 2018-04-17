using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour {
    public GameObject player;
    public Button exitMenu;
    public Button unlockDragonsBreath;
    public Button unlockTeleport;
    public Button increaseDamage;
    public Button addMana;
    public Text upgradeHealth;
    public Button addHealth;
    public GameObject talents;
    public Image currentHealthBar;
    public Image currentManaBar;
    public int numUpgrades = 0;
    int soulModifier = 10;
    public Text healthBarText;
    public Text manaBarText;
    public int numSouls = 10;
    public Text numberSouls;
    public GameObject spell;
    Player playerScript;
    int storedHP = -1;
    int storedMaxHP = -1;
    int storedMana = -1;
    int storedMaxMana = -1;
    float ratio;

    int itemPrice = 40;

	void Start () {
        //Screen.height = 535;
        //Screen.width = 1100;
        numSouls = 2000;
        Screen.SetResolution(800, 535, true, 60);
        playerScript = player.GetComponent<Player>();
        exitMenu.GetComponent<Button>().onClick.AddListener(exit);
        addHealth.GetComponent<Button>().onClick.AddListener(addHealthButton);
        addMana.GetComponent<Button>().onClick.AddListener(addManaButton);
        increaseDamage.GetComponent<Button>().onClick.AddListener(addDamageButton);
        updateTalentText();
	}

    void updateTalentText(){
        upgradeHealth.text = "Souls Required for Each Upgrade: " + (itemPrice + soulModifier); 
    }

    void addSoulModifier(){
        soulModifier += (int) (soulModifier * .1);
    }

    void addDamageButton(){
        if(numSouls >= itemPrice + soulModifier){
            numUpgrades++;
            numSouls -= itemPrice + soulModifier;
            spell.GetComponent<Spell>().playerDamage += (int) (.1 * spell.GetComponent<Spell>().playerDamage);
        }
    }

    void healFully(){
        player.GetComponent<Player>().addSubtractHP(player.GetComponent<Player>().maxHealth);
        player.GetComponent<Player>().addSubtractMana(player.GetComponent<Player>().maxMana);
    }

    void addManaButton(){
        if(numSouls >= itemPrice + soulModifier){
            numUpgrades++;
            numSouls -= itemPrice + soulModifier;
            player.GetComponent<Player>().maxMana += (int) (player.GetComponent<Player>().maxMana * .1);
            healFully();
            addSoulModifier();
            updateTalentText();
        }
    }

    void addHealthButton(){
        if(numSouls >= itemPrice + soulModifier){
            numUpgrades++;
            numSouls -= itemPrice + soulModifier;
            player.GetComponent<Player>().maxHealth += (int) (player.GetComponent<Player>().maxHealth * .1);
            healFully();
            addSoulModifier();
            updateTalentText();
        }
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
        if(playerScript.mana != storedMana || playerScript.maxMana != storedMaxMana)
        {
            storedMana = playerScript.mana;
            storedMaxMana = playerScript.maxMana;
            ratio = ((float) storedMana) / ((float) storedMaxMana);
            currentManaBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
            manaBarText.text = "Mana: " + (ratio*100).ToString("0") + '%';
        }
        numberSouls.text = "Number of Souls Collected: " + numSouls;
	}
}
