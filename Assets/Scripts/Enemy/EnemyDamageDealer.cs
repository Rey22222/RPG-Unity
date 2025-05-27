using System.Collections;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    bool canDealDamage;
    bool hasDealtDamage;

    [SerializeField] float weaponLength = 1f;
    [SerializeField] float weaponDamage = 10f;

    [Header("Optional Default Effects")]
    [SerializeField] private ElementEffects defaultElementEffects; 

    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = false;
    }

    void Update()
    {
        if (canDealDamage && !hasDealtDamage)
        {
            RaycastHit hit;
            int layerMask = 1 << 8;

            Debug.DrawRay(transform.position, -transform.up * weaponLength, Color.red);

            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(weaponDamage);
                }

                ElementEffects targetEffects = hit.transform.GetComponent<ElementEffects>();

                if (targetEffects != null)
                {
                    targetEffects.ApplyElementEffect();
                }
                else if (defaultElementEffects != null)
                {
                    defaultElementEffects.ApplyElementEffect();
                }

                hasDealtDamage = true;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
