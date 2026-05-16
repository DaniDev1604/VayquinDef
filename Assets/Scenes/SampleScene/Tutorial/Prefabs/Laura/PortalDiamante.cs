using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalDiamante : MonoBehaviour
{
    [Header("Configuración del Portal")]
    public string nombreEscenaDestino; // Ej: "Scenes/Nivel1"
    public bool usarIndice = false;
    public int indiceEscenaDestino;    // Alternativa: usar número 0,1,2,3

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si es la mano o el cuerpo del jugador XR
        if (other.CompareTag("Player") ||
            other.CompareTag("MainCamera") ||
            other.gameObject.layer == LayerMask.NameToLayer("XR"))
        {
            CargarSiguienteEscena();
        }
    }

    private void CargarSiguienteEscena()
    {
        if (usarIndice)
            SceneManager.LoadScene(indiceEscenaDestino);
        else
            SceneManager.LoadScene(nombreEscenaDestino);
    }
}
