using System.Collections.Generic;
using UnityEngine;

public class YellingEnemyDamageSystem : MonoBehaviour
{
    [Header("Roar (Area Attack)")]
    [SerializeField] private float yellRadius = 8f;
    [SerializeField] private float yellDamage = 10f;
    [SerializeField] private Transform yellOrigin;

    private bool canSwipe = false;
    private bool hasSwiped = false;
    private bool isYelling = false;

    private int playerLayerMask;

    void Start()
    {
        playerLayerMask = 1 << 8;
    }

    void Update()
    {

        if (isYelling)
        {
            Collider[] hits = Physics.OverlapSphere(yellOrigin.position, yellRadius, playerLayerMask);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(yellDamage);
                }
            }

            isYelling = false;
        }
    }


    public void StartYell()
    {
        isYelling = true;
    }

    public void EndYell()
    {

    }

    private void OnDrawGizmos()
    {
        if (yellOrigin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(yellOrigin.position, yellRadius);
        }
    }
}


