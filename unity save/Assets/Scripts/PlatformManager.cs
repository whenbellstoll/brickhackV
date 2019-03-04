using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    [SerializeField] public List<GameObject> buildables;
    List<GameObject> platforms = new List<GameObject>();
    public static List<GameObject> traps = new List<GameObject>();

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
        platforms.Add(Instantiate(buildables[2], new Vector3(-18.5f, -11, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(-2.5f, -6, 0), Quaternion.identity));
        traps.Add(Instantiate(buildables[4], new Vector3(-2, -12, 0), Quaternion.identity));

        //right side
        platforms.Add(Instantiate(buildables[0], new Vector3(16.5f, -9, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[2], new Vector3(18.5f, -11, 0), Quaternion.identity));
        platforms.Add(Instantiate(buildables[1], new Vector3(2.5f, -6, 0), Quaternion.identity));
        traps.Add(Instantiate(buildables[4], new Vector3(2, -12, 0), Quaternion.identity));
    }

    /// <summary>
    /// Handles the placement of objects
    /// </summary>
    /// <param name="cursor"></param>
    public void PlaceObject(GameObject cursor)
    {
        //TODO: placement made a sound before I ripped it out of SceneManager. Shouldn't be a hard fix but it's very low priority
        GameObject objToBuild = cursor.GetComponent<StoreObjectToBuild>().obj;

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
}
