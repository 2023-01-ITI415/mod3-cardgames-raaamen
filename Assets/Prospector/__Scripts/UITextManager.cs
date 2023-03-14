using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextManager : MonoBehaviour
{
    private static UITextManager S;

    public TMP_Text gameOverText;
    public TMP_Text roundResultText;
    public TMP_Text highScoreText;

    [SerializeField]
    private bool _resultsUIFieldsVisible = false;

    public bool resultsUIFieldsVisible{
        get{return _resultsUIFieldsVisible;}
        private set{
            _resultsUIFieldsVisible = value;
            gameOverText.gameObject.SetActive(value);
            roundResultText.gameObject.SetActive(value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(S!=null){
            S=this;
            ShowHighScore();
            resultsUIFieldsVisible=false;
        }
    }
    void ShowHighScore(){
        string str = $"HighScore: {ScoreManager.HIGH_SCORE:#,##0}";
        highScoreText.text=str;
    }
    public static void GAME_OVER_UI(bool won){
        S.GameOverUI(won);
    }
    public void GameOverUI(bool won){
        int score = ScoreManager.SCORE;
        string str;
        if(won){
            gameOverText.text = "Round Over";
            str = "You won this round!\n" + $"Round Score:{ScoreManager.SCORE_THIS_ROUND:#,##0}";
        }
        else{
            gameOverText.text = "Game Over";
            if (ScoreManager.HIGH_SCORE <= score)
            {
                str = $"You got the high score\nHigh Score: {score:#,##0}";
            }
            else{
                str = $"Your final score was\n{score:#,##0}";
            }
        }
        roundResultText.text=str;
        resultsUIFieldsVisible=true;
        ShowHighScore();
    }
}
