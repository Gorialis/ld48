using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public float globalVolume = 0.1f;
    public int phase = 0;
    public float smoothTime = 1.0f;

    public AudioClip[] layers;

    private AudioSource[] layerSources;
    private float[] layerVelocities;

    // Start is called before the first frame update
    void Start()
    {
        layerSources = new AudioSource[layers.Length];
        layerVelocities = new float[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            layerSources[i] = gameObject.AddComponent<AudioSource>();
            layerSources[i].clip = layers[i];
            layerSources[i].volume = 0.0f;
            layerSources[i].loop = true;
            layerSources[i].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layerSources.Length; i++)
        {
            AudioSource source = layerSources[i];
            float target = (i <= phase) ? globalVolume : 0.0f;
            source.volume = Mathf.SmoothDamp(source.volume, target, ref layerVelocities[i], smoothTime);
        }
    }
}
