using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public void LoadScene(int value)
    {
        SceneManager.LoadScene(value);
    }
    
}
