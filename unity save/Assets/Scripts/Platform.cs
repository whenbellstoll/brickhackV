using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    //used to detect collision between cursor and platform
    GameObject cursor;

    public float speed = 0.1f;

    Vector3 position;

	// Use this for initialization
	void Start () {
        position = transform.position;

        cursor = GameObject.Find("Cursor");
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = position;
	}
}
