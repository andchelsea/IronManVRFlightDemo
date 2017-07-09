using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public static HealthManager Instance;
    private Text health;
    // Use this for initialization
    void Start ()
    {
        health = GetComponent<Text>();
        health.text = VrPlayer.Instance.GetHealth().ToString();
       
	}
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;
    }
    // Update is called once per frame
    void Update ()
        {
            
        }

    public void Damaged()
    {
        health.text = VrPlayer.Instance.GetHealth().ToString();
    }
}
