using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    picking,
    building,
    survival,
    win
}


public class SceneManager : MonoBehaviour {

    /// <summary>
    /// manages the rounds and handles reset
    /// </summary>

    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;

    [SerializeField] private Vector2 startPositionOne;
    [SerializeField] private Vector2 startPositionTwo;

    bool positionsGetSwapped = true;

    [SerializeField] private float pickTime;
    [SerializeField] private float buildTime;
    [SerializeField] private float roundTime;
    [SerializeField] private float winTime;

    [SerializeField] private Digit[] timerDigits;

    [SerializeField] private GameObject winText;

    private float timer;

    [SerializeField] public int playerHealth;

    [HideInInspector] public int playerOneHealth = 0;
    [HideInInspector] public int playerTwoHealth = 0;

    [SerializeField] private Sprite winRed;
    [SerializeField] private Sprite winBlue;

    private GameState state = GameState.building;

    [SerializeField] GameObject cursorPrefab;
    GameObject p1Cursor;
    GameObject p2Cursor;

    [SerializeField] List<GameObject> buildables;
    List<GameObject> platforms = new List<GameObject>();
    List<GameObject> newPlatforms = new List<GameObject>();

    public static List<GameObject> traps = new List<GameObject>();

    int itemindexOne;
    int itemindexTwo; //Keeps track of which items the players want to set down.

    int hurtTimerOne = 0;
    int hurtTimerTwo = 0;
    public static bool heartsPositionCorrect = true;
    private void Awake()
    {
        ResetGame();
        itemindexOne = 0;
        itemindexTwo = 0;
    }

    /// <summary>
    /// reset variables back to state
    /// </summary>
    public void ResetGame()
    {
        winText.GetComponent<SpriteRenderer>().enabled = false;
        timer = pickTime;
        playerOne.GetComponent<Player>().currentHealth = playerOne.GetComponent<Player>().baseHealth;
        playerTwo.GetComponent<Player>().currentHealth = playerTwo.GetComponent<Player>().baseHealth;

        for(int i = 0; i < platforms.Count; i++)
        {
            Destroy(platforms[i]);
        }
        for(int i = 0; i < traps.Count; i++)
        {
            Destroy(traps[i]);
        }
        platforms.Clear();
        traps.Clear();

        state = GameState.picking;
        BeginPickingPhase();
    }

	void Update () {
        HandlePlayerTrapCollisions();

        playerOneHealth = playerOne.GetComponent<Player>().currentHealth;
        playerTwoHealth = playerTwo.GetComponent<Player>().currentHealth;

        //heartTest.GetComponent<Animator>().SetBool("Solid", false);

        switch (state){
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

                //Next four ifs change the prefab the players select.
                if (p1Cursor.GetComponent<StoreObjectToBuild>().obj != null) //Gross, dispicable flag check, but nonetheless necessary
                {


                    if (Input.GetKeyDown("joystick 1 button 4") || Input.GetKeyDown(KeyCode.Q))
                    {
                        itemindexOne--;
                        if (itemindexOne < 0)
                        {
                            itemindexOne = buildables.Count - 1;
                        }
                        GameObject temp = p1Cursor.GetComponent<StoreObjectToBuild>().obj;
                        p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[itemindexOne], p1Cursor.transform);
                        Destroy(temp);
                    }

                    if (Input.GetKeyDown("joystick 1 button 5") || Input.GetKeyDown(KeyCode.E))
                    {
                        itemindexOne++;
                        if (itemindexOne >= buildables.Count)
                        {
                            itemindexOne = 0;
                        }
                        GameObject temp = p1Cursor.GetComponent<StoreObjectToBuild>().obj;
                        p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[itemindexOne], p1Cursor.transform);
                        Destroy(temp);
                    }

                    if (Input.GetKeyDown("joystick 2 button 4") || Input.GetKeyDown(KeyCode.RightControl))
                    {
                        itemindexTwo--;
                        if (itemindexTwo < 0)
                        {
                            itemindexTwo = buildables.Count - 1;
                        }
                        GameObject temp = p2Cursor.GetComponent<StoreObjectToBuild>().obj;
                        p2Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[itemindexTwo], p1Cursor.transform);
                        Destroy(temp);
                    }

