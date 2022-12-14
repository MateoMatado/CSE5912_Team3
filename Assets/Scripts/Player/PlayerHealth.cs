using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Team3.Events;
using Team3.Scripts.Player;
using Cinemachine;

public class PlayerHealth : LivingEntity
{
    public static PlayerHealth Instance = null;
    private static float ChangeValueHP = 0;
    [SerializeField] private float ChangeSpeed = 0.2f;
    public GameObject DieScene;

    private bool neverDie = false;

    //OnEnable is for when player revives  
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    
    private void Awake()
    {
        Instance = this;
        // currentHealth = 1000;
        Instance.currentHealth = HUDManager.Instance.hp;
        Instance.startingHealth = HUDManager.Instance.hp;
        StartCoroutine(EaseDamageMaterial());

    }
    private void Start()
    {
        EventsPublisher.Instance.SubscribeToEvent("HealPlayer", HandleHeal);
        EventsPublisher.Instance.SubscribeToEvent("LavaDamagePlayer", HandleLavaDamage);
        EventsPublisher.Instance.SubscribeToEvent("DamageEnemy", HandleDamage);
        EventsPublisher.Instance.SubscribeToEvent("NeverDie", ToggleNeverDie);
    }

    void OnDestroy()
    {
        EventsPublisher.Instance.UnsubscribeToEvent("HealPlayer", HandleHeal);        
        EventsPublisher.Instance.UnsubscribeToEvent("LavaDamagePlayer", HandleLavaDamage);        
        EventsPublisher.Instance.UnsubscribeToEvent("DamageEnemy", HandleDamage);
        EventsPublisher.Instance.UnsubscribeToEvent("NeverDie", ToggleNeverDie);
    }


    private void HandleHeal(object sender, object data)
    {
        float amount = (float)data;
        currentHealth = Math.Clamp(currentHealth + amount, 0, startingHealth);
    }

    private void HandleDamage(object sender, object data)
    {
        GetComponent<CinemachineImpulseSource>()?.GenerateImpulse();
    }

    private void HandleLavaDamage(object sender, object data)
    {
        float amount = (float)data;
        OnDamage(amount);
    }


    private IEnumerator EaseDamageMaterial()
    {
        float pct = Math.Clamp(currentHealth / startingHealth, 0, 1);
        float lerpSpeed = 1f;
        while (!isDead)
        {
            try
            {
                Renderer[] renderers = gameObject.GetComponent<PlayerSwap>().current.GetComponentsInChildren<Renderer>();
                
                float healthPct = Math.Clamp(currentHealth / startingHealth, 0, 1);
                pct += (healthPct - pct) * lerpSpeed * Time.deltaTime;
                //Debug.Log(gameObject.name + ' ' + pct);
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.SetFloat("_Health", pct);
                }
            }
            catch
            {
                
            }
            yield return null;
        }
    }

    private void Update()
    {
        /*for health potions*/
        if (ChangeValueHP != 0)
        {
            if (ChangeValueHP > 0)
            {
                Instance.currentHealth += ChangeSpeed;
                ChangeValueHP -= ChangeSpeed;

            }
            else
            {
                Instance.currentHealth -= ChangeSpeed;
                ChangeValueHP += ChangeSpeed;
            }
        }
        //Debug.Log("PlayerHealth.cs : " + Instance.currentHealth);
    }

    public override void OnDamage(float damage)
    {
        if (!isDead)
        {
            //on damage sound effect code here
        }

        //base.OnDamage(damage);
        //OnDamage(damage);
        Instance.currentHealth -= damage;
        GetComponent<CinemachineImpulseSource>()?.GenerateImpulse();

        if (Instance.currentHealth <= 0 && !isDead)
        {
            if (neverDie)
            {
                Instance.currentHealth = startingHealth;
            }
            else
            {
                Die();
            }
        }

        Debug.Log("PLAYER HP:" + Instance.currentHealth);
        //change in HP UI code here
        //playerStatus.HealthChange(-damage);
        //slider.value = currentHealth;
    }

    public override void Die()
    {
        base.Die();

        //death sound and animation here

        //disable player movement here
        StartCoroutine(DieSceneAction());
    }

    private IEnumerator DieSceneAction()
    {
        HUDManager.Instance.Die();
        yield return new WaitForSeconds(3f);
        GameStateMachine.Instance.SwitchState(GameStateMachine.MainMenuState);

    }
    public float GetHP()
    {
        return Instance.currentHealth;
    }


    public void HealthChange(float amount)
    {
        ChangeValueHP += amount;
    }

    private void ToggleNeverDie(object sender, object data)
    {
        if(neverDie)
        {
            neverDie = false;
        }
        else
        {
            neverDie = true;
        }
    }
}
