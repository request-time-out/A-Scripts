// Decompiled with JetBrains decompiler
// Type: AIProject.ItemIconIDDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public class ItemIconIDDefine
  {
    [SerializeField]
    private int _umbrella = -1;
    [SerializeField]
    private int _torch = -1;
    [SerializeField]
    private int _lamp = -1;
    [SerializeField]
    private int _flashlight = -1;

    public int Umbrella
    {
      get
      {
        return this._umbrella;
      }
    }

    public int Torch
    {
      get
      {
        return this._torch;
      }
    }

    public int Lamp
    {
      get
      {
        return this._lamp;
      }
    }

    public int Flashlight
    {
      get
      {
        return this._flashlight;
      }
    }
  }
}
