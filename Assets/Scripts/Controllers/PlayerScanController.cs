using UnityEngine;

public class PlayerScanController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private PlayerScore playerScore;
    [SerializeField] private ScanListController scanListController;

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
            return false;
        }
        
        scanListController.ScanArticle(item.name);
        
        playerScore.Increment(1);
        
        return true;
    }

}
