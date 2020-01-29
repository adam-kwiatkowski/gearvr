using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    public Pointer m_Pointer;
    public SpriteRenderer m_CircleRenderer;

    public Sprite m_OpenSprite;
    public Sprite m_ClosedSprite;

    private Camera m_camera = null;
    // Start is called before the first frame update
    private void Awake()
    {
        m_Pointer.OnPointerUpdate += UpdateSprite;

        m_camera = Camera.main;
    }

    private void OnDestroy()
    {
        m_Pointer.OnPointerUpdate -= UpdateSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_camera.gameObject.transform);
    }

    private void UpdateSprite(Vector3 point, GameObject hitObject)
    {
        transform.position = point;
        if (hitObject)
        {
            m_CircleRenderer.sprite = m_ClosedSprite;
        }
        else
        {
            m_CircleRenderer.sprite = null;
        }
    }
}
