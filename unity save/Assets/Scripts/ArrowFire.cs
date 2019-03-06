using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFire : MonoBehaviour {
    [SerializeField] private GameObject ArrowPrefab;

    private GameObject myArrow;
    short firingDirection; //direction that the arrow is fired based on one of four values.


    private void Awake()
    {
        firingDirection = 0;
        myArrow = Instantiate(ArrowPrefab);
        myArrow.SetActive(false);
    }

    private void Update()
    {
      
    }

    //called by animator
    public void FireArrow()
    {
        PlatformManager.traps.Add(myArrow);
        myArrow.SetActive(true);
        Arrow myArrowArrowC = myArrow.GetComponent<Arrow>();
        if (firingDirection == 0)
        {
            myArrowArrowC.velocity.x = -10;
            myArrowArrowC.velocity.y = 0;
            myArrow.transform.right = new Vector2(1, 0);
            gameObject.transform.right = new Vector2(1, 0);
        }
        else if( firingDirection == 1)
        {
            myArrowArrowC.velocity.x = 0;
            myArrowArrowC.velocity.y = 10;
            myArrow.transform.right = new Vector2(0, -1);
            gameObject.transform.right = new Vector2(0, -1);

        }
        else if( firingDirection == 2)
        {
            myArrowArrowC.velocity.x = 10;
            myArrowArrowC.velocity.y = 0;
            myArrow.transform.right = new Vector2(-1,0);
            gameObject.transform.right = new Vector2(-1, 0);
        }
        else if( firingDirection == 3 )
        {
            myArrowArrowC.velocity.x = 0;
            myArrowArrowC.velocity.y = -10;
            myArrow.transform.right = new Vector2(0, 1);
            gameObject.transform.right = new Vector2(0, 1);
        }
        myArrow.transform.position = transform.position;
    }

    public void RotateArrow()
    {
        firingDirection++;

        if(firingDirection > 3)
        {
            firingDirection = 0;
        }
    }

}
