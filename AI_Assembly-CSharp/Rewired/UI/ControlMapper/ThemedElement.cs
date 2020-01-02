// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ThemedElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class ThemedElement : MonoBehaviour
  {
    [SerializeField]
    private ThemedElement.ElementInfo[] _elements;

    public ThemedElement()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      Rewired.UI.ControlMapper.ControlMapper.ApplyTheme(this._elements);
    }

    [Serializable]
    public class ElementInfo
    {
      [SerializeField]
      private string _themeClass;
      [SerializeField]
      private Component _component;

      public string themeClass
      {
        get
        {
          return this._themeClass;
        }
      }

      public Component component
      {
        get
        {
          return this._component;
        }
      }
    }
  }
}
