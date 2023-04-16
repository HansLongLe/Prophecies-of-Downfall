using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private Vector2 parallaxEffectMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    // Start is called before the first frame update
    private void Start()
    {
        if (Camera.main != null) cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        var sprite = GetComponent<SpriteRenderer>().sprite;
        var texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var position = cameraTransform.position;
        var deltaMovement = position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = position;

        if (!(Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)) return;
        var transform1 = transform;
        var position1 = cameraTransform.position;
        var position2 = transform1.position;
        var offsetPositionX = (position1.x - position2.x) % textureUnitSizeX;
        position2 = new Vector3(position1.x + offsetPositionX, position2.y);
        transform1.position = position2;
    }
}
