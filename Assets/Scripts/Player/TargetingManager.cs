using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Team3.Player
{
    public class TargetingManager : MonoBehaviour
    {
        [SerializeField] private GameObject defaultCamera;
        [SerializeField] private GameObject targetingCamera;
        [SerializeField] private Transform enemyTarget;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float sphereRadius = 30;
        [SerializeField] private float tooClose = 5;
        private PlayerStateManager stateManager;
        private GameObject currentEnemy = null;
        private bool targeting = false;

        void Awake()
        {
            stateManager = GetComponent<PlayerStateManager>();

            Events.EventsPublisher.Instance.SubscribeToEvent("EnterTargetingState", StartTargeting);
            Events.EventsPublisher.Instance.SubscribeToEvent("ExitTargetingState", StopTargeting);
            Events.EventsPublisher.Instance.SubscribeToEvent("Target", HandleTargetEvent);
        }

        private void StartTargeting(object sender, object data)
        {
            targetingCamera.SetActive(true);
            defaultCamera.SetActive(false);
            targeting = true;
            StartCoroutine(Target());
        }

        private void StopTargeting(object sender, object data)
        {
            defaultCamera.SetActive(true);
            targetingCamera.SetActive(false);
            targeting = false;
        }

        private void HandleTargetEvent(object sender, object data)
        {
            if (targeting)
            {
                TargetClosestEnemy();
            }
            else
            {
                stateManager.StartTargeting();
            }
        }

        private IEnumerator Target()
        {
            currentEnemy = null;
            TargetClosestEnemy();
            while (targeting && currentEnemy != null)
            {
                if (Vector3.Distance(currentEnemy.transform.position, transform.position) > sphereRadius)
                {
                    stateManager.StopTargeting();
                    break;
                }
                enemyTarget.position = currentEnemy.transform.position;
                if (Vector3.Distance(enemyTarget.position, transform.position) < tooClose)
                {
                    Vector3 temp = transform.position + (enemyTarget.position - transform.position).normalized * tooClose;
                    temp.y = enemyTarget.position.y;
                    enemyTarget.position = temp;
                }
                transform.LookAt(enemyTarget, Vector3.up);
                Debug.DrawRay(transform.position, enemyTarget.position - transform.position, Color.blue);
                yield return null;
            }
        }

        private void TargetClosestEnemy()
        {
            var colliders = Physics.OverlapSphere(transform.position, sphereRadius, layerMask).ToList();
            colliders.Sort((a, b) => { return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)); });
            if (colliders.Count > 0)
            {
                if (colliders[0].gameObject == currentEnemy)
                {
                    if (colliders.Count > 1)
                    {
                        currentEnemy = colliders[1].gameObject;
                    }
                    else 
                    {
                        currentEnemy = null;
                    }
                }
                else 
                {
                    currentEnemy = colliders[0].gameObject;
                }
            }
            else
            {
                currentEnemy = null;
            }
            if (currentEnemy == null)
            {
                stateManager.StopTargeting();
            }
        }
    }
}
