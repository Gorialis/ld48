using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public float globalVolume = 0.1f;
    public int phase = 0;
    public float smoothTime = 1.0f;

    public AudioClip[] layers;
    public int[] triggers;

    public bool interactionOn;
    public AudioClip interactionClip;
    private AudioSource interactionSource;
    private float interactionVelocity;

    private AudioSource[] layerSources;
    private float[] layerVelocities;

    // Start is called before the first frame update
    void Start()
    {
        layerSources = new AudioSource[layers.Length];
        layerVelocities = new float[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            layerSources[i] = CreateSource(layers[i]);
        }

        interactionSource = CreateSource(interactionClip);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layerSources.Length; i++)
        {
            HandleClip(layerSources[i], ref layerVelocities[i], triggers[i] <= phase);
        }

        HandleClip(interactionSource, ref interactionVelocity, interactionOn, 0.5f);
    }

    AudioSource CreateSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0.0f;
        source.loop = true;
        source.Play();
        return source;
    }

    void HandleClip(AudioSource source, ref float velocity, bool predicate, float smoothingMultiplier = 1.0f)
    {
        source.volume = Mathf.SmoothDamp(source.volume, predicate ? globalVolume : 0.0f, ref velocity, smoothTime * smoothingMultiplier);
    }
}
