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
    private UIManager uiManager;    
    private GameEventView m_gameEventView;

    public GameEventView GameEventView {get {return m_gameEventView;}}    
    public UIManager UIManager { get { return  uiManager;} }
    public LevelManager LevelManager {get {return levelManager;}} 


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
        m_gameEventView = GetComponent<GameEventView>();
    }
}
