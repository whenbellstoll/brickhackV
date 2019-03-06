using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    [SerializeField] public List<GameObject> buildables;
    List<GameObject> platforms = new List<GameObject>();
    public static List<GameObject> traps = new List<GameObject>();

    [SerializeField] public List<GameObject> spikedBuildables;

    [SerializeField] public GameObject tintP1;
    [SerializeField] public GameObject tintP2;
    private Vector3 tintPosLeft = new Vector3(-10, -1, -1);
    private Vector3 tintPosRight = new Vector3(10, -1, -1);

    [SerializeField] private Rect safeAreaLeft;
    [SerializeField] private Rect safeAreaRight;

    int hurtTimerOne = 0;
    int hurtTimerTwo = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hurtTimerOne > 0)
        {
            hurtTimerOne--;
        }

        if (hurtTimerTwo > 0)
        {
            hurtTimerTwo--;
        }
    }

    public void ResetPlatforms()
    {
        //Destroy all traps and platforms previously placed
        for (int i = 0; i < platforms.Count; i++)
        {
            Destroy(platforms[i]);
        }
        for (int i = 0; i < traps.Count; i++)
        {
            Destroy(traps[i]);
        }

        platforms.Clear();
        traps.Clear();
    }

    /// <summary>
    /// Creates the initial platforms and spikeball for each side
    /// </summary>
    public void LoadInitialLevel()
    {
        //left side
        platforms.Add(Instantiate(buildables[0], new Vector3(-16.5f,  -9, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(-18.5f, -11, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(-2.5f, -6, 0), Quaternion.identity));
        traps.Add(Instantiate(buildables[3], new Vector3(-2, -12, 0), Quaternion.identity));

        //right side
        platforms.Add(Instantiate(buildables[0], new Vector3(16.5f, -9, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(18.5f, -11, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(2.5f, -6, 0), Quaternion.identity));
        traps.Add(Instantiate(buildables[3], new Vector3(2, -12, 0), Quaternion.identity));
    }

    /// <summary>
    /// Handles the placement of objects
    /// </summary>
    /// <param name="cursor"></param>
    public void PlaceObject(GameObject cursor)
    {
        //TODO: placement made a sound before I ripped it out of SceneManager. Shouldn't be a hard fix but it's very low priority
        GameObject objToBuild = cursor.GetComponent<StoreObjectToBuild>().obj;

        if (!IsInSafeArea(objToBuild.GetComponent<BoxCollider2D>().bounds))
        {
            //if the object is a platform add it to platforms, if it's a trap add it to traps. 
            if (cursor.GetComponent<StoreObjectToBuild>().obj.tag == "Platform")
            {
                //cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                platforms.Add(Instantiate(objToBuild, objToBuild.transform.position, objToBuild.transform.rotation));
                cursor.GetComponent<StoreObjectToBuild>().obj = null;
            }
            else
            {
                //cursor.GetComponent<StoreObjectToBuild>().obj.transform.parent = null;
                traps.Add(Instantiate(objToBuild, objToBuild.transform.position, objToBuild.transform.rotation));
                cursor.GetComponent<StoreObjectToBuild>().obj = null;
            }
        }
    }


    /// <summary>
    /// handles picking up of objects by cursor, including collision detections
    /// </summary>
    /// <param name="cursor"></param>
    public void PlatformMovability(GameObject cursor)
    {
        foreach (GameObject platform in platforms)
        {
            if (platform.GetComponent<BoxCollider2D>().bounds.Intersects(cursor.GetComponent<BoxCollider2D>().bounds))
            {
                cursor.GetComponent<StoreObjectToBuild>().obj = platform;
            }
        }
        foreach (GameObject trap in traps)
        {
            if (trap.GetComponent<BoxCollider2D>().bounds.Intersects(cursor.GetComponent<BoxCollider2D>().bounds))
            {
                cursor.GetComponent<StoreObjectToBuild>().obj = trap;
            }
        }
    }

    /// <summary>
    /// Loops through the trap lists to see if either player jhas collided, plays sound and subtracts health accordingly.
    /// </summary>
    public void HandlePlayerTrapCollisions(GameObject playerOne, GameObject playerTwo, GameState state)
    {
        if (state == GameState.survival)
        {
            foreach (GameObject trap in traps)
            {
                if (playerOne.GetComponent<BoxCollider2D>().bounds.Intersects(trap.GetComponent<BoxCollider2D>().bounds) && hurtTimerOne <= 0)
                {

                    //audioPlayer.clip = audioClips[playerOneHealth - 1];
                    //audioPlayer.Play();
                    playerOne.GetComponent<Player>().currentHealth--;
                    hurtTimerOne = 60;

                }

                if (playerTwo.GetComponent<BoxCollider2D>().bounds.Intersects(trap.GetComponent<BoxCollider2D>().bounds) && hurtTimerTwo <= 0)
                {
                    //audioPlayer.clip = audioClips[playerTwoHealth + 2];
                    //audioPlayer.Play();
                    playerTwo.GetComponent<Player>().currentHealth--;
                    hurtTimerTwo = 60;
                }
            }
        }
    }

    public void Rotation(GameObject cursor)
    {
        if (cursor.GetComponent<StoreObjectToBuild>().obj != null)
        {
            if (cursor.GetComponent<StoreObjectToBuild>().obj.GetComponent<Platform>() != null)
            {
                cursor.GetComponent<StoreObjectToBuild>().obj.GetComponent<Platform>().Rotate();
            }
        }
    }

    private bool IsInSafeArea(Bounds obj)
    {
        //left side
        if(obj.min.x < safeAreaLeft.x + safeAreaLeft.width &&
            obj.max.x > safeAreaLeft.x &&
            obj.min.y < safeAreaLeft.y + safeAreaLeft.height &&
            obj.max.y > safeAreaLeft.y)
        {
            return true;
        }

        //right side
        else if(obj.min.x < safeAreaRight.x + safeAreaRight.width &&
            obj.max.x > safeAreaRight.x &&
            obj.min.y < safeAreaRight.y + safeAreaRight.height &&
            obj.max.y > safeAreaRight.y)
        {
            return true;
        }
        return false;
    }

    public void HandlePlacing(GameObject p1Cursor, GameObject p2Cursor, float delayTimer)
    {
        //TODO: I hate that this code is just repeated and I know it can be fixed but I'm not gonna do it right now
        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown(KeyCode.F))
        {
            if (tintP1.GetComponent<SpriteRenderer>().enabled)
            {
                if (delayTimer <= 0)
                {
                    tintP1.GetComponent<SpriteRenderer>().enabled = false;
                    p1Cursor.GetComponent<ControlWithJoystick>().enabled = true;
                    p2Cursor.GetComponent<ControlWithJoystick>().enabled = true;
                }
            }
            //if player 1 has an object
            else if (p1Cursor.GetComponent<StoreObjectToBuild>().obj != null)
            {
                PlaceObject(p1Cursor);
            }
            else
            {
                PlatformMovability(p1Cursor);
            }
        }


        if (Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (tintP2.GetComponent<SpriteRenderer>().enabled)
            {
                if (delayTimer <= 0)
                {
                    tintP2.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            //if player 2 has an object
            else if (p2Cursor.GetComponent<StoreObjectToBuild>().obj != null)
            {
                PlaceObject(p2Cursor);
            }
            else
            {
                PlatformMovability(p2Cursor);
            }
        }
    }

    public void GameReset()
    {
        tintP1.GetComponent<SpriteRenderer>().enabled = true;
        tintP2.GetComponent<SpriteRenderer>().enabled = true;
        ClearLevel();
        LoadInitialLevel();

    }

    public void GameStateBuilding()
    {
        tintP1.GetComponent<SpriteRenderer>().enabled = true;
        tintP2.GetComponent<SpriteRenderer>().enabled = true;
        SetPlatormMovement(false);
    }

    public void GameStateSurvival()
    {
        tintP1.GetComponent<SpriteRenderer>().enabled = false;
        tintP2.GetComponent<SpriteRenderer>().enabled = false;
        SetPlatormMovement(true);
    }

    public void SwapTints(bool positionsGetSwapped)
    {
        if (positionsGetSwapped)
        {
            //Swap tints to the other side
            tintP1.transform.position = tintPosRight;
            tintP2.transform.position = tintPosLeft;
        }
        else
        {
            //From the opposite side to their original starting positions.
            tintP1.transform.position = tintPosLeft;
            tintP2.transform.position = tintPosRight;
        }
    }

    private void ClearLevel()
    {
        foreach (GameObject platform in platforms)
        {
            Destroy(platform);
        }

        foreach (GameObject trap in traps)
        {
            Destroy(trap);
        }

        platforms.Clear();
        traps.Clear();
    }

    private void SetPlatormMovement(bool b = false)
    {
        foreach (GameObject platform in platforms)
        {
            Debug.Log(platform.name);
            platform.GetComponent<Platform>().SetMoving(b);
        }
    }
}
