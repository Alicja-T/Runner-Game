using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorPooling : MonoBehaviour {

    [SerializeField]
    private Transform platform, platformParent;

    [SerializeField]
    private Transform monster, monsterParent;

    [SerializeField]
    private Transform health_Collectible, health_Collectible_Parent;

    [SerializeField]
    private int levelLength = 100;

    [SerializeField]
    private float distanceBetweenPlatforms = 15f;

    [SerializeField]
    private float minPositionY = 0f, maxPositionY = 7f;

    [SerializeField]
    private float chanceForMonster = 0.25f, chanceForHealth = 0.1f;

    [SerializeField]
    private float healthCollectibleMinY = 1f, healthCollectibleMaxY = 3f;

    private float platformLastPositionX;
    private Transform[] platformArray;

	// Use this for initialization
	void Start () {
        CreatePlatforms();
	}

    void CreatePlatforms() {
        platformArray = new Transform[levelLength];
        for (int i = 0; i < levelLength; i++) {
            Transform newPlatform = (Transform)Instantiate(platform, Vector3.zero, Quaternion.identity);
            platformArray[i] = newPlatform;
        }

        for (int i = 0; i < levelLength; i++) {
            float platformPositionY = Random.Range(minPositionY, maxPositionY);
            Vector3 platformPosition;
            if (i < 2) {
                platformPositionY = 0f;
            }
            platformPosition = new Vector3(distanceBetweenPlatforms * i, platformPositionY, 0);
            platformLastPositionX = platformPosition.x;
            platformArray[i].position = platformPosition;
            platformArray[i].parent = platformParent;

            //spawn monsters etc
            SpawnHealthAndMonster(platformPosition, i, true);
        } 
    }
	
    public void PoolingPlatforms(){
        for (int i = 0; i < platformArray.Length; i++) {
            if (!platformArray[i].gameObject.activeInHierarchy) {
                platformArray[i].gameObject.SetActive(true);
                float platformPositionY = Random.Range(minPositionY, maxPositionY);
                Vector3 platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, platformPositionY, 0);
                platformArray[i].position = platformPosition;
                platformLastPositionX = platformPosition.x;
                SpawnHealthAndMonster(platformPosition, i, false);
            }
        }

    }


    // Update is called once per frame
    void Update () {
		
	}

    void SpawnHealthAndMonster(Vector3 platformPosition, int i, bool gameStarted) {
        if (i > 2) {
            if (Random.Range(0, 1f) < chanceForMonster) {
                if (gameStarted) {
                    platformPosition = new Vector3(distanceBetweenPlatforms * i,
                        platformPosition.y + 0.1f, 0f);
                }
                else {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX,
                        platformPosition.y + 0.1f, 0f);
                }
                Transform newMonster = (Transform)Instantiate(monster, platformPosition, Quaternion.Euler(0, -90, 0));
                newMonster.parent = monsterParent;
            } // if for monster

            if (Random.Range(0, 1f) < chanceForHealth) {
                if (gameStarted) {
                    platformPosition = new Vector3(distanceBetweenPlatforms * i,
                        platformPosition.y + Random.Range(healthCollectibleMinY, healthCollectibleMaxY), 0f);
                }
                else {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX,
                        platformPosition.y + Random.Range(healthCollectibleMinY, healthCollectibleMaxY), 0f);
                }
                Transform newHealth = (Transform)Instantiate(health_Collectible, platformPosition, Quaternion.identity);
                newHealth.parent = health_Collectible_Parent;
            }
        }

    }





}// class
