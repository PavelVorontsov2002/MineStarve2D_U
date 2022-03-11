using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Numerics;

public class Player : MonoBehaviour
{
    int size;
    public Camera cam;
    Animator animator;
    Generate_map gen;
    List<string> animations = new List<string>();

    Rigidbody2D rigidbody;

    float[] dirs = new float[4];
    int dir = 0;
    float speed = 0.05f;

    float deltaTime;
    public Text fpsUI;

    public bool isReady = false;

    public List<GameObject> gameObjects = new List<GameObject>();
    public List<UnityEngine.Vector3> coord = new List<UnityEngine.Vector3>();

    void Start()
    {
        size = Generate_map.size;

        animations.Add("anim_up");
        animations.Add("anim_down");
        animations.Add("anim_left");
        animations.Add("anim_right");
        animations.Add("anim_idle_up");
        animations.Add("anim_idle_down");
        animations.Add("anim_idle_left");
        animations.Add("anim_idle_right");

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        gen = transform.parent.GetComponent<Generate_map>();
        cam = transform.GetChild(0).gameObject.GetComponent<Camera>();

        showMap();
    }

    void Update()
    {
        if (isReady)
        {
            move();
            anim();
            fps();
            showMap();
        }
    }

    public void showMap() {
        int xs = (int)cam.ScreenToWorldPoint(new UnityEngine.Vector3(0, 0, 0)).x;
        int ys = (int)cam.ScreenToWorldPoint(new UnityEngine.Vector3(0, 0, 0)).y;
        int xe = (int)cam.ScreenToWorldPoint(new UnityEngine.Vector3(cam.pixelWidth, cam.pixelHeight, 0)).x;
        int ye = (int)cam.ScreenToWorldPoint(new UnityEngine.Vector3(cam.pixelWidth, cam.pixelHeight, 0)).y;

        for (int x = xs - 6; x <= xe + 6; x++)
        {
            if (x >= 0 && x < size && ys >= 0 && ye < size)
            {
                gen.map_tilemap.SetTile(new Vector3Int(x, ys - 6, 0), null);
                gen.map_tilemap.SetTile(new Vector3Int(x, ye + 6, 0), null);
            }
        }
        for (int y = ys - 6; y <= ye + 6; y++)
        {
            if (y >= 0 && y < size && xs >= 0 && xe < size)
            {
                gen.map_tilemap.SetTile(new Vector3Int(xs - 6, y, 0), null);
                gen.map_tilemap.SetTile(new Vector3Int(xe + 6, y, 0), null);
            }
        }

        for (int x = xs - 5; x <= xe + 5; x++)
        {
            for (int y = ys - 5; y <= ye + 5; y++)
            {
                if (x >= 0 && x < size && y >= 0 && y < size)
                {
                    if (gen.map[x, y] > 0.7)
                        gen.map_tilemap.SetTile(new Vector3Int(x, y, 0), gen.water);
                    else
                        gen.map_tilemap.SetTile(new Vector3Int(x, y, 0), gen.ground);

                    if (gen.resource_map[x, y] >= 0 && coord.IndexOf(new UnityEngine.Vector3(x, y, 0)) == -1)
                    {
                        coord.Add(new UnityEngine.Vector3(x, y, 0));
                        gameObjects.Add(Instantiate(gen.resourses[gen.resource_map[x, y]], gen.transform));
                        gameObjects[gameObjects.Count-1].transform.position = new UnityEngine.Vector3(x, y, 0);
                    }
                }
            }
        }

        for ( int i = 0; i < gameObjects.Count; i++) {
            float d = UnityEngine.Vector3.Distance(gameObjects[i].transform.position, transform.position);
            if (d > 20) {
                coord.RemoveAt(i);
                Destroy(gameObjects[i]);
                gameObjects.RemoveAt(i);
            }
        }
    }

    void anim()
    {
        bool walk = false;

        /*foreach (float move in dirs)
        {
            if (move != 0)
            {
                walk = true;
                break;
            }
        }*/
        UnityEngine.Vector2 input = GetComponent<PlayerInput>().actions["Move"].ReadValue<UnityEngine.Vector2>();
        if (input.x != 0 || input.y != 0)
            walk = true;

        animator.Play(animations[dir + (!walk ? 4 : 0)]);
    }
    void fps()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsUI.text = Mathf.Ceil(fps).ToString();
    }
    void move()
    {
        /*dirs[0] = Input.GetKey(KeyCode.UpArrow) && transform.position.y < size - 10 ? speed : 0;
        dirs[1] = Input.GetKey(KeyCode.DownArrow) && transform.position.y > 10 ? speed : 0;
        dirs[2] = Input.GetKey(KeyCode.LeftArrow) && transform.position.x > 10 ? speed : 0;
        dirs[3] = Input.GetKey(KeyCode.RightArrow) && transform.position.x < size - 10 ? speed : 0;
        
        if (dirs[0] != 0 && dirs[1] == 0)
            dir = 0;
        else if (dirs[1] != 0 && dirs[0] == 0)
            dir = 1;
        else if (dirs[2] != 0 && dirs[3] == 0)
            dir = 2;
        else if (dirs[3] != 0 && dirs[2] == 0)
            dir = 3;
         */

        UnityEngine.Vector2 input = GetComponent<PlayerInput>().actions["Move"].ReadValue<UnityEngine.Vector2>();
        if (input.y > input.x) {
            if (input.y < -input.x)
                dir = 2;
            else 
                dir = 0;
        }
        else {
            if (input.y > -input.x)
                dir = 3;
            else
                dir = 1;
        }

        UnityEngine.Vector2 currentPos = transform.position;

        //UnityEngine.Vector2 futurePos = currentPos + new UnityEngine.Vector2(dirs[3] - dirs[2], dirs[0] - dirs[1]);

        UnityEngine.Vector2 futurePos = currentPos + new UnityEngine.Vector2(input.x * speed, input.y * speed);

        rigidbody.MovePosition(futurePos);
    }
}
