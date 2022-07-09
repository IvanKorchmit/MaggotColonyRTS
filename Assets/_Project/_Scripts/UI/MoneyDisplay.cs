using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    private void OnGUI()
    {
        text.text = $"Money: {Economics.Money} Steel: {Economics.Steel} Fuel: {Economics.Fuel}\n" +
            $"Units: {Economics.Units}/{Economics.MaxUnits} ME: {Economics.ME}/{Economics.MaxME} Buildings: {Economics.Buildings}/{Economics.MaxBuildings}";
    }
}
