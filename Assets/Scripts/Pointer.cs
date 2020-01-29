using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pointer : MonoBehaviour
{
    public float m_Distance = 10.0f;
    public LineRenderer m_LineRenderer = null;
    public LayerMask m_EverythingMask = 0;
    public LayerMask m_InteractableMask = 0;
    public UnityAction<Vector3, GameObject> OnPointerUpdate = null;
    public GameObject m_ControllerObject = null;

    private Transform m_currentOrigin = null;
    private GameObject m_currentObject = null;

    private void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
        PlayerEvents.OnTouchpadDown += ProcessTouchpadDown;
        PlayerEvents.OnTriggerDown += ProcessTriggerDown;
    }

    private void Start()
    {
        SetLineColor();
    }

    private void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
        PlayerEvents.OnTouchpadDown -= ProcessTouchpadDown;
        PlayerEvents.OnTriggerDown -= ProcessTriggerDown;
    }

    private void Update()
    {
        Vector3 hitPoint = UpdateLine();

        m_currentObject = UpdatePointerStatus();

        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, m_currentObject);
    }

    private Vector3 UpdateLine()
    {
        RaycastHit hit = CreateRaycast(m_EverythingMask);

        Vector3 endPosition = m_currentOrigin.position + (m_currentOrigin.forward * m_Distance);

        if (hit.collider != null)
            endPosition = hit.point;

        m_LineRenderer.SetPosition(0, m_currentOrigin.position);
        m_LineRenderer.SetPosition(1, endPosition);
        m_ControllerObject.transform.position = m_currentOrigin.position;
        m_ControllerObject.transform.LookAt(endPosition);

        OVRInput.Controller controller = OVRInput.GetActiveController();
        if (controller != OVRInput.Controller.Touchpad)
        {
            m_ControllerObject.transform.rotation = OVRInput.GetLocalControllerRotation(controller);
        }

        return endPosition;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        m_currentOrigin = controllerObject.transform;

        if (controller == OVRInput.Controller.Touchpad)
            m_LineRenderer.enabled = false;
        else
            m_LineRenderer.enabled = true;
    }

    private GameObject UpdatePointerStatus()
    {
        RaycastHit hit = CreateRaycast(m_InteractableMask);

        if (hit.collider != null)
            return hit.collider.gameObject;

        return null;
    }

    private RaycastHit CreateRaycast(int layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(m_currentOrigin.position, m_currentOrigin.forward);

        Physics.Raycast(ray, out hit, m_Distance, layer);

        return hit;
    }
    
    private void SetLineColor()
    {
        if (!m_LineRenderer)
            return;

        Color endColor = Color.white;
        endColor.a = 0.0f;

        m_LineRenderer.endColor = endColor;
    }

    private void ProcessTouchpadDown()
    {
        if (!m_currentObject)
            return;

        Interactable interactable = m_currentObject.gameObject.GetComponent<Interactable>();
        interactable.TouchpadPressed();
    }

    private void ProcessTriggerDown()
    {
        if (!m_currentObject)
            return;

        Interactable interactable = m_currentObject.gameObject.GetComponent<Interactable>();
        interactable.TriggerPressed();
    }
}
