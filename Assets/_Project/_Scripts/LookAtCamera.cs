using UnityEngine;
using UnityEngine.SceneManagement;

public class LookAtCamera : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        cam = Camera.main;
    }



    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,cam.transform.rotation * Vector3.up);
    }
}
