using UnityEngine;

public class SkyboxSkifter : MonoBehaviour
{
    public Material newSkybox; // Reference til den nye skybox, som du vil bruge i denne scene

    void Start()
    {
        RenderSettings.skybox = newSkybox; // Ændrer skyboxen til den nye, når scenen indlæses
    }
}
