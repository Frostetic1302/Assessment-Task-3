using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public Transform[] points;
    public float speed = 5f;
    int index = 0;
    float t = 0f;
    float duration = 1f;
    Animator anim;
    AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (IsValidSetup())
        {
            UpdateDuration();
        }
    }

    void Update()
    {
        if(!IsValidSetup()) return;
        Transform start = points[index];
        Transform end = points[(index + 1) % points.Length];
        t += Time.deltaTime / duration;
        transform.position = Vector3.Lerp(start.position, end.position, t);

        Vector3 dir = (end.position - start.position).normalized;
        if (anim != null)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                    anim.Play("PacStudent_WalkRight");
                else
                    anim.Play("PacStudent_WalkLeft");
            }
            else
            {
                if (dir.y > 0)
                    anim.Play("PacStudent_WalkUp");
                else
                    anim.Play("PacStudent_WalkDown");
            }
        }

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (t >= 1f)
        {
            t = 0f;
            index = (index + 1) % points.Length;
            UpdateDuration();

        }
    }

    void UpdateDuration()
    {
        Transform start = points[index];
        Transform end = points[(index + 1) % points.Length];

        float distance = Vector3.Distance(start.position, end.position);
        if (distance < 0.001f)
        {
            duration = 0.1f;
            return;
        }
        duration = distance / speed;
    }

    bool IsValidSetup()
    {
        if (points == null || points.Length < 2) return false;
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null) return false;
        }
        return true;
    }
}
