using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpuBehaviour : MonoBehaviour
{
    public gameController gameController;
    public GameObject[] gridButtons;
    public TMPro.TextMeshProUGUI[] buttonText;


    public string[] board;      //virtual board used for planning moves ahead in the minmax algorithm, updated in BoardInit


    public string playersymbol, cpusymbol;

    // Start is called before the first frame update
    void Start()
    {

        board = new string[9];

        buttonText = new TMPro.TextMeshProUGUI[9]; //initialize the list of 9 symbol placements postions

        for (int i = 0; i < gridButtons.Length; i++)
        {
            buttonText[i] = gridButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>(); //associate each position with the corresponding button

        }


    }

    public int EasyMove()      //random move on the board
    {
        
        int bestMove = Random.Range(0, 9);

        return bestMove;

    }

    public int MediumMove()     //50/50 between an easy and a hard move
    {
        int chance = Random.Range(0, 1);
        if (chance==0)
        {
            return EasyMove();
        }
        else
        {
            return HardMove();
        }
    }

    public int HardMove()
    {

        int score;
        int bestScore = int.MinValue;
        int bestMove = 0;

        for (int i = 0; i < 9; i++)
        {

            if (board[i] == "")
            {
                board[i] = cpusymbol;       //start placing symbols on the empty spots

                if (cpusymbol == "X")       //if the computer starts, it's maximising, else minimising
                {
                    score = Minimax(0, false);
                }
                else
                {
                    score = Minimax(0, true);

                }


                board[i] = "";
                if (score > bestScore)
                {
                    bestScore = score;      //chose the move that has the best total score after the minmax recursion
                    bestMove = i;
                }
            }

        }
        return bestMove;        //send the move index to the gameController
    }


    public void InitBoard()     //updates the virtual board with the real board status
    {
        for (int i = 0; i < 9; i++)
        {
            board[i] = buttonText[i].text;
        }
    }


    public string CheckWinner()     //checks if either the cpu or player won
    {

        for (int i = 0; i < 3; i++)     
        {
            //collums
            if (board[i] == board[i+ 3] && board[i + 3] == board[i+ 6] && board[i + 6] == playersymbol)
            {
                return playersymbol;
            }
            else if (board[i] == board[i + 3] && board[i + 3] == board[i + 6] && board[i + 6] == cpusymbol)
            {
                return cpusymbol;
            }

            //rows 
            if (board[i * 3] == board[i * 3 + 1] && board[i * 3 + 1] == board[i * 3 + 2] && board[i * 3 + 2] == playersymbol)
            {
                return playersymbol;
            }
            else if (board[i * 3] == board[i * 3 + 1] && board[i * 3 + 1] == board[i * 3 + 2] && board[i * 3 + 2] == cpusymbol)
            {
                return cpusymbol;
            }

        }
        //diagonals
        if ((board[0] == board[4] && board[4] == board[8] && board[8] == playersymbol) ||
            (board[2] == board[4] && board[4] == board[6] && board[6] == playersymbol))
        {
            return playersymbol;
        }
        else if ((board[0] == board[4] && board[4] == board[8] && board[8] == cpusymbol) ||
                (board[2] == board[4] && board[4] == board[6] && board[6] == cpusymbol))
        {
            return cpusymbol;
        }

        if (BoardFull())
        {
            return "tie";
        }

        return "";
    }

    public int EvaluateBoard()  //transform the winner string into an int used for scoring
    {
        string winner = CheckWinner();
        
        if (winner == cpusymbol)
        {
            return 1;
        }
        else if (winner == playersymbol)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }


    public int Minimax(int depth, bool isMaximizingPlayer)
    {
        if (cpusymbol == "O")       //in case the computer is second, start as a minimizing player in te minmax algorithm
        {
            isMaximizingPlayer = !isMaximizingPlayer;
        }

        if (EvaluateBoard() == 1)
        {
            return 10 - depth; // Computer wins
        }
        if (EvaluateBoard() == -1)
        {
            return depth - 10; // Player wins
        }
        if (BoardFull())
        {
            return 0; // Draw
        }

        int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;       //if is maximising or minimising set best score to +/- infinity

        for (int i = 0; i < 9; i++)
        {
          
                if (board[i] == "") //if a space is avaliable
                {
                    board[i] = isMaximizingPlayer ? cpusymbol : playersymbol;      //place the symbol of the players turn 
                    int score = Minimax(depth + 1, !isMaximizingPlayer);       //run the minmax algorithm on the updated board
                    board[i] = "";              //reset the board to the previous state
                    if (isMaximizingPlayer)
                    {
                        bestScore = System.Math.Max(bestScore, score);
                    }
                    else
                    {
                        bestScore = System.Math.Min(bestScore, score);
                    }
                }
            
        }
        return bestScore;       //when out of moves return the best score
    }

    

    public bool BoardFull()
    {
        
        for (int i = 0; i < 9; i++)
        {
            if (buttonText[i].text == "")
            {
                return false;
            } 
        }
        return true;
    }




    



}
