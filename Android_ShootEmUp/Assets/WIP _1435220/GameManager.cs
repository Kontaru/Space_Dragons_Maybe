using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public string Name;
    public int Score;
    public int Type;        //Identifier to distinguish between the current player and previous scores (previous scores have type 0)

    public Player()         //Default constructur - if we don't fill the () with data then the player will, by default, take these values.
    {
        Name = "Bob";
        Score = 1;
        Type = 0;
    }
                            //Constructor - if data is filled in the () then we can start setting parameters to initialise a Player variable
    public Player(string name, int score, int type)
    {
        Name = name;
        Score = score;
        Type = type;
    }
}

public class GameManager : MonoBehaviour {

    public static GameManager instance;                     //Sets the instance of the GameManager so multiple GameManagers can't exist
    private float SceneSwitchCooldown;                      //So that the scene doesn't instantly change
    private bool BL_SwitchScenes;                           //Whether to switch or not

    [Header("Inputs")]
    public KeyCode KC_TurnLeft;
    public KeyCode KC_TurnRight;
    public KeyCode KC_Dethrottle;
    public KeyCode KC_Shoot;
    public KeyCode KC_FTL;
    public InputField mainInputField;                       //Input field for player name

    [Header("Entities")]
    public GameObject GO_Player;
    public GameObject PF_Bullet;
    protected List<GameObject> GO_Enemies;

    [Header("HighScore Data")]
    public GameObject HighScore;                            //Reference for the high score so we can enable/disable
    public Player[] Score = new Player[11];                 //A player has variables which contain data for the score, so we store our top 11 players for safe keeping
    public Player CurrentPlayer = new Player();             //The current player. This will have a Type value of "1" as an identifier
    public Text[] TX_ScoreList;                             //Text reference for the score list. A more elegant solution, possibly, would be to child these to an object and use "getChild"
    public Text PlayerScore;                                //Top right player score                  

    [Header("PlayerUI")]
    public Image[] HPbar;                                   //HP bar placeholders
    public Image[] Energybar;                               //Energy bar placeholders
    private Color stockcolor;                               //The default alpha value of an image
    private Color offcolor;                                 //This colour will have an alpha of 0, indicating "off"
    public GameObject Othercrap;                            //Sorry
    public GameObject OtherOthercrap;                       //I was pissed - I'll fix this later
    public Text FTLJuice;                                   //FTL Juice

    [Header("Global Parameters")]
    public bool BL_FTL = false;
    public float FL_FTLbar = 20f;
    public bool BL_KillAll = false;
    public int Player_HP = 3;
    private float RespawnCoolDown;
    private bool Respawn = false;
    public double DBL_CoolDown;

    //We make a singleton of the GameManager, but we need to make sure that if we have another GameManager, we only keep one
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    void Start () {
        //Adds a listener that invokes the "LockInput" method when the player finishes editing the main input field.
        //Passes the main input field into the method when "LockInput" is invoked
        if (mainInputField == null)
            return;
        else
            mainInputField.onEndEdit.AddListener(delegate { PlayerName(mainInputField); });

        #region --- Colour Setting ---

        stockcolor = HPbar[1].color;
        offcolor = HPbar[1].color;
        offcolor.a = 0f;

        #endregion

        #region --- Initialise Highscore ---

        CurrentPlayer = new Player("You", 0, 1);
        //Set the fake player names into the score list
        InitialiseScores();
        //To execute a proper bubble sort. You won't need to do a bubble sort everytime you reorganise the scoreboard, so we only perform a bubble sort once rather than create a method.
        for (int i = 0; i < Score.Length; i++)
        {
            ReorganiseStandings();
        }
        Debug.Log(Score[0].Name);       //Check to see if the bubble sort has worked by seeing who's on the top of the list

        #endregion
    }

