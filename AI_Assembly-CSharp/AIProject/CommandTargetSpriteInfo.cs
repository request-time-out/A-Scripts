// Decompiled with JetBrains decompiler
// Type: AIProject.CommandTargetSpriteInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public class CommandTargetSpriteInfo
  {
    [SerializeField]
    private float _fps = 1f;
    [SerializeField]
    private Sprite[] _sprites;
    [SerializeField]
    private Sprite[] _selectedSprites;
    [SerializeField]
    private Sprite[] _disableSprites;
    [SerializeField]
    private Sprite[] _coolTimeSprites;

    public float FPS
    {
      get
      {
        return this._fps;
      }
    }

    public Sprite[] Sprites
    {
      get
      {
        return this._sprites;
      }
    }

    public Sprite[] SelectedSprites
    {
      get
      {
        return this._selectedSprites;
      }
    }

    public Sprite[] DisableSprites
    {
      get
      {
        return this._disableSprites;
      }
    }

    public Sprite[] CoolTimeSprites
    {
      get
      {
        return this._coolTimeSprites;
      }
    }
  }
}
