using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Fire : MonoBehaviour {

    [SerializeField] private float fireTime;
    [SerializeField] private float waitTime;

    private float timer;
    private bool onFire;

    private BoxCollider2D coll;
    private ParticleSystem part;

    private void Awake()
    {
        onFire = false;

        coll = GetComponent<BoxCollider2D>();
        part = GetComponent<ParticleSystem>();
        coll.isTrigger = true;

        coll.enabled = onFire;
        part.enableEmission = onFire;
        timer = onFire ? fireTime : waitTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            onFire = !onFire;
            timer = onFire ? fireTime : waitTime;
            coll.enabled = onFire;
            part.enableEmission = onFire;
        }
    }

}
