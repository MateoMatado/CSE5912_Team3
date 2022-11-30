using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using static UnityEngine.Rendering.DebugUI.Table;
using Team3.Events;

public class EnemySpawner : LivingEntity
{
    //[SerializeField] private GhoulTemp ghoulPrefab;
    [SerializeField] private LivingEntity enemyPrefab;
    public bool IsTriggerOn
    { get; set; }
    [SerializeField] private int numberOfSpawnAtOnce = 6;
    [SerializeField] private int maxEnemyCount = 20;

    //private List<GhoulTemp> ghoulsList = new List<GhoulTemp>();
    private List<LivingEntity> ghoulsList = new List<LivingEntity>();
    private const float radius = 12f;
    

    private const float nextSpawnTime = 5f;
    private float spawnTimer = 0;

    private const float spawnDelayTime = 0.1f;

    private int totalEnemyCount;
    



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
            //Instantiate enemies as child of this

            //var originalScale = enemyPrefab.transform.localScale;
            //LivingEntity ghoul = Instantiate(enemyPrefab, spawnPosition[i], rot[i], transform);
            //ghoul.transform.localScale /= 4;

            LivingEntity ghoul = Instantiate(enemyPrefab, spawnPosition[i], rot[i]);
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

    private bool dying = false;
    public override void Die()
    {
        if (dying) return;
        base.Die();
        //ghoulAnimator.SetTrigger("Die");
        //ghoulAudioPlayer.clip = deathSound;
        //ghoulAudioPlayer.PlayOneShot(deathSound);

        Collider BeaconCollider = GetComponent<Collider>();
        BeaconCollider.enabled = false;
        dying = true;
        EventsPublisher.Instance.PublishEvent("SinkSpawner", null, gameObject);
        GetComponent<AudioSource>().Play();
        var ps = transform.Find("ExplosionEffect").GetComponentsInChildren<ParticleSystem>();
        foreach (var p in ps)
        {
            p.Play();
        }
        StartCoroutine(Dying());
    }

    private IEnumerator Dying()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}