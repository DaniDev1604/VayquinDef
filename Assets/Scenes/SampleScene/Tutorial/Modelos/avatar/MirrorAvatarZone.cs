using UnityEngine;

public class MirrorAvatarZone : MonoBehaviour
{
    public GameObject fullAvatar;

    public GameObject leftHandVR;
    public GameObject rightHandVR;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entró: " + other.name);

        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Es la cámara");

            fullAvatar.SetActive(true);

            leftHandVR.SetActive(false);
            rightHandVR.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            fullAvatar.SetActive(false);

            leftHandVR.SetActive(true);
            rightHandVR.SetActive(true);
        }
    }
}