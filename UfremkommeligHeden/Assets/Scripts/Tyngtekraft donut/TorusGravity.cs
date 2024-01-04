using UnityEngine;

public class TorusGravity : MonoBehaviour
{
    public Transform playerTransform; // Spillerens transform komponent
    public float gravityStrength = 9.8f; // Tyngdekraftens styrke
    public float torusRadius = 10f; // Radius af torus-objektet
    public float coreRadius = 2f; // Radius af kernen i donutringen

    void Update()
    {
        // Beregn retningen fra spilleren til kernen af donutringen
        Vector3 corePosition = transform.position + transform.up * coreRadius;
        Vector3 gravityDirection = (corePosition - playerTransform.position).normalized;

        // Beregn afstanden fra spilleren til kernen af donutringen
        float distanceToCore = Vector3.Distance(playerTransform.position, corePosition);

        // Beregn tyngdekraftens p�virkning p� spilleren
        Vector3 gravityForce = gravityDirection * (gravityStrength / Mathf.Max(distanceToCore, 0.1f));

        // Anvend tyngdekraften p� spillerens transform komponent
        playerTransform.up = -gravityDirection; // S�rg for, at spillerens "up"-retning er vendt mod donutringens kerne

        // Anvend tyngdekraften for at bev�ge spilleren mod donutringens kerne
        playerTransform.position += gravityForce * Time.deltaTime;
    }
}
