using UnityEngine;
using UnityEngine.UI;

public class SwarmUIControls : MonoBehaviour
{
    public SwarmManager SwarmManager;
    public Slider cohesionSlider;
    public Slider avoidanceSlider;
    public Slider alignmentSlider;
    public Slider randomSlider;
    public Slider avoidanceRadSlider;


    private void Start()
    {
        cohesionSlider.value = SwarmManager.SettingsClone.cohesionWeight;
        avoidanceSlider.value = SwarmManager.SettingsClone.avoidanceRadius;
        alignmentSlider.value = SwarmManager.SettingsClone.alignmentWeight;
        randomSlider.value = SwarmManager.SettingsClone.randomWeight;
        avoidanceRadSlider.minValue = 0;
        avoidanceRadSlider.maxValue = SwarmManager.SettingsClone.senseRadius;
        avoidanceRadSlider.value = SwarmManager.SettingsClone.avoidanceRadius;
    }

    // Start is called before the first frame update
    public void UpdateCohesionWeight(float weight)
    {
        SwarmManager.SettingsClone.cohesionWeight = weight;
    }

    public void UpdateAvoidanceWeignt(float weight)
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

    public void UpdateAvoidanceRadius(float rad)
    {
        SwarmManager.SettingsClone.avoidanceRadius = rad;
        SwarmManager.SettingsClone.OnValuesChanged();
    }
}
