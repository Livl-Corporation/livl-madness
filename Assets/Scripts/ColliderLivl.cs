using Unity.VisualScripting;
using UnityEngine;

public class ColliderLivl : MonoBehaviour
{
    [Tooltip("The clip to play when entering the Livl")]
    [SerializeField] private AudioClip LivlClip;
    
    [Tooltip("The name of the layer for the local player")]
    [SerializeField] private string localPlayerLayerName = "LocalPlayer";
    
    private void OnTriggerEnter(Collider other)
    {
        // If the Local Player enters the trigger
        if (LayerMask.LayerToName(other.gameObject.layer) == localPlayerLayerName)
        {
            PlaySupermarketInteriorSound();
        }
    }
    
    private void PlaySupermarketInteriorSound()
    {
        GameObject parent = this.GameObject().transform.parent.gameObject;

        if (this.GameObject().tag == "EnterLivl")
        {
            parent.GetComponent<AudioSource>().clip = LivlClip;
            parent.GetComponent<AudioSource>().Play();
        }
        else if(this.GameObject().tag == "ExitLivl")
        {
            parent.GetComponent<AudioSource>().Stop();
        }
    }
}