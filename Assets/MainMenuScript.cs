using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void ProspectorScene(){
        SceneManager.LoadScene("__Prospector_Scene_0");
    }
    public void GolfScene(){
        SceneManager.LoadScene("__GolfSolitaire");
    }
}
