using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpDeDerp : MonoBehaviour {

    [SerializeField] private float speed;

    private bool swappin = false;

    private Vector2 target;
    private Vector2 moveVector;

    public void SetTarget(Vector2 newTarget)
    {
        target = newTarget;
        swappin = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (swappin)
        {
            moveVector = target - (Vector2)transform.position;
            if(moveVector.magnitude < speed * Time.deltaTime)
            {
                transform.position = target;
                swappin = false;
            }
            else
            {
                transform.position += (Vector3)moveVector * speed * Time.deltaTime;
            }
        }
	}
}
