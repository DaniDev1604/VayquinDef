using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Proyectil")]
    public GameObject shurikenPrefab;
    public float fuerzaLanzamiento = 20f;
    public float tiempoAutoDestruir = 5f;

    [Header("Sonido")]
    public AudioClip sonidoDisparo;
    private AudioSource audioSource;

    private Transform puntoDisparo;

    void Start()
    {
        if (Camera.main != null)
            puntoDisparo = Camera.main.transform;
        else
            Debug.LogError("❌ No se encontró Camera.main");

        // Añadir AudioSource automáticamente
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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
            Debug.LogError("❌ Falta cámara o prefab");
            return;
        }

        // Reproducir sonido
        if (sonidoDisparo != null)
            audioSource.PlayOneShot(sonidoDisparo);

        Vector3 spawnPos = puntoDisparo.position + puntoDisparo.forward * 0.5f;
        GameObject shuriken = Instantiate(shurikenPrefab, spawnPos, puntoDisparo.rotation);

        Rigidbody rb = shuriken.GetComponent<Rigidbody>();
        if (rb == null)
            rb = shuriken.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.linearVelocity = puntoDisparo.forward * fuerzaLanzamiento;

        ShurikenProjectile sp = shuriken.GetComponent<ShurikenProjectile>();
        if (sp != null)
            sp.tiempoAutoDestruir = tiempoAutoDestruir;
    }
}