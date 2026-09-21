using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip IntoBGM;
    public AudioClip GhostNormalState;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StopAllCoroutines();
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        audioSource.loop = false;
        audioSource.clip = IntoBGM;
        audioSource.Play();
        float waitTime = Mathf.Min(IntoBGM.length, 3f);
        yield return new WaitForSeconds(waitTime);
        audioSource.loop = true;
        audioSource.clip = GhostNormalState;
        audioSource.Play();
    }
}
