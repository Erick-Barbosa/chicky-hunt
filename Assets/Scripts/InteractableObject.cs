using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour {
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem particleInstance;
    [SerializeField] private Counter counter;
    [SerializeField] private ChickyManager manager;
    private bool Interacted;

    private void Start() {
        counter = GameObject.Find("Counter").GetComponent<Counter>();
        manager = GameObject.Find("ChickySpawner").GetComponent<ChickyManager>();

        manager.CountChickies(1);
    }

    public void Interact(Vector3 mousePos) {
        Debug.Log($"Interagiu com {gameObject.name}!");
        particleInstance = Instantiate(_particleSystem, new Vector3(mousePos.x, mousePos.y, transform.position.z - 1), Quaternion.identity);

        float duration = particleInstance.main.duration + particleInstance.main.startLifetime.constant;

        Destroy(particleInstance.gameObject, duration);

        Destroy(gameObject);

        if (!Interacted) {
            counter.UpdatePoints();
            manager.CountChickies(-1);
            Interacted = true;
        }
    }
}
