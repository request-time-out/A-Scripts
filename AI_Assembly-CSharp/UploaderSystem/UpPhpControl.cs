// Decompiled with JetBrains decompiler
// Type: UploaderSystem.UpPhpControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;

namespace UploaderSystem
{
  public class UpPhpControl : NetPhpControl
  {
    public UploadScene upScene;
    public UpUIControl uiCtrl;

    [DebuggerHidden]
    public IEnumerator UploadChara(IObserver<bool> observer, bool update)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new UpPhpControl.\u003CUploadChara\u003Ec__Iterator0()
      {
        update = update,
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator UploadHousing(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new UpPhpControl.\u003CUploadHousing\u003Ec__Iterator1()
      {
        observer = observer,
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
