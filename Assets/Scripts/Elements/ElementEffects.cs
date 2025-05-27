using UnityEngine;

public class ElementEffects : MonoBehaviour
{
    public ElementType currentElement;

    [Header("Particles")]
    public ParticleSystem iceEffect;
    public ParticleSystem fireEffect;
    public ParticleSystem earthEffect;
    public ParticleSystem windEffect;

    [Header("Effect References")]
    
    public CameraShakeTrigger cameraShake;
    public Character playerMovement;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip iceSound;


    public void ApplyElementEffect()
    {
        DisableAllEffects();

        switch (currentElement)
        {
            case ElementType.Ice:
                if (iceEffect) iceEffect.Play();
                ApplyIceEffect();
                break;
            case ElementType.Fire:
                if (fireEffect) fireEffect.Play();
                ApplyFireEffect();
                break;
            case ElementType.Earth:
                if (earthEffect) earthEffect.Play();
                ApplyEarthEffect();
                break;
            case ElementType.Wind:
                if (windEffect) windEffect.Play();
                ApplyWindEffect();
                break;
        }
    }

    public void DisableAllEffects()
    {
        if (iceEffect) iceEffect.Stop();
        if (fireEffect) fireEffect.Stop();
        if (earthEffect) earthEffect.Stop();
        if (windEffect) windEffect.Stop();
    }

    private void ApplyIceEffect()
    {
     
        if (audioSource != null && iceSound != null)
        {
            audioSource.PlayOneShot(iceSound);
        }
    }


    private void ApplyFireEffect()
    {

        if (playerMovement != null)
        {         
            playerMovement.RunAwayForSeconds(3f); 
        }
    }


    private void ApplyEarthEffect()
    {
        if (cameraShake != null)
        {
            cameraShake.TriggerShake();
        }
    }

    private void ApplyWindEffect()
    {
        if (playerMovement != null)
        {
            playerMovement.ModifySpeed(0.5f, 3f);
        }
    }
}
