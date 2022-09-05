using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMonoBehavior : MonoBehaviour
{
    private static GameObject dummyGameObject = null;

    private static DummyMonoBehavior dummy = null;

    public static DummyMonoBehavior Dummy
    {
        get
        {
            if (dummyGameObject == null)
            {
                dummyGameObject = new GameObject();
                dummy = dummyGameObject.AddComponent<DummyMonoBehavior>();
            }
            return dummy;
        }
    }
}
