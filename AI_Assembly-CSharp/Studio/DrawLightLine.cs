// Decompiled with JetBrains decompiler
// Type: Studio.DrawLightLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Studio
{
  public class DrawLightLine : MonoBehaviour
  {
    [SerializeField]
    private Shader m_Shader;
    private Dictionary<Light, bool> dicLight;

    public DrawLightLine()
    {
      base.\u002Ector();
    }

    public void Add(Light _light)
    {
      this.dicLight.Add(_light, true);
    }

    public void Remove(Light _light)
    {
      this.dicLight.Remove(_light);
    }

    public void Clear()
    {
      this.dicLight.Clear();
    }

    public void SetEnable(Light _light, bool _value)
    {
      if (!this.dicLight.ContainsKey(_light))
        return;
      this.dicLight[_light] = _value;
    }

    private void Start()
    {
      LightLine.shader = this.m_Shader;
    }

    public void OnPostRender()
    {
      if (this.dicLight.Count <= 0)
        return;
      using (Dictionary<Light, bool>.Enumerator enumerator = this.dicLight.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<Light, bool> current = enumerator.Current;
          if (current.Value)
            LightLine.DrawLine(current.Key);
        }
      }
    }
  }
}