                    if (Input.GetKeyDown("joystick 2 button 5") || Input.GetKeyDown(KeyCode.Insert))
                    {
                        itemindexTwo++;
                        if (itemindexTwo >= buildables.Count)
                        {
                            itemindexTwo = 0;
                        }
                        GameObject temp = p2Cursor.GetComponent<StoreObjectToBuild>().obj;
                        p2Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[itemindexTwo], p1Cursor.transform);
                        Destroy(temp);
                    }
                }
                if (playerOneHealth <= 0 || playerTwoHealth <= 0)
                {
                    BeginWinPhase();
                }
                break;
            case GameState.building:
                timerDigits[1].SetSprite(Mathf.FloorToInt(timer % 10));
                timerDigits[0].SetSprite(Mathf.FloorToInt((timer % 100) / 10));

                foreach (Digit digit in timerDigits)
                {
                    digit.SetSprite(0);
                }

                if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown(KeyCode.X))
                {
                    if(itemindexOne < 4)
                    {
                        p1Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                        platforms.Add(p1Cursor.GetComponent<StoreObjectToBuild>().obj);
                        p1Cursor.GetComponent<StoreObjectToBuild>().obj = null;
                    }
                    else
                    {
                        p1Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                        traps.Add(p1Cursor.GetComponent<StoreObjectToBuild>().obj);
                        p1Cursor.GetComponent<StoreObjectToBuild>().obj = null;
                    }
                }

                if(Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.RightShift))
                {
                    if (itemindexTwo < 4)
                    {
                        p2Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                        platforms.Add(p2Cursor.GetComponent<StoreObjectToBuild>().obj);
                        p2Cursor.GetComponent<StoreObjectToBuild>().obj = null;
                    }
                    else
                    {
                        p2Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                        traps.Add(p2Cursor.GetComponent<StoreObjectToBuild>().obj);
                        p2Cursor.GetComponent<StoreObjectToBuild>().obj = null;
                    }
                }



               
                timer -= Time.deltaTime;

                if (timer <= 0) {
                    timer = roundTime;
                    BeginSurvivalPhase();
                }
                if (playerOneHealth <= 0 || playerTwoHealth <= 0)
                {
                    BeginWinPhase();
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
                if (playerOneHealth <= 0 || playerTwoHealth <= 0)
                {
                    BeginWinPhase();
                }
                break;
            case GameState.win:
                timer -= Time.deltaTime;
                Debug.Log(timer);

                if (timer <= 0)
                {
                    if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.RightShift))
                    {
                        Debug.Log("reset button pressed");
                        ResetGame();
                    }
                }
                break;
        }

        if (hurtTimerOne > 0)
        {
            hurtTimerOne--;
        }

        if (hurtTimerTwo > 0)
        {
            hurtTimerTwo--;
        }
        Debug.Log(state);
	}

    /// <summary>
    /// sets the players position
    /// </summary>
    /// <param name="one">true when players are on their origional sides</param>
    private void SetPlayers()
    {
        if (positionsGetSwapped)
        {
            playerOne.transform.position = startPositionTwo;
            playerTwo.transform.position = startPositionOne;
        }
        else
        {
            playerOne.transform.position = startPositionOne;
            playerTwo.transform.position = startPositionTwo;
        }
        positionsGetSwapped = !positionsGetSwapped;
        heartsPositionCorrect = false;
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

        itemindexOne = 0;
        itemindexTwo = 0;
        p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[0], p1Cursor.transform);
        p2Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[0], p2Cursor.transform);
    }

    private void BeginBuildPhase()
    {
        state = GameState.building;
    }

    private void BeginSurvivalPhase()
    {
        if(p1Cursor.GetComponent<StoreObjectToBuild>().obj != null)
        {
            if (itemindexOne < 3)
            {
                p1Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                platforms.Add(p1Cursor.GetComponent<StoreObjectToBuild>().obj);
                p1Cursor.GetComponent<StoreObjectToBuild>().obj = null;
            }
            else
            {
                p1Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                traps.Add(p1Cursor.GetComponent<StoreObjectToBuild>().obj);
                p1Cursor.GetComponent<StoreObjectToBuild>().obj = null;
            }

        }
        if (p2Cursor.GetComponent<StoreObjectToBuild>().obj != null)
        {
            if (itemindexOne < 3)
            {
                p2Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                platforms.Add(p2Cursor.GetComponent<StoreObjectToBuild>().obj);
                p2Cursor.GetComponent<StoreObjectToBuild>().obj = null;
            }
            else
            {
                p2Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                traps.Add(p2Cursor.GetComponent<StoreObjectToBuild>().obj);
                p2Cursor.GetComponent<StoreObjectToBuild>().obj = null;
            }
        }

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

    private void BeginWinPhase()
    {
        state = GameState.win;
        timer = winTime;
        winText.GetComponent<SpriteRenderer>().enabled = true;
        if(playerOneHealth <= 0)
        {
            winText.GetComponent<SpriteRenderer>().sprite = winRed;
        }
        else
        {
            winText.GetComponent<SpriteRenderer>().sprite = winBlue;
        }
    }

    private void HandlePlayerTrapCollisions()
    {
        foreach (GameObject trap in traps)
        {
            if (playerOne.GetComponent<BoxCollider2D>().bounds.Intersects(trap.GetComponent<BoxCollider2D>().bounds) && hurtTimerOne <= 0)
            {
                playerOne.GetComponent<Player>().currentHealth--;
                hurtTimerOne = 60;
            }

            if (playerTwo.GetComponent<BoxCollider2D>().bounds.Intersects(trap.GetComponent<BoxCollider2D>().bounds) && hurtTimerTwo <= 0)
            {
                playerTwo.GetComponent<Player>().currentHealth--;
                hurtTimerTwo = 60;
            }
        }
    }
}
