using UnityEngine;

public class CannonController : MonoBehaviour {
    [SerializeField] private AudioSource player;
    [SerializeField] private AimController controller;
    [SerializeField] private ParticleSystem particle;

    private void OnEnable() {
        controller.OnFire += PlaySound;
    }
    private void OnDisable() {
        controller.OnFire -= PlaySound;
    }

    private void PlaySound() {
        player.PlayOneShot(player.clip);
        particle.Play();
    }
}
