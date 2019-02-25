using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
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

    [SerializeField] public AudioSource audioPlayer;
    [SerializeField] private AudioSource winAnnoucer;
    [SerializeField] public List<AudioClip> audioClips = new List<AudioClip>();

    int itemindexOne;
    int itemindexTwo; //Keeps track of which items the players want to set down.

    int hurtTimerOne = 0;
    int hurtTimerTwo = 0;
    public static bool heartsPositionCorrect = true;


    public GameObject roundUI;
    public int roundNumber = 1;

    public int GetState()
    {
        switch (state)
        {
            case GameState.building: return 1;
            case GameState.survival: return 2;
        }
        return 2;
    }

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
        //reset round count
        roundNumber = 1;

        //Get rid of the red/blue wins
        winText.GetComponent<SpriteRenderer>().enabled = false;
        //set the timer to the build time
        timer = buildTime;

        //reset the health of the players.
        playerOne.GetComponent<Player>().currentHealth = playerOne.GetComponent<Player>().baseHealth;
        playerTwo.GetComponent<Player>().currentHealth = playerTwo.GetComponent<Player>().baseHealth;

        //Destroy all traps and platforms previously placed
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

        LoadInitialLevel();

        //Set the phase to build
        state = GameState.building;
        roundUI.SetActive(true);
        BeginPickingPhase();
    }

	void Update () {
        //Looks at the players and the list, collides accordingly and sets health
        HandlePlayerTrapCollisions();

        //The main logic of the game
        switch (state)
        {
            ///
            /// BUILDING STATE
            ///
            case GameState.building:
                //Set the timer sprites.
                timerDigits[1].SetSprite(Mathf.FloorToInt(timer % 10));
                timerDigits[0].SetSprite(Mathf.FloorToInt((timer % 100) / 10));

                //When the players want to change the item they're using, do it.
                HandleSelectionChanging();

                //decrease the timer by delta time
                timer -= Time.deltaTime;

                //The next two if statement place objects when the players press the appropriate buttons.
                if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown(KeyCode.F))
                {
                    PlaceObject(p1Cursor);
                }

                if(Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.RightShift))
                {
                    PlaceObject(p2Cursor);
                }



                //When the timer reaches 0, go to the survival phase.
                if (timer <= 0)
                {
                    timer = roundTime;
                    BeginSurvivalPhase();
                }

                //if one of the players has died (impossible in the build phase, but just a failsafe)
                //go to the win phase.
                if (playerOneHealth <= 0 || playerTwoHealth <= 0)
                {
                    BeginWinPhase();
                }


                break;
            
            ///
            /// SURVIVAL STATE
            ///
            case GameState.survival:
                p1Cursor = null;
                p2Cursor = null;

                timerDigits[1].SetSprite(Mathf.FloorToInt(timer % 10));
                timerDigits[0].SetSprite(Mathf.FloorToInt((timer % 100) / 10));

                //increase timer and check if round is over
                timer -= Time.deltaTime;


                if (timer <= 0)
                {
                    timer = buildTime; //reset the timer
                    //iterate the round number
                    roundNumber++;

                    //change the game state to build
                    state = GameState.building;
                    // Begins the item selection phase, brings the cursors into existence, creates the object the player can select from
                    BeginPickingPhase();
                }
                if (playerOneHealth <= 0 || playerTwoHealth <= 0)
                {
                    BeginWinPhase();
                }

                if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    audioPlayer.clip = audioClips[8];
                    audioPlayer.Play();
                }
                break;

            ///
            ///WIN PHASE
            ///
            case GameState.win:

                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.RightShift))
                    {
                       
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
        
	}


    private void PlaceObject(GameObject cursor)
    {
        //if the object is a platform add it to platforms, if it's a trap add it to traps. 
        if (cursor.GetComponent<StoreObjectToBuild>().obj.tag == "Platform")
        {
            cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
            platforms.Add(cursor.GetComponent<StoreObjectToBuild>().obj);
            cursor.GetComponent<StoreObjectToBuild>().obj = null;

            //play placement audio
            audioPlayer.clip = audioClips[8];
            audioPlayer.Play();
            
        }
        else
        {
            cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
            traps.Add(cursor.GetComponent<StoreObjectToBuild>().obj);
            cursor.GetComponent<StoreObjectToBuild>().obj = null;

            //Play placement audio
            audioPlayer.clip = audioClips[8];
            audioPlayer.Play();
            
        }
    }

    /// <summary>
    /// sets the players position
    /// </summary>
    /// <param name="one">true when players are on their origional sides</param>
    private void SetPlayers()
    {
        if (positionsGetSwapped)
        {
            //Swap players to the other side
            playerOne.transform.position = startPositionTwo;
            playerTwo.transform.position = startPositionOne;
        }
        else
        {
            //From the opposite side to their original starting positions.
            playerOne.transform.position = startPositionOne;
            playerTwo.transform.position = startPositionTwo;
        }
        //Set the bool for the next round
        positionsGetSwapped = !positionsGetSwapped;

        //This flag is jankily used to swap the heart positions
        heartsPositionCorrect = false;
    }

    /// <summary>
    /// Begins the item selection phase, brings the cursors into existence, creates the object the player can select from
    /// </summary>
    private void BeginPickingPhase()
    {
        //Pull up the round UI
        roundUI.SetActive(true);
        
        //Instantiate the cursors
        p1Cursor = Instantiate(cursorPrefab);
        p2Cursor = Instantiate(cursorPrefab);

        //Set the appropriate color
        p1Cursor.GetComponent<SpriteRenderer>().color = Color.blue;
        p2Cursor.GetComponent<SpriteRenderer>().color = Color.red;

        //Set their speed
        p1Cursor.GetComponent<ControlWithJoystick>().speed = 0.25f;
        p2Cursor.GetComponent<ControlWithJoystick>().speed = 0.25f;

        //Set their position to the players plus ten
        p1Cursor.transform.position = new Vector3(playerOne.transform.position.x, playerOne.transform.position.y + 10, 0);
        p2Cursor.transform.position = new Vector3(playerTwo.transform.position.x, playerTwo.transform.position.y + 10, 0);

        //Keep the cursor in bounds
        p1Cursor.AddComponent<ContainInBoxes>();
        p2Cursor.AddComponent<ContainInBoxes>();
        
        //Control with Joy stick
        p1Cursor.GetComponent<ControlWithJoystick>().controllerNum = 1;
        p2Cursor.GetComponent<ControlWithJoystick>().controllerNum = 2;

        //Players become disabled
        playerOne.GetComponent<Player>().enabled = false;
        playerTwo.GetComponent<Player>().enabled = false;

        //Item indexes are reset
        itemindexOne = 0;
        itemindexTwo = 0;
        //Get the item 
        p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[0], p1Cursor.transform);
        p2Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[0], p2Cursor.transform);
    }

    /// <summary>
    /// Handles when the player changes the item they want to build
    /// </summary>
    private void HandleSelectionChanging()
    {
        //Next four ifs change the prefab the players select.
        if (p1Cursor.GetComponent<StoreObjectToBuild>().obj != null) //Gross, dispicable flag check, but nonetheless necessary
        {


            if (Input.GetKeyDown("joystick 1 button 4") || Input.GetKeyDown(KeyCode.Q))
            {
                //Move the item index down
                itemindexOne--;

                //if we go outside the range of the index set it to the highest possible index
                if (itemindexOne < 0)
                {
                    itemindexOne = buildables.Count - 1;
                }
                
                //Get the old object that the player currently has
                GameObject temp = p1Cursor.GetComponent<StoreObjectToBuild>().obj;
                //Set the player's object to the new indexed object
                p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[itemindexOne], p1Cursor.transform);
                //Destroy the old object
                Destroy(temp);
            }

            if (Input.GetKeyDown("joystick 1 button 5") || Input.GetKeyDown(KeyCode.E))
            {
                //Move the item index up
                itemindexOne++;
                
                //if we move abive the range of the possible incex set the index to 0
                if (itemindexOne >= buildables.Count)
                {
                    itemindexOne = 0;
                }

                //Get the old object that the player currently has
                GameObject temp = p1Cursor.GetComponent<StoreObjectToBuild>().obj;
                //Set the player's object to the new indexed object
                p1Cursor.GetComponent<StoreObjectToBuild>().obj = Instantiate(buildables[itemindexOne], p1Cursor.transform);
                //Destroy the old object
                Destroy(temp);
            }

            //Copy of the above code, just for the second player
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
    }

    /// <summary>
    /// Moving from the build phase we, handle the externals of the build phase, give the players control over their characters, and swap sides.
    /// </summary>
    private void BeginSurvivalPhase()
    {
        roundUI.SetActive(false);
        if (p1Cursor.GetComponent<StoreObjectToBuild>().obj != null)
        {
            if (p1Cursor.GetComponent<StoreObjectToBuild>().obj.tag == "Platform")
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
            if (p2Cursor.GetComponent<StoreObjectToBuild>().obj.tag == "Trap")
            {
                p2Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                traps.Add(p2Cursor.GetComponent<StoreObjectToBuild>().obj);
                p2Cursor.GetComponent<StoreObjectToBuild>().obj = null;
            }
            else
            {
                p2Cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                platforms.Add(p2Cursor.GetComponent<StoreObjectToBuild>().obj);
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

        //Swaps the players
        SetPlayers();

        state = GameState.survival;
    }

    /// <summary>
    /// When a player has won we determine the winner based on which player has died, audio is played and label is assigned accordingly.
    /// </summary>
    private void BeginWinPhase()
    {
        roundUI.SetActive(false);
        state = GameState.win;
        timer = winTime;
        winText.GetComponent<SpriteRenderer>().enabled = true;
        if(playerOneHealth <= 0)
        {
            winAnnoucer.clip = audioClips[7];
            winAnnoucer.Play(44100);
            winText.GetComponent<SpriteRenderer>().sprite = winRed;
        }
        else
        {
            winAnnoucer.clip = audioClips[6];
            winAnnoucer.Play(44100);
            winText.GetComponent<SpriteRenderer>().sprite = winBlue;
        }
    }

    /// <summary>
    /// Loops through the trap lists to see if either player jhas collided, plays sound and subtracts health accordingly.
    /// </summary>
    private void HandlePlayerTrapCollisions()
    {
        if (state == GameState.survival)
        {
            foreach (GameObject trap in traps)
            {
                if (playerOne.GetComponent<BoxCollider2D>().bounds.Intersects(trap.GetComponent<BoxCollider2D>().bounds) && hurtTimerOne <= 0)
                {
                    
                    audioPlayer.clip = audioClips[playerOneHealth - 1];
                    audioPlayer.Play();
                    playerOne.GetComponent<Player>().currentHealth--;
                    hurtTimerOne = 60;
                    
                }

                if (playerTwo.GetComponent<BoxCollider2D>().bounds.Intersects(trap.GetComponent<BoxCollider2D>().bounds) && hurtTimerTwo <= 0)
                {
                    audioPlayer.clip = audioClips[playerTwoHealth + 2];
                    audioPlayer.Play();
                    playerTwo.GetComponent<Player>().currentHealth--;
                    hurtTimerTwo = 60;
                }
            }
        }
        //set the scene manager's variables for the player health
        playerOneHealth = playerOne.GetComponent<Player>().currentHealth;
        playerTwoHealth = playerTwo.GetComponent<Player>().currentHealth;
    }

    /// <summary>
    /// Creates the initial platforms and spikeball for each side
    /// </summary>
    private void LoadInitialLevel()
    {
        //left side
        platforms.Add(Instantiate(buildables[0], new Vector3(startPositionOne.x - 7, startPositionOne.y + 1, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[2], new Vector3(startPositionOne.x - 9, startPositionOne.y - 1, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(startPositionOne.x + 8, startPositionOne.y + 4, 0), Quaternion.identity));
        traps.Add(Instantiate(buildables[4], new Vector3(startPositionOne.x + 8, startPositionOne.y - 2, 0), Quaternion.identity));

        //right side
        platforms.Add(Instantiate(buildables[0], new Vector3(startPositionTwo.x + 7, startPositionTwo.y + 1, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[2], new Vector3(startPositionTwo.x + 9, startPositionTwo.y - 1, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(startPositionTwo.x - 8, startPositionTwo.y + 4, 0), Quaternion.identity));
        traps.Add(Instantiate(buildables[4], new Vector3(startPositionTwo.x - 8, startPositionTwo.y - 2, 0), Quaternion.identity));
    }
}
