using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [Tooltip("Índice de esta zona (0 = tutorial, 1 = zona1, etc.)")]
    public int zoneIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Detecta cuando el XR Origin entra a la zona
        if (other.CompareTag("Player"))
        {
            DeathManager.instance.SetCurrentZone(zoneIndex);
            Debug.Log($"Zona actual: {zoneIndex}");
        }
    }
}