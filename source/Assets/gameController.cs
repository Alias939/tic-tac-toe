using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameController : MonoBehaviour
{
    public GameObject cpuBehaviour;

    [Header("Game buttons")]
    public GameObject[] gridButtons;
    public TMPro.TextMeshProUGUI[] buttonText;
    public GameObject restartButton;
    
    [Header("Player identification")]

    public bool playerMove;
    public bool vsCpu;
    public string playerSide;
    public string computerSide;
    public int difficulty;
    private int moveCount;

    [Header("Menu objects")]

    public TMPro.TextMeshProUGUI gameOverText;
    public TMPro.TextMeshProUGUI currentPlayer;
    public GameObject gameOverPanel;
    public GameObject mainMenu;
    public GameObject gameModeMenu;
    public GameObject difficultyMenu;
    public GameObject playerSlider;




    // Start is called before the first frame update
    void Start()
    {

        
        gameOverText = gameOverPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();


        //Set the default symbols for players
        playerSide = "X";
        computerSide = "O";





        buttonText = new TMPro.TextMeshProUGUI[9]; //initialize the list of 9 symbol placements postions

        for (int i = 0; i < gridButtons.Length; i++)
        {
            buttonText[i] = gridButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>(); //associate each position with the corresponding button
        }
        SetGameController();    //set the game controller in each button to control the button text (SetSpace() function)

        
        playerMove = true;     //by default start the game as the player


    }

    public void makeCpuMove()
    {
        int value;     //set by difficulty menu buttons
        switch (difficulty)
        {
            case 0:
                value = cpuBehaviour.GetComponent<cpuBehaviour>().EasyMove();
                break;
            case 1:
                value = cpuBehaviour.GetComponent<cpuBehaviour>().MediumMove();
                break;
            case 2:
                value = cpuBehaviour.GetComponent<cpuBehaviour>().HardMove();
                break;
            default:
                value = cpuBehaviour.GetComponent<cpuBehaviour>().EasyMove();
                break;

        }
        if (gridButtons[value].GetComponent<Button>().interactable == true)     //after a move is chosen, and the move is valid, update the button and end turn
        {
            buttonText[value].text = computerSide;
            gridButtons[value].GetComponent<Button>().interactable = false;
            EndTurn();
        }

    }



    void Update()
    {
        if (vsCpu)      //in case the vsCpu mode is selected
        {
            if (playerMove == false)        //check if it's the cpu move and make a move
            {
                makeCpuMove();
            }
        }
        
        
    }



    void SetGameController()        
    {
        for (int i = 0; i < gridButtons.Length; i++)
        {
            gridButtons[i].GetComponent<tileScript>().SetGameController(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;   
    }


    public void EndTurn()
    {

        string side;
        

        if (playerMove == true)      //set the side depending on who has to make a move
        {
            side = playerSide;
        }
        else
        {
            side = computerSide;
        }

        moveCount++;       //used to check if boar is full

        for (int i = 0; i < 3; i++)    //checks 3 collums and 3 rows
        {
            //checks collums for same symbol
            if (buttonText[i].text == buttonText[i + 3].text && buttonText[i + 3].text == buttonText[i + 6].text && buttonText[i + 6].text == side)
            {
                SetGameOverText(side + " Wins");       //could be turned into a function, but is pretty clear
                moveCount = 0;
                StartCoroutine(EndGame());
            }

            //checks rows for same symbol
            if (buttonText[i * 3].text == buttonText[i * 3 + 1].text && buttonText[i * 3 + 1].text == buttonText[i * 3 + 2].text && buttonText[i * 3 + 2].text == side)
            {
                SetGameOverText(side + " Wins");
                moveCount = 0;
                StartCoroutine(EndGame());
            }

        }
        //checks diagonals for same symbol
        if ((buttonText[0].text == buttonText[4].text && buttonText[4].text  == buttonText[8].text && buttonText[8].text == side) || 
            (buttonText[2].text == buttonText[4].text && buttonText[4].text == buttonText[6].text && buttonText[6].text == side))
        {
            SetGameOverText(side + " Wins");
            moveCount = 0;
            StartCoroutine(EndGame());
        }

        //if the board is full
        if (moveCount >= 9)
        {
           SetGameOverText("Draw");
           StartCoroutine(EndGame());
        }


       
        cpuBehaviour.GetComponent<cpuBehaviour>().InitBoard();  //updates the virtual board in cpu behaviour script

        ChangeSides();  //if none of the win or draw conditions are met, the sides are changed and the game continues

    }

    IEnumerator EndGame()
    {


        restartButton.GetComponent<Button>().interactable = false;     //because of the couroutine the restart button can still be clicked after gameover and causes a bug

        gameOverPanel.SetActive(true);      //shows the gameover message

        for (int i = 0; i < gridButtons.Length; i++)        //make all the buttons non-interactable to freeze board status at the end of a game
        {
            gridButtons[i].GetComponent<Button>().interactable = false;    

        }

        yield return new WaitForSeconds(2);     //delay between games

        RestartGame();      
    }

    public void RestartGame()
    {
        
        cpuBehaviour.GetComponent<cpuBehaviour>().playersymbol = playerSide;        //updates the sides for the behaviour script
        cpuBehaviour.GetComponent<cpuBehaviour>().cpusymbol = computerSide;

        for (int i = 0; i < gridButtons.Length; i++)    //reinitializes buttons with no text and interactable
        {
            gridButtons[i].GetComponent<Button>().interactable = true;
            buttonText[i].text = "";

        }
        moveCount = 0;      

        int value = (int)playerSlider.GetComponent<Slider>().value;     //checks the value of player and computer symbols

        switch (value)
        {
            case 0:
                playerSide = "X";
                playerMove = true;
                break;
            case 1:
                playerSide = "O";
                playerMove = false;
                break;
            default:
                playerSide = "X";
                playerMove = true;
                break;
        }

        //sets players depending on the slider

        if (playerSide == "X")
        {
            computerSide = "O";
        }
        else
        {
            computerSide = "X";
        }
        

        SetCurrentPlayer();
        gameOverPanel.SetActive(false);
        restartButton.GetComponent<Button>().interactable = true;
    }


    void ChangeSides()
    {
        if (vsCpu)      //in case of vscpu, change sides after a move
        {
            if (playerMove == true)
            {
                playerMove = false;
            }
            else
            {
                playerMove = true;
            }
        }
        else           //else change symbols after a move
        {
            playerMove = true;
            if (playerSide == "X")
            {
                playerSide = "O";
            }
            else
            {
                playerSide = "X";
            }
        }

        SetCurrentPlayer();
    }

    void SetCurrentPlayer()
    {           //sets the text in the current player pannel. If it's vsCpu the text does not exist because the computer move is instant 
        if (vsCpu)
        {
            currentPlayer.text = "";
        }
        else
        {
            currentPlayer.text = playerSide + "'s turn";
        }
        
    }

    void SetGameOverText(string text)
    {
        gameOverText.text = text; 
    }

    public void DifficultySet(int diff)
    {
        MenuToggle(false, false, false);    //starts the vsCpu game
        difficulty = diff;
        vsCpu = true;
        RestartGame();
    }


    //Menu switching

    public void MainStart()
    {
        MenuToggle(false, true, false);
    }
    public void MainExit()
    {
        Application.Quit();
    }
    public void GameBack()
    {
        if (vsCpu == true)
            MenuToggle(false, false, true);
        else
            MenuToggle(false, true, false);
        RestartGame();
    }
    public void ModeCpu()
    {
        MenuToggle(false, false, true);
        vsCpu = true;
        playerSlider.SetActive(true);
    }
    public void ModePlayer()
    {
        
        playerSlider.GetComponent<Slider>().value = 0;
        MenuToggle(false, false, false);        //starts the vsPlayer game
        vsCpu = false;
        playerSlider.SetActive(false);
        RestartGame();
    }
    public void ModeBack()
    {

        MenuToggle(true, false, false);
    }
    public void DifficultyBack()
    {
        MenuToggle(false, true, false);
    }

    public void MenuToggle(bool main,bool mode, bool difficulty)
    {
        mainMenu.SetActive(main);
        gameModeMenu.SetActive(mode);
        difficultyMenu.SetActive(difficulty);
    }

}
