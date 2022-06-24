using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FollowMouse : MonoBehaviour
{
    public void GoTo()
    {
        GetComponent<MovementAstar>().FindPath(transform.position, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));    
    }
}
