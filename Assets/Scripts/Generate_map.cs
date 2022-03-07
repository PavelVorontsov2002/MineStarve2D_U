using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generate_map : MonoBehaviour
{
    public const int size = 150;

    public float[,] map = new float[size, size];
    public int[,] resource_map = new int[size, size];

    public Tilemap map_tilemap;
    public Tilemap resource_map_tilemap;
    public Tilemap resourceColliders_map_tilemap;
    public Tile ground;
    public AnimatedTile water;
    public List<Tile> resource_pallete = new List<Tile>();
    public List<Tile> resourceCollider_pallete = new List<Tile>();

    public GameObject player;

    void Start()
    {
        float sid_map = Random.Range(0, 100000);
        float sid_resource_map = Random.Range(0, 100000);
        int zoom = 30;
        int zoom_resource = 6;

        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                if (x < 15 || y < 15 || x > (int)Mathf.Sqrt(map.Length) - 16 || y > (int)Mathf.Sqrt(map.Length) - 16)
                    map[x, y] = 1;
                else
                    map[x, y] = Mathf.PerlinNoise((x + sid_map) / zoom, (y + sid_map) / zoom);

                if (map[x, y] > 0.7)
                {
                    map_tilemap.SetTile(new Vector3Int(x, y, 0), water);
                    resource_map[x, y] = -1;
                }
                else
                {
                    map_tilemap.SetTile(new Vector3Int(x, y, 0), ground);

                    if (x % 2 == 0 && y % 2 == 0 && Random.Range(0, 10000) % 3 == 0)
                    {
                        float num = Mathf.PerlinNoise((x + sid_resource_map) / zoom_resource, (y + sid_resource_map) / zoom_resource);
                        if (num < 0.4)
                        {
                            resource_map[x, y] = Random.Range(16, 20);
                            resourceColliders_map_tilemap.SetTile(new Vector3Int(x, y, -y), resourceCollider_pallete[resource_map[x, y] - 9]);
                        }
                        else if (num > 0.4 && num < 0.8)
                        {
                            resource_map[x, y] = Random.Range(9, 16);
                            resourceColliders_map_tilemap.SetTile(new Vector3Int(x, y, -y), resourceCollider_pallete[resource_map[x, y] - 9]);
                        }
                        else
                            resource_map[x, y] = Random.Range(0, 9);

                        resource_map_tilemap.SetTile(new Vector3Int(x, y, -y), resource_pallete[resource_map[x, y]]);
                    }
                    else
                        resource_map[x, y] = -1;
                }
            }
        }
        resource_map_tilemap.RefreshAllTiles();
        Instantiate(player, transform, true);
    }

    void Update()
    {
		
    }
}
