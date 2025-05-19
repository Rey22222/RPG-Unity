using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public bool IsDead { get; private set; } = false;
    [SerializeField] float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject restartMenu;

    Animator animator;
    private PlayerStatsController statsController;
    void Start()
    {
        animator = GetComponent<Animator>();
        statsController = FindObjectOfType<PlayerStatsController>();

        if (statsController != null)
        {
            health = statsController.GetCurrentHP();
        }
        else
        {
            Debug.LogWarning("HealthSystem: statsController или его данные не готовы!");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        animator.SetTrigger("damage");
        Debug.Log(health);

        if (statsController != null)
        {
            statsController.SetCurrentHP(health);
        }

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
    

}