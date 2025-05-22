using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float weaponDamage = 25f;
    [SerializeField] Transform attackOrigin;
    [SerializeField] LayerMask damageLayer;

    private bool canDealDamage = false;
    private bool hasDealtDamage = false;

    void Update()
    {
        if (canDealDamage && !hasDealtDamage)
        {
            Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRadius, damageLayer);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out HealthSystem health))
                {
                    Debug.Log("Hit: " + hit.name);
                    health.TakeDamage(weaponDamage);
                    hasDealtDamage = true;
                    break;
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage = false;
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        if (attackOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }
}
