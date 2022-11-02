using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Ragdoll
{
    public class ToggleRagdoll : MonoBehaviour
    {
        [SerializeField] GameObject animated;
        [SerializeField] GameObject ragdoll;
        [SerializeField] Animator anim;
        [SerializeField] int transSteps = 60;
        [SerializeField] float rotSpeed = 5;
        [SerializeField] Vector3 offset = new Vector3(0, 1.5f, 0);

        Transform[] animatedTransforms;
        Transform[] ragdollTransforms;
        SkinnedMeshRenderer[] animMesh;
        SkinnedMeshRenderer[] ragMesh;

        RagRoot ragRoot;
        Rigidbody parentBody;

        private static bool rag = false;
        public static bool isRag { get { return rag; } }

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

        private void LateUpdate() // Need to move the player box to the ragdoll for the camera movement
        {
            if (rag)
            {
                Transform tempParent = ragRoot.transform.parent;
                ragRoot.transform.parent = transform.parent.parent;
                transform.parent.position = ragRoot.transform.position + offset;
                ragRoot.transform.parent = tempParent;
            }
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
            if (rag)
            {
                return;
            }
            rag = true;

            ragRoot.rootBody.velocity = (parentBody != null) ? parentBody.velocity : new Vector3(0, 0, 0);

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
            if (!rag)
            {
                return;
            }
            EnAnimDisenRag();
            StartCoroutine(MoveRagToAnim());
        }

        IEnumerator MoveRagToAnim()
        {
            anim.enabled = false;

            Vector3[] posInc = new Vector3[ragdollTransforms.Length];
            Vector3[] posResult = new Vector3[ragdollTransforms.Length];
            Quaternion[] rotResult = new Quaternion[ragdollTransforms.Length];

            for (int i = 0; i < posResult.Length; i++)
            {
                posResult[i] = animatedTransforms[i].position;
                rotResult[i] = animatedTransforms[i].rotation;
            }

            for (int i = 0; i < posInc.Length; i++)
            {
                posInc[i] = animatedTransforms[i].position - ragdollTransforms[i].position;
            }

            for (int i = 0; i < animatedTransforms.Length; i++)
            {
                animatedTransforms[i].position = ragdollTransforms[i].position;
                animatedTransforms[i].rotation = ragdollTransforms[i].rotation;
            }
            for (int j = 0; j < transSteps; j++)
            {
                for (int i = 0; i < animatedTransforms.Length; i++)
                {
                    animatedTransforms[i].position = Vector3.MoveTowards(animatedTransforms[i].position, posResult[i], posInc[i].magnitude / transSteps);
                    animatedTransforms[i].rotation = Quaternion.RotateTowards(animatedTransforms[i].rotation, rotResult[i], rotSpeed);
                }
                yield return null;
            }

            rag = false;
            anim.enabled = true;
            yield return null;
        }

        void EnAnimDisenRag()
        {
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