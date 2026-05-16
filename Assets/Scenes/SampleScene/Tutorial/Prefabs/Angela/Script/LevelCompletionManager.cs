using UnityEngine;

public class LevelCompletionManager : MonoBehaviour
{
    public static LevelCompletionManager instance;

    [Header("Condiciones")]
    public int coleccionablesRequeridos = 3;
    public int enemigosRequeridos = 1;

    [Header("Referencias")]
    public GameObject canvasInstruccionesFinal;
    public GameObject portalDiamante;

    [Header("Sonido")]
    public AudioClip sonidoNivelCompletado;
    [Range(0f, 1f)]
    public float volumen = 1f;

    private int coleccionablesRecogidos = 0;
    private int enemigosMuertos = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (portalDiamante != null) portalDiamante.SetActive(false);
        if (canvasInstruccionesFinal != null) canvasInstruccionesFinal.SetActive(false);
    }

    public void ColeccionableRecogido()
    {
        coleccionablesRecogidos++;
        Debug.Log($"Coleccionables: {coleccionablesRecogidos}/{coleccionablesRequeridos}");
        VerificarCondiciones();
    }

    public void EnemigoMuerto()
    {
        enemigosMuertos++;
        Debug.Log($"Enemigos: {enemigosMuertos}/{enemigosRequeridos}");
        VerificarCondiciones();
    }

    void VerificarCondiciones()
    {
        if (coleccionablesRecogidos >= coleccionablesRequeridos &&
            enemigosMuertos >= enemigosRequeridos)
        {
            Debug.Log("✅ Nivel completado!");
            Completar();
        }
    }

    void Completar()
    {
        if (canvasInstruccionesFinal != null) canvasInstruccionesFinal.SetActive(true);
        if (portalDiamante != null) portalDiamante.SetActive(true);

        if (sonidoNivelCompletado != null)
            AudioSource.PlayClipAtPoint(sonidoNivelCompletado, Camera.main.transform.position, volumen);
    }

    public void Reset()
    {
        coleccionablesRecogidos = 0;
        enemigosMuertos = 0;
        if (canvasInstruccionesFinal != null) canvasInstruccionesFinal.SetActive(false);
        if (portalDiamante != null) portalDiamante.SetActive(false);
    }
}