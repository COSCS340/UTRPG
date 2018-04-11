﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
    public int maxHealth;
    public int health;
    public int maxMana;
    public int mana;
    float lastDamageTaken = 0f;
	// Use this for initialization
	void Start () {
        maxHealth = 100;
        health = maxHealth;
        maxMana = 100;
        mana = maxMana;
        Physics.IgnoreLayerCollision(0, 9);
	}




    public bool addSubtractHP(int amount)
    {
        Debug.Log(amount);
        if(amount < 0 && Time.time < lastDamageTaken + 1)
        {
            return true;
        } else
        {
            lastDamageTaken = Time.time;
        }
        if(health + amount > maxHealth)
        {
            health = maxHealth;
            return true;
        } else if(health + amount < 0)
        {
            Debug.Log(health + amount);
            health = 0;
            return false;
        }
        Debug.Log(health + amount);
        health += amount;
        return true;
    }

    public bool addSubtractMana(int amount)
    {
        if(mana + amount > maxMana)
        {
            mana = maxMana;
            return true;
        } else if(mana + amount < 0)
        {
            return false;
        }
        mana += amount;
        return true;
    }


    // Update is called once per frame
    
}
