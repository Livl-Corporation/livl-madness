using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{

    [Tooltip("The weapon used by the player")]
    public PlayerWeapon weapon;

    [SerializeField]
    [Tooltip("The camera used to shoot")]
    private GameObject cam;

    [SerializeField]
    [Tooltip("The layer on which the raycast will be casted")]
    private LayerMask mask;
    
    [Header("Scanner Sound")]
    [SerializeField] private AudioClip ScanningAudioClip;
    [SerializeField] private AudioSource ScanningAudioSource;
    
    private PlayerScanController playerScanController;
    private InputManager inputManager;
    
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Aucune caméra associé au système de tir.");
            this.enabled = false;
        }
        
        playerScanController = GetComponent<PlayerScanController>();
        inputManager = GetComponent<InputManager>();
        
        ScanningAudioSource.volume = 1f;
        ScanningAudioSource.clip = ScanningAudioClip;
        
    }

    private void Update()
    {
        if (PlayerUI.isPaused)
            return;

        if (Input.GetButtonDown("Fire1") && inputManager.Aiming) // if we click on the left mouse button
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, weapon.range, mask)) return;
        
        if(!hit.collider.CompareTag("Product"))
        {
            Debug.Log("This is not a scannable product");
            return;
        }
        
        var hitName = hit.collider.name;

        StoreItem storeItem = hit.collider.gameObject.GetComponent<StoreItem>();
        Debug.Log("You hitted " + storeItem.displayedName);
        ScanningAudioSource.Play();

        var isScanValid = playerScanController.Scan(storeItem.displayedName);

        if(!isScanValid)

        {
            Debug.Log("This product is not in your scan list !");
        }
    }
    
    
    /**
     * TODO : Make the sound be also heard to all clients near the scanned article
    [Command]
    private void CmdPlayScanningAudio(Vector3 position)
    {
        RpcPlayScanningAudio(position);
    }
    
    [ClientRpc]
    private void RpcPlayScanningAudio(Vector3 position)
    {
        // Create an audio source object at the specified position
        GameObject audioSourceObject = new GameObject("AudioSource");
        audioSourceObject.transform.position = position;

        // Add an AudioSource component to the object and configure it
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 0f;
        audioSource.maxDistance = 5f;
        audioSource.clip = ScanningAudioClip;
        audioSource.Play();

        // Destroy the audio source object after the clip has finished playing
        Destroy(audioSourceObject, ScanningAudioClip.length);
    }
    **/
}
