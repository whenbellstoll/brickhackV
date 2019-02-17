using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private string gameScene;
    [SerializeField] private string instructionsScene;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
        }

        if (Input.GetKeyDown("joystick 1 button 1") || Input.GetKeyDown("joystick 2 button 1") || Input.GetKeyDown(KeyCode.I))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(instructionsScene, LoadSceneMode.Single);
        }

    }
}
