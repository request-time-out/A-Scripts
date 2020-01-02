// Decompiled with JetBrains decompiler
// Type: Studio.BGMCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.IO;
using UnityEngine;

namespace Studio
{
  public class BGMCtrl
  {
    private BGMCtrl.Repeat m_Repeat = BGMCtrl.Repeat.All;
    private int m_No;
    private bool m_Play;
    private AudioSource audioSource;

    public BGMCtrl.Repeat repeat
    {
      get
      {
        return this.m_Repeat;
      }
      set
      {
        if (!Utility.SetStruct<BGMCtrl.Repeat>(ref this.m_Repeat, value) || !Object.op_Implicit((Object) this.audioSource))
          return;
        this.audioSource.set_loop(this.repeat == BGMCtrl.Repeat.All);
      }
    }

    public int no
    {
      get
      {
        return this.m_No;
      }
      set
      {
        if (this.m_No == value)
          return;
        this.Play(value);
      }
    }

    public bool play
    {
      get
      {
        return this.m_Play;
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_Play, value))
          return;
        if (this.m_Play)
          this.Play();
        else
          this.Stop();
      }
    }

    public bool isPause { get; private set; }

    public void Play()
    {
      if (this.isPause)
      {
        this.isPause = false;
        Singleton<Sound>.Instance.PlayBGM(0.0f);
      }
      else
      {
        this.m_Play = true;
        this.Play(this.m_No);
      }
    }

    public void Play(int _no)
    {
      this.m_No = _no;
      if (!this.m_Play)
        return;
      Info.LoadCommonInfo loadCommonInfo = (Info.LoadCommonInfo) null;
      if (!Singleton<Info>.Instance.dicBGMLoadInfo.TryGetValue(this.m_No, out loadCommonInfo))
        return;
      if (Singleton<Studio.Studio>.Instance.outsideSoundCtrl.play)
        Singleton<Studio.Studio>.Instance.outsideSoundCtrl.Stop();
      Transform transform = Singleton<Sound>.Instance.Play(Sound.Type.BGM, loadCommonInfo.bundlePath, loadCommonInfo.fileName, 0.0f, 0.0f, true, false, -1, true);
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      this.audioSource = (AudioSource) ((Component) transform).GetComponent<AudioSource>();
      this.audioSource.set_loop(this.repeat == BGMCtrl.Repeat.All);
      this.isPause = false;
    }

    public void Stop()
    {
      this.m_Play = false;
      this.isPause = false;
      Singleton<Sound>.Instance.StopBGM(0.0f);
    }

    public void Pause()
    {
      if (!this.m_Play)
        return;
      this.isPause = true;
      Singleton<Sound>.Instance.PauseBGM();
    }

    public void Save(BinaryWriter _writer, Version _version)
    {
      _writer.Write((int) this.m_Repeat);
      _writer.Write(this.m_No);
      _writer.Write(this.m_Play);
    }

    public void Load(BinaryReader _reader, Version _version)
    {
      this.m_Repeat = (BGMCtrl.Repeat) _reader.ReadInt32();
      this.m_No = _reader.ReadInt32();
      this.m_Play = _reader.ReadBoolean();
    }

    public enum Repeat
    {
      None,
      All,
    }
  }
}
