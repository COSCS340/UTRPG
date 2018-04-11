using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    Dictionary<Vector3, List<GameObject>> chunks;
    public GameObject player;
    public GameObject[] tiles;
    public List<GameObject> map;
    //0 grass 1 ice 2 cityscape 3
    int tilesetSizes = 4;
    int chunkSize = 30;
    void Start()
    {
        GenerateMap(chunkSize, 3, -1, 0, 0);
        GenerateMap(chunkSize, 3, -1, -30, -30);
        //tilesets
        //map = new List();
    }
    /*
     * size refers to the size of the map.
     * hills refers to how likely curY will jump a unit, the lower, the more likely. A random number will be assigned between 0 and hills.
     * If the number is hills, it will jump up 1, if it is 0, it will jump down 1. Set to a negative number if you want a flat map.
     */
    public void GenerateMap(int size, int tileset, int hills, int startX, int startZ)
    {
        map = new List<GameObject>();
        size = startX + size;
        int curX = startX;
        int curY = 0;
        int curZ = startZ;
        Debug.Log("size is " + size + " curX is " + curX + " curZ is " + curZ);
        for (curX = startX; curX < size; curX++)
        {
            for (curZ = startZ; curZ < size; curZ++)
            {
                Debug.Log("curX is " + curX + " curZ is " + curZ);
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
                Debug.Log("here");
                map[map.Count - 1].AddComponent<BoxCollider>();
                map[map.Count - 1].tag = "GroundObject";
                map[map.Count - 1] = Instantiate(tiles[curTileRand], new Vector3(curX, curY, curZ), transform.rotation);
                Debug.Log("there");
            }
        }
        Vector3 v = new Vector3(startX, 1, startZ);
        chunks[v] = map;
    }

    private void Update()
    {
        
    }

    /*
     * to be implemented later to use maps that we want to look a certain way... such as a castle map
     */
    public void MapReader(string FilePath)
    {

    }
}