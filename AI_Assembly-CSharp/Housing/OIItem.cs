// Decompiled with JetBrains decompiler
// Type: Housing.OIItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Housing
{
  [MessagePackObject(false)]
  public class OIItem : IObjectInfo
  {
    public OIItem()
    {
      this.Color1 = Color.get_white();
      this.Color2 = Color.get_white();
      this.Color3 = Color.get_white();
      this.EmissionColor = Color.get_clear();
      this.VisibleOption = true;
    }

    public OIItem(OIItem _src)
    {
      this.Pos = _src.Pos;
      this.Rot = _src.Rot;
      this.Category = _src.Category;
      this.ID = _src.ID;
      this.Color1 = _src.Color1;
      this.Color2 = _src.Color2;
      this.Color3 = _src.Color3;
      this.EmissionColor = _src.EmissionColor;
      this.VisibleOption = _src.VisibleOption;
    }

    public OIItem(OIItem _src, bool _idCopy)
    {
      this.Pos = _src.Pos;
      this.Rot = _src.Rot;
      this.Category = _src.Category;
      this.ID = _src.ID;
      if (_idCopy)
      {
        this.ActionPoints = this.Clone(_src.ActionPoints);
        this.FarmPoints = this.Clone(_src.FarmPoints);
        this.PetHomePoints = this.Clone(_src.PetHomePoints);
        this.JukePoints = this.Clone(_src.JukePoints);
        this.CraftPoints = this.Clone(_src.CraftPoints);
        this.LightSwitchPoints = this.Clone(_src.LightSwitchPoints);
      }
      this.Color1 = _src.Color1;
      this.Color2 = _src.Color2;
      this.Color3 = _src.Color3;
      this.EmissionColor = _src.EmissionColor;
      this.VisibleOption = _src.VisibleOption;
    }

    [IgnoreMember]
    public int Kind
    {
      get
      {
        return 0;
      }
    }

    [Key(0)]
    public Vector3 Pos { get; set; } = Vector3.get_zero();

    [Key(1)]
    public Vector3 Rot { get; set; } = Vector3.get_zero();

    [Key(2)]
    public int Category { get; set; }

    [Key(3)]
    public int ID { get; set; }

    [Key(4)]
    public int[] ActionPoints { get; set; }

    [Key(5)]
    public int[] FarmPoints { get; set; }

    [Key(6)]
    public Color Color1 { get; set; }

    [Key(7)]
    public Color Color2 { get; set; }

    [Key(8)]
    public Color Color3 { get; set; }

    [Key(9)]
    public Color EmissionColor { get; set; }

    [Key(10)]
    public int[] PetHomePoints { get; set; }

    [Key(11)]
    public int[] JukePoints { get; set; }

    [Key(12)]
    public int[] CraftPoints { get; set; }

    [Key(13)]
    public bool VisibleOption { get; set; } = true;

    [Key(14)]
    public int[] LightSwitchPoints { get; set; }

    private int[] Clone(int[] _src)
    {
      if (((IList<int>) _src).IsNullOrEmpty<int>())
        return (int[]) null;
      int[] numArray = new int[_src.Length];
      Array.Copy((Array) _src, (Array) numArray, _src.Length);
      return numArray;
    }
  }
}
