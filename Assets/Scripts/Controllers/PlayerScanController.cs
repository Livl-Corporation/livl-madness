using System.Collections;
using UnityEngine;

public class PlayerScanController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private ScanListController scanListController;
    [SerializeField] private PlayerStatsController playerStatsController;

    [Header("Sounds")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0, 1)] private float volume = 0.3f;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    [Header("Config")] [SerializeField] private float scanSoundDelay = 0.2f;
    
    public void Start()
    {
        if (scanListController == null)
        {
            scanListController = FindObjectOfType<ScanListController>();
        }
        
        if (playerStatsController == null)
        {
            playerStatsController = FindObjectOfType<PlayerStatsController>();
        }

    }

    public bool Scan(string itemName)
    {
        
        if (!scanListController.CanBeScanned(itemName))
        {
            var coroutine = DelayedSound(failSound);
            StartCoroutine(coroutine);
            return false;
        }
        
        var coroutine2 = DelayedSound(successSound);
        StartCoroutine(coroutine2);

        scanListController.CmdScanArticle(itemName, gameObject.name);
        playerStatsController.CmdIncrementScore(Player.LocalPlayerName);
        
        return true;
    }

    private IEnumerator DelayedSound(AudioClip clip)
    {
        yield return new WaitForSeconds(scanSoundDelay);
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }

}
