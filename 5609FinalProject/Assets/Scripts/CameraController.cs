using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Objects")]
    public Camera mainCamera;
    public Camera topDown;

    [Header("Light Objects")]
    public Light directionalLight1;
    public Light directionalLight2;
    public Light topDownLight;

    // NOTE, it's currently bugged cuz I need to update it
    // based on current camera rotation
    [Header("Camera Settings")]
    public float mouseSensitivity = 0.7f;
    public float movementScalar = 10f;

    Vector3 defaultCamPos;
    Quaternion defaultCamRot;
    Vector2 mouseRot;
    bool topDownEnabled;

    // Start is called before the first frame update
    void Start()
    {
        mouseRot = new Vector2(0, 0);
        defaultCamPos = Camera.main.transform.position;
        defaultCamRot = Camera.main.transform.rotation;
        topDown.enabled = false;
        topDownEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!topDownEnabled)
        {
            // press R to reset the camera to default position
            if (Input.GetKey(KeyCode.R))
            {
                Camera.main.transform.position = defaultCamPos;
                Camera.main.transform.rotation = defaultCamRot;
            }



            // move camera with WASD
            if (Input.GetKey(KeyCode.W))
            {
                Camera.main.transform.Translate(new Vector3(0, movementScalar * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                Camera.main.transform.Translate(new Vector3(-movementScalar * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.S))
            {
                Camera.main.transform.Translate(new Vector3(0, -movementScalar * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.D))
            {
                Camera.main.transform.Translate(new Vector3(movementScalar * Time.deltaTime, 0, 0));
            }


            // rotate camera with right click
            if (Input.GetMouseButton(1))
            {
                mouseRot.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                mouseRot.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
                Camera.main.transform.localRotation = Quaternion.Euler(-mouseRot.y, mouseRot.x, 0);
            }


            // zoom in/out with mouse wheel
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Camera.main.fieldOfView--;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.fieldOfView < 90)
            {
                Camera.main.fieldOfView++;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetCameraTopDown();
        }
    }

    void SetCameraTopDown()
    {
        topDownEnabled = !topDownEnabled;
        topDown.enabled = topDownEnabled;
        mainCamera.enabled = !topDown.enabled;
    }
}
