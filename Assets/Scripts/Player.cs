using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{

    private AudioSource audioSource;
    [SerializeField] private float footStepRange = 4f;

    public static string LocalPlayerName = "";
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FootStepAudioSound()
    {
        if (isServerOnly)
            return;
        
        audioSource.volume = 0.1f;

        if (!isLocalPlayer)
        {
            // Get the distance between this player and the local player
            float distance = Vector3.Distance(transform.position, NetworkClient.connection.identity.gameObject.transform.position);

            // If the distance is within the range where other players can hear the footstep sound
            if (distance <= footStepRange)
            {
                // Calculate the volume based on the distance
                
                float volume = Mathf.InverseLerp(GetComponent<AudioSource>().minDistance, GetComponent<AudioSource>().maxDistance, distance);

                // Set the volume of the audio source
                audioSource.volume = volume;
                
                // Smooth the volume curve using an animation curve
                //AnimationCurve curve = new AnimationCurve(new Keyframe(0, volume), new Keyframe(1, 0));
                //audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
            } else
                return;
        } 
        
        audioSource.clip = GetComponent<PlayerController>().FootstepAudioClips[Random.Range(0, GetComponent<PlayerController>().FootstepAudioClips.Length)];
        audioSource.Play();
    }
    
}
