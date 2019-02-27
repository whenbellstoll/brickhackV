using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
    [SerializeField] GameObject p1;
    [SerializeField] GameObject p2;
    [SerializeField] BoxCollider2D myBox;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (p1.GetComponent<BoxCollider2D>().bounds.Intersects(myBox.bounds))
        {
            if(p1.GetComponent<Player>().currentHealth < p1.GetComponent<Player>().baseHealth)
            {
                p1.GetComponent<Player>().currentHealth++;
                Destroy(gameObject);
            }
            
        }

        if (p2.GetComponent<BoxCollider2D>().bounds.Intersects(myBox.bounds))
        {
            if (p2.GetComponent<Player>().currentHealth < p2.GetComponent<Player>().baseHealth)
            {
                p2.GetComponent<Player>().currentHealth++;
                gameObject.SetActive(false);
            }

        }

    }
}

