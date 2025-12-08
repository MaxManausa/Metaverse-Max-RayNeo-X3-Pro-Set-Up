using RayNeo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickerScript : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // === PUBLIC IDENTIFIERS / INTERVALS ===

    [Header("Sequence Link")]
    [Tooltip("The GameObject that will become active when this object is clicked.")]
    public GameObject nextObject;

    [Tooltip("A unique ID for this object (optional, for logging/debugging).")]
    public int myID;

    // === Original Sample Variables (Rotation/Hover Logic) ===
    private bool m_update = false;
    private bool m_rotDirection = false;

    private void Update()
    {
        // Original rotation logic from the sample script
        if (!m_update)
        {
            transform.Rotate(Vector3.forward * (m_rotDirection ? 1 : -1) * 0.1f, Space.World);
            return;
        }

        transform.Rotate(Vector3.forward * (m_rotDirection ? 1 : -1), Space.World);
    }


    // === Pointer Event Implementations ===

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_update = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_update = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 1. Check if a next object is assigned.
        if (nextObject != null)
        {
            // 2. Activate the next object in the sequence.
            nextObject.SetActive(true);

            // 3. Deactivate the current (clicked) object.
            gameObject.SetActive(false);

            Debug.Log($"Object ID {myID} clicked. Deactivated self and activated: **{nextObject.name}**");
        }
        else
        {
            // This is the last object in the sequence.
            Debug.LogWarning($"Object ID {myID} is the end of the sequence.");

            // You may choose to deactivate the final object as well, or leave it active.
            // gameObject.SetActive(false);
        }
    }
}