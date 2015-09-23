using UnityEngine;
using System.Collections;

public class MainMenuButtons : MonoBehaviour
{
    GameManager m_manager;
    
    void Start () 
    {
        m_manager = GameManager.Instance;
	}

    public void StartGame()
    {
        m_manager.StartGame();
    }

}
