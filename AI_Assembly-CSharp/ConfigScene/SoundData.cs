// Decompiled with JetBrains decompiler
// Type: ConfigScene.SoundData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;

namespace ConfigScene
{
  public class SoundData
  {
    private int _volume;
    private bool _munte;

    public SoundData()
    {
      this._volume = 100;
      this._munte = false;
    }

    public event Action<SoundData> ChangeEvent;

    public int Volume
    {
      get
      {
        return this._volume;
      }
      set
      {
        bool flag = this._volume != value;
        this._volume = value;
        if (!flag || this.ChangeEvent.IsNullOrEmpty())
          return;
        this.ChangeEvent(this);
      }
    }

    public bool Mute
    {
      get
      {
        return this._munte;
      }
      set
      {
        bool flag = this._munte != value;
        this._munte = value;
        if (!flag || this.ChangeEvent.IsNullOrEmpty())
          return;
        this.ChangeEvent(this);
      }
    }

    public float GetVolume()
    {
      return this.Mute ? 0.0f : (float) this.Volume * 0.01f;
    }

    public static implicit operator string(SoundData _sd)
    {
      return string.Format("Volume[{0}] : Mute[{1}]", (object) _sd.Volume, (object) _sd.Mute);
    }

    public void Refresh()
    {
      if (this.ChangeEvent == null)
        return;
      this.ChangeEvent(this);
    }

    public void Parse(string _str)
    {
      Match match = Regex.Match(_str, "Volume\\[([0-9]*)\\] : Mute\\[([a-zA-Z]*)\\]");
      if (!match.Success)
        return;
      this.Volume = int.Parse(match.Groups[1].Value);
      this.Mute = bool.Parse(match.Groups[2].Value);
    }
  }
}
