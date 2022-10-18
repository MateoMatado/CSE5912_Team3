using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAllChildrenLayer : MonoBehaviour
{
    [SerializeField] private int layer;
    private void Awake()
    {
        ChildLayerSet(this.transform, layer);
    }
    void ChildLayerSet(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //            Debug.Log(child.name);
            child.gameObject.layer = layer;
        }
    }
}
