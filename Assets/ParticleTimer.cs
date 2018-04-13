using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimer : MonoBehaviour {
    float startTime;
    float duration = 2;
    bool isActive;

    private void Start()
    {
        this.gameObject.GetComponent<ParticleSystem>().Stop();
        isActive = false;
    }

    public void startParticle()
    {
        this.gameObject.GetComponent<ParticleSystem>().Play();
        startTime = Time.time;
        isActive = true;
    }

    void Update () {
		if(isActive && Time.time > startTime + duration)
        {
            this.gameObject.GetComponent<ParticleSystem>().Stop();
            isActive = false;
        }
	}
}
