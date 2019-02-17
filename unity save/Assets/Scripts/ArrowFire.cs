using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFire : MonoBehaviour {
    [SerializeField] private GameObject ArrowPrefab;

    private GameObject myArrow;

    private void Awake()
    {
        myArrow = Instantiate(ArrowPrefab);
        myArrow.SetActive(false);
    }

    //called by animator
    public void FireArrow()
    {
        myArrow.SetActive(true);
        myArrow.transform.position = transform.position;
    }
}
