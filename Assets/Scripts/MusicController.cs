using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public float globalVolume = 0.1f;
    public int phase = 0;
    public float smoothTime = 1.0f;

    private AudioSource[] sources;
    private float[] audioVelocities;

    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        audioVelocities = new float[sources.Length];

        foreach (AudioSource source in sources)
        {
            source.volume = 0.0f;
            source.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            AudioSource source = sources[i];
            float target = (i <= phase) ? globalVolume : 0.0f;
            source.volume = Mathf.SmoothDamp(source.volume, target, ref audioVelocities[i], smoothTime);
        }
    }
}
