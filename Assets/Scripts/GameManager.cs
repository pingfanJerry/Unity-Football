using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //consts
    const int MAXSCORE = 10;

    //members
    float m_countDownTime = 3f;

    //other Components
    TextMesh G1ScoreText;
    TextMesh G2ScoreText;

    Camera m_cam;
    Animation m_camAnimation;
    GameObject m_ball;

    public GUIStyle m_style;

    //components
    Text m_gameOverText;  //still needs to be done

    //params
    private StateType m_state;

    private bool m_doGUI = false;

    private int G1Score;
    private int G2Score;

    //*****singleton
    private static GameManager m_instance;

    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return m_instance;
        }
    }
    //**************

    //***********properties
    public StateType State
    {
        get { return m_state; }
        set { m_state = value; }
    }
    //***********

    //autofunctions
    void Awake()
    {
        m_instance = this;

        G1ScoreText = GameObject.FindGameObjectWithTag("ScoreLeft").GetComponent<TextMesh>();
        G2ScoreText = GameObject.FindGameObjectWithTag("ScoreRight").GetComponent<TextMesh>();

        m_gameOverText = GameObject.FindGameObjectWithTag("GameOverText").GetComponent<Text>();

        m_ball = GameObject.FindGameObjectWithTag("Ball");
        m_cam = Object.FindObjectOfType<Camera>();
        m_camAnimation = m_cam.GetComponent<Animation>();

    }

    void Start()
    {
        m_state = StateType.mainMenu;
    }

    void OnGUI()
    {
        if (m_doGUI)
            GUI.Label(new Rect(Screen.width/2 - 250, Screen.height/2 - 225, Screen.width/2 - 250, Screen.height/2 - 225), "Get Ready", m_style);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("A");
            Application.Quit();
        }
        if (m_camAnimation.isPlaying) //during start animation 
        {
            Goal("Reset");
            State = StateType.startAnimation;
        }
        else if (!m_camAnimation.isPlaying && State == StateType.startAnimation) //when animation ends goes to getReady
            BeginPlay();

        else if (State == StateType.getReady && !m_doGUI) //when countdown ends goes to mainGame
            State = StateType.mainGame;


        else if (State == StateType.mainGame) //when Maingame...
        {
            if (Mathf.Max(G1Score, G2Score) >= MAXSCORE) //...ends goes to endanimation
            {
                State = StateType.endAnimation;
                GameOver();
            }
        }
        else if (State == StateType.endAnimation) //when endanimation ends goes to mainMenu
        {
            if (Quaternion.Angle(m_cam.transform.rotation, Quaternion.Euler(new Vector3(45, 60, 0))) < 0.05f)
            {
                State = StateType.mainMenu;
            }
            else
                EndSlerp();

        }
    }

    //******main game functions
    public void StartGame()
    {
        if (State == StateType.mainMenu)
        {
            m_camAnimation.Play();
        }
    }
    public void BeginPlay()
    {
        State = StateType.getReady;
        m_ball.GetComponent<Ball>().StartPosition();
        StartCoroutine(CountDownTimer());
    }
    public void Goal(string goal)
    {
        switch (goal)
        {
            case "Reset":
                G1Score = 0;
                G2Score = 0;
                break;
            case "GoalLeft":
                G2Score++;
                if (G2Score < MAXSCORE)
                    BeginPlay();
                break;
            case "GoalRight":
                G1Score++;
                if (G1Score < MAXSCORE)
                    BeginPlay();
                break;
        }
        UpdateScore();
    }
    void UpdateScore()
    {
        G1ScoreText.text = "" + G1Score;
        G2ScoreText.text = "" + G2Score;
    }
    void GameOver()
    {
        string result = "";

        if (G1Score == MAXSCORE)
            result += "Blue wins " + G1Score + " : " + G2Score;
        else
            result += "Red wins: " + G2Score + " : " + G1Score;

        result += "\nPress play to start";

        m_gameOverText.text = result;

    }
    void EndSlerp()
    {
        m_cam.transform.position = Vector3.Lerp(m_cam.transform.position, new Vector3(-26, 18, -10), Time.deltaTime * 10);
        m_cam.transform.eulerAngles = new Vector3(
            Mathf.LerpAngle(m_cam.transform.eulerAngles.x, 45, Time.deltaTime * 10),
            Mathf.LerpAngle(m_cam.transform.eulerAngles.y, 60, Time.deltaTime * 10),
            Mathf.LerpAngle(m_cam.transform.eulerAngles.z, 0, Time.deltaTime * 10));
    }

    IEnumerator CountDownTimer()
    {
        m_doGUI = true;
        m_countDownTime = 3;

        while (m_countDownTime >= 0)
        {
            m_countDownTime -= Time.deltaTime;

            yield return null;
        }
        m_doGUI = false;
    }
}

public enum StateType
{
    mainMenu, startAnimation, getReady, mainGame, endAnimation, gameOver
}