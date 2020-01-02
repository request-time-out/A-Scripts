// Decompiled with JetBrains decompiler
// Type: Studio.OutsideSoundCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.IO;
using UnityEngine;

namespace Studio
{
  public class OutsideSoundCtrl
  {
    private BGMCtrl.Repeat m_Repeat = BGMCtrl.Repeat.All;
    private string m_FileName = string.Empty;
    public const string dataPath = "audio";
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

    public string fileName
    {
      get
      {
        return this.m_FileName;
      }
      set
      {
        if (!(this.m_FileName != value))
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

    public void Play()
    {
      this.m_Play = true;
      this.Play(this.m_FileName);
    }

    public void Play(string _file)
    {
      this.m_FileName = _file;
      if (!this.m_Play)
        return;
      string path = UserData.Create("audio") + _file;
      if (!File.Exists(path))
        return;
      if (Singleton<Studio.Studio>.Instance.bgmCtrl.play)
        Singleton<Studio.Studio>.Instance.bgmCtrl.Stop();
      Singleton<Sound>.Instance.StopBGM(0.0f);
      string empty = string.Empty;
      AudioClip clip = ExternalAudioClip.Load(path, 0L, (uAudio.uAudio_backend.uAudio) null, ref empty);
      if (Object.op_Equality((Object) clip, (Object) null))
        Debug.LogWarning((object) string.Format("読めないよ : {0}", (object) _file));
      else
        Singleton<Sound>.Instance.Play(Sound.Type.BGM, clip, 0.0f);
    }

    public void Stop()
    {
      this.m_Play = false;
      Singleton<Sound>.Instance.Stop(Sound.Type.BGM);
    }

    public void Save(BinaryWriter _writer, Version _version)
    {
      _writer.Write((int) this.m_Repeat);
      _writer.Write(this.m_FileName);
      _writer.Write(this.m_Play);
    }

    public void Load(BinaryReader _reader, Version _version)
    {
      this.m_Repeat = (BGMCtrl.Repeat) _reader.ReadInt32();
      this.m_FileName = _reader.ReadString();
      this.m_Play = _reader.ReadBoolean();
    }
  }
}
