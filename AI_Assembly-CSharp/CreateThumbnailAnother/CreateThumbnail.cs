// Decompiled with JetBrains decompiler
// Type: CreateThumbnailAnother.CreateThumbnail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CreateThumbnailAnother
{
  public class CreateThumbnail : BaseLoader
  {
    public CameraControl camCtrl;
    public Camera camMain;
    public Camera camBack;
    public Camera camFront;
    public CreateThumbnail.ImageInfo[] imageInfos;
    public static Action action;

    public bool isInit { get; private set; }

    public int frame
    {
      set
      {
        for (int index = 0; index < this.imageInfos.Length; ++index)
          this.imageInfos[index].active = index == value;
      }
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CreateThumbnail.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [Serializable]
    public class ImageInfo
    {
      public GameObject objFront;
      public GameObject objBack;

      public bool active
      {
        set
        {
          this.objFront.SafeProc<GameObject>((Action<GameObject>) (_o => _o.SetActive(value)));
          this.objBack.SafeProc<GameObject>((Action<GameObject>) (_o => _o.SetActive(value)));
        }
      }
    }
  }
}
