using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;

    [Header("Configuración")]
    public float distanciaDeteccion = 8f;
    public float distanciaAtaque = 1.8f;
    public float tiempoEntreAtaques = 1.5f;
    public int dañoPorAtaque = 10;
    public int vidaMaxima = 100;

    private NavMeshAgent agente;
    private Animator animator;
    private float timerAtaque = 0f;
    private int vidaActual;
    private bool estaMuerto = false;

    private enum Estado { Idle, Persiguiendo, Atacando, Muerto }
    private Estado estadoActual = Estado.Idle;

    // Parámetros exactos de tu AnimatorController
    private const string PARAM_IS_WALKING = "isWalking"; // Bool
    private const string TRIGGER_ATTACK   = "Attack";    // Trigger → Mutant Jump Attack
    private const string TRIGGER_DEATH    = "Death";     // Trigger → Standing Death Forward 01

    void Start()
    {
        agente     = GetComponent<NavMeshAgent>();
        animator   = GetComponent<Animator>();
        vidaActual = vidaMaxima;

        // Si el Animator está en un hijo (el mesh)
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogError("❌ No se encontró Animator en " + gameObject.name);
        else if (animator.runtimeAnimatorController == null)
            Debug.LogError("❌ El Animator no tiene AnimatorController en " + gameObject.name);
        else
            Debug.Log("✅ Animator listo");

        if (jugador == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                jugador = cam.transform;
                Debug.Log("✅ Jugador asignado a: " + jugador.name);
            }
            else
            {
                Debug.LogError("❌ No se encontró Camera.main");
            }
        }
    }

    void Update()
    {
        if (estaMuerto || jugador == null || agente == null) return;

        Vector3 posEnemigo = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posJugador = new Vector3(jugador.position.x, 0, jugador.position.z);
        float distancia = Vector3.Distance(posEnemigo, posJugador);

        timerAtaque += Time.deltaTime;

        switch (estadoActual)
        {
            case Estado.Idle:
                agente.ResetPath();
                SetBool(PARAM_IS_WALKING, false);

                if (distancia <= distanciaDeteccion)
                {
                    estadoActual = Estado.Persiguiendo;
                    Debug.Log("👁 Enemigo detectó al jugador");
                }
                break;

            case Estado.Persiguiendo:
                Vector3 destino = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
                agente.SetDestination(destino);
                SetBool(PARAM_IS_WALKING, true);

                if (distancia <= distanciaAtaque)
                {
                    agente.ResetPath();
                    estadoActual = Estado.Atacando;
                }
                else if (distancia > distanciaDeteccion * 1.5f)
                {
                    estadoActual = Estado.Idle;
                }
                break;

            case Estado.Atacando:
                agente.ResetPath();
                SetBool(PARAM_IS_WALKING, false);

                Vector3 dir = (jugador.position - transform.position).normalized;
                dir.y = 0;
                if (dir != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(dir), Time.deltaTime * 5f);

                if (timerAtaque >= tiempoEntreAtaques)
                {
                    Atacar();
                    timerAtaque = 0f;
                }

                if (distancia > distanciaAtaque)
                    estadoActual = Estado.Persiguiendo;
                break;
        }
    }

    void Atacar()
    {
        Debug.Log("⚔️ Enemigo ataca");
        SetTrigger(TRIGGER_ATTACK);

        if (GameManager.instance != null)
            GameManager.instance.TakeDamage(dañoPorAtaque);
    }

    // Llama este método cuando el enemigo recibe daño
    public void RecibirDaño(int daño)
    {
        if (estaMuerto) return;

        vidaActual -= daño;
        Debug.Log($"💢 Enemigo recibió {daño} de daño. Vida: {vidaActual}/{vidaMaxima}");

        if (vidaActual <= 0)
            Morir();
    }

    void Morir()
    {
        estaMuerto   = true;
        estadoActual = Estado.Muerto;
        Debug.Log("💀 Enemigo muerto");

        SetTrigger(TRIGGER_DEATH);

        if (agente != null)
        {
            agente.ResetPath();
            agente.enabled = false;
        }

        // Destruye el GameObject al terminar la animación de muerte
        Destroy(gameObject, 3f);
    }

    // Helpers con null check incorporado
    void SetBool(string param, bool valor)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool(param, valor);
    }

    void SetTrigger(string param)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetTrigger(param);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}