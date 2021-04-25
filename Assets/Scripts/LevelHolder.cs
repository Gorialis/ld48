using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHolder : MonoBehaviour
{
    List<GameObject> levelObjects = new List<GameObject>();
    List<Vector3> levelVelocities = new List<Vector3>();

    public FOVWinch cameraWinch;
    public Text completionText;

    int levelIndex = 0;
    private float gameTimer = 0.0f;

    // We want to smoothly 'drop' the current level when it is completed
    // Ideally, this constitutes destroying it as well, for performance and, well, the sake of ease
    // Each level object is interpolated towards a target, based on where it is in the upcoming level buffer.
    // When the player reaches the end of a level, the currently dropped level is destroyed.
    // This causes the interpolation targets for every subsequent level to change.

    // Adjustable smoothing, game feel variable
    public float m_Smoothing = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Populate the level objects
        foreach (Transform child in transform)
        {
            levelObjects.Add(child.gameObject);
            levelVelocities.Add(Vector3.zero);

            // Disable all the 2D colliders
            foreach (Collider2D collider in child.gameObject.GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = false;
            }

            // Disable all the Rigidbodies
            foreach (Rigidbody2D rigidbody in child.gameObject.GetComponentsInChildren<Rigidbody2D>())
            {
                if (rigidbody.bodyType != RigidbodyType2D.Static)
                {
                    DontTouchRigidbody dontTouch = rigidbody.gameObject.AddComponent<DontTouchRigidbody>();
                    dontTouch.kinematic = rigidbody.isKinematic;
                    dontTouch.constraints = rigidbody.constraints;

                    rigidbody.isKinematic = true;
                    rigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                }
            }
        }

        EnableCurrentColliders();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levelObjects.Count; ++i)
        {
            GameObject level = levelObjects[i];
            Vector3 velocity = levelVelocities[i];

            Vector3 targetPosition = new Vector3(0.0f, i == 0 ? -50.0f : 0.0f, (i - 1) * 15.0f);

            level.transform.position = Vector3.SmoothDamp(level.transform.position, targetPosition, ref velocity, m_Smoothing);
        }

        gameTimer += Time.deltaTime;
    }

    public float GetLevelDepth()
    {
        return levelObjects[1].transform.position.z;
    }

    public BeamInteractable[] GetLevelInteractables()
    {
        return levelObjects[1].GetComponentsInChildren<BeamInteractable>();
    }

    public Vector3 GetResetPosition()
    {
        Vector3 pos = levelObjects[0].transform.position;

        foreach (Launchpad launchpad in levelObjects[0].GetComponentsInChildren<Launchpad>())
        {
            pos = launchpad.transform.position;
        }

        return levelObjects[1].transform.position + (pos - levelObjects[0].transform.position) + Vector3.up;
    }

    public Vector3 GetEndPosition()
    {
        Vector3 pos = levelObjects[1].transform.position;

        foreach (Launchpad launchpad in levelObjects[1].GetComponentsInChildren<Launchpad>())
        {
            pos = launchpad.transform.position;
        }

        return pos + Vector3.up;
    }

    public void DisableCurrentColliders()
    {
        foreach (Collider2D collider in levelObjects[1].GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        foreach (Rigidbody2D rigidbody in levelObjects[1].GetComponentsInChildren<Rigidbody2D>())
        {
            if (rigidbody.bodyType != RigidbodyType2D.Static)
            {
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
            }

        }
    }

    public void EnableCurrentColliders()
    {
        foreach (Collider2D collider in levelObjects[1].GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = true;
        }

        foreach (Rigidbody2D rigidbody in levelObjects[1].GetComponentsInChildren<Rigidbody2D>())
        {
            DontTouchRigidbody dontTouch = rigidbody.gameObject.GetComponent<DontTouchRigidbody>();

            if (dontTouch != null)
            {
                rigidbody.isKinematic = dontTouch.kinematic;
                rigidbody.constraints = dontTouch.constraints;
            }
        }
    }

    public void OnLevelComplete()
    {
        DisableCurrentColliders();

        // Destroy the first level in the sequence
        GameObject.Destroy(levelObjects[0]);
        levelObjects.RemoveAt(0);
        levelVelocities.RemoveAt(0);

        levelIndex++;

        if (levelIndex == 7)
        {
            cameraWinch.targetHeight = 20.0f;
            cameraWinch.targetDistance = 50.0f;
        }

        int minutes = Mathf.FloorToInt(gameTimer / 60.0f);
        int seconds = Mathf.FloorToInt(gameTimer % 60.0f);

        completionText.text = $"Your completion time:\n{minutes} minutes, {seconds} seconds";

        EnableCurrentColliders();

    }
}
