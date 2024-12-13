using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager> {
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip menuClip;
    [SerializeField] private AudioClip mainClip;
    [SerializeField] private GameManager gameManager;

    private void Start() {
        source.clip = menuClip;
        source.Play();
    }

    private void OnEnable() {
        gameManager.OnGameStart += ChangeMusic;
        gameManager.OnGameFinish += ChangeMusic;
    }

    private void OnDisable() {
        gameManager.OnGameStart -= ChangeMusic;
        gameManager.OnGameFinish -= ChangeMusic;
    }

    private void ChangeMusic(int obj) {
        if (source.clip == menuClip) {
            source.Stop();
            source.clip = mainClip;
            source.Play();
        }
        else {
            source.Stop();
            source.clip = menuClip;
            source.Play();
        }
    }
}
