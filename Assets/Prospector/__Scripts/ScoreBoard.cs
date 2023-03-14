using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    private static ScoreBoard S;
    private int _score = 0;
    public int score{
        get{return _score;}
        set{
            _score=value;
            textMP.text=_score.ToString("#,##0");
        }
    }
    private TMP_Text textMP;
    
    private void Awake() {
        if(S!=null){
            S=this;
            textMP=GetComponent<TMP_Text>();
        }
    }
    public static int SCORE{
        get{return S.score;}
        set{S.score = value;}
    }
    public static void FS_CALLBACK(FloatingScore fs){
        SCORE+=fs.score;
    }
}
