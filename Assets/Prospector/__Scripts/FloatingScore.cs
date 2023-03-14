using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class FloatingScore : MonoBehaviour
{
    static List<FloatingScore> FS_ALL = new List<FloatingScore>();
    public float[] fontSizes={10,65,48};
    private int _score = 0;
    public int score{
        get{return _score;}
        set{
            _score=value;
            textField.text = _score.ToString("#,##0");
        }
    }
    public delegate void FloatingScoreDelegate(FloatingScore fs);
    public event FloatingScoreDelegate FSCallbackEvent;
    private TMP_Text textField;
    private BezierMover mover;
    private void Awake() {
        textField = GetComponent<TMP_Text>();
        mover = GetComponent<BezierMover>();
    }
    void MoverCompleteCallBack(){
        if(FSCallbackEvent!=null){
            FSCallbackEvent(this);
            FSCallbackEvent=null;
            Destroy(gameObject);
        }
    }

    public void FSCallback(FloatingScore fs){
        score+=fs.score;
    }

    private void OnEnable() {
        FS_ALL.Add(this);
    }
    private void OnDisable() {
        FS_ALL.Remove(this);
    }

    public void Init(List<Vector2> ePts, float eTimeD = 1, float eTimeS = 0){
        mover.completionEvent.AddListener(MoverCompleteCallBack);
        mover.Init(ePts,eTimeD,eTimeS);
    }
    private void Update() {
        if(mover.state == BezierMover.eState.active){
            if (fontSizes!=null && fontSizes.Length > 0)
            {
                float size = Utils.Bezier(mover.uCurved, fontSizes);
                textField.fontSize=size;
            }
        }
    }

    public static void REROUTE_TO_SCOREBOARD(){
        Vector2 fsPosEnd = new Vector2(0.5f,0.95f);
        foreach (var fs in FS_ALL)
        {
            fs.mover.bezierPts[fs.mover.bezierPts.Count-1]=fsPosEnd;
            fs.FSCallbackEvent=null;
            fs.FSCallbackEvent+=ScoreBoard.FS_CALLBACK;
        }
        FS_ALL.Clear();
    }
}
