using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {

    public float lifeTime = 5f;
    private float startY;

	// Use this for initialization
	void Start () {
        startY = transform.position.y;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
	}

    IEnumerator TurnOffBullet() {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.MONSTER_TAG || target.tag == Tags.PLAYER_TAG ||
            target.tag == Tags.MONSTER_BULLET_TAG || target.tag == Tags.PLAYER_BULLET_TAG) {
            gameObject.SetActive(false);
        }
    }

}//class
