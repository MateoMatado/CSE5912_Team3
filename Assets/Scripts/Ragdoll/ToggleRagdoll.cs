using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Ragdoll
{
    public class ToggleRagdoll : MonoBehaviour
    {
        [SerializeField] GameObject animated;
        [SerializeField] GameObject ragdoll;

        Transform[] animatedTransforms;
        Transform[] ragdollTransforms;

        // Start is called before the first frame update
        void Start()
        {
            animatedTransforms = animated.GetComponentsInChildren<Transform>();
            ragdollTransforms = ragdoll.GetComponentsInChildren<Transform>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}