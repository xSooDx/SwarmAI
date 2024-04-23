using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwarmUIControls : MonoBehaviour
{
    public static SwarmUIControls Instance { get; private set; }
    public SwarmManager SwarmManager;
    public SwarmSpawner SwarmSpawner;
    public FlowFieldGrid flowFieldGrid;
    public Transform playerSpawn;

    [Space()]
    public Slider cohesionSlider;
    public Slider avoidanceSlider;
    public Slider alignmentSlider;
    public Slider randomSlider;
    public Slider avoidanceRadSlider;
    public Slider flowSlider;
    public Slider minSpeedSlider;
    public Slider maxSpeedSlider;
    public TextMeshProUGUI spawnCount;
    public GameObject playerPrefab;

    public GameObject startButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cohesionSlider.value = SwarmManager.SettingsClone.cohesionWeight;
        avoidanceSlider.value = SwarmManager.SettingsClone.avoidanceRadius;
        alignmentSlider.value = SwarmManager.SettingsClone.alignmentWeight;
        randomSlider.value = SwarmManager.SettingsClone.randomWeight;
        avoidanceRadSlider.minValue = 0;
        avoidanceRadSlider.maxValue = SwarmManager.SettingsClone.senseRadius;
        avoidanceRadSlider.value = SwarmManager.SettingsClone.avoidanceRadius;
        flowSlider.value = SwarmManager.SettingsClone.flowWeight;
        minSpeedSlider.value = SwarmManager.SettingsClone.minSpeed;
        maxSpeedSlider.value = SwarmManager.SettingsClone.maxSpeed;
        SwarmSpawner.OnSpawn += SetSpawnCountText;
    }

    // Start is called before the first frame update
    public void UpdateCohesionWeight(float weight)
    {
        SwarmManager.SettingsClone.cohesionWeight = weight;
    }

    public void UpdateAvoidanceWeight(float weight)
    {
        SwarmManager.SettingsClone.avoidanceWeight = weight;
    }
    public void UpdateAlignmentWeight(float weight)
    {
        SwarmManager.SettingsClone.alignmentWeight = weight;
    }

    public void UpdateRandomWeight(float weight)
    {
        SwarmManager.SettingsClone.randomWeight = weight;
    }

    public void UpdateFlowWeight(float weight)
    {
        SwarmManager.SettingsClone.flowWeight = weight;
    }

    public void UpdateMinSpeed(float weight)
    {
        SwarmManager.SettingsClone.minSpeed = weight;
    }


    public void UpdateMaxSpeed(float weight)
    {
        SwarmManager.SettingsClone.maxSpeed = weight;
    }


    public void UpdateAvoidanceRadius(float rad)
    {
        SwarmManager.SettingsClone.avoidanceRadius = rad;
        SwarmManager.SettingsClone.OnValuesChanged();
    }

    public void Spawn(int count)
    {
        SwarmSpawner.SpawnEntities(count);
    }

    public void SetSpawnCountText(int count)
    {
        spawnCount.text = "Count: " + count;
    }

    public void ResetSim()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        playerSpawn.gameObject.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    internal void ShowStart()
    {
        startButton.gameObject.SetActive(true);
        playerSpawn.gameObject.SetActive(true);
    }
}
