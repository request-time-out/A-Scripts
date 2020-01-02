// Decompiled with JetBrains decompiler
// Type: MiniMapDepthTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class MiniMapDepthTexture : MonoBehaviour
{
  [SerializeField]
  private Shader _shader;
  [SerializeField]
  private float _outlineThreshold;
  [SerializeField]
  private Color _outlineColor;
  [SerializeField]
  private float _outlineThick;
  private Material _material;

  public MiniMapDepthTexture()
  {
    base.\u002Ector();
  }

  public float outlineThick
  {
    get
    {
      return this._outlineThick;
    }
  }

  private void Awake()
  {
  }

  private void Update()
  {
  }

  public void Initialize()
  {
    Camera component = (Camera) ((Component) this).GetComponent<Camera>();
    RenderTexture targetTexture = component.get_targetTexture();
    Camera camera = component;
    camera.set_depthTextureMode((DepthTextureMode) (camera.get_depthTextureMode() | 1));
    if (component.get_allowMSAA() || component.get_allowHDR())
      return;
    this._material = new Material(this._shader);
    this.SetMaterialProperties();
    CommandBuffer commandBuffer = new CommandBuffer();
    int id = Shader.PropertyToID("_PostEffectTempTexture");
    commandBuffer.GetTemporaryRT(id, -1, -1);
    commandBuffer.Blit(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 1), RenderTargetIdentifier.op_Implicit(id));
    commandBuffer.Blit(RenderTargetIdentifier.op_Implicit(id), RenderTargetIdentifier.op_Implicit((Texture) targetTexture), this._material);
    commandBuffer.ReleaseTemporaryRT(id);
    component.AddCommandBuffer((CameraEvent) 20, commandBuffer);
  }

  private void SetMaterialProperties()
  {
    if (!Object.op_Inequality((Object) this._material, (Object) null))
      return;
    this._material.SetFloat("_OutlineThreshold", this._outlineThreshold);
    this._material.SetColor("_OutlineColor", this._outlineColor);
    this._material.SetFloat("_OutlineThick", this._outlineThick);
  }
}
