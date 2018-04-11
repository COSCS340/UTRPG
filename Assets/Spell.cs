using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
    public GameObject player;
    int alivePeriod = 10;
    float lastSpellTime = 0;
    float beginTime = -1;
    public int damage;
    public Vector3 desiredPosition;
    public float moveSpeed = 40f;
    Vector3 normalizePosition;
    Vector3 start;
    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            player.GetComponent<Player>().addSubtractHP(damage * -1);
            //Destroy(gameObject);
        }
    }

    public void createNewSpell(Vector3 startPosition)
    {
        if(Time.time - lastSpellTime > 0.5)
        {
            startPosition = new Vector3(startPosition.x, transform.position.y, startPosition.z);
            Vector3 playerPosition = player.gameObject.transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            float hor = Camera.main.GetComponent<CameraFollow>().horiz;
            float vert = Camera.main.GetComponent<CameraFollow>().vert;
            Vector3 startingSpot = new Vector3(mousePosition.x + (mousePosition.y * vert), 1.5f, mousePosition.z + (mousePosition.y * hor));
            normalizePosition = (startingSpot - new Vector3(playerPosition.x, 0, playerPosition.z)).normalized;
            startingSpot = playerPosition +  (2 * normalizePosition * moveSpeed);
            //startPosition = new Vector3(startPosition.x, transform.position.y, startPosition.z);
            //start = startPosition;
            GameObject g = Instantiate(this.gameObject, startPosition, transform.rotation);
            g.SetActive(true);
            lastSpellTime = Time.time;
        }
    }

    private void OnEnable()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        float hor = Camera.main.GetComponent<CameraFollow>().horiz;
        float vert = Camera.main.GetComponent<CameraFollow>().vert;
        desiredPosition = new Vector3((mousePosition.x + (mousePosition.y * vert)), 1.5f, (mousePosition.z + (mousePosition.y * hor)));
        normalizePosition = (desiredPosition - new Vector3(player.gameObject.transform.position.x, 1.5f, player.gameObject.transform.position.z));
        //normalizePosition = (desiredPosition - start).normalized;
        beginTime = Time.time;
    }

    void Start () {
        //if(gameObject.activeSelf == true)
        //{
        //    gameObject.SetActive(false);
        //} else
        //{
        //    gameObject.SetActive(true);
        //}
        damage = 5;
        
	}
	
	// Update is called once per frame
	void Update () {
        if(beginTime != -1 && Time.time - beginTime > alivePeriod)
        {
            Debug.Log("destroying the object!");
            Destroy(gameObject);
        }
        Debug.Log("not destroying the object");
        moveTowardsDesiredPosition();
	}

    private void moveTowardsDesiredPosition()
    {
        this.gameObject.transform.position += normalizePosition * moveSpeed * Time.deltaTime;
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, desiredPosition, moveSpeed * Time.deltaTime);
    }
}
