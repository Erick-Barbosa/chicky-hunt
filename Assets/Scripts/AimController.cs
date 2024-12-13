using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AimController : MonoBehaviour {
    [SerializeField] private GameObject cannon;
    private Transform cannonTransform;
    private CannonController cannonController;
    public Camera mainCamera; // A câmera principal, arraste-a no Inspector

    public event Action OnFire;

    Vector3 worldMousePos;

    private void Start() {
        Cursor.visible = false;

        cannonTransform = cannon.transform;
        cannonController = cannon.GetComponent<CannonController>();
    }

    private void Update() {
        Vector3 mousePos = Input.mousePosition;

        transform.position = mousePos;
        // Converte a posição do mouse para coordenadas do mundo
        worldMousePos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));

        // Atualiza a posição do objeto (apenas no eixo X)
        cannonTransform.position = new Vector3(worldMousePos.x, cannonTransform.position.y, cannonTransform.position.z);

        if (Input.GetMouseButtonDown(0)) {
            HandleClick();
            OnFire?.Invoke();
        }
    }

    void HandleClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Raio da posição do mouse
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // Verifica se o raio colidiu com algo
        {
            // Objeto clicado
            GameObject clickedObject = hit.collider.gameObject;

            // Realiza a ação desejada
            HandleInteraction(clickedObject);
        }
    }

    void HandleInteraction(GameObject obj) {
        if (obj.CompareTag("Interactable")) {
            // Exemplo: Executa algo específico do objeto
            obj.GetComponent<InteractableObject>()?.Interact(worldMousePos);
        }
        else {
            Debug.Log($"Você clicou em: {obj.name}");
        }
    }

}
