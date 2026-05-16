using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectiblePickup : MonoBehaviour
{
    public int collectibleValue = 1;

    [Header("Sonido")]
    public AudioClip pickupSound;
    [Range(0f, 1f)]
    public float volume = 1f;

    private void Awake()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        AddCollectible();
        PlayPickupSound();
        DestroySelf();
    }

    public void AddCollectible()
    {
        GameManager.instance.AddCollectible(collectibleValue);
        LevelCompletionManager.instance?.ColeccionableRecogido();
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}