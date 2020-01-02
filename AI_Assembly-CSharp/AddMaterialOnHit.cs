// Decompiled with JetBrains decompiler
// Type: AddMaterialOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddMaterialOnHit : MonoBehaviour
{
  public float RemoveAfterTime;
  public bool RemoveWhenDisable;
  public EffectSettings EffectSettings;
  public Material Material;
  public bool UsePointMatrixTransform;
  public Vector3 TransformScale;
  private FadeInOutShaderColor[] fadeInOutShaderColor;
  private FadeInOutShaderFloat[] fadeInOutShaderFloat;
  private UVTextureAnimator uvTextureAnimator;
  private Renderer renderParent;
  private Material instanceMat;
  private int materialQueue;
  private bool waitRemove;
  private float timeToDelete;

  public AddMaterialOnHit()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.EffectSettings, (Object) null))
      return;
    if (this.EffectSettings.IsVisible)
    {
      this.timeToDelete = 0.0f;
    }
    else
    {
      this.timeToDelete += Time.get_deltaTime();
      if ((double) this.timeToDelete <= (double) this.RemoveAfterTime)
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
  }

  public void UpdateMaterial(RaycastHit hit)
  {
    if (!Object.op_Inequality((Object) ((RaycastHit) ref hit).get_transform(), (Object) null))
      return;
    if (!this.RemoveWhenDisable)
      Object.Destroy((Object) ((Component) this).get_gameObject(), this.RemoveAfterTime);
    this.fadeInOutShaderColor = (FadeInOutShaderColor[]) ((Component) this).GetComponents<FadeInOutShaderColor>();
    this.fadeInOutShaderFloat = (FadeInOutShaderFloat[]) ((Component) this).GetComponents<FadeInOutShaderFloat>();
    this.uvTextureAnimator = (UVTextureAnimator) ((Component) this).GetComponent<UVTextureAnimator>();
    this.renderParent = (Renderer) ((Component) ((Component) this).get_transform().get_parent()).GetComponent<Renderer>();
    Material[] sharedMaterials = this.renderParent.get_sharedMaterials();
    int length = sharedMaterials.Length + 1;
    Material[] materialArray = new Material[length];
    sharedMaterials.CopyTo((Array) materialArray, 0);
    this.renderParent.set_material(this.Material);
    this.instanceMat = this.renderParent.get_material();
    materialArray[length - 1] = this.instanceMat;
    this.renderParent.set_sharedMaterials(materialArray);
    if (this.UsePointMatrixTransform)
      this.instanceMat.SetMatrix("_DecalMatr", Matrix4x4.TRS(((RaycastHit) ref hit).get_transform().InverseTransformPoint(((RaycastHit) ref hit).get_point()), Quaternion.Euler(180f, 180f, 0.0f), this.TransformScale));
    if (this.materialQueue != -1)
      this.instanceMat.set_renderQueue(this.materialQueue);
    if (this.fadeInOutShaderColor != null)
    {
      foreach (FadeInOutShaderColor inOutShaderColor in this.fadeInOutShaderColor)
        inOutShaderColor.UpdateMaterial(this.instanceMat);
    }
    if (this.fadeInOutShaderFloat != null)
    {
      foreach (FadeInOutShaderFloat inOutShaderFloat in this.fadeInOutShaderFloat)
        inOutShaderFloat.UpdateMaterial(this.instanceMat);
    }
    if (!Object.op_Inequality((Object) this.uvTextureAnimator, (Object) null))
      return;
    this.uvTextureAnimator.SetInstanceMaterial(this.instanceMat, ((RaycastHit) ref hit).get_textureCoord());
  }

  public void UpdateMaterial(Transform transformTarget)
  {
    if (!Object.op_Inequality((Object) transformTarget, (Object) null))
      return;
    if (!this.RemoveWhenDisable)
      Object.Destroy((Object) ((Component) this).get_gameObject(), this.RemoveAfterTime);
    this.fadeInOutShaderColor = (FadeInOutShaderColor[]) ((Component) this).GetComponents<FadeInOutShaderColor>();
    this.fadeInOutShaderFloat = (FadeInOutShaderFloat[]) ((Component) this).GetComponents<FadeInOutShaderFloat>();
    this.uvTextureAnimator = (UVTextureAnimator) ((Component) this).GetComponent<UVTextureAnimator>();
    this.renderParent = (Renderer) ((Component) ((Component) this).get_transform().get_parent()).GetComponent<Renderer>();
    Material[] sharedMaterials = this.renderParent.get_sharedMaterials();
    int length = sharedMaterials.Length + 1;
    Material[] materialArray = new Material[length];
    sharedMaterials.CopyTo((Array) materialArray, 0);
    this.renderParent.set_material(this.Material);
    this.instanceMat = this.renderParent.get_material();
    materialArray[length - 1] = this.instanceMat;
    this.renderParent.set_sharedMaterials(materialArray);
    if (this.materialQueue != -1)
      this.instanceMat.set_renderQueue(this.materialQueue);
    if (this.fadeInOutShaderColor != null)
    {
      foreach (FadeInOutShaderColor inOutShaderColor in this.fadeInOutShaderColor)
        inOutShaderColor.UpdateMaterial(this.instanceMat);
    }
    if (this.fadeInOutShaderFloat != null)
    {
      foreach (FadeInOutShaderFloat inOutShaderFloat in this.fadeInOutShaderFloat)
        inOutShaderFloat.UpdateMaterial(this.instanceMat);
    }
    if (!Object.op_Inequality((Object) this.uvTextureAnimator, (Object) null))
      return;
    this.uvTextureAnimator.SetInstanceMaterial(this.instanceMat, Vector2.get_zero());
  }

  public void SetMaterialQueue(int matlQueue)
  {
    this.materialQueue = matlQueue;
  }

  public int GetDefaultMaterialQueue()
  {
    return this.instanceMat.get_renderQueue();
  }

  private void OnDestroy()
  {
    if (Object.op_Equality((Object) this.renderParent, (Object) null))
      return;
    List<Material> list = ((IEnumerable<Material>) this.renderParent.get_sharedMaterials()).ToList<Material>();
    list.Remove(this.instanceMat);
    this.renderParent.set_sharedMaterials(list.ToArray());
  }
}
