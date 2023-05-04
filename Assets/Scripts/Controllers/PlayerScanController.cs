using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScanController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private PlayerScore playerScore;
    [SerializeField] private ScanListController scanListController;

    [Header("Sounds")]
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    [Header("Conig")] [SerializeField] private float scanSoundDelay = 0.2f;
    
    public void Start()
    {
        if (scanListController == null)
        {
            scanListController = FindObjectOfType<ScanListController>();
        }
        
    }

    public bool Scan(GameObject item)
    {
        
        if (!scanListController.CanBeScanned(item.name))
        {
            var coroutine = DelayedSound(failSound);
            StartCoroutine(coroutine);
            return false;
        }
        
        var coroutine2 = DelayedSound(successSound);
        StartCoroutine(coroutine2);

        scanListController.CmdScanArticle(item.name, gameObject.name);
        playerScore.Increment(1);
        
        return true;
    }

    public IEnumerator DelayedSound(AudioClip clip)
    {
        yield return new WaitForSeconds(scanSoundDelay);
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

}