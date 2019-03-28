using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreObjectToBuild : MonoBehaviour {

    public GameObject obj;

    public bool editing = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if( obj != null )
        obj.transform.position = transform.position;
	}
}
