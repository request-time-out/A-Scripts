// Decompiled with JetBrains decompiler
// Type: Studio.IKCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class IKCtrl : MonoBehaviour
  {
    private List<OCIChar.IKInfo> listIKInfo;

    public IKCtrl()
    {
      base.\u002Ector();
    }

    public OCIChar.IKInfo addIKInfo
    {
      set
      {
        this.listIKInfo.Add(value);
      }
    }

    public int count
    {
      get
      {
        return this.listIKInfo.Count;
      }
    }

    public void InitTarget()
    {
      this.StartCoroutine("InitTargetCoroutine");
    }

    public void CopyBone(OIBoneInfo.BoneGroup _target)
    {
      foreach (OCIChar.IKInfo ikInfo in this.listIKInfo.Where<OCIChar.IKInfo>((Func<OCIChar.IKInfo, bool>) (l => (l.boneGroup & _target) != (OIBoneInfo.BoneGroup) 0)))
        ikInfo.CopyBone();
    }

    public void CopyBoneRotation(OIBoneInfo.BoneGroup _target)
    {
      foreach (OCIChar.IKInfo ikInfo in this.listIKInfo.Where<OCIChar.IKInfo>((Func<OCIChar.IKInfo, bool>) (l => (l.boneGroup & _target) != (OIBoneInfo.BoneGroup) 0)))
        ikInfo.CopyBoneRotation();
    }

    [DebuggerHidden]
    private IEnumerator InitTargetCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new IKCtrl.\u003CInitTargetCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void Awake()
    {
      this.listIKInfo = new List<OCIChar.IKInfo>();
    }
  }
}
