using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float damage = 5f;
    [SerializeField] float lifetime = 5f;

    private Vector3 direction;
    private GameObject activeEffectInstance;
    private ElementType elementType;
   

    public void Initialize(Vector3 dir, ElementType element, GameObject effectPrefab)
    {
        direction = dir;
        elementType = element;

        if (effectPrefab != null)
        {
            activeEffectInstance = Instantiate(effectPrefab, transform);
            activeEffectInstance.transform.localPosition = Vector3.zero;

            ParticleSystem ps = activeEffectInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                
            }
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out HealthSystem health))
            {
                health.TakeDamage(damage);
            }

            if (other.TryGetComponent(out ElementEffects playerElementEffects))
            {
                playerElementEffects.currentElement = elementType;
                playerElementEffects.ApplyElementEffect();
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
