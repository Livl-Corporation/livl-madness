using UnityEngine;

//public class StoreItemPrefabConfigurator : MonoBehaviour
//{
//    [SerializeField]
//    public string itemDisplayedName;

//    private void Awake()
//    {
//        ApplyScriptRecursively(transform);
//        gameObject.AddComponent<StoreItem>();

//        var storeItem = gameObject.GetComponent<StoreItem>();
//        storeItem.displayedName = itemDisplayedName;
//        this.tag = "Product";
//        this.gameObject.layer = LayerMask.NameToLayer("Shootable");
//        this.gameObject.AddComponent<MeshCollider>();
//    }

//    private void ApplyScriptRecursively(Transform parent)
//    {
//        foreach (Transform child in parent)
//        {
//            var component = child.gameObject.AddComponent<StoreItem>();

//            if (component is StoreItem myCustomScript)
//            {
//                myCustomScript.displayedName = this.itemDisplayedName;
//            }

//            child.tag = "Product";
//            child.gameObject.layer = LayerMask.NameToLayer("Shootable");

//            child.gameObject.AddComponent<MeshCollider>();


//            if (parent.childCount == 0) return;
//            ApplyScriptRecursively(child);
//        }

//    }
//}

public class StoreItemPrefabConfigurator : MonoBehaviour
{
    [SerializeField]
    public string itemDisplayedName;

    private void Awake()
    {
        ConfigurePrefab();
    }

    private void ConfigurePrefab()
    {
        AddComponentsAndProperties(transform);
    }

    private void AddComponentsAndProperties(Transform target)
    {
        var storeItem = target.gameObject.AddComponent<StoreItem>();
        storeItem.displayedName = itemDisplayedName;

        target.tag = "Product";
        target.gameObject.layer = LayerMask.NameToLayer("Shootable");
        target.gameObject.AddComponent<MeshCollider>();

        for (int i = 0; i < target.childCount; i++)
        {
            var child = target.GetChild(i);
            AddComponentsAndProperties(child);
        }
    }
}






