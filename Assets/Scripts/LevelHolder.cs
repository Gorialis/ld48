using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    int currentLevel = 1;

    List<GameObject> levelObjects = new List<GameObject>();
    List<Vector3> levelVelocities = new List<Vector3>();

    // We want to smoothly 'drop' the current level when it is completed
    // Ideally, this constitutes destroying it as well, for performance and, well, the sake of ease
    // Each level object is interpolated towards a target, based on where it is in the upcoming level buffer.
    // When the player reaches the end of a level, changingLevel becomes true, and changingLevelProgress tracks the time since this happened.
    // Thus, when changingLevel is true, the interpolation target for each level changes to being 1 in advance, creating the movement.
    // After changingLevelProgress has reached a certain amount, the first level (the completed one) is destroyed and changingLevel becomes false again.
    bool changingLevel = false;
    float changingLevelProgress = 0.0f;

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
            Debug.Log(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levelObjects.Count; ++i)
        {
            GameObject level = levelObjects[i];
            Vector3 velocity = levelVelocities[i];

            int j = i - (changingLevel ? 1 : 0);
            Vector3 targetPosition = new Vector3(0.0f, j < 0 ? -50.0f : 0.0f, j * 15.0f);

            level.transform.position = Vector3.SmoothDamp(level.transform.position, targetPosition, ref velocity, m_Smoothing);
        }

        changingLevelProgress = changingLevel ? changingLevelProgress + Time.deltaTime : 0.0f;
    }

    public float GetLevelDepth()
    {
        return levelObjects[changingLevel ? 1 : 0].transform.position.z;
    }

    public void OnLevelComplete()
    {
        changingLevel = true;
    }
}
