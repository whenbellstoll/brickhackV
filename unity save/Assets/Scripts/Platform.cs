using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    [SerializeField] private float range;
    [SerializeField] private float velocity = 0.1f;
    private float posInRange;
    private bool isMoving = false;

    Vector3 position;

	// Use this for initialization
	void Start ()
    {
        //starts platform at random point in it's range to combat uniformity in movement
        posInRange = Random.Range(-range / 2, range / 2);
        position = transform.position;
	}

    // Update is called once per frame
    protected virtual void Update()
    {
        if (posInRange > range / 2 || posInRange < -range / 2)
        {
            velocity *= -1;
        }

        if (isMoving)
        {
            position.x += velocity;
            posInRange += velocity;

            transform.position = position;
        }
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, 90));
    }

}
