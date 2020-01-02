// Decompiled with JetBrains decompiler
// Type: Correct.FrameCorrect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Correct.Process;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Correct
{
  public class FrameCorrect : BaseCorrect
  {
    public static string[] FrameNames = new string[50]
    {
      "cf_J_Hips",
      "cf_J_Spine01",
      "cf_J_Spine02",
      "cf_J_Spine03",
      "cf_J_Neck",
      "cf_J_Head",
      "cf_J_Kosi01",
      "cf_J_Kosi02",
      "cf_J_Shoulder_L",
      "cf_J_Shoulder_R",
      "cf_J_Toes01_L",
      "cf_J_Toes01_R",
      "cf_J_Hand_Thumb01_L",
      "cf_J_Hand_Thumb02_L",
      "cf_J_Hand_Thumb03_L",
      "cf_J_Hand_Index01_L",
      "cf_J_Hand_Index02_L",
      "cf_J_Hand_Index03_L",
      "cf_J_Hand_Middle01_L",
      "cf_J_Hand_Middle02_L",
      "cf_J_Hand_Middle03_L",
      "cf_J_Hand_Ring01_L",
      "cf_J_Hand_Ring02_L",
      "cf_J_Hand_Ring03_L",
      "cf_J_Hand_Little01_L",
      "cf_J_Hand_Little02_L",
      "cf_J_Hand_Little03_L",
      "cf_J_Hand_Thumb01_R",
      "cf_J_Hand_Thumb02_R",
      "cf_J_Hand_Thumb03_R",
      "cf_J_Hand_Index01_R",
      "cf_J_Hand_Index02_R",
      "cf_J_Hand_Index03_R",
      "cf_J_Hand_Middle01_R",
      "cf_J_Hand_Middle02_R",
      "cf_J_Hand_Middle03_R",
      "cf_J_Hand_Ring01_R",
      "cf_J_Hand_Ring02_R",
      "cf_J_Hand_Ring03_R",
      "cf_J_Hand_Little01_R",
      "cf_J_Hand_Little02_R",
      "cf_J_Hand_Little03_R",
      "cf_J_Mune00_L",
      "cf_J_Mune01_L",
      "cf_J_Mune02_L",
      "cf_J_Mune03_L",
      "cf_J_Mune00_R",
      "cf_J_Mune01_R",
      "cf_J_Mune02_R",
      "cf_J_Mune03_R"
    };
    private static string[] bodyNames = new string[22]
    {
      "cf_J_Hips",
      "cf_J_Spine01",
      "cf_J_Spine02",
      "cf_J_Spine03",
      "cf_J_Neck",
      "cf_J_Head",
      "cf_J_Kosi01",
      "cf_J_Kosi02",
      "cf_J_Shoulder_L",
      "cf_J_Foot01_L",
      "cf_J_Foot02_L",
      "cf_J_Shoulder_R",
      "cf_J_Foot01_R",
      "cf_J_Foot02_R",
      "cf_J_Mune00_L",
      "cf_J_Mune01_L",
      "cf_J_Mune02_L",
      "cf_J_Mune03_L",
      "cf_J_Mune00_R",
      "cf_J_Mune01_R",
      "cf_J_Mune02_R",
      "cf_J_Mune03_R"
    };
    public static string[] RestoreBodyNames = new string[8]
    {
      "cf_J_Hips",
      "cf_J_Spine01",
      "cf_J_Spine02",
      "cf_J_Spine03",
      "cf_J_Neck",
      "cf_J_Head",
      "cf_J_Kosi01",
      "cf_J_Kosi02"
    };

    private void Start()
    {
      FrameCorrect.Init(this);
    }

    public static void Init(FrameCorrect frameCorrect)
    {
      List<BaseCorrect.Info> infoList = new List<BaseCorrect.Info>();
      for (int index1 = 0; index1 < FrameCorrect.FrameNames.Length; ++index1)
      {
        for (int index2 = 0; index2 < frameCorrect.list.Count; ++index2)
        {
          if (!(FrameCorrect.FrameNames[index1] != ((Object) ((Component) frameCorrect.list[index2].data).get_transform()).get_name()))
          {
            infoList.Add(frameCorrect.list[index2]);
            if (Object.op_Equality((Object) infoList[infoList.Count - 1].data.bone, (Object) null))
            {
              infoList[infoList.Count - 1].data.bone = ((Component) infoList[infoList.Count - 1].data).get_transform();
              break;
            }
            break;
          }
        }
      }
      frameCorrect.list = new List<BaseCorrect.Info>((IEnumerable<BaseCorrect.Info>) infoList);
    }

    public static void AddFrameCorrect(GameObject gameObject)
    {
      if (!Object.op_Equality((Object) gameObject.GetComponent<FrameCorrect>(), (Object) null))
        return;
      FrameCorrect correct = (FrameCorrect) gameObject.AddComponent<FrameCorrect>();
      correct.list.Clear();
      Transform[] componentsInChildren = (Transform[]) ((Component) correct).GetComponentsInChildren<Transform>(true);
      IEnumerable<Transform> frames = correct.GetFrames(componentsInChildren, correct);
      List<BaseCorrect.Info> infoList = new List<BaseCorrect.Info>();
      bool flag = false;
      using (IEnumerator<Transform> enumerator = frames.GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Transform current = enumerator.Current;
          BaseCorrect.Info info = new BaseCorrect.Info((Component) current);
          foreach (string bodyName in FrameCorrect.bodyNames)
          {
            if (!(bodyName != ((Object) current).get_name()))
              flag = true;
          }
          info.type = !flag ? BaseCorrect.Info.ProcOrderType.Last : BaseCorrect.Info.ProcOrderType.First;
          info.bone = current;
          infoList.Add(info);
        }
      }
      correct.list.AddRange((IEnumerable<BaseCorrect.Info>) infoList);
      foreach (BaseCorrect.Info info in correct.list)
        info.process.type = BaseProcess.Type.Target;
    }

    private IEnumerable<Transform> GetFrames(
      Transform[] t,
      FrameCorrect correct)
    {
      List<string> frameNames = (List<string>) null;
      if (!Object.op_Inequality((Object) ((Component) ((Component) correct).get_transform().get_parent().get_parent()).GetComponent<TestChara>(), (Object) null))
        return (IEnumerable<Transform>) null;
      frameNames = correct.GetFrameNames(FrameCorrect.FrameNames);
      return (IEnumerable<Transform>) ((IEnumerable<Transform>) t).Where<Transform>((Func<Transform, bool>) (frame => frameNames.Contains(((Object) frame).get_name()))).OrderBy<Transform, int>((Func<Transform, int>) (frame =>
      {
        for (int index = 0; index < frameNames.Count; ++index)
        {
          if (FrameCorrect.FrameNames[index] == ((Object) frame).get_name())
            return index;
        }
        return -1;
      }));
    }
  }
}
