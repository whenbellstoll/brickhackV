using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFire : MonoBehaviour {
    [SerializeField] private GameObject ArrowPrefab;

    private GameObject myArrow;
    bool facingLeft;

    private void Awake()
    {
        if(transform.position.x < 0)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
        myArrow = Instantiate(ArrowPrefab);
        myArrow.SetActive(false);
    }

    private void Update()
    {
        if (transform.position.x < 0)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
        GetComponent<SpriteRenderer>().flipX = !facingLeft;
    }

    //called by animator
    public void FireArrow()
    {
        SceneManager.traps.Add(myArrow);
        myArrow.SetActive(true);
        if (facingLeft)
        {
            myArrow.GetComponent<Arrow>().velocity.x = -10;
        }
        else
        {
            myArrow.GetComponent<Arrow>().velocity.x = 10;

        }
        myArrow.transform.position = transform.position;
    }
}
