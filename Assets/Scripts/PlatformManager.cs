using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{

    public GameObject platformPrefab;
    public Transform previousPlatformTransform;
    [Header("Platforms properties"), Space]
    public float initialPlatformWidth = 10f;
    public float minPlatformWidth = 2f;
    public float widthDecreaseRate = 0.2f;
    public float initialGapDistance = 5f;
    public float maxGapDistance = 15f;
    public float gapIncreaseRate = 0.2f;
    public float minVerticalDistance = 1f;
    public float maxVerticalDistance = 5f;
    public float timeBeforeSpawn = 2f;
    [Header("Initial and Visible Platforms"), Space]
    public int initialPlatformCount = 3;
    public int visiblePlatforms = 5;

    void Start()
    {
        for (var i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
        
        StartCoroutine(GeneratePlatforms());
    }

    private IEnumerator GeneratePlatforms()
    {
        while (true)
        {
            SpawnPlatform();

            if (transform.childCount > visiblePlatforms)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            yield return new WaitForSeconds(timeBeforeSpawn);
        }
    }

    private void SpawnPlatform()
    {
        GameObject newPlatform = Instantiate(platformPrefab, transform);

        var platform = newPlatform.GetComponent<Platform>();
        platform.SetWidth(initialPlatformWidth);

        var horizontalPos = previousPlatformTransform != null ? previousPlatformTransform.position.x + initialGapDistance : GameDirector.Instance.player.position.x + initialGapDistance;
        
        var verticalRange = Random.Range(minVerticalDistance, maxVerticalDistance);
        var verticalPos = previousPlatformTransform != null
            ? Mathf.Max(1f, previousPlatformTransform.position.y + verticalRange)
            : GameDirector.Instance.player.position.y + verticalRange;
        
        newPlatform.transform.position = new Vector3(horizontalPos, verticalPos, 0f);
        previousPlatformTransform = newPlatform.transform;

        initialGapDistance = Mathf.Min(maxGapDistance, initialGapDistance + gapIncreaseRate);
        initialPlatformWidth = Mathf.Max(minPlatformWidth, initialPlatformWidth - widthDecreaseRate);
    }
    
    
}
