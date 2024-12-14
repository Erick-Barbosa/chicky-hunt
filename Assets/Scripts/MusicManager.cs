using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager> {
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip menuClip;
    [SerializeField] private AudioClip mainClip;

    private void OnEnable() {
        SceneManager.sceneLoaded += ChangeMusic;
    }

    private void ChangeMusic(Scene scene, LoadSceneMode sceneMode) {
        if (scene.buildIndex == 0) {
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
