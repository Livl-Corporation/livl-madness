using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScanController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private PlayerScore playerScore;
    [SerializeField] private ScanListController scanListController;
<<<<<<< HEAD:Assets/Scripts/Controllers/PlayerScanController.cs
=======

    [Header("Sounds")]
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    [Header("Conig")] [SerializeField] private float scanSoundDelay = 0.2f;
>>>>>>> b519dff (feat: add scan success & fail sounds):Assets/Scripts/Items/PlayerScanController.cs
    
    public void Start()
    {
        if (scanListController == null)
        {
            scanListController = FindObjectOfType<ScanListController>();
        }
<<<<<<< HEAD:Assets/Scripts/Controllers/PlayerScanController.cs

=======
        
>>>>>>> b519dff (feat: add scan success & fail sounds):Assets/Scripts/Items/PlayerScanController.cs
    }

    public bool Scan(GameObject item)
    {
        
        if (!scanListController.CanBeScanned(item.name))
        {
            var coroutine = DelayedSound(failSound);
            StartCoroutine(coroutine);
            return false;
        }
        
<<<<<<< HEAD:Assets/Scripts/Controllers/PlayerScanController.cs
        scanListController.CmdScanArticle(item.name, name);
=======
        var coroutine2 = DelayedSound(successSound);
        StartCoroutine(coroutine2);
>>>>>>> b519dff (feat: add scan success & fail sounds):Assets/Scripts/Items/PlayerScanController.cs
        
        scanListController.ScanArticle(item.name);
        playerScore.Increment(1);
        
        return true;
    }

    public IEnumerator DelayedSound(AudioClip clip)
    {
        yield return new WaitForSeconds(scanSoundDelay);
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

}
