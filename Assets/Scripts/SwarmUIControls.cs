using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwarmUIControls : MonoBehaviour
{
    public SwarmManager SwarmManager;
    public SwarmSpawner SwarmSpawner;
    public Slider cohesionSlider;
    public Slider avoidanceSlider;
    public Slider alignmentSlider;
    public Slider randomSlider;
    public Slider avoidanceRadSlider;
    public Slider flowSlider;
    public TextMeshProUGUI spawnCount;
    public FlowFieldGrid flowFieldGrid;


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
        SwarmSpawner.OnSpawn += SetSpawnCountText;

        StartCoroutine(UpdateFlowField());
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

    IEnumerator UpdateFlowField()
    {
        //Thread t = null;
        while (true)
        {
            //t?.Join();
            yield return new WaitForSeconds(0.1f);
            //t = new Thread(() => { flowFieldGrid.CalculateFlowField(); });
            //t.Start();
            flowFieldGrid.CalculateFlowField();
        }
    }
}
