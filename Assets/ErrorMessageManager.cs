using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessageManager : MonoBehaviour
{
    private static ErrorMessageManager instance;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    private void Start()
    {
        instance = this;

    }
    public static void LogError(string message)
    {
        instance.text.text = message;
        TimerUtils.AddTimer(1, () => instance.text.text = "");
    }
}
