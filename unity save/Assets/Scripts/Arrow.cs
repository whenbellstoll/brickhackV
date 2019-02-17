using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Arrow : MonoBehaviour {

    private Vector2 velocity;

    private void Update()
    {
        transform.position += (Vector3)velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow")) { return; }

        if (collision.CompareTag("Player"))
        {
            //DO DAMAGE TO PLAYER HERE
        }

        gameObject.SetActive(false);
    }
}
