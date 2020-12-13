using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager uiCurrent;

    public TextMeshProUGUI orbText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gameoverText;
    private void Awake()
    {
        if (uiCurrent != null)
        {
            Destroy(gameObject);
            return;
        }

        uiCurrent = this;

        DontDestroyOnLoad(this);
    }
    
    public static void UpdateOrbUI(int orbCount)
    {
        uiCurrent.orbText.text = orbCount.ToString();
    }

    public static void UpdateDeathUI(int deathCount)
    {
        uiCurrent.deathText.text = deathCount.ToString();
    }

    public static void UpdateTimeUI(float timeCount)
    {
        int minutes = (int)timeCount / 60;
        float seconds = timeCount % 60;

        uiCurrent.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public static void DisplayGameOver()
    {
        uiCurrent.gameoverText.enabled = true;
    }
}
