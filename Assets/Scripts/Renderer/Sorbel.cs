using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Sorbel")]
public sealed class Sorbel : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

    [Tooltip("Colour of the outline.")]
    public ColorParameter outlineColor  = new ColorParameter(Color.black);

    [Tooltip("Outline thickness")]
    public FloatParameter outlineThickness = new FloatParameter(1f);

    [Tooltip("Scales the depth calculation linearly")]
    public FloatParameter depthMultiplier = new FloatParameter(1f);

    [Tooltip("Depth bias scaled to the depth value")]
    public FloatParameter depthBias = new FloatParameter(1f);

    [Tooltip("Scales the normal calculation linearly")]
    public FloatParameter normalMultiplier = new FloatParameter(1f);

    [Tooltip("normal bias scaled to the normal value")]
    public FloatParameter normalBias = new FloatParameter(1f);




    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > HDRP Default Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/Sorbel";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume Sorbel is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetColor("_Color", outlineColor.value);
        m_Material.SetFloat("_Thickness", outlineThickness.value);
        m_Material.SetFloat("_DepthMultiplier", depthMultiplier.value);
        m_Material.SetFloat("_DepthBias", depthBias.value);
        m_Material.SetFloat("_NormalMultiplier", normalMultiplier.value);
        m_Material.SetFloat("_NormalBias", normalBias.value);
        m_Material.SetTexture("_InputTexture", source);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
