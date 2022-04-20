using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Vector3 GetRandomPosition(){
        float radius = transform.localScale.x/2;
        float x = Random.Range(-radius, radius);
        float z = Random.Range(-radius, radius);
        float y = transform.localScale.y/2;

        return new Vector3(x, y, z);
    }
}
