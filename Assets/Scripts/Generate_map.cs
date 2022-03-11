using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;
using UnityEngine.UI;

public class Generate_map : MonoBehaviour
{
    public const int size = 3000;

    public float[,] map = new float[size, size];
    public int[,] resource_map = new int[size, size];

    public Tilemap map_tilemap;
    public Tile ground;
    public AnimatedTile water;
    public List<GameObject> flowers = new List<GameObject>();
    public List<GameObject> rocks = new List<GameObject>();
    public List<GameObject> trees = new List<GameObject>();
    public List<GameObject> resourses;

    public GameObject player;
    public GameObject cameraLoading;
    public GameObject playerSpawned;

    int percent = 0;
    bool isReady = false;

    void Start()
    {
        StartCoroutine(generateMap());
    }

    IEnumerator generateMap() {
        foreach (GameObject resourse in flowers)
            resourses.Add(resourse);
        foreach (GameObject resourse in rocks)
            resourses.Add(resourse);
        foreach (GameObject resourse in trees)
            resourses.Add(resourse);

        float sid_map = Random.Range(0, 100000);
        float sid_resource_map = Random.Range(0, 100000);
        int zoom = 30;
        int zoom_resource = 6;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                percent = (x * size + y) * 100 / (size * size);
                if (x < 15 || y < 15 || x > (int)Mathf.Sqrt(map.Length) - 16 || y > (int)Mathf.Sqrt(map.Length) - 16)
                    map[x, y] = 1;
                else
                    map[x, y] = Mathf.PerlinNoise((x + sid_map) / zoom, (y + sid_map) / zoom);

                if (map[x, y] > 0.7)
                    resource_map[x, y] = -1;
                else
                {
                    if (x % 2 == 0 && y % 2 == 0 && Random.Range(0, 10000) % 3 == 0)
                    {
                        float num = Mathf.PerlinNoise((x + sid_resource_map) / zoom_resource, (y + sid_resource_map) / zoom_resource);
                        if (num < 0.4)
                        {
                            resource_map[x, y] = Random.Range(16, 20);
                        }
                        else if (num > 0.4 && num < 0.8)
                        {
                            resource_map[x, y] = Random.Range(9, 16);
                        }
                        else
                            resource_map[x, y] = Random.Range(0, 9);
                    }
                    else
                        resource_map[x, y] = -1;
                }
            }
            if (percent % 3 == 0)
                yield return null;
        }
        yield return null;
    }

    void Update()
    {
        cameraLoading.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Scrollbar>().size = percent / 100.0f;
        cameraLoading.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = (percent).ToString() + "%";

        if (percent == 99 && !isReady)
        {
            int value = size / 2 - 10;
            float playerPosX = transform.position.x + size / 2 + Random.Range(-value, value);
            float playerPosY = transform.position.y + size / 2 + Random.Range(-value, value);
            playerSpawned = Instantiate(player, transform, true);
            playerSpawned.transform.position = new Vector3(playerPosX, playerPosY, 0);

            isReady = true;            
        }

        if (isReady) {
            playerSpawned.GetComponent<Player>().cam.gameObject.SetActive(true);
            cameraLoading.SetActive(false);
            playerSpawned.GetComponent<Player>().isReady = true;
        }
    }
}
