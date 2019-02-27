using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGroup : MonoBehaviour {

    [Header("Menu Buttons")]
    [SerializeField] private MenuButton[] menuButtons;
    [SerializeField] private int selectedButton = 0;

    [Space]
    [Header("Materials")]
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material notSelectedMaterial;

    [Space]
    [Header("Button Pulse")]
    [SerializeField] [Range(0, 5)]private float pulseSpeed = 1.0f;
    [SerializeField] [Range(0, 1)]private float pulseMagnitude = 0.1f;
    private float buttonOscilator = 0;

    private void Awake()
    { 
        ButtonChange();
    }

    void Update () {

        //scroll up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButton = Mathf.Clamp(selectedButton - 1, 0, menuButtons.Length -1);
            ButtonChange();
        }
        //scroll down
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedButton = Mathf.Clamp(selectedButton + 1, 0, menuButtons.Length - 1);
            ButtonChange();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(menuButtons[selectedButton].sceneChange);
        }


        buttonOscilator = (buttonOscilator + (pulseSpeed * Time.deltaTime)) % (Mathf.PI * 2);
        menuButtons[selectedButton].transform.localScale = Vector2.one * (1 + pulseMagnitude + (Mathf.Sin(buttonOscilator) * pulseMagnitude));
	}

    /// <summary>
    /// sets the materials of the menu button renderers to match selected
    /// </summary>
    private void ButtonChange()
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.render.material = notSelectedMaterial; //set the materials to deselected 
            mb.transform.localScale = Vector3.one; //reset size
        }
        
        menuButtons[selectedButton].render.material = selectedMaterial; //set material to selected
        buttonOscilator = 0;
        menuButtons[selectedButton].transform.localScale = Vector2.one * (1 + pulseMagnitude + (Mathf.Sin(buttonOscilator) * pulseMagnitude));
    }

}




[System.Serializable]
public struct MenuButton {

    public Transform transform;
    public SpriteRenderer render;
    public string sceneChange;
    
}
