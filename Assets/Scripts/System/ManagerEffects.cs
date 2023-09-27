using System.Collections;
using UnityEngine;

public class ManagerEffects : MonoBehaviour
{
    public static ManagerEffects Instance;

    private AudioSource _audioSource;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayEffect(GameObject particle, AudioClip clip)
    {
        PlayParticle(particle);
        PlaySound(clip);
    }
    public void PlayEffectIn(GameObject particle, AudioClip clip, Vector3 position)
    {
        transform.position = position;
        PlayEffect(particle, clip);

    }
    public void PlayParticle(GameObject particle)
    {
        if(Options.particles)
            Instantiate(particle, transform.position, Quaternion.identity);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void SquashPiece(Transform transform, bool value, float amount, float speed)
    {
        if (Options.squash)
            StartCoroutine(Squash(transform, value, amount, speed));
    }
    public void MovePiece(Transform transform, Tile tile)
    {
        if (Options.lerp)
            StartCoroutine(Move(transform, tile));
    }

    private IEnumerator Move(Transform transform, Tile tile)
    {
        while(Vector3.Distance(transform.localPosition, tile.gameObject.transform.localPosition) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, tile.gameObject.transform.localPosition, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(Move(transform, tile));
    }


    private IEnumerator Squash(Transform transform, bool value, float amount, float speed)
    {
        Vector3 initialScale = transform.localScale;
        while (value)
        {
            float scale = 1.0f + Mathf.Sin(Time.time * speed) * amount;
            transform.localScale = new Vector3(initialScale.x * scale, initialScale.y / scale, initialScale.z);
            yield return new WaitForSeconds(0.01f);
        }
        StopCoroutine(Squash(transform, false, 0 , 0));
    }


}
