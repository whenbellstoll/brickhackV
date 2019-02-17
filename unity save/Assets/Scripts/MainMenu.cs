using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private string gameScene;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
        }
	}
}
