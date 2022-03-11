using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{

    SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        renderer.color = new Color(1f, 1f, 1f, .5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        renderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
