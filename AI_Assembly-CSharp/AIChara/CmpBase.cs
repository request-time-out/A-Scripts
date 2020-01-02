// Decompiled with JetBrains decompiler
// Type: AIChara.CmpBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  public abstract class CmpBase : MonoBehaviour
  {
    private bool baseDB;
    private DynamicBone[] dynamicBones;
    [Header("カメラ内判定用")]
    public Renderer[] rendCheckVisible;
    [Button("Reacquire", "再取得", new object[] {})]
    public int reacquire;

    public CmpBase(bool _baseDB)
    {
      this.\u002Ector();
      this.baseDB = _baseDB;
    }

    protected virtual void Reacquire()
    {
      this.rendCheckVisible = (Renderer[]) ((Component) this).GetComponentsInChildren<Renderer>(true);
    }

    private void Reset()
    {
      this.SetReferenceObject();
      this.rendCheckVisible = (Renderer[]) ((Component) this).GetComponentsInChildren<Renderer>(true);
    }

    public abstract void SetReferenceObject();

    public void InitDynamicBones()
    {
      this.dynamicBones = (DynamicBone[]) null;
      if (!this.baseDB)
        return;
      this.dynamicBones = (DynamicBone[]) ((Component) this).GetComponentsInChildren<DynamicBone>(true);
    }

    public void ResetDynamicBones(bool includeInactive = false)
    {
      if (!this.baseDB || this.dynamicBones == null)
        return;
      foreach (DynamicBone dynamicBone in this.dynamicBones)
      {
        if (((Behaviour) dynamicBone).get_enabled() || includeInactive)
          dynamicBone.ResetParticlesPosition();
      }
    }

    public void EnableDynamicBones(bool enable)
    {
      if (!this.baseDB || this.dynamicBones == null)
        return;
      foreach (DynamicBone dynamicBone in this.dynamicBones)
      {
        if (((Behaviour) dynamicBone).get_enabled() != enable)
        {
          ((Behaviour) dynamicBone).set_enabled(enable);
          if (enable)
            dynamicBone.ResetParticlesPosition();
        }
      }
    }

    public bool isVisible
    {
      get
      {
        if (this.rendCheckVisible == null || this.rendCheckVisible.Length == 0)
          return false;
        foreach (Renderer renderer in this.rendCheckVisible)
        {
          if (renderer.get_isVisible())
            return true;
        }
        return false;
      }
    }
  }
}
