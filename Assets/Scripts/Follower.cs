using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private Transform target;

    public void Follow(Transform target){
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);

        this.target = target;
        transform.position = target.position;
    }

    public void Unfollow(){
        target = null;
        gameObject.SetActive(false);
    }

    private void Update(){
        if(target == null){
            if(gameObject.activeSelf)
                Unfollow();
            return;
        }

        transform.position = target.position;
    }
}
