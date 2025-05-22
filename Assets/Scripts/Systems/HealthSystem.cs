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
        health -= damageAmount;
        animator.SetTrigger("damage");
<<<<<<< Updated upstream
        Debug.Log(health);
=======

        if (statsController != null)
        {
            statsController.SetCurrentHP(health);
        }

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);

    }
=======
    

>>>>>>> Stashed changes
}