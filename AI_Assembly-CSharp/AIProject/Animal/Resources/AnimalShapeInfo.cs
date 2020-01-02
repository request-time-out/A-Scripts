// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Resources.AnimalShapeInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using UnityEngine;

namespace AIProject.Animal.Resources
{
  public struct AnimalShapeInfo
  {
    public bool Enabled;
    public string Name;
    public int Index;
    public Transform Root;
    public SkinnedMeshRenderer Renderer;

    public AnimalShapeInfo(string _name, int _index)
    {
      this.Name = _name;
      this.Index = _index;
      this.Enabled = !this.Name.IsNullOrEmpty() && 0 <= this.Index;
      this.Root = (Transform) null;
      this.Renderer = (SkinnedMeshRenderer) null;
    }

    public AnimalShapeInfo(bool _enabled, string _name, int _index)
    {
      this.Enabled = _enabled;
      this.Name = _name;
      this.Index = _index;
      this.Root = (Transform) null;
      this.Renderer = (SkinnedMeshRenderer) null;
    }

    public bool Active
    {
      get
      {
        return this.Enabled && !this.Name.IsNullOrEmpty() && 0 <= this.Index && Object.op_Inequality((Object) this.Renderer, (Object) null);
      }
    }

    public void ClearState()
    {
      this.Root = (Transform) null;
      this.Renderer = (SkinnedMeshRenderer) null;
    }

    public bool SetRenderer(Transform _transform, string _findName = null)
    {
      if (Object.op_Equality((Object) _transform, (Object) null))
        return false;
      GameObject loop = _transform.FindLoop(_findName ?? this.Name);
      if (Object.op_Equality((Object) loop, (Object) null))
        return false;
      SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer) loop.GetComponent<SkinnedMeshRenderer>();
      if (Object.op_Equality((Object) skinnedMeshRenderer, (Object) null))
        skinnedMeshRenderer = (SkinnedMeshRenderer) loop.GetComponentInChildren<SkinnedMeshRenderer>(true);
      bool flag = Object.op_Inequality((Object) skinnedMeshRenderer, (Object) null);
      if (flag)
      {
        this.Root = _transform;
        this.Renderer = skinnedMeshRenderer;
      }
      return flag;
    }

    public float GetBlendShape()
    {
      return !this.Active ? 0.0f : this.Renderer.GetBlendShapeWeight(this.Index);
    }

    public void SetBlendShape(float _value)
    {
      if (!this.Active)
        return;
      this.Renderer.SetBlendShapeWeight(this.Index, Mathf.Clamp(_value, 0.0f, 100f));
    }
  }
}
