using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Proyectil")]
    public GameObject shurikenPrefab;
    public float fuerzaLanzamiento = 20f;
    public float tiempoAutoDestruir = 5f; // por si no golpea nada

    [Header("Configuración")]
    public float alcance = 30f;

    private Transform puntoDisparo;

    void Start()
    {
        if (Camera.main != null)
        {
            puntoDisparo = Camera.main.transform;
            Debug.Log("✅ Punto de disparo: " + puntoDisparo.name);
        }
        else
        {
            Debug.LogError("❌ No se encontró Camera.main");
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            LanzarShuriken();
    }

    public void LanzarShuriken()
    {
        if (puntoDisparo == null || shurikenPrefab == null)
        {
            Debug.LogError("❌ Falta el punto de disparo o el prefab del shuriken");
            return;
        }

        Debug.Log("🌀 Lanzando shuriken!");

        // Instanciar un poco adelante de la cámara para que no choque con el player
        Vector3 spawnPos = puntoDisparo.position + puntoDisparo.forward * 0.5f;
        GameObject shuriken = Instantiate(shurikenPrefab, spawnPos, puntoDisparo.rotation);

        // Necesita Rigidbody para la física
        Rigidbody rb = shuriken.GetComponent<Rigidbody>();
        if (rb == null) rb = shuriken.AddComponent<Rigidbody>();

        rb.useGravity = false; // va recto, como shuriken
        rb.linearVelocity = puntoDisparo.forward * fuerzaLanzamiento;

        // Añadir el script de colisión al shuriken instanciado
        ShurikenProjectile sp = shuriken.GetComponent<ShurikenProjectile>();
        if (sp == null) sp = shuriken.AddComponent<ShurikenProjectile>();

        sp.tiempoAutoDestruir = tiempoAutoDestruir;

        Destroy(shuriken, tiempoAutoDestruir);
    }
}