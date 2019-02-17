using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Arrow : MonoBehaviour {

    [SerializeField] private Vector2 velocity;

    private void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger enter");
        if(collision.gameObject.layer == 8)
        {
            gameObject.SetActive(false);
        }
    }
}
