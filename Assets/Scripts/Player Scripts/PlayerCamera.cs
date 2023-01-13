using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX, sensY;
    private float xRotation, yRotation;

    public Transform orientation;

    public bool mouseLock;

    private void Start()
    {
        mouseLock = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update()
    {
        if (mouseLock)
        {

            //Mouse input
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;

            //Player can't go past 90 degrees on the camera
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Rotate camera and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        }
     

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (mouseLock)
            {
                mouseLock = false;
                Debug.Log("release the mouse");
                ReleaseMouse();
            }
            else 
            {
                mouseLock = true;
                LockMouse();
            }
            
        }

    }
    private void ReleaseMouse()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Need a delay because it's so fricken fast if I press the lock mouse button it unlocks and then locks in the same frame.
    IEnumerator DelayTimer(bool boolean)
    {
        yield return new WaitForSeconds(0.25f);
        mouseLock = boolean;
    }

}
