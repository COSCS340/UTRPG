using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Dictionary<Vector3, List<GameObject>> chunks = new Dictionary<Vector3, List<GameObject>>();
    public GameObject player;
    public GameObject[] tiles;
    //public List<GameObject> map;
    Vector3 currentVector;
    //0 grass 1 ice 2 cityscape 3
    int tilesetSizes = 4;
    int chunkSize = 10;
    public List<GameObject> curMap;
    void Start()
    {
        List<GameObject> map;

        for(int i = -1 * chunkSize * 2; i <= chunkSize * 2; i += chunkSize)
        {
            for(int j = -1 * chunkSize * 2; j <= chunkSize * 2; j += chunkSize)
            {
                map = new List<GameObject>();
                currentVector = new Vector3(i, 1, j);
                chunks.Add(currentVector, map);
            }
        }

        foreach (KeyValuePair<Vector3, List<GameObject>> entry in chunks)
        {
            map = entry.Value;
            GenerateMap(chunkSize, 3, -1, (int) entry.Key.x, (int) entry.Key.z, ref map);
        }
    }

    void destroyMap(ref List<GameObject> currentMap)
    {
        //GameObject g;
        /*foreach(GameObject g in currentMap)
        {
            DestroyImmediate(g);
        }*/
        while (currentMap.Count > 0)
        {
            //g = currentMap[currentMap.Count - 1];
            Destroy(currentMap[currentMap.Count - 1]);
            currentMap.Remove(currentMap[currentMap.Count - 1]);
        }
    }

    /*
     * size refers to the size of the map.
     * hills refers to how likely curY will jump a unit, the lower, the more likely. A random number will be assigned between 0 and hills.
     * If the number is hills, it will jump up 1, if it is 0, it will jump down 1. Set to a negative number if you want a flat map.
     */
    public void GenerateMap(int size, int tileset, int hills, int startX, int startZ, ref List<GameObject> map)
    {
        //map = new List<GameObject>();
        int xSize = startX + size;
        int zSize = startZ + size;
        //size = startX + size;
        int curX = startX;
        int curY = 0;
        int curZ = startZ;
        for (curX = startX; curX < xSize; curX++)
        {
            for (curZ = startZ; curZ < zSize; curZ++)
            {
                if (curX != startX && curZ != startZ)
                {
                    curY = (int)(map[map.Count - chunkSize].transform.position.y + map[map.Count - 1].transform.position.y) / 2;
                }
                else if (curX != startX)
                {
                    curY = (int)map[map.Count - chunkSize].transform.position.y;
                }
                else if (curZ != startZ)
                {
                    curY = (int)map[map.Count - 1].transform.position.y;
                }
                if (hills >= 0)
                {
                    bool randBool = false;
                    int rand = Random.Range(0, hills + 1);
                    while (!randBool)
                    {
                        if (rand == hills)
                        {
                            curY += 1;
                            randBool = true;
                        }
                        else if (rand == 0 && curY > 0)
                        {
                            curY -= 1;
                            randBool = true;
                        }
                        else if (rand != 0)
                        {
                            randBool = true;
                        }
                        else
                        {
                            rand = Random.Range(0, hills + 1);
                        }
                    }
                }
                int curTileRand = Random.Range(0 + (tileset * tilesetSizes), tilesetSizes + (tileset * tilesetSizes));
                map.Add(Instantiate(tiles[curTileRand], new Vector3(curX, curY, curZ), transform.rotation));
                for (int i = 0; i < curY; i++)
                {
                    Instantiate(tiles[curTileRand], new Vector3(curX, i, curZ), transform.rotation);
                }
                map[map.Count - 1].AddComponent<BoxCollider>();
                map[map.Count - 1].tag = "GroundObject";
            }
        }
    }
    List<GameObject> currentMap;
    private void Update()
    {
        foreach(KeyValuePair<Vector3, List<GameObject>> entry in chunks)
        {
            currentMap = entry.Value;
            if(entry.Key.x + (chunkSize * 3) - 1 < player.transform.position.x)
            {
                destroyMap(ref currentMap);
                currentVector = new Vector3(entry.Key.x + (chunkSize * 5), 1, entry.Key.z);
                chunks.Remove(entry.Key);
                curMap = new List<GameObject>();
                chunks.Add(currentVector, curMap);
                GenerateMap(chunkSize, 3, -1, (int)currentVector.x, (int)currentVector.z, ref curMap);
                break;
                //entry.Value.Clear();
            } else if(entry.Key.z + (chunkSize * 3) < player.transform.position.z)
            {
                destroyMap(ref currentMap);
                currentVector = new Vector3(entry.Key.x, 1, entry.Key.z + (chunkSize * 5));
                chunks.Remove(entry.Key);
                curMap = new List<GameObject>();
                chunks.Add(currentVector, curMap);
                GenerateMap(chunkSize, 3, -1, (int) currentVector.x, (int) currentVector.z, ref curMap);
                break;
            } else if(entry.Key.x - (chunkSize * 3) + 10 > player.transform.position.x)
            {
                destroyMap(ref currentMap);
                currentVector = new Vector3(entry.Key.x - (chunkSize * 5), 1, entry.Key.z);
                chunks.Remove(entry.Key);
                curMap = new List<GameObject>();
                chunks.Add(currentVector, curMap);
                GenerateMap(chunkSize, 3, -1, (int)currentVector.x, (int)currentVector.z, ref curMap);
                break;
            } else if(entry.Key.z - (chunkSize * 3) + 10 > player.transform.position.z)
            {
                destroyMap(ref currentMap);
                currentVector = new Vector3(entry.Key.x, 1, entry.Key.z - (chunkSize * 5));
                chunks.Remove(entry.Key);
                curMap = new List<GameObject>();
                chunks.Add(currentVector, curMap);
                GenerateMap(chunkSize, 3, -1, (int) currentVector.x, (int) currentVector.z, ref curMap);
                break;
            }
        }
    }
}