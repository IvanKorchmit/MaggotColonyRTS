using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenu;
    public void UpdateMasterVolume(System.Single v)
    {
        AudioListener.volume = v;
    }
    private void UpdateState()
    {
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        UpdateState();
    }
}
