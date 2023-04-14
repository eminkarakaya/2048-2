using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<Image> levelIcons; 
    public int selectedLevel;
    private void Start() {
        foreach (var item in levelIcons)
        {
            Color color = item.color;
            color.a = .6f;
            item.color = color;
            Color color4 = item.GetComponentInChildren<TextMeshProUGUI>().color;
            color4.a = .6f;
            item.GetComponentInChildren<TextMeshProUGUI>().color = color4;
        }
        selectedLevel = 1;
        Color color1 = levelIcons[0].color;
        color1.a = 1; 
        levelIcons[0].color = color1;
        
        Color color2 = levelIcons[0].GetComponentInChildren<TextMeshProUGUI>().color;
        color2.a = 1;
        levelIcons[0].GetComponentInChildren<TextMeshProUGUI>().color = color2;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(selectedLevel);
    }
    public void SetSelectedLevel(int value)
    {
        selectedLevel = value;
        foreach (var item in levelIcons)
        {
            Color color = item.color;
            color.a = .6f;
            item.color = color;
            Color color2 = item.GetComponentInChildren<TextMeshProUGUI>().color;
            color2.a = .6f;
            item.GetComponentInChildren<TextMeshProUGUI>().color = color2;
        }
        Color color1 = levelIcons[selectedLevel-1].color;
        color1.a = 1; 
        levelIcons[selectedLevel-1].color = color1;
        
        Color color3 = levelIcons[selectedLevel-1].GetComponentInChildren<TextMeshProUGUI>().color;
        color3.a = 1;
        levelIcons[selectedLevel-1].GetComponentInChildren<TextMeshProUGUI>().color = color3;
    }
}
