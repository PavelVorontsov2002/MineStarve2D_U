using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class Player : MonoBehaviour
{
    int size;
    Animator animator;
    Renderer renderer;
    List<string> animations = new List<string>();
    Rigidbody2D rigidbody;

    float[] dirs = new float[4];
    int dir = 0;
    float speed = 0.05f;

    float deltaTime;
    public Text fpsUI;

    public int offset = 0;

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
        renderer = GetComponent<Renderer>();

        spawnPlayer();
    }

    void Update()
    {
        move();
        anim();
        fps();
    }

    void spawnPlayer() {
        int value = size / 2 - 10;
        float playerPosX = transform.position.x + size / 2 + Random.Range(-value, value);
        float playerPosY = transform.position.y + size / 2 + Random.Range(-value, value);
        transform.position = new UnityEngine.Vector3(playerPosX, playerPosY, 0);
    }
	
    void anim() {
        bool walk = false;

        foreach (float move in dirs) {
            if (move != 0) {
                walk = true;
                break;
            }
        }

        animator.Play(animations[dir + (!walk ? 4 : 0)]);
    }
    void fps() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsUI.text = Mathf.Ceil(fps).ToString();
    }
    void move() {
        dirs[0] = Input.GetKey(KeyCode.UpArrow) && transform.position.y < size - 10 ? speed : 0;
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

        UnityEngine.Vector2 currentPos = transform.position;

        UnityEngine.Vector2 futurePos = currentPos + new UnityEngine.Vector2(dirs[3] - dirs[2], dirs[0] - dirs[1]);

        rigidbody.MovePosition(futurePos);
    }
}
