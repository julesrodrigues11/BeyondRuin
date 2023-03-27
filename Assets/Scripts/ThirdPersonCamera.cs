using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Rigidbody rigidbody;

    public float rotationSpeed;

    public CameraStyle currentStyle;
    public Transform combatLookAt;

    public GameObject thirdPersonCamera;
    public GameObject combatCamera;
    public GameObject topDownCamera;

    public enum CameraStyle
    {
        Basic,
        Combat,
        TopDown
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.TopDown);

        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.TopDown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDirection != Vector3.zero)
            {
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 directionToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = directionToCombatLookAt.normalized;

            playerObject.forward = directionToCombatLookAt.normalized;
        }
        
    }

    void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCamera.SetActive(false);
        thirdPersonCamera.SetActive(false);
        topDownCamera.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCamera.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCamera.SetActive(true);
        if (newStyle == CameraStyle.TopDown) topDownCamera.SetActive(true);

        currentStyle = newStyle;
    }
}
