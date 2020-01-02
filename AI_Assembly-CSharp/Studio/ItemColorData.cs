// Decompiled with JetBrains decompiler
// Type: Studio.ItemColorData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class ItemColorData : SerializedScriptableObject
  {
    [SerializeField]
    [ReadOnly]
    private Dictionary<int, Dictionary<int, Dictionary<int, ItemColorData.ColorData>>> colorDatas;

    public ItemColorData()
    {
      base.\u002Ector();
    }

    public Dictionary<int, Dictionary<int, Dictionary<int, ItemColorData.ColorData>>> ColorDatas
    {
      get
      {
        return this.colorDatas;
      }
      set
      {
        this.colorDatas = value;
      }
    }

    [Serializable]
    public class ColorData
    {
      [SerializeField]
      [ReadOnly]
      private bool[] colors;

      public ColorData(ItemComponent _itemComponent, ParticleComponent _particleComponent)
      {
        bool[] useColor = _itemComponent.useColor;
        bool flag = Object.op_Inequality((Object) _particleComponent, (Object) null) && _particleComponent.check;
        this.colors = new bool[4]
        {
          useColor.SafeGet<bool>(0) | flag,
          useColor.SafeGet<bool>(1),
          useColor.SafeGet<bool>(2),
          _itemComponent.checkGlass
        };
      }

      public ColorData(ItemColorData.ColorData _src)
      {
        this.colors = new bool[4]
        {
          _src.IsColor1,
          _src.IsColor2,
          _src.IsColor3,
          _src.IsColor4
        };
      }

      public bool IsColor1
      {
        get
        {
          return this.colors.SafeGet<bool>(0);
        }
      }

      public bool IsColor2
      {
        get
        {
          return this.colors.SafeGet<bool>(1);
        }
      }

      public bool IsColor3
      {
        get
        {
          return this.colors.SafeGet<bool>(2);
        }
      }

      public bool IsColor4
      {
        get
        {
          return this.colors.SafeGet<bool>(3);
        }
      }

      public int Count
      {
        get
        {
          return ((IEnumerable<bool>) this.colors).Count<bool>((Func<bool, bool>) (_b => _b));
        }
      }
    }
  }
}
