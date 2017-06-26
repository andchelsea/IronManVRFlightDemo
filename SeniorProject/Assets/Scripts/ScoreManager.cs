using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance;
    private Text score;

    void Start()
    {
        score = GetComponent<Text>();
        score.text = "Score: " + VrPlayer.Instance.GetScore().ToString();
    }

    void Awake()
    {
        //Basic Singleton
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;
    }

    //Called when points are scored
    public void UpdateScore()
    {
        score.text = "Score: " + VrPlayer.Instance.GetScore().ToString();
    }
}
