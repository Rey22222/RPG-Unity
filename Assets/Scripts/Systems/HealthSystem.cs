using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public bool IsDead { get; private set; } = false;
    [SerializeField] float health = 200;
    [SerializeField] GameObject restartMenu;
    public TMP_Text healthText;

    [SerializeField] GameObject hitVFX; 
    [SerializeField] PlayerStatsController statsController; 

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (healthText != null)
            healthText.text = $"Health: {health}";
    }

    public void TakeDamage(float damageAmount)
    {
        if (IsDead) return;

        Debug.Log($"[HealthSystem] Taking damage: {damageAmount}. Current HP before: {health}");

        health -= damageAmount;

        if (statsController != null)
        {
            statsController.SetCurrentHP(health);
        }

        animator.SetTrigger("damage");

        Debug.Log($"[HealthSystem] HP after damage: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    void ShowRestartMenu()
    {
        restartMenu.SetActive(true);
    }

    void Die()
    {
        IsDead = true;
        animator.SetTrigger("die");
        gameObject.tag = "Untagged";
        Invoke("ShowRestartMenu", 3f);
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }

    public void RestoreHealth(float amount)
    {
        Debug.Log($"[HealthSystem] Restoring health: {amount}. Current HP before: {health}");

        health = Mathf.Min(200, health + amount);  

        Debug.Log($"[HealthSystem] HP after restore: {health}");
    }
}
