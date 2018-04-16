using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject playerGameObject;
    public GameObject enemyGameObject;
    public GameObject uiManager;
    public HashSet<GameObject> enemies;
    int maxDistance;

    int currentMaxEnemies;

	// Use this for initialization
	void Start () {
        enemies = new HashSet<GameObject>();
        currentMaxEnemies = 2;
        maxDistance = 30;
	}

    void newEnemy(int levelModifier)
    {
        Vector3 enemyPosition = new Vector3(playerGameObject.transform.position.x + Random.Range(-10.0f, 10.0f), 
            1, playerGameObject.transform.position.z + Random.Range(-10.0f, 10.0f));
        GameObject currentEnemy = Instantiate(enemyGameObject, enemyPosition, transform.rotation);
        enemies.Add(currentEnemy);
        currentEnemy.SetActive(true);
    }

    float getDistancePlayer(GameObject currentEnemy)
    {
        return Vector3.Distance(currentEnemy.transform.position, playerGameObject.transform.position);
    }

	// Update is called once per frame
	void Update () {
        if(enemies.Count < currentMaxEnemies)
        {
            Debug.Log("adding an enemy");
            newEnemy(0);
        }
		foreach(GameObject currentEnemy in enemies)
        {
            if(currentEnemy == null){
                uiManager.GetComponent<UIManagement>().numSouls++;
                enemies.Remove(currentEnemy);
            }
            if(getDistancePlayer(currentEnemy) > maxDistance)
            {
                Destroy(currentEnemy);
                enemies.Remove(currentEnemy);
                break;
            }
        }
	}
}
