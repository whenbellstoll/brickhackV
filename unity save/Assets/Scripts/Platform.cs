﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    [SerializeField] private float range;
    [SerializeField] private float velocity = 0.1f;
    private float posInRange;

    Vector3 position;

	// Use this for initialization
	void Start ()
    {
        posInRange = Random.Range(-range / 2, range / 2);
        position = transform.position;
        position.x += posInRange;
        transform.position = position;
	}

    // Update is called once per frame
    protected virtual void Update()
    {
        if (posInRange > range / 2 || posInRange < -range / 2)
        {
            velocity *= -1;
        }

        position.x += velocity;
        posInRange += velocity;

        transform.position = position;
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, 90));
    }

}
