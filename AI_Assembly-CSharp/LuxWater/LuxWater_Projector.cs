// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_Projector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuxWater
{
  public class LuxWater_Projector : MonoBehaviour
  {
    [NonSerialized]
    public static List<LuxWater_Projector> FoamProjectors = new List<LuxWater_Projector>();
    [NonSerialized]
    public static List<LuxWater_Projector> NormalProjectors = new List<LuxWater_Projector>();
    [Space(8f)]
    public LuxWater_Projector.ProjectorType Type;
    [NonSerialized]
    public Renderer m_Rend;
    [NonSerialized]
    public Material m_Mat;
    private bool added;
    private Vector3 origPos;

    public LuxWater_Projector()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      ((Component) this).get_transform().get_position().y = this.origPos.y;
    }

    private void OnEnable()
    {
      this.origPos = ((Component) this).get_transform().get_position();
      if (!Object.op_Inequality((Object) ((Component) this).GetComponent<Renderer>(), (Object) null))
        return;
      this.m_Rend = (Renderer) ((Component) this).GetComponent<Renderer>();
      this.m_Mat = this.m_Rend.get_sharedMaterials()[0];
      this.m_Rend.set_enabled(false);
      if (this.Type == LuxWater_Projector.ProjectorType.FoamProjector)
        LuxWater_Projector.FoamProjectors.Add(this);
      else
        LuxWater_Projector.NormalProjectors.Add(this);
      this.added = true;
    }

    private void OnDisable()
    {
      if (!this.added)
        return;
      if (this.Type == LuxWater_Projector.ProjectorType.FoamProjector)
        LuxWater_Projector.FoamProjectors.Remove(this);
      else
        LuxWater_Projector.NormalProjectors.Remove(this);
      this.m_Rend.set_enabled(true);
    }

    public enum ProjectorType
    {
      FoamProjector,
      NormalProjector,
    }
  }
}
