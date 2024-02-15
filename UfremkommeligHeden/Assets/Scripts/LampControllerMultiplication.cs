using UnityEngine;

public class LampControllerMultiplication: MonoBehaviour
{
    public string mathGameObjectName; // navnet p� GameObjectet, der har MathGame scriptet
    private MathGame mathGame; // reference til MathGame scriptet
    public Material redMaterial; // materiale for r�d farve
    public Material greenMaterial; // materiale for gr�n farve

    private MeshRenderer meshRenderer; // mesh renderer af lampen

    void Start()
    {
        GameObject mathGameObject = GameObject.Find(mathGameObjectName); // find GameObjectet
        if (mathGameObject != null)
        {
            mathGame = mathGameObject.GetComponent<MathGame>(); // f� MathGame scriptet
        }

        meshRenderer = GetComponent<MeshRenderer>(); // f� mesh renderer komponenten
    }

    void Update()
    {
        // �ndre farven baseret p� v�rdien af bools i MathGame scriptet
        if (mathGame != null && mathGame.isMultiplicationEnabled)
        {
            meshRenderer.material = greenMaterial; // s�t farven til gr�n
        }
        else
        {
            meshRenderer.material = redMaterial; // s�t farven til r�d
        }
    }
}
