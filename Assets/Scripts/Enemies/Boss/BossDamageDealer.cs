using System.Collections.Generic;
using UnityEngine;

public class BossDamageDealer : MonoBehaviour
{
    [Header("Swipe (Close Attack)")]
    [SerializeField] private float swipeRange = 2f;
    [SerializeField] private float swipeDamage = 20f;

    [Header("Roar (Area Attack)")]
    [SerializeField] private float roarRadius = 8f;
    [SerializeField] private float roarDamage = 10f;
    [SerializeField] private Transform roarOrigin;

    private bool canSwipe = false;
    private bool hasSwiped = false;
    private bool isRoaring = false;

    private int playerLayerMask;

    [SerializeField] private ElementEffects elementEffects;
    public ElementType currentElement;

    void Start()
    {
        playerLayerMask = 1 << 8;
    }

    void Update()
    {
        if (canSwipe && !hasSwiped)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, -transform.up * swipeRange, Color.red);

            if (Physics.Raycast(transform.position, -transform.up, out hit, swipeRange, playerLayerMask))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(swipeDamage);
                    hasSwiped = true;
                }
            }
        }

        if (isRoaring)
        {
            Collider[] hits = Physics.OverlapSphere(roarOrigin.position, roarRadius, playerLayerMask);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(roarDamage);
                }
            }

            isRoaring = false;
        }
    }

    public void StartSwiping()
    {
        canSwipe = true;
        hasSwiped = false;

        elementEffects.currentElement = currentElement;
        elementEffects.PlayElementEffect("Swipe");

    }

    public void EndSwiping()
    {
        canSwipe = false;
        elementEffects.DisableAllEffects();
    }

    public void StartRoar()
    {
        isRoaring = true;
        elementEffects.currentElement = currentElement;
        elementEffects.PlayElementEffect("Roar");

    }

    public void EndRoar()
    {
        elementEffects.DisableAllEffects();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * swipeRange);

        if (roarOrigin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(roarOrigin.position, roarRadius);
        }
    }

    public void SetElement(ElementType element)
    {
        currentElement = element;
    }
}