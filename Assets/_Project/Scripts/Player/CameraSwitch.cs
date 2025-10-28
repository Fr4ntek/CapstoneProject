using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook indoorCamera;
    [SerializeField] private int indoorPriority = 11;
    [SerializeField] private float blendTime = 1f;

    private CinemachineBrain brain;

    private void Awake()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, blendTime);
        indoorCamera.Priority = indoorPriority;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
    }

}
