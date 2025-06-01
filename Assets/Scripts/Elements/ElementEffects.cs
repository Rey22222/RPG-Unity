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

    public Character playerMovement;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip iceSound;
    public AudioClip fireSound;
    public AudioClip earthSound;

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

    public void PlayElementEffect(string attackType)
    {
        DisableAllEffects();

        switch (currentElement)
        {
            case ElementType.Ice:
                if (attackType == "Swipe" && audioSource && iceSound)
                    audioSource.PlayOneShot(iceSound);

                if (attackType == "Roar" && iceEffect)
                    iceEffect.Play();
                break;

            case ElementType.Fire:
                if (attackType == "Swipe" && fireEffect)
                    fireEffect.Play();

                if (attackType == "Roar" && audioSource && fireSound)
                    audioSource.PlayOneShot(fireSound);
                break;

            case ElementType.Earth:
                if (attackType == "Swipe" && earthEffect)
                    earthEffect.Play();

                if (attackType == "Roar" && audioSource && earthSound)
                    audioSource.PlayOneShot(earthSound);
                break;

            case ElementType.Wind:
                if (attackType == "Swipe" && playerMovement != null)
                    playerMovement.ModifySpeed(1.5f, 3f);

                if (attackType == "Roar" && windEffect)
                    windEffect.Play();
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
        if (audioSource != null && earthSound != null)
        {
            audioSource.PlayOneShot(earthSound);
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

