using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class ContextMenu
{
    private List<Option> options;
    public void Add(string text, Option.OptionDelegate action)
    {
        if (options == null)
        {
            options = new List<Option>();
        }
        options.Add(new Option(text, action));
    }
    
    public bool Invoke(int index)
    {
        return options[index].Invoke();
    }


    public class Option
    {
        public delegate bool OptionDelegate();
        private OptionDelegate action;
        private string summary;
        public bool Invoke()
        {
            return action();
        }
        public Option(string summary, OptionDelegate action)
        {
            this.summary = summary;
            this.action = action;
        }
        public string Summary => summary;
    }

} 
