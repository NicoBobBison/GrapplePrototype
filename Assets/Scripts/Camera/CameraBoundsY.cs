using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class CameraBoundsY : CinemachineExtension
{
    public float m_YMin;
    public float m_YMax;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Finalize)
        {
            var pos = state.RawPosition;
            pos.y = Mathf.Clamp(pos.y, m_YMin, m_YMax);
            state.RawPosition = pos;

        }
    }
}
