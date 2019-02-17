using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    private List<Animator> playerOneHearts;
    private List<Animator> playerTwoHearts;

    public SceneManager manager;


    [SerializeField] private Digit[] roundDigits;
    public int roundNumber = 0;

    private void Awake()
    {
        manager = FindObjectOfType<SceneManager>();

        playerOneHearts = new List<Animator>();
        playerTwoHearts = new List<Animator>();

        int height = 13;
        int distancefromCenter = 5;
        float spacing = 1.5f;

        for (int i = 0; i < manager.playerHealth; i++)
        {
            GameObject hrt = Instantiate(heartPrefab, transform);
            hrt.transform.position = new Vector2(-distancefromCenter - (i * spacing), height);
            playerOneHearts.Add(hrt.GetComponent<Animator>());

            hrt = Instantiate(heartPrefab, transform);
            hrt.transform.position = new Vector2(distancefromCenter + (i * spacing), height);
            playerTwoHearts.Add(hrt.GetComponent<Animator>());
        }

        
    }

    private void Update()
    {
        if(!SceneManager.heartsPositionCorrect)
        {
            List<Vector3> hrt1transforms = new List<Vector3>();
            for(int i = 0; i < playerOneHearts.Count; i++)
            {
                hrt1transforms.Add(playerOneHearts[i].transform.position);
                playerOneHearts[i].transform.position = playerTwoHearts[i].transform.position;
            }
            for(int i = 0; i < hrt1transforms.Count; i++)
            {
                playerTwoHearts[i].transform.position = hrt1transforms[i];
            }

            SceneManager.heartsPositionCorrect = true;
        }
        for(int h = 0; h<manager.playerHealth; h++)
        {
            playerOneHearts[h].SetBool("Solid", manager.playerOneHealth > h);
            playerTwoHearts[h].SetBool("Solid", manager.playerTwoHealth > h);
        }

        roundDigits[1].SetSprite(Mathf.FloorToInt(roundNumber % 10));

        roundDigits[0].gameObject.SetActive(roundNumber >= 10);
        roundDigits[0].SetSprite(Mathf.FloorToInt((roundNumber % 100) / 10));
    }
}
