using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemySpawner : LivingEntity
{
    [SerializeField] private GhoulTemp ghoulPrefab;
    public bool IsTriggerOn
    { get; set; }

    private List<GhoulTemp> ghoulsList = new List<GhoulTemp>();

    private const float radius = 8f;
    private const int numberOfSpawnAtOnce = 10; 

    private const float nextSpawnTime = 5f;
    private float spawnTimer = 0;

    private const float spawnDelayTime = 0.1f;

    private int totalEnemyCount;
    private const int maxEnemyCount = 100;



    private void Start()
    {
        IsTriggerOn = false;
    }


    
    private void Update()
    {
        if (IsTriggerOn)
        {
            totalEnemyCount = ghoulsList.Count;
            spawnTimer -= Time.deltaTime;

            if (spawnTimer < 0 && totalEnemyCount < maxEnemyCount)
            {
                Spawn();
                spawnTimer = nextSpawnTime;
            }
        }       
    }
    

    //Enemies will spawn around the beacon making a circle
    public void Spawn()
    {
        Vector3[] spawnPosition = new Vector3[numberOfSpawnAtOnce];
        Quaternion[] rot = new Quaternion[numberOfSpawnAtOnce];
        for (int i = 0; i < numberOfSpawnAtOnce; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSpawnAtOnce;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            spawnPosition[i] = transform.position + new Vector3(x, 0, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            rot[i] = Quaternion.Euler(0, angleDegrees, 0);
        }

         StartCoroutine(CreateEnemy(spawnPosition, rot));
    }


    IEnumerator CreateEnemy(Vector3[] spawnPosition, Quaternion[] rot)
    {       
        for (int i = 0; i < numberOfSpawnAtOnce; i++)
        {
            yield return new WaitForSeconds(spawnDelayTime);
            GhoulTemp ghoul = Instantiate(ghoulPrefab, spawnPosition[i], rot[i]);
            ghoulsList.Add(ghoul);

            ghoul.onDeath += () => ghoulsList.Remove(ghoul);
            ghoul.onDeath += () => Destroy(ghoul.gameObject, 10f);
        }
    }




    public override void OnDamage(float damage)
    {
        if (!isDead)
        {
            //hitEffect2.Play();
            //ghoulAudioPlayer.clip = onDamageSound;
            //ghoulAudioPlayer.PlayOneShot(onDamageSound);
        }

        //affect damage on hp
        base.OnDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        //ghoulAnimator.SetTrigger("Die");
        //ghoulAudioPlayer.clip = deathSound;
        //ghoulAudioPlayer.PlayOneShot(deathSound);

        Collider BeaconCollider = GetComponent<Collider>();
        BeaconCollider.enabled = false;
        Destroy(gameObject);
    }
}