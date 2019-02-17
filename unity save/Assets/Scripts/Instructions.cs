using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour {

    [SerializeField] private string menuScene;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("joystick 1 button 1") || Input.GetKeyDown("joystick 2 button 1") || Input.GetKeyDown(KeyCode.I))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
        }
    }
}
