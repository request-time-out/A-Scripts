// Decompiled with JetBrains decompiler
// Type: AIProject.ReverbSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class ReverbSetting : MonoBehaviour
  {
    public ReverbSetting()
    {
      base.\u002Ector();
    }

    public void LoadFromExcelData(ExcelData data)
    {
      if (Object.op_Equality((Object) data, (Object) null) || data.MaxCell <= 1)
        return;
      List<ReverbSetting.ReverbInfo> toRelease1 = ListPool<ReverbSetting.ReverbInfo>.Get();
      for (int index1 = 1; index1 < data.MaxCell; ++index1)
      {
        List<string> list = data.list[index1].list;
        if (!list.IsNullOrEmpty<string>())
        {
          int num1 = 0;
          List<string> source1 = list;
          int index2 = num1;
          int num2 = index2 + 1;
          string s = source1.GetElement<string>(index2) ?? string.Empty;
          if (!(s == "end"))
          {
            float result1;
            if (float.TryParse(s, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              float result2;
              if (float.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                int result3;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                {
                  if (result3 < 0 || 27 < result3)
                    result3 = 1;
                  AudioReverbPreset audioReverbPreset = (AudioReverbPreset) result3;
                  if (audioReverbPreset != 27)
                  {
                    ReverbSetting.ReverbInfo reverbInfo = new ReverbSetting.ReverbInfo()
                    {
                      MinDistance = result1,
                      MaxDistance = result2,
                      ReverbPreset = audioReverbPreset
                    };
                    toRelease1.Add(reverbInfo);
                  }
                  else
                  {
                    List<string> source4 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    int result4;
                    if (!int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result4))
                      result4 = -1000;
                    List<string> source5 = list;
                    int index6 = num5;
                    int num6 = index6 + 1;
                    int result5;
                    if (!int.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result5))
                      result5 = -100;
                    List<string> source6 = list;
                    int index7 = num6;
                    int num7 = index7 + 1;
                    int result6;
                    if (!int.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result6))
                      result6 = 0;
                    List<string> source7 = list;
                    int index8 = num7;
                    int num8 = index8 + 1;
                    float result7;
                    if (!float.TryParse(source7.GetElement<string>(index8) ?? string.Empty, out result7))
                      result7 = 1.49f;
                    List<string> source8 = list;
                    int index9 = num8;
                    int num9 = index9 + 1;
                    float result8;
                    if (!float.TryParse(source8.GetElement<string>(index9) ?? string.Empty, out result8))
                      result8 = 0.83f;
                    List<string> source9 = list;
                    int index10 = num9;
                    int num10 = index10 + 1;
                    int result9;
                    if (!int.TryParse(source9.GetElement<string>(index10) ?? string.Empty, out result9))
                      result9 = -2602;
                    List<string> source10 = list;
                    int index11 = num10;
                    int num11 = index11 + 1;
                    float result10;
                    if (!float.TryParse(source10.GetElement<string>(index11) ?? string.Empty, out result10))
                      result10 = 0.007f;
                    List<string> source11 = list;
                    int index12 = num11;
                    int num12 = index12 + 1;
                    int result11;
                    if (!int.TryParse(source11.GetElement<string>(index12) ?? string.Empty, out result11))
                      result11 = 200;
                    List<string> source12 = list;
                    int index13 = num12;
                    int num13 = index13 + 1;
                    float result12;
                    if (!float.TryParse(source12.GetElement<string>(index13) ?? string.Empty, out result12))
                      result12 = 11f / 1000f;
                    List<string> source13 = list;
                    int index14 = num13;
                    int num14 = index14 + 1;
                    int result13;
                    if (!int.TryParse(source13.GetElement<string>(index14) ?? string.Empty, out result13))
                      result13 = 5000;
                    List<string> source14 = list;
                    int index15 = num14;
                    int num15 = index15 + 1;
                    int result14;
                    if (!int.TryParse(source14.GetElement<string>(index15) ?? string.Empty, out result14))
                      result14 = 250;
                    List<string> source15 = list;
                    int index16 = num15;
                    int num16 = index16 + 1;
                    float result15;
                    if (!float.TryParse(source15.GetElement<string>(index16) ?? string.Empty, out result15))
                      result15 = 100f;
                    List<string> source16 = list;
                    int index17 = num16;
                    int num17 = index17 + 1;
                    float result16;
                    if (!float.TryParse(source16.GetElement<string>(index17) ?? string.Empty, out result16))
                      result16 = 100f;
                    ReverbSetting.ReverbInfo reverbInfo = new ReverbSetting.ReverbInfo()
                    {
                      MinDistance = result1,
                      MaxDistance = result2,
                      ReverbPreset = audioReverbPreset,
                      Room = result4,
                      RoomHF = result5,
                      RoomLF = result6,
                      DecayTime = result7,
                      DecayHFRatio = result8,
                      Reflections = result9,
                      ReflectionsDelay = result10,
                      Reverb = result11,
                      ReverbDelay = result12,
                      HFReference = result13,
                      LFReference = result14,
                      Diffusion = result15,
                      Density = result16
                    };
                    toRelease1.Add(reverbInfo);
                  }
                }
              }
            }
          }
          else
            break;
        }
      }
      List<AudioReverbZone> toRelease2 = ListPool<AudioReverbZone>.Get();
      AudioReverbZone[] componentsInChildren = (AudioReverbZone[]) ((Component) this).GetComponentsInChildren<AudioReverbZone>(true);
      toRelease2.AddRange((IEnumerable<AudioReverbZone>) componentsInChildren);
      int num = toRelease1.Count - toRelease2.Count;
      int count = toRelease2.Count;
      for (int index = 0; index < num; ++index)
      {
        Transform transform = new GameObject(string.Format("Reverb Zone {0:00}", (object) count++)).get_transform();
        transform.SetParent(((Component) this).get_transform(), false);
        toRelease2.Add(((Component) transform).GetOrAddComponent<AudioReverbZone>());
      }
      for (int index = 0; index < toRelease1.Count; ++index)
      {
        AudioReverbZone audioReverbZone = toRelease2[index];
        ReverbSetting.ReverbInfo reverbInfo = toRelease1[index];
        audioReverbZone.set_minDistance(reverbInfo.MinDistance);
        audioReverbZone.set_maxDistance(reverbInfo.MaxDistance);
        audioReverbZone.set_reverbPreset(reverbInfo.ReverbPreset);
        if (reverbInfo.ReverbPreset == 27)
        {
          audioReverbZone.set_room(reverbInfo.Room);
          audioReverbZone.set_roomHF(reverbInfo.RoomHF);
          audioReverbZone.set_roomLF(reverbInfo.RoomLF);
          audioReverbZone.set_decayTime(reverbInfo.DecayTime);
          audioReverbZone.set_decayHFRatio(reverbInfo.DecayHFRatio);
          audioReverbZone.set_reflections(reverbInfo.Reflections);
          audioReverbZone.set_reflectionsDelay(reverbInfo.ReflectionsDelay);
          audioReverbZone.set_reverb(reverbInfo.Reverb);
          audioReverbZone.set_reverbDelay(reverbInfo.ReverbDelay);
          audioReverbZone.set_HFReference((float) reverbInfo.HFReference);
          audioReverbZone.set_LFReference((float) reverbInfo.LFReference);
          audioReverbZone.set_diffusion(reverbInfo.Diffusion);
          audioReverbZone.set_density(reverbInfo.Density);
        }
      }
      ListPool<AudioReverbZone>.Release(toRelease2);
      ListPool<ReverbSetting.ReverbInfo>.Release(toRelease1);
    }

    public class ReverbInfo
    {
      public float MinDistance { get; set; } = 10f;

      public float MaxDistance { get; set; } = 15f;

      public AudioReverbPreset ReverbPreset { get; set; } = (AudioReverbPreset) 1;

      public int Room { get; set; } = -1000;

      public int RoomHF { get; set; } = -100;

      public int RoomLF { get; set; }

      public float DecayTime { get; set; } = 1.49f;

      public float DecayHFRatio { get; set; } = 0.83f;

      public int Reflections { get; set; } = -2602;

      public float ReflectionsDelay { get; set; } = 0.007f;

      public int Reverb { get; set; } = 200;

      public float ReverbDelay { get; set; } = 11f / 1000f;

      public int HFReference { get; set; } = 5000;

      public int LFReference { get; set; } = 250;

      public float Diffusion { get; set; } = 100f;

      public float Density { get; set; } = 100f;
    }
  }
}
