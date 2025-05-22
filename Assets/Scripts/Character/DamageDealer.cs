using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    bool yellCombo;
    List<GameObject> hasDealtDamage;
    [SerializeField] float yellRadius = 10f;
    [SerializeField] Transform yellOrigin;
    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;

    void Start()
    {
        yellCombo = false;
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    void Update()
    {
        int layerMask = (1 << 9) | (1 << 10) | (1 << 11);
        if (canDealDamage)
        {
            RaycastHit hit;


            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                GameObject hitObject = hit.transform.gameObject;

                if (!hasDealtDamage.Contains(hitObject) && hitObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(weaponDamage);
                    hasDealtDamage.Add(hitObject);
                }
            }
        }
        if (yellCombo)
        {
            Collider[] hits = Physics.OverlapSphere(yellOrigin.position, yellRadius, layerMask);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(15);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }
    public void StartYell()
    {
        yellCombo = true;
    }
    public void EndYell()
    {
        yellCombo = false;
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
