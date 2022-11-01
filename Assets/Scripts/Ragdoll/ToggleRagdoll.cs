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
        SkinnedMeshRenderer[] animMesh;
        SkinnedMeshRenderer[] ragMesh;

        RagRoot ragRoot;
        Rigidbody parentBody;

        bool rag = false;

        // Start is called before the first frame update
        void Start()
        {
            animatedTransforms = animated.GetComponentsInChildren<Transform>();
            ragdollTransforms = ragdoll.GetComponentsInChildren<Transform>();
            animMesh = animated.GetComponentsInChildren<SkinnedMeshRenderer>();
            ragMesh = ragdoll.GetComponentsInChildren<SkinnedMeshRenderer>();
            ragRoot = ragdoll.GetComponentInChildren<RagRoot>();
            parentBody = transform.parent.GetComponentInChildren<Rigidbody>();

            foreach (SkinnedMeshRenderer mesh in ragMesh)
            {
                mesh.enabled = false;
            }
            foreach (SkinnedMeshRenderer mesh in animMesh)
            {
                mesh.enabled = true;
            }

            Events.EventsPublisher.Instance.SubscribeToEvent("ToggleRagdoll", Toggle);
            Events.EventsPublisher.Instance.SubscribeToEvent("TurnAnimated", TurnAnimated);
            Events.EventsPublisher.Instance.SubscribeToEvent("TurnRagdoll", TurnRagdoll);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ToggleRagdoll", Toggle);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("TurnAnimated", TurnAnimated);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("TurnRagdoll", TurnRagdoll);
        }

        void Toggle(object data, object sender)
        {
            if (rag)
            {
                Events.EventsPublisher.Instance.PublishEvent("TurnAnimated", null, null);
            }
            else
            {
                Events.EventsPublisher.Instance.PublishEvent("TurnRagdoll", null, null);
            }
        }

        void TurnRagdoll(object sender, object data)
        {
            rag = true;
            ragRoot.body.velocity = parentBody != null ? parentBody.velocity : new Vector3(0, 0, 0);

            for(int i = 0; i < animatedTransforms.Length; i++)
            {
                ragdollTransforms[i].position = animatedTransforms[i].position;
                ragdollTransforms[i].rotation = animatedTransforms[i].rotation;
            }

            foreach(SkinnedMeshRenderer mesh in ragMesh)
            {
                mesh.enabled = true;
            }
            foreach(SkinnedMeshRenderer mesh in animMesh)
            {
                mesh.enabled = false;
            }
        }

        void TurnAnimated(object sender, object data)
        {
            rag = false;

            foreach (SkinnedMeshRenderer mesh in ragMesh)
            {
                mesh.enabled = false;
            }
            foreach (SkinnedMeshRenderer mesh in animMesh)
            {
                mesh.enabled = true;
            }
        }
    }
}