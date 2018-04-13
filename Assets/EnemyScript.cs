﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public GameObject playerGameObject;
    public GameObject spellGameObject;
    bool currentlyRunning = false;
    public Animator anim;
    float maxDistance;
    float speed;
    float waitTime;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        speed = 4;
	}

    private void Awake()
    {
        maxDistance = Random.Range(5.0f, 12.0f);
    }

    void attackPlayer()
    {
        waitTime = Time.time;
        Vector3 result = playerGameObject.transform.position - gameObject.transform.position;
        spellGameObject.GetComponent<Spell>().createNewSpell(gameObject.transform.position + ((3 / result.magnitude) * result), false, this.gameObject);
    }

    void moveTowardsPlayer()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerGameObject.transform.position, speed * Time.deltaTime);
        gameObject.transform.LookAt(playerGameObject.transform.position);
    }

    float getDistancePlayer()
    {
        return Vector3.Distance(this.gameObject.transform.position, playerGameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > waitTime + 2)
        {
            if (getDistancePlayer() > maxDistance)
            {
                if (!currentlyRunning)
                {
                    currentlyRunning = true;
                    anim.Play("RUN00_F", -1, 0f);
                }
                moveTowardsPlayer();
            }
            else
            {
                if (currentlyRunning)
                {
                    anim.Play("WAIT00", -1, 0f);
                    currentlyRunning = false;
                }
                attackPlayer();
            }
        }
    }
}
