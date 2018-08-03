using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private int levelLength;

    [SerializeField]
    private int startPlatformLength = 5, endPlatformLength = 5;

    [SerializeField]
    private int distanceBetweenPlatforms;

    [SerializeField]
    private Transform platformPrefab, platformParent;

    [SerializeField]
    private Transform monster, monsterParent;

    [SerializeField]
    private Transform healthCollectable, healthCollectableParent;

    [SerializeField]
    private float platformPositionMinY = 0f, platformPositionMaxY = 10f;

    [SerializeField]
    private int platformLengthMin = 1, platformLengthMax = 4;

    [SerializeField]
    private float chanceForMonsterExistence = 0.25f, chanceForCollectibleExistence = 0.1f;

    [SerializeField]
    private float healthCollectibleMinY = 1f, healthCollectiveMaxY = 3f;

    private float platformLastPositionX;

    private enum PlatformType {
        None,
        Flat
    }

    private class PlatformPositionInfo {

        public PlatformType platformType;
        public float positionY;
        public bool hasMonster;
        public bool hasHealthCollectible;

        public PlatformPositionInfo(PlatformType type, float posY, bool has_monster, bool has_collectible) {
            platformType = type;
            positionY = posY;
            hasMonster = has_monster;
            hasHealthCollectible = has_collectible;
        }
    }//class PlatformPositionInfo

    private void Start()
    {
        GenerateLevel(true);
        Instantiate(platformPrefab, new Vector3(0, 0, 0), Quaternion.identity);
           
    }

    void FillOutPositionInfo(PlatformPositionInfo[] platformInfo) {
        int currentPlatformInfoIndex = 0;

        for (int i = 0; i < startPlatformLength; i++) {
            platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
            platformInfo[currentPlatformInfoIndex].positionY = 0f;
            currentPlatformInfoIndex++;
        }


        while (currentPlatformInfoIndex < levelLength - endPlatformLength) {
            if (platformInfo[currentPlatformInfoIndex - 1].platformType != PlatformType.None) {
                currentPlatformInfoIndex++;
                continue;
            }

            float platformPositionY = Random.Range(platformPositionMinY, platformPositionMaxY);
            int platformLength = Random.Range(platformLengthMin, platformLengthMax);

            for (int i = 0; i < platformLength; i++) {
                bool has_Monster = ( Random.Range(0f, 1f) < chanceForMonsterExistence );
                bool has_healthCollectible = (Random.Range(0f, 1f) < chanceForCollectibleExistence);

                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = platformPositionY;
                platformInfo[currentPlatformInfoIndex].hasMonster = has_Monster;
                platformInfo[currentPlatformInfoIndex].hasHealthCollectible = has_healthCollectible;

                currentPlatformInfoIndex++;
                if (currentPlatformInfoIndex > (levelLength - endPlatformLength)) {
                    currentPlatformInfoIndex = levelLength - endPlatformLength;
                    break;
                }
            }

            for (int i = 0; i < endPlatformLength; i++) {
                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = 0f;
                currentPlatformInfoIndex++;
            }
            
        }//while loop
    }

    void CreatePlatformsFromPositionInfo( PlatformPositionInfo[] platformPositionInfo, bool gameStarted ) {

        for (int i = 0; i < platformPositionInfo.Length; i++) {
            PlatformPositionInfo positionInfo = platformPositionInfo[i];
            if (positionInfo.platformType == PlatformType.None) {
                continue;
            }

            Vector3 platformPosition;
            
            //here we going to check if the game is started or not
            if (gameStarted){
                platformPosition = new Vector3(distanceBetweenPlatforms * i, positionInfo.positionY, 0);
            }
            else {
                platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, positionInfo.positionY, 0);
            }
            
            //save the platform position x for later use
            platformLastPositionX = platformPosition.x;
            Transform createBlock = (Transform) Instantiate(platformPrefab, platformPosition, Quaternion.identity);
            createBlock.parent = platformParent;

            if (positionInfo.hasMonster) {
                if (gameStarted) {
                    platformPosition = new Vector3(distanceBetweenPlatforms * i, positionInfo.positionY + 0.1f, 0);
                }
                else {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, positionInfo.positionY + 0.1f, 0);
                }

                Transform createMonster = (Transform) Instantiate(monster, platformPosition, Quaternion.Euler(0, -90, 0) );
                createMonster.parent = monsterParent;
            }

            if (positionInfo.hasHealthCollectible) {
                if (gameStarted) {
                    platformPosition = new Vector3(distanceBetweenPlatforms * i,
                        positionInfo.positionY + Random.Range(healthCollectibleMinY, healthCollectiveMaxY), 0);
                }
                else {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX,
                        positionInfo.positionY + Random.Range(healthCollectibleMinY, healthCollectiveMaxY), 0);
                }

                Transform healthCollectible = (Transform)Instantiate(healthCollectable, platformPosition, Quaternion.identity);
                healthCollectible.parent = healthCollectableParent;
            }
        }// for loop
    }

    public void GenerateLevel(bool gameStarted) {
        PlatformPositionInfo[] platformInfo = new PlatformPositionInfo[levelLength];

        for (int i = 0; i < platformInfo.Length; i++) {
            platformInfo[i] = new PlatformPositionInfo(PlatformType.None, -1f, false, false);
        }

        FillOutPositionInfo(platformInfo);
        CreatePlatformsFromPositionInfo(platformInfo, gameStarted);
   
    }



	
	

}//class
