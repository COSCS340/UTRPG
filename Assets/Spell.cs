using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
    public GameObject player;
    int alivePeriod = 10;
    float beginTime;
    public int damage;
    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            player.GetComponent<Player>().addSubtractHP(damage * -1);
            Destroy(gameObject);
        }
    }

    void Start () {
        damage = 5;
        beginTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time - beginTime > alivePeriod)
        {
            //Destroy(gameObject);
        }
	}
}
