using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    
	// Use this for initialization
	void Start ()
    {

	}

    // Update is called once per frame
    protected virtual void Update()
    {


        
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, 90));
    }

}
