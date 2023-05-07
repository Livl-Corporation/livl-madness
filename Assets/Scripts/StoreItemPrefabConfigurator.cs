using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemPrefabConfigurator : MonoBehaviour
{
    public int itemId;
    public string itemDisplayedName;

    private void Start()
    {
        ApplyScriptRecursively(transform);
        gameObject.AddComponent<StoreItem>();

        var storeItem = gameObject.GetComponent<StoreItem>();
        storeItem.id = itemId;
        storeItem.displayedName = itemDisplayedName;
        this.tag = "Product";
        this.gameObject.layer = LayerMask.NameToLayer("Shootable");
        this.gameObject.AddComponent<MeshCollider>();
    }

    private void ApplyScriptRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            var component = child.gameObject.AddComponent<StoreItem>();

            if (component is StoreItem myCustomScript) // check if the added component is of the expected type
            {
                myCustomScript.id = this.itemId;
                myCustomScript.displayedName = this.itemDisplayedName;
            }

            child.tag = "Product";
            child.gameObject.layer = LayerMask.NameToLayer("Shootable");

            child.gameObject.AddComponent<MeshCollider>();
            

            ApplyScriptRecursively(child);
        }

        if (parent.childCount == 0) return;
    }
}
