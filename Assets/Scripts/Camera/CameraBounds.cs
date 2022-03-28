using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class CameraBoundsX : CinemachineExtension
{
    public float m_XMin;
    public float m_XMax;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Finalize)
        {
            var pos = state.RawPosition;
            pos.x = Mathf.Clamp(pos.x, m_XMin, m_XMax);
            state.RawPosition = pos;
        }
    }
}

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
