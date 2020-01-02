// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.WarningViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class WarningViewer : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private IntReactiveProperty _language;
    [Header("not used msgID")]
    [SerializeField]
    private StringReactiveProperty _message;
    private IDisposable fadeDisposable;

    public WarningViewer()
    {
      base.\u002Ector();
    }

    [DebuggerHidden]
    public static IEnumerator Load(
      Transform viewerParent,
      Action<WarningViewer> onComplete)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new WarningViewer.\u003CLoad\u003Ec__Iterator0()
      {
        onComplete = onComplete,
        viewerParent = viewerParent
      };
    }

    public bool initialized { get; private set; }

    public int msgID
    {
      get
      {
        return ((ReactiveProperty<int>) this._msgID).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._msgID).set_Value(value);
      }
    }

    public bool visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this._visible).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._visible).set_Value(value);
      }
    }

    public int langage
    {
      get
      {
        return ((ReactiveProperty<int>) this._language).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._language).set_Value(value);
      }
    }

    public string message
    {
      get
      {
        return ((ReactiveProperty<string>) this._message).get_Value();
      }
      set
      {
        ((ReactiveProperty<string>) this._message).set_Value(value);
      }
    }

    private IntReactiveProperty _msgID { get; }

    private BoolReactiveProperty _visible { get; }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new WarningViewer.\u003CStart\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
      if (this.fadeDisposable != null)
        this.fadeDisposable.Dispose();
      this.fadeDisposable = (IDisposable) null;
    }
  }
}
