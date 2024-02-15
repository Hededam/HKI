using UnityEngine;

public class LampControllerMultiplication: MonoBehaviour
{
    public string mathGameObjectName; // navnet på GameObjectet, der har MathGame scriptet
    private MathGame mathGame; // reference til MathGame scriptet
    public Material redMaterial; // materiale for rød farve
    public Material greenMaterial; // materiale for grøn farve

    private MeshRenderer meshRenderer; // mesh renderer af lampen

    void Start()
    {
        GameObject mathGameObject = GameObject.Find(mathGameObjectName); // find GameObjectet
        if (mathGameObject != null)
        {
            mathGame = mathGameObject.GetComponent<MathGame>(); // få MathGame scriptet
        }

        meshRenderer = GetComponent<MeshRenderer>(); // få mesh renderer komponenten
    }

    void Update()
    {
        // ændre farven baseret på værdien af bools i MathGame scriptet
        if (mathGame != null && mathGame.isMultiplicationEnabled)
        {
            meshRenderer.material = greenMaterial; // sæt farven til grøn
        }
        else
        {
            meshRenderer.material = redMaterial; // sæt farven til rød
        }
    }
}
