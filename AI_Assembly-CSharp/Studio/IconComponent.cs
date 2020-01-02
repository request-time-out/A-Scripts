// Decompiled with JetBrains decompiler
// Type: Studio.IconComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Studio
{
  public class IconComponent : MonoBehaviour
  {
    [SerializeField]
    private Renderer renderer;
    private Transform transTarget;
    private Transform transRender;

    public IconComponent()
    {
      base.\u002Ector();
    }

    public bool Active
    {
      set
      {
        ((Component) this.renderer).get_gameObject().SetActive(value);
      }
    }

    public bool Visible
    {
      set
      {
        this.renderer.set_enabled(value);
      }
    }

    public int Layer
    {
      set
      {
        ((Component) this.renderer).get_gameObject().set_layer(value);
      }
    }

    private void Awake()
    {
      this.transRender = ((Component) this.renderer).get_transform();
      this.transTarget = ((Component) Camera.get_main()).get_transform();
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, bool>) (_ => this.renderer.get_enabled())), (Action<M0>) (_ => this.transRender.LookAt(this.transTarget.get_position())));
    }
  }
}
