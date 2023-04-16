using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    // Update is called once per frame
    private void Update()
    {
        //camera targets player
        var transform1 = transform;
        var position = transform1.position;
        position = new Vector3(player.transform.position.x, position.y, position.z);
        transform1.position = position;
    }
}
