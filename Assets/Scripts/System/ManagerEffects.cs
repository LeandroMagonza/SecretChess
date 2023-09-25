using UnityEngine;

public class ManagerEffects : MonoBehaviour
{
    public static ManagerEffects Instance;

    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();
    }
    
    public void PlayEffect(GameObject particle, AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
        _particleSystem = particle.GetComponent<ParticleSystem>();
        _particleSystem.Play();
    }
    public void PlayEffectIn(GameObject particle, AudioClip clip, Vector3 position)
    {
        transform.position = position;
        _audioSource.PlayOneShot(clip);
        _particleSystem = particle.GetComponent<ParticleSystem>();
        ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
        _particleSystem.Play();
    }
    public void PlayParticle(GameObject particle)
    {
        _particleSystem = particle.GetComponent<ParticleSystem>();
        _particleSystem.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
