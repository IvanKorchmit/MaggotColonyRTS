using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    private void OnGUI()
    {
        text.text = Economics.Money.ToString();
    }
}