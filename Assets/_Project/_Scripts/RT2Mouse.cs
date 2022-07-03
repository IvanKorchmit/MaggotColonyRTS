using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RT2Mouse : MonoBehaviour
{
    private static RT2Mouse instance;
    [SerializeField] private RawImage output;
    
    private void Start()
    {
        instance = this;   
    }


    public static Vector2 GetMousePosition()
    {
        Vector2 result = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(result);
    }
}