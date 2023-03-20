using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tileScript : MonoBehaviour
{
    public gameController gameController;

    public Button button;
    public TMPro.TextMeshProUGUI buttonText;
    

    public void SetSpace()      //sets the text of a button on the grid with the current player side, and disables the button
    {

        buttonText.text = gameController.GetPlayerSide();
        button.interactable = false;
        gameController.EndTurn();
    }

    void Start()        //populate the buttons with the text component for easy editing
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void SetGameController(gameController controller)    //give every button a refference to the game controller for easy access
    {
        gameController = controller;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
