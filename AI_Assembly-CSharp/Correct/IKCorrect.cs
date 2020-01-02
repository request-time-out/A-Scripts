// Decompiled with JetBrains decompiler
// Type: Correct.IKCorrect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Correct.Process;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Correct
{
  public class IKCorrect : BaseCorrect
  {
    public static string[] FrameNames = new string[13]
    {
      "f_t_hips",
      "f_t_thigh_L",
      "f_t_thigh_R",
      "f_t_shoulder_L",
      "f_t_shoulder_R",
      "f_t_arm_L",
      "f_t_arm_R",
      "f_t_elbo_L",
      "f_t_elbo_R",
      "f_t_knee_L",
      "f_t_knee_R",
      "f_t_leg_L",
      "f_t_leg_R"
    };

    public static void AddIKCorrect(GameObject gameObject)
    {
      if (!Object.op_Equality((Object) gameObject.GetComponent<IKCorrect>(), (Object) null))
        return;
      IKCorrect correct = (IKCorrect) gameObject.AddComponent<IKCorrect>();
      Transform parent = ((Component) correct).get_transform().get_parent().get_parent();
      correct.list.Clear();
      Transform[] componentsInChildren = (Transform[]) ((Component) correct).GetComponentsInChildren<Transform>(true);
      IEnumerable<Transform> ikFrames = correct.GetIKFrames(componentsInChildren, parent, correct);
      List<BaseCorrect.Info> infoList = new List<BaseCorrect.Info>();
      using (IEnumerator<Transform> enumerator = ikFrames.GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
          infoList.Add(new BaseCorrect.Info((Component) enumerator.Current)
          {
            type = BaseCorrect.Info.ProcOrderType.Second,
            bone = (Transform) null
          });
      }
      correct.list.AddRange((IEnumerable<BaseCorrect.Info>) infoList);
      foreach (BaseCorrect.Info info in correct.list)
        info.process.type = BaseProcess.Type.Sync;
    }

    private IEnumerable<Transform> GetIKFrames(
      Transform[] t,
      Transform correctRoot,
      IKCorrect correct)
    {
      List<string> frameNames = (List<string>) null;
      if (!Object.op_Inequality((Object) ((Component) correctRoot).GetComponent<TestChara>(), (Object) null))
        return (IEnumerable<Transform>) null;
      frameNames = correct.GetFrameNames(IKCorrect.FrameNames);
      return (IEnumerable<Transform>) ((IEnumerable<Transform>) t).Where<Transform>((Func<Transform, bool>) (frame => frameNames.Contains(((Object) frame).get_name()))).OrderBy<Transform, int>((Func<Transform, int>) (frame =>
      {
        for (int index = 0; index < frameNames.Count; ++index)
        {
          if (frameNames[index] == ((Object) frame).get_name())
            return index;
        }
        return -1;
      }));
    }
  }
}
