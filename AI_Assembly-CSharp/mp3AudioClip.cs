// Decompiled with JetBrains decompiler
// Type: mp3AudioClip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using NAudio.Wave;
using NLayer;
using System;
using System.IO;
using uAudio.uAudio_backend;
using uAudioDemo.Mp3StreamingDemo;
using UnityEngine;

internal class mp3AudioClip
{
  private static AudioClip.PCMReaderCallback SongReadLoop;
  private static ReadFullyStream readFullyStream;
  private static MpegFile playbackDevice;
  public static uAudio.uAudio_backend.uAudio _uAudio;
  public static bool SongDone;
  public static bool flare_SongEnd;

  public static string szErrorMs { get; set; }

  public static AudioClip LoadMp3(string targetFile, long SongLength)
  {
    // ISSUE: method pointer
    mp3AudioClip.SongReadLoop = new AudioClip.PCMReaderCallback((object) null, __methodptr(Song_Stream_Loop));
    Stream stream = (Stream) File.OpenRead(targetFile);
    if (mp3AudioClip.readFullyStream != null)
      ((Stream) mp3AudioClip.readFullyStream).Dispose();
    mp3AudioClip.readFullyStream = new ReadFullyStream(stream);
    mp3AudioClip.readFullyStream.stream_CanSeek = (__Null) 1;
    byte[] numArray = new byte[1024];
    MpegFile mpegFile = new MpegFile((Stream) mp3AudioClip.readFullyStream, true);
    mpegFile.ReadSamples(numArray, 0, numArray.Length);
    mp3AudioClip.playbackDevice = mpegFile;
    long num1 = SongLength;
    int num2;
    if (num1 > (long) int.MaxValue)
    {
      Debug.LogWarning((object) "uAudioPlayer - Song size over size on int #4sgh54h45h45");
      num2 = int.MaxValue;
    }
    else
      num2 = (int) num1;
    return AudioClip.Create("uAudio_song", num2, mpegFile.get_WaveFormat().get_Channels(), mpegFile.get_WaveFormat().get_SampleRate(), true, mp3AudioClip.SongReadLoop);
  }

  private static void Song_Stream_Loop(float[] data)
  {
    try
    {
      if (!mp3AudioClip.SongDone)
      {
        if (((AudioFileReader) ((AudioPlayback) mp3AudioClip._uAudio.get_uwa().audioPlayback).inputStream).Read(data, 0, data.Length) > 0)
          return;
        mp3AudioClip.SongDone = true;
      }
      else
        mp3AudioClip.flare_SongEnd = true;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("#trg56hgtyhty" + ex.Message));
      Debug.LogError((object) "Decode Error #8f76s8dsvsd");
      mp3AudioClip.szErrorMs = "Failed Play Sound\0";
    }
  }
}
