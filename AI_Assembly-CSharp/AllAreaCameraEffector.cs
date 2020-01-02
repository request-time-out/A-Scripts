// Decompiled with JetBrains decompiler
// Type: AllAreaCameraEffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class AllAreaCameraEffector : MonoBehaviour
{
  [SerializeField]
  private Shader _shader;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float alpha;
  [SerializeField]
  private Color BGColor;
  [SerializeField]
  private float _outlineThreshold;
  [SerializeField]
  private MiniMapDepthTexture depthTexture;
  private Material _material;
  private RenderTexture renderTexture;

  public AllAreaCameraEffector()
  {
    base.\u002Ector();
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
    this.renderTexture = component.get_targetTexture();
    Camera camera = component;
    camera.set_depthTextureMode((DepthTextureMode) (camera.get_depthTextureMode() | 1));
    if (component.get_allowMSAA() || component.get_allowHDR())
      return;
    this._material = new Material(this._shader);
    this.SetMaterialProperties();
    CommandBuffer commandBuffer = new CommandBuffer();
    int id = Shader.PropertyToID("_PostEffectTempTexture");
    commandBuffer.GetTemporaryRT(id, -1, -1);
    commandBuffer.Blit((Texture) this.renderTexture, RenderTargetIdentifier.op_Implicit(id));
    commandBuffer.Blit(RenderTargetIdentifier.op_Implicit(id), RenderTargetIdentifier.op_Implicit((Texture) this.renderTexture), this._material);
    commandBuffer.ReleaseTemporaryRT(id);
    component.AddCommandBuffer((CameraEvent) 20, commandBuffer);
  }

  private void SetMaterialProperties()
  {
    if (!Object.op_Inequality((Object) this._material, (Object) null))
      return;
    this._material.SetTexture("_MainTex", (Texture) this.renderTexture);
    this._material.SetFloat("_Alpha", this.alpha);
    this._material.SetFloat("_OutlineThreshold", this._outlineThreshold);
    this._material.SetFloat("_OutlineThick", this.depthTexture.outlineThick);
    this._material.SetColor("_Color", this.BGColor);
  }
}
