using UnityEngine;

public class SkyboxSkifter : MonoBehaviour
{
    public Material newSkybox; // Reference til den nye skybox, som du vil bruge i denne scene

    void Start()
    {
        RenderSettings.skybox = newSkybox; // �ndrer skyboxen til den nye, n�r scenen indl�ses
    }
}
