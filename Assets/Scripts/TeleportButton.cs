using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public float positionX;
    public float positionY;
    public float originX;
    public float originY;
    public float teleportSpeed = 1f;
    public SoundController soundController;
    public AudioClip teleportClip;

    void Start()
    {
        originX = transform.position.x;
        originY = transform.position.y;
    }

    IEnumerator Teleport ()
    {
        yield return new WaitForSeconds(teleportSpeed);
        transform.position = new Vector2(positionX, positionY);
        soundController.PlayAudioOnce(teleportClip);
    }

    private void ResetPosition()
    {
        transform.position = new Vector2(originX, originY);
    }
}
