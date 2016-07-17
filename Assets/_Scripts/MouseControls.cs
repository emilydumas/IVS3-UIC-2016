using UnityEngine;
using System.Collections;

public class MouseControls : MonoBehaviour
{
    public float mouseSensitivityX;
    public float mouseSensitivityY;

    public static bool rotate = true;
    public static bool cursor = false;

	// Use this for initialization
	void Start ()
    {
    	// Hide the cursor
	Cursor.visible = false;
	// keep it in the game window
	Cursor.lockState = CursorLockMode.Locked;

	}

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.LeftShift))
	        rotate = false;
	    if (Input.GetKeyUp(KeyCode.LeftShift))
	        rotate = true;

	    if (!rotate) return;

	    if (Input.GetKeyDown(KeyCode.Escape))

	{
		Application.Quit();
	}

	if (Input.GetKey(KeyCode.L))
	{
		cursor = !cursor;
	}

	if(cursor)
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
	else{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

      //   float dx = Input.GetAxis("Mouse X");
      //   float dy = -Input.GetAxis("Mouse Y");
      //
      //   transform.Rotate(Vector3.right, dy * Time.deltaTime * mouseSensitivityY);
	    // transform.Rotate(Vector3.up, dx*Time.deltaTime*mouseSensitivityX);
    }
}
