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
    public Camera mainCamera; // A c�mera principal, arraste-a no Inspector

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
        // Converte a posi��o do mouse para coordenadas do mundo
        worldMousePos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));

        // Atualiza a posi��o do objeto (apenas no eixo X)
        cannonTransform.position = new Vector3(worldMousePos.x, cannonTransform.position.y, cannonTransform.position.z);

        if (Input.GetMouseButtonDown(0)) {
            HandleClick();
            OnFire?.Invoke();
        }
    }

    void HandleClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Raio da posi��o do mouse
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // Verifica se o raio colidiu com algo
        {
            // Objeto clicado
            GameObject clickedObject = hit.collider.gameObject;

            // Realiza a a��o desejada
            HandleInteraction(clickedObject);
        }
    }

    void HandleInteraction(GameObject obj) {
        if (obj.CompareTag("Interactable")) {
            // Exemplo: Executa algo espec�fico do objeto
            obj.GetComponent<InteractableObject>()?.Interact(worldMousePos);
        }
        else {
            Debug.Log($"Voc� clicou em: {obj.name}");
        }
    }

}