    // Update is called once per frame
    void Update () {

        #region --- Player UI ---

        PlayerScore.text = string.Format("{0}", CurrentPlayer.Score);
        FTLJuice.text = string.Format("{0}", (int)FL_FTLbar);

        #endregion

        #region --- HP Handler ---

        if (Player_HP == 3)
        {
            HPbar[2].color = stockcolor;
            HPbar[1].color = stockcolor;
            HPbar[0].color = stockcolor;
        }
        else if (Player_HP == 2)
        {
            HPbar[2].color = offcolor;
            HPbar[1].color = stockcolor;
            HPbar[0].color = stockcolor;
        }
        else if (Player_HP == 1)
        {
            HPbar[2].color = offcolor;
            HPbar[1].color = offcolor;
            HPbar[0].color = stockcolor;
        }
        else if (Player_HP == 0)
        {
            HPbar[2].color = offcolor;
            HPbar[1].color = offcolor;
            HPbar[0].color = offcolor;
        }

        #endregion

        #region --- Energy Handler ---

        #endregion

        if (BL_KillAll)
            Invoke("Unkill", 0.01f);

        if (Respawn)
        {
            if (Time.time > RespawnCoolDown)
            {
                Respawn = false;
                GO_Player.SetActive(true);
            }
        }

        #region --- End Game ---

        if (Player_HP <= 0)
        {
            BL_SwitchScenes = true;
        }

        if (BL_SwitchScenes)
        {
            if (SceneSwitchCooldown < Time.time)
            {
                Player_HP = 3;

                CurrentPlayer.Type = 0;
                CurrentPlayer = new Player("YOU", 0, 1);
                Score[10] = CurrentPlayer;

                ReorganiseStandings();
                SetPositions();

                GO_Player.SetActive(true);
                BL_SwitchScenes = false;
                NextScene();
                RestartConditions();

                OtherOthercrap.SetActive(true);
            }
        }

        #endregion
    }

    void RestartConditions()
    {
        DBL_CoolDown = 3.0d;
    }

    void PlayerName(InputField input)
    {
        if (input.text.Length > 0)
        {
            CurrentPlayer.Name = input.text;
            Debug.Log(input.text + " has been entered");
            ReorganiseStandings();
        }
        else if (input.text.Length == 0)
        {
            Debug.Log("Main Input Empty");
        }
    }

    public void RespawnPlayer()
    {

        RespawnCoolDown = Time.time + 0.5f;
        GO_Player.transform.position = new Vector3(0, 0, -1);
        GO_Player.SetActive(false);
        ReorganiseStandings();
        SetPositions();
        SceneSwitchCooldown = Time.time + 2.0f;

        if (Player_HP > 0)
            Respawn = true;

    }

    void Unkill()
    {
        BL_KillAll = false;
    }

    #region ~ High Score ~

    //Displaying the highscore requires...
    public void DisplayHighScore()
    {
        ReorganiseStandings();          //... For us to reorganise the standings so that players are in the right rank
        SetPositions();                 //... To then actually set those positions onto the scoreboard
        HighScore.SetActive(true);      //... And then for us to display it to the player
    }

    public void CloseHighScore()
    {
        HighScore.SetActive(false);
    }

    public void ReorganiseStandings()
    {
        for (int i = Score.Length - 1; i > 0; i--)
        {
            if (Score[i].Type == 1)
                Score[i] = CurrentPlayer;

            if (Score[i].Score > Score[i - 1].Score)
            {
                Player Temp = Score[i - 1];
                Score[i - 1] = Score[i];
                Score[i] = Temp;
            }
        }
    }

    //Actually puts the scores onto the UI
    void SetPositions()
    {
        for (int i = 0; i < Score.Length - 1; i++)
        {
            TX_ScoreList[i].text = string.Format(Score[i].Name);
            TX_ScoreList[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = string.Format(Score[i].Score.ToString());
        }
    }

    //This is a bunch of fake players that we initialise on first play, just to populate the score board and to give the player a sense of competition.
    void InitialiseScores()
    {
        Score[9] = new Player("Shio", 766999, 0);
        Score[1] = new Player("Froggy", 150000, 0);
        Score[2] = new Player("Ricky", 150000, 0);
        Score[3] = new Player("Takumi", 50000, 0);
        Score[4] = new Player("GaS", 1300, 0);
        Score[5] = new Player("gAs", 1100, 0);
        Score[6] = new Player("gAS", 1000, 0);
        Score[7] = new Player("I_Wanna", 900, 0);
        Score[8] = new Player("StepOn", 500, 0);
        Score[0] = new Player("TheGas", 200, 0);

        Score[10] = CurrentPlayer;
    }

    #endregion

    #region ~ Scene Related ~

    public void NextScene()
    {
        Othercrap.SetActive(false);
        OtherOthercrap.SetActive(false);
        /*Check to see if the current scene isn't the last one*/
        if (SceneManager.GetActiveScene().buildIndex < 2) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            Othercrap.SetActive(true);
        }
        /*If it was the last scene, load back up tot he first level (the last level will always be the quit screen*/
    }

    public void EndGame()
    {
        /*End game*/
        Application.Quit();
    }

    #endregion
}
