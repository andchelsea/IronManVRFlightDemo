using UnityEngine;
using System.Collections;

public class UiManager : MonoBehaviour {

    public static UiManager Instance;
    private bool paused = false;
    private bool updatable = true;

	// Use this for initialization
	void Awake ()
    {
        Instance = this;

        paused = false;
        updatable = true;
}
	
    public bool IsUpdatable() { return updatable; }
    public void SetUpdatable(bool update) { updatable = update; }

    public void TogglePause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None; 
            //hide PC crosshair
            Cursor.visible = true;

            updatable = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked; 
            //unhide PC crosshair
            Cursor.visible = false;

            updatable = true;
        }

    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
