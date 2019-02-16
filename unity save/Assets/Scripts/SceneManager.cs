﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
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

    [SerializeField] private float roundTime;

    [SerializeField] private Digit[] timerDigits;

    private float timer;

    [SerializeField] public int playerHealth;

    [HideInInspector] public int playerOneHealth = 0;
    [HideInInspector] public int playerTwoHealth = 0;

    private GameState state = GameState.building;

    [SerializeField] List<GameObject> platformPrefabs;
    List<GameObject> platforms = new List<GameObject>();
    List<GameObject> newPlatforms = new List<GameObject>();

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

        state = GameState.building;
        SetPlayers(state); //players start on their own side

        playerOneHealth = playerHealth;
        playerTwoHealth = playerHealth;
    }

	void Update () {

        //heartTest.GetComponent<Animator>().SetBool("Solid", false);

        switch(state){
            case GameState.building:

                foreach (Digit digit in timerDigits)
                {
                    digit.SetSprite(0);
                }

                if (Input.GetKeyDown(KeyCode.A)) {

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
                    timer = roundTime; //reset the timer

                    state = GameState.building;
                    SetPlayers(state); //return players to own side
                }                

                break;
        }
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

}
