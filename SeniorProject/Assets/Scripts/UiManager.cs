using UnityEngine;
using System.Collections;

public class UiManager : MonoBehaviour {

    public static UiManager Instance;
    private bool paused = false;
    [SerializeField]   Canvas VRPause;
	// Use this for initialization
	void Awake () {
        Instance = this;

    }
	
    public void PCTogglePause()
    {
        paused = !paused;
        if (paused)
        {
            GetComponentInChildren<Canvas>().enabled = true;
            VRPause.enabled = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None; //hide crosshair
            Cursor.visible = true;
        }
        else
        {
            GetComponentInChildren<Canvas>().enabled = false;
            VRPause.enabled = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked; //unhide crosshair
            Cursor.visible = false;
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
