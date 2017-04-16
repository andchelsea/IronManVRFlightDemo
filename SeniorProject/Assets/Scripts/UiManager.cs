using UnityEngine;
using System.Collections;

public class UiManager : MonoBehaviour {

    public static UiManager Instance;
    private bool paused = false;

	// Use this for initialization
	void Awake () {
        Instance = this;

    }
	
    public void PCTogglePause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None; //hide crosshair
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Locked; //unhide crosshair
            Cursor.visible = false;
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
