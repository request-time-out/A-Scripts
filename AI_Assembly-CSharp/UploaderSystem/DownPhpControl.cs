// Decompiled with JetBrains decompiler
// Type: UploaderSystem.DownPhpControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;

namespace UploaderSystem
{
  public class DownPhpControl : NetPhpControl
  {
    public DownloadScene downScene;
    public DownUIControl uiCtrl;

    [DebuggerHidden]
    public IEnumerator AddApplauseCount(
      IObserver<bool> observer,
      DataType type,
      NetworkInfo.BaseIndex info)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DownPhpControl.\u003CAddApplauseCount\u003Ec__Iterator0()
      {
        info = info,
        type = type,
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator GetThumbnail(IObserver<bool> observer, DataType type)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DownPhpControl.\u003CGetThumbnail\u003Ec__Iterator1()
      {
        type = type,
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator DownloadPNG(IObserver<byte[]> observer, DataType type)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DownPhpControl.\u003CDownloadPNG\u003Ec__Iterator2()
      {
        type = type,
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator DeleteMyData(IObserver<bool> observer, DataType type)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DownPhpControl.\u003CDeleteMyData\u003Ec__Iterator3()
      {
        observer = observer,
        type = type,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator DeleteCache(IObserver<bool> observer, DataType type)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DownPhpControl.\u003CDeleteCache\u003Ec__Iterator4()
      {
        observer = observer,
        type = type,
        \u0024this = this
      };
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
  }
}
