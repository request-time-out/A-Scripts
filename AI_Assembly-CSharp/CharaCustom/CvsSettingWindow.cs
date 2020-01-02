// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsSettingWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsSettingWindow : MonoBehaviour
  {
    public CanvasGroup cgbaseWindow;
    public Button[] btnClose;

    public CvsSettingWindow()
    {
      base.\u002Ector();
    }

    public virtual void Start()
    {
      if (!((IEnumerable<Button>) this.btnClose).Any<Button>())
        return;
      ((IEnumerable<Button>) this.btnClose).Where<Button>((Func<Button, bool>) (item => Object.op_Inequality((Object) null, (Object) item))).ToList<Button>().ForEach((Action<Button>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item), (Action<M0>) (_ =>
      {
        if (!Object.op_Implicit((Object) this.cgbaseWindow))
          return;
        this.cgbaseWindow.Enable(false, false);
      }))));
    }
  }
}
