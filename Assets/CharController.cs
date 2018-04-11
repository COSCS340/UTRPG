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
    //public GameObject[] groundObjects;
    public Dictionary<Vector3, GameObject> touchingObjects;
    public GameObject test;
    public GameObject spellGameObject;
    //blic GameObject cam;
    Vector3 mouse;
    public GameObject g;

    bool currentlyRunning = false;
    private void OnTriggerExit(Collider other)
    {
        touchingObjects.Remove(other.gameObject.transform.position);
        //touchingObjects.Remove(test.GetComponent<Map>().map[new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.z)].transform.position);
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
            //GameObject closest = findClosestGroundObject(test.GetComponent<Map>().map);
            if(currentlyRunning == false)
            {
                currentlyRunning = true;
                anim.Play("RUN00_F", -1, 0f);
            }
            
            Move();
            //MoveY();
            needToCheckMovement = true;
        }
        else if (needToCheckMovement == true)
        {
            //GameObject closest = findClosestGroundObject(test.GetComponent<Map>().map);
            //moveTowards(closest);
            //Debug.Log((currentlyRunning));
            if(currentlyRunning == true)
            {
                anim.Play("WAIT00", -1, 0f);
            }
            currentlyRunning = false;
            needToCheckMovement = false;
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            float hor = Camera.main.GetComponent<CameraFollow>().horiz;
            float vert = Camera.main.GetComponent<CameraFollow>().vert;
            Vector3 startingSpot = new Vector3(mousePosition.x + (mousePosition.y * vert), transform.position.y, mousePosition.z + (mousePosition.y * hor));
            //startingSpot = (3 / (startingSpot.magnitude)) * startingSpot;
            Vector3 result = startingSpot - gameObject.transform.position;
            spellGameObject.GetComponent<Spell>().createNewSpell(gameObject.transform.position + ((3/result.magnitude) * result));
            
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            //GameObject g = findClosestGroundObjectWVector(test.GetComponent<Map>().map, mousePosition);
            float hor = Camera.main.GetComponent<CameraFollow>().horiz;
            float vert = Camera.main.GetComponent<CameraFollow>().vert;
            //transform.position = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
            transform.position = new Vector3(mousePosition.x + (mousePosition.y * vert), transform.position.y, mousePosition.z + (mousePosition.y * hor));
            
            
            

        }
    }

    void MoveY()
    {
        GameObject closest = findClosestGroundObject(test.GetComponent<Map>().map);
        //GameObject closest = findClosestGroundObject(touchingObjects.Values.ToList());
        if (closest)
        {
            //if((closest.transform.position.y + .5)> this.gameObject.transform.position.y)
            //{
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, (float)(closest.transform.position.y + 1), transform.position.z), 5);
                //transform.position += new Vector3(0, closest.transform.position.y - this.gameObject.transform.position.y);
            //} else if(transform.position.y > (closest.transform.position.y + .5))
            //{
                //transform.position -= new Vector3(0, this.gameObject.transform.position.y - closest.transform.position.y);
            //}
        } else
        {
            if(transform.position.y != 1.5)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, (float)1.5, transform.position.y), 5);
            }
        }
    }

    void Move()
    {
        //Vector3 direction;
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

    double distanceCalc(GameObject g1, GameObject g2)
    {
        //return Math.Sqrt (Math.Pow (g2.transform.position.x - g1.transform.position.x, 2) + Math.Pow (g2.transform.position.z - g1.transform.position.z, 2));
        Renderer rend1 = g1.GetComponent<Renderer>();
        Renderer rend2 = g2.GetComponent<Renderer>();
        return Math.Sqrt(Math.Pow(rend2.bounds.center.x - rend1.bounds.center.x, 2) + Math.Pow(rend2.bounds.center.y - rend1.bounds.center.y, 2)
            + Math.Pow(rend2.bounds.center.z - rend1.bounds.center.z, 2));
    }

    GameObject findClosestGroundObject(List<GameObject> gameObjectsList)
    {
        //Debug.Log(groundObjects.Length);
        if (gameObjectsList.Count == 0)
        {
            return null;
            //groundObjects = GameObject.FindGameObjectsWithTag("GroundObject");
        }
        double distance;
        double bestDistance = distanceCalc(this.gameObject, gameObjectsList[0]);
        GameObject closest = gameObjectsList[0];
        CapsuleCollider playerCollider = this.gameObject.GetComponent<CapsuleCollider>();
        for (int i = 1; i < gameObjectsList.Count; i++)
        {
            //if (playerCollider.IsTouching(groundObjects[i].GetComponent<Collider2D>())) {
            distance = distanceCalc(this.gameObject, gameObjectsList[i]);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                closest = gameObjectsList[i];
            }
        }
        return closest;
    }

    double distanceCalcVector(Vector3 g1, GameObject g2)
    {
        Renderer rend2 = g2.GetComponent<Renderer>();
        return Math.Sqrt(Math.Pow(rend2.bounds.center.x - g1.x, 2) + Math.Pow(rend2.bounds.center.y - g1.y, 2) + Math.Pow(rend2.bounds.center.z - g1.z, 2));
    }
    
    GameObject findClosestGroundObjectWVector(List<GameObject> gameObjectsList, Vector3 g1)
    {
        if(gameObjectsList.Count == 0)
        {
            return null;
        }
        double distance;
        double bestDistance = distanceCalcVector(g1, gameObjectsList[0]);
        GameObject closest = gameObjectsList[0];
        for(int i = 1; i < gameObjectsList.Count; i++)
        {
            distance = distanceCalcVector(g1, gameObjectsList[i]);
            if(distance < bestDistance)
            {
                bestDistance = distance;
                closest = gameObjectsList[i];
            }
        }
        return closest;
    }
    

    void moveTowards(GameObject target)
    {
        float playerRad = this.gameObject.transform.position.z / 2;
        float targetRad = target.transform.position.z / 2;
        Renderer playerRend = this.gameObject.GetComponent<Renderer>();
        Renderer targetRend = target.GetComponent<Renderer>();
        Vector3 desiredPosition = new Vector3(targetRend.bounds.center.x, (float)(target.transform.position.y + 1.5), targetRend.bounds.center.z);
        while (Input.anyKey == false && this.gameObject.transform.position != desiredPosition)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, desiredPosition, moveSpeed * Time.deltaTime);
        }
    }
}