// Decompiled with JetBrains decompiler
// Type: ExternalAudioClip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class ExternalAudioClip
{
  public static readonly float RangeValue8Bit = 1f / Mathf.Pow(2f, 7f);
  public static readonly float RangeValue16Bit = 1f / Mathf.Pow(2f, 15f);
  public static readonly float RangeValue24Bit = 1f / Mathf.Pow(2f, 23f);
  public static readonly float RangeValue32Bit = 1f / Mathf.Pow(2f, 31f);
  public const int BaseConvertSamples = 20480;

  public static AudioClip Load(
    string path,
    long SongLength,
    uAudio.uAudio_backend.uAudio uAudio,
    ref string szErrorMs)
  {
    string extension = Path.GetExtension(path);
    if (extension == ".wav" || extension == ".WAV")
    {
      WaveHeader waveHeader = WaveHeader.ReadWaveHeader(path);
      float[] rangedRawData = ExternalAudioClip.CreateRangedRawData(path, waveHeader.TrueWavBufIndex, waveHeader.TrueSamples, (int) waveHeader.Channels, (int) waveHeader.BitPerSample);
      return rangedRawData.Length == 0 ? (AudioClip) null : ExternalAudioClip.CreateClip(Path.GetFileNameWithoutExtension(path), rangedRawData, waveHeader.TrueSamples, (int) waveHeader.Channels, waveHeader.Frequency);
    }
    if (!(extension == ".mp3") && !(extension == ".MP3"))
      return (AudioClip) null;
    mp3AudioClip._uAudio = uAudio;
    AudioClip audioClip = mp3AudioClip.LoadMp3(path, SongLength);
    szErrorMs = mp3AudioClip.szErrorMs;
    return audioClip;
  }

  public static AudioClip CreateClip(
    string name,
    float[] rawData,
    int lengthSamples,
    int channels,
    int frequency)
  {
    AudioClip audioClip = AudioClip.Create(name, lengthSamples, channels, frequency, false);
    audioClip.SetData(rawData, 0);
    return audioClip;
  }

  public static float[] CreateRangedRawData(
    string path,
    int wavBufIndex,
    int samples,
    int channels,
    int bitPerSample)
  {
    byte[] data = File.ReadAllBytes(path);
    return data.Length == 0 ? (float[]) null : ExternalAudioClip.CreateRangedRawData(data, wavBufIndex, samples, channels, bitPerSample);
  }

  public static float[] CreateRangedRawData(
    byte[] data,
    int wavBufIndex,
    int samples,
    int channels,
    int bitPerSample)
  {
    float[] numArray = new float[samples * channels];
    int num = bitPerSample / 8;
    int index1 = wavBufIndex;
    try
    {
      for (int index2 = 0; index2 < numArray.Length; ++index2)
      {
        numArray[index2] = ExternalAudioClip.ByteToFloat(data, index1, bitPerSample);
        index1 += num;
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.ToString() + ": 対応してない音声ファイル");
      return Enumerable.Empty<float>().ToArray<float>();
    }
    return numArray;
  }

  private static float ByteToFloat(byte[] data, int index, int bitPerSample)
  {
    if (bitPerSample == 8)
      return (float) ((int) data[index] - 128) * ExternalAudioClip.RangeValue8Bit;
    if (bitPerSample == 16)
      return (float) BitConverter.ToInt16(data, index) * ExternalAudioClip.RangeValue16Bit;
    throw new Exception(bitPerSample.ToString() + "bit is not supported.");
  }

  public static void LoadFile(
    string targetFileIN,
    string targetFile,
    ref bool _loadedTarget,
    uAudio.uAudio_backend.uAudio uAudio)
  {
    if (_loadedTarget && !((string) uAudio.targetFile != targetFile))
      return;
    _loadedTarget = true;
    uAudio.LoadFile(targetFileIN);
  }
}
