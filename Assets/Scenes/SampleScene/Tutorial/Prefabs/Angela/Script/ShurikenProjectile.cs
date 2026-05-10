using UnityEngine;

public class ShurikenProjectile : MonoBehaviour
{
    public float tiempoAutoDestruir = 5f;
    public int daño = 1;
    public float velocidadRotacion = 720f;

    [Header("Trail")]
    public float trailTime = 0.3f;
    public float trailStartWidth = 0.05f;
    public float trailEndWidth = 0f;
    public Color trailColor = new Color(0.5f, 0.8f, 1f, 1f);

    void Start()
    {
        TrailRenderer trail = gameObject.AddComponent<TrailRenderer>();
        trail.time = trailTime;
        trail.startWidth = trailStartWidth;
        trail.endWidth = trailEndWidth;
        trail.material = new Material(Shader.Find("Sprites/Default"));

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(trailColor, 0f),
                new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        trail.colorGradient = gradient;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, velocidadRotacion * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Shuriken tocó: {other.name} | Tag: {other.tag}");

        // Si es enemigo, hacerle daño y destruirse
        if (other.CompareTag("Enemigo"))
        {
            EnemyHealth enemigo = other.GetComponentInParent<EnemyHealth>();
            if (enemigo != null)
            {
                Debug.Log("🎯 Golpeó enemigo: " + other.name);
                enemigo.RecibirDaño(daño);
            }
            Destroy(gameObject);
            return;
        }

        // Si es trigger (como limite_general) lo ignora y sigue volando
        if (other.isTrigger) return;

        // Si golpea el player lo ignora
        if (other.CompareTag("Player")) return;

        // Cualquier otra cosa sólida lo destruye
        Debug.Log("💥 Golpeó: " + other.name);
        Destroy(gameObject);
    }
}