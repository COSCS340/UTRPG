using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CharController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 6f;
    Vector3 forward, right;
    public Animator anim;
    bool needToCheckMovement;
    public Dictionary<Vector3, GameObject> touchingObjects;
    public GameObject test;
    public GameObject spellGameObject;
    public GameObject particleManager;
    float teleportTimer = 0f;
    float spellTimer = 0f;
    float manaTimer = 0f;
    Vector3 mouse;
    public GameObject g;
    public bool teleportUnlocked;
    public bool dragonsBreathUnlocked;

    bool currentlyRunning = false;
    private void OnTriggerExit(Collider other)
    {
        touchingObjects.Remove(other.gameObject.transform.position);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        needToCheckMovement = false;
        forward = Camera.main.transform.forward;
        touchingObjects = new Dictionary<Vector3, GameObject>();
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if(currentlyRunning == false)
            {
                currentlyRunning = true;
                anim.Play("RUN00_F", -1, 0f);
            }
            Move();
            needToCheckMovement = true;
        }
        else if (needToCheckMovement == true)
        {
            if(currentlyRunning == true)
            {
                anim.Play("WAIT00", -1, 0f);
            }
            currentlyRunning = false;
            needToCheckMovement = false;
        }
        if (Input.GetKey(KeyCode.Alpha1) && Time.time > spellTimer + .5 && gameObject.GetComponent<Player>().mana >= 1)
        {
            spellTimer = Time.time;
            gameObject.GetComponent<Player>().addSubtractMana(-1);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            float hor = Camera.main.GetComponent<CameraFollow>().horiz;
            float vert = Camera.main.GetComponent<CameraFollow>().vert;
            Vector3 startingSpot = new Vector3(mousePosition.x + (mousePosition.y * vert), transform.position.y, mousePosition.z + (mousePosition.y * hor));
            Vector3 result = startingSpot - gameObject.transform.position;
            spellGameObject.GetComponent<Spell>().createNewSpell(gameObject.transform.position + ((3/result.magnitude) * result), true, this.gameObject);
            
        } else if (Input.GetKey(KeyCode.Alpha3) && Time.time > teleportTimer + 2 && gameObject.GetComponent<Player>().mana >= 5 && teleportUnlocked) {
            teleportTimer = Time.time;
            gameObject.GetComponent<Player>().addSubtractMana(-5);
            particleManager.GetComponent<ParticleTimer>().startParticle();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));            float hor = Camera.main.GetComponent<CameraFollow>().horiz;
            float vert = Camera.main.GetComponent<CameraFollow>().vert;
            transform.position = new Vector3(mousePosition.x + (mousePosition.y * vert), transform.position.y, mousePosition.z + (mousePosition.y * hor));
        }else if(Input.GetKey(KeyCode.Alpha4) && Time.time > spellTimer + .5 && gameObject.GetComponent<Player>().mana >= 5 && dragonsBreathUnlocked){
            spellTimer = Time.time;
            gameObject.GetComponent<Player>().addSubtractMana(-5);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            float hor = Camera.main.GetComponent<CameraFollow>().horiz;
            float vert = Camera.main.GetComponent<CameraFollow>().vert;
            Vector3 startingSpot = new Vector3(mousePosition.x + (mousePosition.y * vert), transform.position.y, mousePosition.z + (mousePosition.y * hor));
            Vector3 result = startingSpot - gameObject.transform.position;
            spellGameObject.GetComponent<Spell>().createNewSpell(gameObject.transform.position + ((3/result.magnitude) * result), true, this.gameObject);
            for(int i = 1; i < 10; i++){
                spellGameObject.GetComponent<Spell>().createNewSpell(new Vector3(gameObject.transform.position.x + i, gameObject.transform.position.y, gameObject.transform.position.z + i) + ((3/result.magnitude) * result), true, this.gameObject);
                spellGameObject.GetComponent<Spell>().createNewSpell(new Vector3(gameObject.transform.position.x - i, gameObject.transform.position.y, gameObject.transform.position.z - i) + ((3/result.magnitude) * result), true, this.gameObject);
            }
        }
        if(Time.time > manaTimer + 2){
            manaTimer = Time.time;
            gameObject.GetComponent<Player>().addSubtractMana(1);
        }
    }

    void Move()
    {
        Vector3 rightMovement;
        Vector3 upMovement;
        if (!Camera.main.GetComponent<CameraFollow>().flip)
        {
            rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey") * Camera.main.GetComponent<CameraFollow>().horiz;
            upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey") * Camera.main.GetComponent<CameraFollow>().vert;
        } else
        {
            rightMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey") * Camera.main.GetComponent<CameraFollow>().horiz;
            upMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey") * Camera.main.GetComponent<CameraFollow>().vert;
        }
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }
}