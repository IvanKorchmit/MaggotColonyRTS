using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MouseScroll : MonoBehaviour
{
    private Camera[] cams;
    [SerializeField] private float sensitivity;
    public void Scroll(InputAction.CallbackContext ctx)
    {
        float d = ctx.ReadValue<Vector2>().y;
        d = Mathf.Clamp(d, -1, 1);
        foreach (var cam in cams)
        {
            cam.orthographicSize += d * sensitivity * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 5, 30);

        }
    }
    private void Start()
    {
        cams = GetComponentsInChildren<Camera>();
    }
}
