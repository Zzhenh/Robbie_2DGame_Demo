using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager gameCurrent;

    SceneFader sf;
    List<Orb> orbs;
    Door door_a;
    float gameTime;
    bool gameIsOver;

    public int deathNum;


    private void Awake()
    {
        if (gameCurrent != null)
        {
            Destroy(gameObject);
            return;
        }

        gameCurrent = this;

        orbs = new List<Orb>();

        DontDestroyOnLoad(this);

    }

    private void Update()
    {
        if (gameIsOver)
        {
            return;
        }

        gameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(gameCurrent.gameTime);
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        gameCurrent.sf = obj;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (gameCurrent == null)
        {
            return;
        }
        if (!gameCurrent.orbs.Contains(orb))
        {
            gameCurrent.orbs.Add(orb);
        }

        UIManager.UpdateOrbUI(gameCurrent.orbs.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!gameCurrent.orbs.Contains(orb))
        {
            return;
        }
        gameCurrent.orbs.Remove(orb);

        if (gameCurrent.orbs.Count == 0)
        {
            gameCurrent.door_a.Open();
        }

        UIManager.UpdateOrbUI(gameCurrent.orbs.Count);
    }

    public static void PlayerDied()
    {
        gameCurrent.sf.FadeOut();
        gameCurrent.deathNum++;
        UIManager.UpdateDeathUI(gameCurrent.deathNum);
        gameCurrent.Invoke("RestartScene", 1.5f);
    }

    public static void RegisterDoor(Door door)
    {
        gameCurrent.door_a = door;
    }

    public static void PlayerWin()
    {
        gameCurrent.gameIsOver = true;

        UIManager.DisplayGameOver();
        AudioManager.PlayWinAudio();
    }

    public static bool GameOver()
    {
        return gameCurrent.gameIsOver;
    }

    void RestartScene()
    {
        gameCurrent.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
