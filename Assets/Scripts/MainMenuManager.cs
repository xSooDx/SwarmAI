using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public SwarmSpawner spawner;
    public Transform mouseTransform;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        spawner.SpawnEntities(300);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mp = cam.ScreenToWorldPoint(Input.mousePosition);
        mp.y = 0;
        mouseTransform.position = mp;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
