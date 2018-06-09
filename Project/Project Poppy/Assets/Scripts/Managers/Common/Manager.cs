using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance
    {
        get
        {
            return instance;
        }
    }

    //Managers
    private LevelManager levelManager;
    public LevelManager LevelManager
    {
        get
        {
            return levelManager;
        }
    }

    private UIManager uiManager;
    public UIManager UIManager { get { return  uiManager;} }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        uiManager = GetComponent<UIManager>();
    }
}
