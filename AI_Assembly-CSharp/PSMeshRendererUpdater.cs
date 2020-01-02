// Decompiled with JetBrains decompiler
// Type: PSMeshRendererUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class PSMeshRendererUpdater : MonoBehaviour
{
  public GameObject MeshObject;
  public Color Color;
  private const string materialName = "MeshEffect";
  private List<Material[]> rendererMaterials;
  private List<Material[]> skinnedMaterials;
  public bool IsActive;
  public float FadeTime;
  private bool currentActiveStatus;
  private bool needUpdateAlpha;
  private Color oldColor;
  private float currentAlphaTime;

  public PSMeshRendererUpdater()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (Application.get_isPlaying())
      this.CheckFading();
    if (!Color.op_Inequality(this.Color, this.oldColor))
      return;
    this.oldColor = this.Color;
    this.UpdateColor(this.Color);
  }

  public void CheckFading()
  {
    if (this.currentActiveStatus != this.IsActive)
    {
      this.currentActiveStatus = this.IsActive;
      this.needUpdateAlpha = true;
      foreach (ParticleSystem componentsInChild in (ParticleSystem[]) ((Component) this).GetComponentsInChildren<ParticleSystem>())
      {
        if (this.currentActiveStatus)
        {
          componentsInChild.Clear();
          componentsInChild.Play();
        }
        else
          componentsInChild.Stop();
      }
      foreach (ME_TrailRendererNoise componentsInChild in (ME_TrailRendererNoise[]) ((Component) this).GetComponentsInChildren<ME_TrailRendererNoise>())
        componentsInChild.IsActive = this.currentActiveStatus;
    }
    if (!this.needUpdateAlpha)
      return;
    if (this.currentActiveStatus)
      this.currentAlphaTime += Time.get_deltaTime();
    else
      this.currentAlphaTime -= Time.get_deltaTime();
    if ((double) this.currentAlphaTime < 0.0 || (double) this.currentAlphaTime > (double) this.FadeTime)
      this.needUpdateAlpha = false;
    this.SetAlpha(Mathf.Clamp01(this.currentAlphaTime / this.FadeTime));
  }

  public void SetAlpha(float alpha)
  {
    if (Object.op_Equality((Object) this.MeshObject, (Object) null))
      return;
    Light componentInChildren1 = (Light) this.MeshObject.GetComponentInChildren<Light>();
    if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
      componentInChildren1.set_intensity(alpha);
    MeshRenderer componentInChildren2 = (MeshRenderer) this.MeshObject.GetComponentInChildren<MeshRenderer>();
    if (Object.op_Inequality((Object) componentInChildren2, (Object) null))
    {
      foreach (Material material in ((Renderer) componentInChildren2).get_materials())
      {
        if (((Object) material).get_name().Contains("MeshEffect"))
        {
          this.UpdateAlphaByPropertyName(material, "_TintColor", alpha);
          this.UpdateAlphaByPropertyName(material, "_MainColor", alpha);
        }
      }
    }
    SkinnedMeshRenderer componentInChildren3 = (SkinnedMeshRenderer) this.MeshObject.GetComponentInChildren<SkinnedMeshRenderer>();
    if (!Object.op_Inequality((Object) componentInChildren3, (Object) null))
      return;
    foreach (Material material in ((Renderer) componentInChildren3).get_materials())
    {
      if (((Object) material).get_name().Contains("MeshEffect"))
      {
        this.UpdateAlphaByPropertyName(material, "_TintColor", alpha);
        this.UpdateAlphaByPropertyName(material, "_MainColor", alpha);
      }
    }
  }

  private void UpdateAlphaByPropertyName(Material mat, string name, float alpha)
  {
    if (!mat.HasProperty(name))
      return;
    Color color = mat.GetColor(name);
    color.a = (__Null) (double) alpha;
    mat.SetColor(name, color);
  }

  public void UpdateColor(Color color)
  {
    if (Object.op_Equality((Object) this.MeshObject, (Object) null))
      return;
    ME_ColorHelper.ChangeObjectColorByHUE(this.MeshObject, ME_ColorHelper.ColorToHSV(color).H);
  }

  public void UpdateColor(float HUE)
  {
    if (Object.op_Equality((Object) this.MeshObject, (Object) null))
      return;
    ME_ColorHelper.ChangeObjectColorByHUE(this.MeshObject, HUE);
  }

  public void UpdateMeshEffect()
  {
    ((Component) this).get_transform().set_localPosition(Vector3.get_zero());
    ((Component) this).get_transform().set_localRotation((Quaternion) null);
    this.rendererMaterials.Clear();
    this.skinnedMaterials.Clear();
    if (Object.op_Equality((Object) this.MeshObject, (Object) null))
      return;
    this.UpdatePSMesh(this.MeshObject);
    this.AddMaterialToMesh(this.MeshObject);
  }

  private void CheckScaleIncludedParticles()
  {
  }

  public void UpdateMeshEffect(GameObject go)
  {
    this.rendererMaterials.Clear();
    this.skinnedMaterials.Clear();
    if (Object.op_Equality((Object) go, (Object) null))
    {
      Debug.Log((object) "You need set a gameObject");
    }
    else
    {
      this.MeshObject = go;
      this.UpdatePSMesh(this.MeshObject);
      this.AddMaterialToMesh(this.MeshObject);
    }
  }

  private void UpdatePSMesh(GameObject go)
  {
    ParticleSystem[] componentsInChildren1 = (ParticleSystem[]) ((Component) this).GetComponentsInChildren<ParticleSystem>();
    MeshRenderer componentInChildren1 = (MeshRenderer) go.GetComponentInChildren<MeshRenderer>();
    SkinnedMeshRenderer componentInChildren2 = (SkinnedMeshRenderer) go.GetComponentInChildren<SkinnedMeshRenderer>();
    Light[] componentsInChildren2 = (Light[]) ((Component) this).GetComponentsInChildren<Light>();
    float num = 1f;
    if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
    {
      Bounds bounds = ((Renderer) componentInChildren1).get_bounds();
      Vector3 size = ((Bounds) ref bounds).get_size();
      num = ((Vector3) ref size).get_magnitude();
    }
    if (Object.op_Inequality((Object) componentInChildren2, (Object) null))
    {
      Bounds bounds = ((Renderer) componentInChildren2).get_bounds();
      Vector3 size = ((Bounds) ref bounds).get_size();
      num = ((Vector3) ref size).get_magnitude();
    }
    Vector3 lossyScale = go.get_transform().get_lossyScale();
    float magnitude = ((Vector3) ref lossyScale).get_magnitude();
    foreach (ParticleSystem particleSystem in componentsInChildren1)
    {
      ((Component) ((Component) particleSystem).get_transform()).get_gameObject().SetActive(false);
      ParticleSystem.ShapeModule shape = particleSystem.get_shape();
      if (((ParticleSystem.ShapeModule) ref shape).get_enabled())
      {
        if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
        {
          ((ParticleSystem.ShapeModule) ref shape).set_shapeType((ParticleSystemShapeType) 13);
          ((ParticleSystem.ShapeModule) ref shape).set_meshRenderer(componentInChildren1);
        }
        if (Object.op_Inequality((Object) componentInChildren2, (Object) null))
        {
          ((ParticleSystem.ShapeModule) ref shape).set_shapeType((ParticleSystemShapeType) 14);
          ((ParticleSystem.ShapeModule) ref shape).set_skinnedMeshRenderer(componentInChildren2);
        }
      }
      ParticleSystem.MainModule main = particleSystem.get_main();
      ref ParticleSystem.MainModule local = ref main;
      ((ParticleSystem.MainModule) ref local).set_startSizeMultiplier(((ParticleSystem.MainModule) ref local).get_startSizeMultiplier() * (num / magnitude));
      ((Component) ((Component) particleSystem).get_transform()).get_gameObject().SetActive(true);
    }
    if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
    {
      foreach (Component component in componentsInChildren2)
      {
        Transform transform = component.get_transform();
        Bounds bounds = ((Renderer) componentInChildren1).get_bounds();
        Vector3 center = ((Bounds) ref bounds).get_center();
        transform.set_position(center);
      }
    }
    if (!Object.op_Inequality((Object) componentInChildren2, (Object) null))
      return;
    foreach (Component component in componentsInChildren2)
    {
      Transform transform = component.get_transform();
      Bounds bounds = ((Renderer) componentInChildren2).get_bounds();
      Vector3 center = ((Bounds) ref bounds).get_center();
      transform.set_position(center);
    }
  }

  private void AddMaterialToMesh(GameObject go)
  {
    ME_MeshMaterialEffect componentInChildren1 = (ME_MeshMaterialEffect) ((Component) this).GetComponentInChildren<ME_MeshMaterialEffect>();
    if (Object.op_Equality((Object) componentInChildren1, (Object) null))
      return;
    MeshRenderer componentInChildren2 = (MeshRenderer) go.GetComponentInChildren<MeshRenderer>();
    SkinnedMeshRenderer componentInChildren3 = (SkinnedMeshRenderer) go.GetComponentInChildren<SkinnedMeshRenderer>();
    if (Object.op_Inequality((Object) componentInChildren2, (Object) null))
    {
      this.rendererMaterials.Add(((Renderer) componentInChildren2).get_sharedMaterials());
      ((Renderer) componentInChildren2).set_sharedMaterials(this.AddToSharedMaterial(((Renderer) componentInChildren2).get_sharedMaterials(), componentInChildren1));
    }
    if (!Object.op_Inequality((Object) componentInChildren3, (Object) null))
      return;
    this.skinnedMaterials.Add(((Renderer) componentInChildren3).get_sharedMaterials());
    ((Renderer) componentInChildren3).set_sharedMaterials(this.AddToSharedMaterial(((Renderer) componentInChildren3).get_sharedMaterials(), componentInChildren1));
  }

  private Material[] AddToSharedMaterial(
    Material[] sharedMaterials,
    ME_MeshMaterialEffect meshMatEffect)
  {
    if (meshMatEffect.IsFirstMaterial)
      return new Material[1]{ meshMatEffect.Material };
    List<Material> list = ((IEnumerable<Material>) sharedMaterials).ToList<Material>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (((Object) list[index]).get_name().Contains("MeshEffect"))
        list.RemoveAt(index);
    }
    list.Add(meshMatEffect.Material);
    return list.ToArray();
  }

  private void OnDestroy()
  {
    if (Object.op_Equality((Object) this.MeshObject, (Object) null))
      return;
    MeshRenderer[] componentsInChildren1 = (MeshRenderer[]) this.MeshObject.GetComponentsInChildren<MeshRenderer>();
    SkinnedMeshRenderer[] componentsInChildren2 = (SkinnedMeshRenderer[]) this.MeshObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    for (int index1 = 0; index1 < componentsInChildren1.Length; ++index1)
    {
      if (this.rendererMaterials.Count == componentsInChildren1.Length)
        ((Renderer) componentsInChildren1[index1]).set_sharedMaterials(this.rendererMaterials[index1]);
      List<Material> list = ((IEnumerable<Material>) ((Renderer) componentsInChildren1[index1]).get_sharedMaterials()).ToList<Material>();
      for (int index2 = 0; index2 < list.Count; ++index2)
      {
        if (((Object) list[index2]).get_name().Contains("MeshEffect"))
          list.RemoveAt(index2);
      }
      ((Renderer) componentsInChildren1[index1]).set_sharedMaterials(list.ToArray());
    }
    for (int index1 = 0; index1 < componentsInChildren2.Length; ++index1)
    {
      if (this.skinnedMaterials.Count == componentsInChildren2.Length)
        ((Renderer) componentsInChildren2[index1]).set_sharedMaterials(this.skinnedMaterials[index1]);
      List<Material> list = ((IEnumerable<Material>) ((Renderer) componentsInChildren2[index1]).get_sharedMaterials()).ToList<Material>();
      for (int index2 = 0; index2 < list.Count; ++index2)
      {
        if (((Object) list[index2]).get_name().Contains("MeshEffect"))
          list.RemoveAt(index2);
      }
      ((Renderer) componentsInChildren2[index1]).set_sharedMaterials(list.ToArray());
    }
    this.rendererMaterials.Clear();
    this.skinnedMaterials.Clear();
  }
}
