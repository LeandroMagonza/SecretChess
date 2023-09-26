using UnityEngine;
using UnityEngine.UIElements;

public class ManagerEffects : MonoBehaviour
{
    public static ManagerEffects Instance;

    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private Camera _camera;
    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();
        _camera = Camera.main;
    }
    
    public void PlayEffect(GameObject particle, AudioClip clip)
    {
        PlayParticle(particle);
        PlaySound(clip);
    }
    public void PlayEffectIn(GameObject particle, AudioClip clip, Vector3 position)
    {
        transform.position = position;
        Debug.Log(position);
        PlayEffect(particle, clip);

    }
    public void PlayParticle(GameObject particle)
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
