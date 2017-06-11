using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    private Text Ammo;

    //Find components
    void Start()
    {
        Ammo = GetComponent<Text>();
        Ammo.text = VrPlayer.Instance.Ammo.ToString();
    }

    //Basic Singleton
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;
    }

    //Called when you shot a bullet, to update the amount of ammo you have
    public void AmmoUpdate()
    {
        Ammo.text = VrPlayer.Instance.Ammo.ToString();
    }
}
