using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ContextMenuButton : MonoBehaviour
{
    [SerializeField] private ContextMenu.Option option;
    [SerializeField] private TextMeshProUGUI text;
    public void OnClick()
    {
        option.Invoke();
    }
    public void Init(ContextMenu.Option option) => this.option = option;
    private void Start()
    {
        text.text = option.Summary;
    }

}
