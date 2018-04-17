using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject playerGameObject;
    public GameObject enemyGameObject;
    public GameObject uiManager;
    public GameObject spell;
    public HashSet<GameObject> enemies;
    int numUpgrades;
    int maxDistance;
    bool needToCheck = false;

    int currentMaxEnemies;

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
        currentEnemy.GetComponent<Player>().maxHealth += Random.Range(0, numUpgrades);
        currentEnemy.SetActive(true);
    }

    float getDistancePlayer(GameObject currentEnemy)
    {
        return Vector3.Distance(currentEnemy.transform.position, playerGameObject.transform.position);
    }

	void Update () {
        if(enemies.Count < currentMaxEnemies)
        {
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
        numUpgrades = uiManager.GetComponent<UIManagement>().numUpgrades;
        if(needToCheck && numUpgrades % 10 == 0){
            needToCheck = false;
            currentMaxEnemies++;
        } else if(needToCheck && numUpgrades % 5 == 0){
            needToCheck = false;
            spell.GetComponent<Spell>().addDamage(1);
        } else if(!needToCheck && numUpgrades % 10 != 0 && numUpgrades % 5 != 0){
            needToCheck = true;
        }
	}
}
