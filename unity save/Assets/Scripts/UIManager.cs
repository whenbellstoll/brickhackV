using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private int playerHealth;
    [SerializeField] private GameObject heartPrefab;
    private List<Animator> playerOneHearts;
    private List<Animator> playerTwoHearts;
    private int playerOneHealth = 0;
    private int playerTwoHealth = 0;

    private void Awake()
    {
        playerOneHearts = new List<Animator>();
        playerTwoHearts = new List<Animator>();

        int height = 13;
        int distancefromCenter = 5;
        float spacing = 1.5f;

        for (int i = 0; i < playerHealth; i++)
        {
            GameObject hrt = Instantiate(heartPrefab, transform);
            hrt.transform.position = new Vector2(-distancefromCenter - (i * spacing), height);
            playerOneHearts.Add(hrt.GetComponent<Animator>());

            hrt = Instantiate(heartPrefab, transform);
            hrt.transform.position = new Vector2(distancefromCenter + (i * spacing), height);
            playerTwoHearts.Add(hrt.GetComponent<Animator>());
        }

        playerOneHealth = playerHealth;
        playerTwoHealth = playerHealth;
    }

    private void Update()
    {
        for(int h = 0; h<playerHealth; h++)
        {
            playerOneHearts[h].SetBool("Solid", playerOneHealth > h);
            playerTwoHearts[h].SetBool("Solid", playerTwoHealth > h);
        }
    }
}
