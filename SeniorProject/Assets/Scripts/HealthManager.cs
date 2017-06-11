using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public static HealthManager Instance;
    private Text health;

    void Start ()
    {
        health = GetComponent<Text>();
        health.text = VrPlayer.Instance.GetHealth().ToString();
       
	}

    //Basic Singleton
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;
    }

    //Called when VR player takes damage
    public void Damaged()
    {
        health.text = VrPlayer.Instance.GetHealth().ToString();
    }
}
