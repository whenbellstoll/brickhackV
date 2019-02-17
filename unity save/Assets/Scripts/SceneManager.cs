﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    picking,
    building,
    survival
}


public class SceneManager : MonoBehaviour {

    /// <summary>
    /// manages the rounds and handles reset
    /// </summary>

    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;

    [SerializeField] private Vector2 startPositionOne;
    [SerializeField] private Vector2 startPositionTwo;

    bool positionsAreSwapped = false;

    [SerializeField] private float pickTime;
    [SerializeField] private float buildTime;
    [SerializeField] private float roundTime;

    [SerializeField] private Digit[] timerDigits;

    private float timer;

    [SerializeField] public int playerHealth;

    [HideInInspector] public int playerOneHealth = 0;
    [HideInInspector] public int playerTwoHealth = 0;

    private GameState state = GameState.building;

    [SerializeField] GameObject cursorPrefab;
    GameObject p1Cursor;
    GameObject p2Cursor;

    [SerializeField] List<GameObject> platformPrefabs;
    List<GameObject> platforms = new List<GameObject>();
    List<GameObject> newPlatforms = new List<GameObject>();

    List<GameObject> traps = new List<GameObject>();

    private void Awake()
    {
        ResetGame();
    }

    /// <summary>
    /// reset variables back to state
    /// </summary>
    public void ResetGame()
    {
        timer = roundTime;

        state = GameState.picking;
        BeginPickingPhase();

        playerOneHealth = playerOne.GetComponent<Player>().currentHealth;
        playerTwoHealth = playerTwo.GetComponent<Player>().currentHealth;
    }

	void Update () {

        //heartTest.GetComponent<Animator>().SetBool("Solid", false);

        switch(state){
            case GameState.picking:
                timerDigits[1].SetSprite(Mathf.FloorToInt(timer % 10));
                timerDigits[0].SetSprite(Mathf.FloorToInt((timer % 100) / 10));

                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    timer = buildTime;
                    state = GameState.building;
                    BeginBuildPhase();
                }
                break;
            case GameState.building:
                timerDigits[1].SetSprite(Mathf.FloorToInt(timer % 10));
                timerDigits[0].SetSprite(Mathf.FloorToInt((timer % 100) / 10));

                foreach (Digit digit in timerDigits)
                {
                    digit.SetSprite(0);
                }

                //this will all be changed when we start differentaiting between controllers
                if (Input.GetKeyDown("joystick 1 button 0"))
                {
                    p1Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                    platforms.Add(p1Cursor.GetComponent<StoreObjectToBuild>().obj);
                    p1Cursor.GetComponent<StoreObjectToBuild>().obj = null;
                }

                if(Input.GetKeyDown("joystick 2 button 0"))
                {
                    p2Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                    platforms.Add(p2Cursor.GetComponent<StoreObjectToBuild>().obj);
                    p2Cursor.GetComponent<StoreObjectToBuild>().obj = null;
                }

                timer -= Time.deltaTime;

                if (timer <= 0) {
                    timer = roundTime;
                    BeginSurvivalPhase();
                }

                break;
            case GameState.survival:
                p1Cursor = null;
                p2Cursor = null;

                timerDigits[1].SetSprite(Mathf.FloorToInt(timer % 10));
                timerDigits[0].SetSprite(Mathf.FloorToInt((timer % 100) / 10));

                //increase timer and check if round is over
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = pickTime; //reset the timer

                    state = GameState.picking;
                    BeginPickingPhase();
                }                

                break;
        }
        Debug.Log(state);
	}

    /// <summary>
    /// sets the players position
    /// </summary>
    /// <param name="one">true when players are on their origional sides</param>
    private void SetPlayers()
    {
        if (positionsAreSwapped)
        {
            playerOne.transform.position = startPositionTwo;
            playerTwo.transform.position = startPositionOne;
        }
        else
        {
            playerOne.transform.position = startPositionOne;
            playerTwo.transform.position = startPositionTwo;
        }
        positionsAreSwapped = !positionsAreSwapped;
    }

    private void BeginPickingPhase()
    {
        p1Cursor = Instantiate(cursorPrefab);
        p2Cursor = Instantiate(cursorPrefab);

        p1Cursor.GetComponent<SpriteRenderer>().color = Color.blue;
        p2Cursor.GetComponent<SpriteRenderer>().color = Color.red;

        p1Cursor.transform.position = new Vector3(playerOne.transform.position.x, playerOne.transform.position.y + 10, 0);
        p2Cursor.transform.position = new Vector3(playerTwo.transform.position.x, playerTwo.transform.position.y + 10, 0);

        if(p1Cursor.transform.position.x < 0 )
        {
            p1Cursor.AddComponent<LeftCursorBounds>();
            p2Cursor.AddComponent<RightCursorBounds>();
        }
        else
        {
            p1Cursor.AddComponent<RightCursorBounds>();
            p2Cursor.AddComponent<LeftCursorBounds>();
        }
        p1Cursor.GetComponent<ControlWithJoystick>().controllerNum = 1;
        p2Cursor.GetComponent<ControlWithJoystick>().controllerNum = 2;

        playerOne.GetComponent<Player>().enabled = false;
        playerTwo.GetComponent<Player>().enabled = false;
    }

    private void BeginBuildPhase()
    {
        p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Count)], p1Cursor.transform);
        p2Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Count)], p2Cursor.transform);
        state = GameState.building;
    }

    private void BeginSurvivalPhase()
    {
        Destroy(p1Cursor);
        Destroy(p2Cursor);

        for(int i = 0; i < newPlatforms.Count; i++)
        {
            Destroy(newPlatforms[i]);
        }
        newPlatforms.Clear();

        playerOne.GetComponent<Player>().enabled = true;
        playerTwo.GetComponent<Player>().enabled = true;

        SetPlayers();

        state = GameState.survival;
    }
}
