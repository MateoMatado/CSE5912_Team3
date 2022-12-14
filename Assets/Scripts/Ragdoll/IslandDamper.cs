using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Ragdoll
{
    public class IslandDamper : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerBox")
            {
                other.GetComponentInParent<ActiveRagdoll>().Dampen();

                //other.transform.parent.parent.parent.parent.parent.GetComponentInChildren<ParticleSystem>().Stop();
                //other.transform.parent.parent.parent.parent.parent.GetComponentInChildren<ParticleSystem>().Pause();
                other.transform.parent.parent.parent.parent.parent.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
            }
        }
    }
}