using System;
using System.Collections;
using UnityEngine;

public class ManagerEffects : MonoBehaviour
{
    public static ManagerEffects Instance;

    private AudioSource _audioSource;
    private IEnumerator squash;
    public bool finishMove = false;


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
        if(Options.Options_Graphics.particles)
            Instantiate(particle, transform.position, Quaternion.identity);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void SquashPiece(Transform transform, bool value, float amount, float speed)
    {
        if (Options.Options_Graphics.squash)
        {
            this.squash = Squash(transform, value, amount, speed);
            StartCoroutine(this.squash);
        }
    }
    public void StopSquash()
    {
        if (this.squash != null)
            StopCoroutine(this.squash);
    }
    public void MovePiece(Transform transform, Tile tile)
    {
        if (Options.Options_Graphics.lerp)
            StartCoroutine(Move(transform, tile));
    }

    public IEnumerator Move(Transform transform, Tile tile)
    {
        float time = 0;
        while (Vector3.Distance(transform.position, tile.gameObject.transform.position) > 0.02f)
        {
            finishMove = false;
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, tile.gameObject.transform.position, time);
            yield return new WaitForSeconds(0.01f);
        }
        finishMove = true;
        transform.position = tile.transform.position;
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
        
    }


}
