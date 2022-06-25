using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerCameraTranform;
    [SerializeField] float speed = 20f;
    [SerializeField] float screenBorderThickness = 10f;
    [SerializeField] Vector2 screenXLimits = Vector2.zero;
    [SerializeField] Vector2 screenZLimits = Vector2.zero;

    Vector2 previousInput;
    Controls controls;
    

    private void Start()
    {
        playerCameraTranform.gameObject.SetActive(true);

        controls = new Controls();

        controls.Player.MoveCamera.performed += SetPreviousInput;
        controls.Player.MoveCamera.canceled += SetPreviousInput;

        controls.Enable();
    }

    private void Update()
    {
        

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 pos = playerCameraTranform.position;

        if (previousInput == Vector2.zero)
        {
            Vector3 cursorMovement = Vector2.zero;
            Vector3 cursourPosition = Mouse.current.position.ReadValue();

            if (cursourPosition.y >= Screen.height - screenBorderThickness)
            {
                cursorMovement.y += 1;
            }
            if (cursourPosition.y <= screenBorderThickness)
            {
                cursorMovement.y -= 1;
            }
            if (cursourPosition.x >= Screen.width - screenBorderThickness)
            {
                cursorMovement.x += 1;
            }
            if (cursourPosition.x <= screenBorderThickness)
            {
                cursorMovement.x -= 1;
            }

            pos += cursorMovement.normalized * speed * Time.deltaTime;
        }
        else
        {
            pos += new Vector3(previousInput.x, previousInput.y) * speed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, screenXLimits.x, screenZLimits.y);
        pos.y = Mathf.Clamp(pos.y, screenXLimits.x, screenZLimits.y);
        pos.z = playerCameraTranform.position.z;
        playerCameraTranform.position = pos;
    }

    private void SetPreviousInput(InputAction.CallbackContext ctx)
    {
        previousInput = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
}
