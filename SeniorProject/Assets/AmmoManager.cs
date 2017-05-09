using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    private Text Ammo;
    // Use this for initialization
    void Start()
    {
        Ammo = GetComponent<Text>();
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void AmmoUpdate()
    {
        Ammo.text = VrPlayer.Instance.Ammo.ToString();
    }
}
