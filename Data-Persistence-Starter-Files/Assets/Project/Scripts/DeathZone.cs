using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;

    private void OnCollisionEnter(Collision other)
    {
        FinishGame(other);
    }

    // Finish the game: Destroy the ball and send the finish game over main manager method.
    private void FinishGame(Collision ball)
    {
        Destroy(ball.gameObject);
        Manager.GameOver();
    }
}
