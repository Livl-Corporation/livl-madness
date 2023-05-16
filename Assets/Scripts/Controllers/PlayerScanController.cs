using System.Collections;
using UnityEngine;

public class PlayerScanController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private ScanListController scanListController;
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private ProductsController productsController;
    [SerializeField] private ChatBehaviour chatBehaviour;

    [Header("Sounds")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0, 1)] private float volume = 0.3f;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    [Header("Config")] [SerializeField] private float scanSoundDelay = 0.1f;
    
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
        
        if (productsController == null)
        {
            productsController = FindObjectOfType<ProductsController>();
        }
        
        if (chatBehaviour == null)
        {
            chatBehaviour = FindObjectOfType<ChatBehaviour>();
        }

    }

    public bool Scan(string itemName)
    {
        
        if (!scanListController.CanBeScanned(itemName))
        {
            var coroutine = DelayedSound(failSound);
            StartCoroutine(coroutine);
            playerStatsController.CmdDecrementScore(Player.LocalPlayerName);
            chatBehaviour.CmdSendSystemMessage($"<color=red>{Player.LocalPlayerName} s'est trompé en scannant {itemName} !</color>");
            return false;
        }
        
        var coroutine2 = DelayedSound(successSound);
        StartCoroutine(coroutine2);

        scanListController.CmdScanArticle(itemName, gameObject.name);
        playerStatsController.CmdIncrementScore(Player.LocalPlayerName);
        productsController.CmdSetOutOfStock(itemName);
        chatBehaviour.CmdSendSystemMessage($"<color=green>{Player.LocalPlayerName} a scanné {itemName} !</color>");
        
        return true;
    }

    private IEnumerator DelayedSound(AudioClip clip)
    {
        yield return new WaitForSeconds(scanSoundDelay);
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }

}
