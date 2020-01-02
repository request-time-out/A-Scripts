// Decompiled with JetBrains decompiler
// Type: CustomSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.IO;
using uAudio.uAudio_backend;
using UnityEngine;

public class CustomSound : MonoBehaviour
{
  public AudioSource myAudioSource;
  public PlayBackState State;
  public static uAudio.uAudio_backend.uAudio _uAudio;
  public string targetFile;
  private string szListFileName;
  private Action<PlayBackState> _sendPlaybackState;
  private bool _loadedTarget;
  private TimeSpan endSongTime;

  public CustomSound()
  {
    base.\u002Ector();
  }

  public Action<PlayBackState> sendPlaybackState
  {
    get
    {
      return this._sendPlaybackState;
    }
    set
    {
      this._sendPlaybackState = value;
    }
  }

  public uAudio.uAudio_backend.uAudio UAudio
  {
    get
    {
      if (CustomSound._uAudio == null)
      {
        CustomSound._uAudio = new uAudio.uAudio_backend.uAudio();
        CustomSound._uAudio.SetAudioFile(this.targetFile);
        CustomSound._uAudio.set_Volume(1f);
        CustomSound._uAudio.set_sendPlaybackState((Action<PlayBackState>) (c => this._sendPlaybackState(c)));
      }
      return CustomSound._uAudio;
    }
  }

  public TimeSpan CurrentTime
  {
    get
    {
      return CustomSound._uAudio != null && this.State != null ? CustomSound._uAudio.get_CurrentTime() : this.endSongTime;
    }
    set
    {
      if (CustomSound._uAudio == null)
        return;
      CustomSound._uAudio.set_CurrentTime(value);
    }
  }

  private void Start()
  {
    this.szListFileName = contents.AudioFileDirectory + "/BGMList.dat";
  }

  private void Update()
  {
    if (!mp3AudioClip.flare_SongEnd)
      return;
    mp3AudioClip.flare_SongEnd = false;
    this.SongEnd();
    mp3AudioClip.flare_SongEnd = false;
  }

  public void LoadFile(string szTargetName)
  {
    if (szTargetName != this.targetFile)
      this.SongEnd();
    this.targetFile = szTargetName;
    ExternalAudioClip.LoadFile(szTargetName, this.targetFile, ref this._loadedTarget, this.UAudio);
  }

  public void SongPlay()
  {
    if (this.State == 1)
      return;
    this.State = (PlayBackState) 1;
    try
    {
      mp3AudioClip.SongDone = false;
      mp3AudioClip.flare_SongEnd = false;
      this.UAudio.targetFile = (__Null) this.targetFile;
      if (Object.op_Equality((Object) this.myAudioSource.get_clip(), (Object) null))
      {
        if (this.UAudio.LoadMainOutputStream())
        {
          long songLength = (long) this.UAudio.get_SongLength();
          string szErrorMs = (string) null;
          this.myAudioSource.set_clip(ExternalAudioClip.Load(this.targetFile, (long) this.UAudio.get_SongLength(), this.UAudio, ref szErrorMs));
          this.CurrentTime = TimeSpan.Zero;
          try
          {
            if (this.sendPlaybackState != null)
              this.sendPlaybackState((PlayBackState) 1);
          }
          catch
          {
            Debug.LogWarning((object) "theAudioStream_sendStartLoopPump #32fw46hw465h45h");
          }
        }
        else
          this.myAudioSource.set_clip((AudioClip) null);
      }
      if (Object.op_Inequality((Object) this.myAudioSource.get_clip(), (Object) null))
      {
        if (this.myAudioSource.get_isPlaying())
          return;
        this.myAudioSource.Play();
      }
      else
        this.State = (PlayBackState) 0;
    }
    catch (Exception ex)
    {
      this.State = (PlayBackState) 0;
      Debug.LogWarning((object) "uAudioPlayer - Play #j356j536j356j56j");
      Debug.LogWarning((object) ex);
    }
  }

  public void SongEnd()
  {
    if (this.State == null)
      return;
    try
    {
      this.endSongTime = this.CurrentTime;
      this.myAudioSource.Stop();
      CustomSound._uAudio.Stop();
      this.myAudioSource.set_clip((AudioClip) null);
      this._loadedTarget = false;
      this.State = (PlayBackState) 0;
      try
      {
        if (this.sendPlaybackState == null)
          return;
        this.sendPlaybackState((PlayBackState) 0);
      }
      catch
      {
        Debug.LogWarning((object) "sendPlaybackState #897j8h2432a1q");
      }
    }
    catch
    {
      throw new Exception("Song end #7cgf87dcf7sd8csd");
    }
  }

  public void BGMListToFile(string[] SongPath)
  {
    this.szListFileName = contents.AudioFileDirectory + "/BGMList.dat";
    this.szListFileName.Remove(0, Application.get_dataPath().ToString().Length - "Assets".Length);
    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.szListFileName);
    try
    {
      StreamWriter text = fileInfo.CreateText();
      for (int index = 0; index < SongPath.Length; ++index)
        text.WriteLine(SongPath[index]);
      text.Flush();
      text.Close();
    }
    catch
    {
      Debug.LogWarning((object) "Failed CreateText");
    }
  }

  public void FileToBGMList(string[] SongPath)
  {
    this.szListFileName = contents.AudioFileDirectory + "/BGMList.dat";
    this.szListFileName.Remove(0, Application.get_dataPath().ToString().Length - "Assets".Length);
    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.szListFileName);
    try
    {
      StreamReader streamReader = fileInfo.OpenText();
      for (int index = 0; index < SongPath.Length; ++index)
      {
        SongPath[index] = streamReader.ReadLine();
        if (SongPath[index] == null)
          SongPath[index] = "Empty";
      }
      streamReader.Close();
    }
    catch
    {
      Debug.LogWarning((object) "Failed FileOpen Error");
    }
  }

  private void OnApplicationQuit()
  {
    this.Dispose();
  }

  public void Dispose()
  {
    if (CustomSound._uAudio != null)
    {
      CustomSound._uAudio.Dispose();
      CustomSound._uAudio = (uAudio.uAudio_backend.uAudio) null;
    }
    this._loadedTarget = false;
  }

  public void Resume()
  {
    throw new NotImplementedException();
  }
}
