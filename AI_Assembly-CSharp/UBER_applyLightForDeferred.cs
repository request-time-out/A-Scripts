// Decompiled with JetBrains decompiler
// Type: UBER_applyLightForDeferred
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("UBER/Apply Light for Deferred")]
[ExecuteInEditMode]
public class UBER_applyLightForDeferred : MonoBehaviour
{
  public Light lightForSelfShadowing;
  private Renderer _renderer;

  public UBER_applyLightForDeferred()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.Reset();
  }

  private void Reset()
  {
    if (Object.op_Implicit((Object) ((Component) this).GetComponent<Light>()) && Object.op_Equality((Object) this.lightForSelfShadowing, (Object) null))
      this.lightForSelfShadowing = (Light) ((Component) this).GetComponent<Light>();
    if (!Object.op_Implicit((Object) ((Component) this).GetComponent<Renderer>()) || !Object.op_Equality((Object) this._renderer, (Object) null))
      return;
    this._renderer = (Renderer) ((Component) this).GetComponent<Renderer>();
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.lightForSelfShadowing))
      return;
    if (Object.op_Implicit((Object) this._renderer))
    {
      if (this.lightForSelfShadowing.get_type() == 1)
      {
        for (int index = 0; index < this._renderer.get_sharedMaterials().Length; ++index)
          this._renderer.get_sharedMaterials()[index].SetVector("_WorldSpaceLightPosCustom", Vector4.op_Implicit(Vector3.op_UnaryNegation(((Component) this.lightForSelfShadowing).get_transform().get_forward())));
      }
      else
      {
        for (int index = 0; index < this._renderer.get_materials().Length; ++index)
          this._renderer.get_sharedMaterials()[index].SetVector("_WorldSpaceLightPosCustom", new Vector4((float) ((Component) this.lightForSelfShadowing).get_transform().get_position().x, (float) ((Component) this.lightForSelfShadowing).get_transform().get_position().y, (float) ((Component) this.lightForSelfShadowing).get_transform().get_position().z, 1f));
      }
    }
    else if (this.lightForSelfShadowing.get_type() == 1)
      Shader.SetGlobalVector("_WorldSpaceLightPosCustom", Vector4.op_Implicit(Vector3.op_UnaryNegation(((Component) this.lightForSelfShadowing).get_transform().get_forward())));
    else
      Shader.SetGlobalVector("_WorldSpaceLightPosCustom", new Vector4((float) ((Component) this.lightForSelfShadowing).get_transform().get_position().x, (float) ((Component) this.lightForSelfShadowing).get_transform().get_position().y, (float) ((Component) this.lightForSelfShadowing).get_transform().get_position().z, 1f));
  }
}
