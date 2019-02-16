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

    [SerializeField] private float pickTime;
    [SerializeField] private float buildTime;
    [SerializeField] private float roundTime;

    [SerializeField] private Digit[] timerDigits;

    private float timer;

    private GameState state = GameState.building;

    [SerializeField] GameObject cursorPrefab;
    GameObject p1Cursor;
    GameObject p2Cursor;

    [SerializeField] List<GameObject> platformPrefabs;
    List<GameObject> platforms = new List<GameObject>();
    List<GameObject> newPlatforms = new List<GameObject>();
    bool buildingComplete = false;

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
        SetPlayers(state); //players start on their own side
    }

	void Update () {

        //heartTest.GetComponent<Animator>().SetBool("Solid", false);

        switch(state){
            case GameState.picking:
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    timer = buildTime;
                    state = GameState.building;
                    BeginBuildPhase();
                }
                break;
            case GameState.building:

                foreach (Digit digit in timerDigits)
                {
                    digit.SetSprite(0);
                }

                //this will all be changed when we start differentaiting between controllers
                if (Input.GetKeyDown("joystick button 0"))
                {
                    p1Cursor.GetComponent<StoreObjectToBuild>().obj.GetComponent<ControlWithJoystick>().enabled = false;
                    platforms.Add(p1Cursor.GetComponent<StoreObjectToBuild>().obj);
                    p1Cursor.GetComponent<StoreObjectToBuild>().obj = null;
                }

                if (buildTime <= 0) {
                    timer = roundTime;

                    state = GameState.survival;
                    SetPlayers(state); //place players on opponents side   
                }

                break;
            case GameState.survival:

                timerDigits[1].SetSprite(Mathf.FloorToInt(timer % 10));
                timerDigits[0].SetSprite(Mathf.FloorToInt((timer % 100) / 10));

                //increase timer and check if round is over
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = pickTime; //reset the timer

                    state = GameState.picking;
                    BeginPickingPhase();
                    SetPlayers(state); //return players to own side
                }                

                break;
        }
        Debug.Log(state);
	}

    /// <summary>
    /// sets the players position
    /// </summary>
    /// <param name="one">true when players are on their origional sides</param>
    private void SetPlayers(GameState state)
    {
        switch (state)
        {
            case GameState.building:
                playerOne.transform.position = startPositionOne;
                playerTwo.transform.position = startPositionTwo;
                break;
            case GameState.survival:
                playerOne.transform.position = startPositionTwo;
                playerTwo.transform.position = startPositionOne;
                break;

        }
           
    }

    private void BeginPickingPhase()
    {
        p1Cursor = Instantiate(cursorPrefab);
        p2Cursor = Instantiate(cursorPrefab);

        float xPos = -5f;
        for(int i = 0; i < 2; i++)
        {
            
        }
    }

    private void BeginBuildPhase()
    {
        p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Count)]);
        p2Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Count)]);

        p1Cursor.GetComponent<StoreObjectToBuild>().obj.GetComponent<ControlWithJoystick>().enabled = true;
        p2Cursor.GetComponent<StoreObjectToBuild>().obj.GetComponent<ControlWithJoystick>().enabled = true;
    }
}
