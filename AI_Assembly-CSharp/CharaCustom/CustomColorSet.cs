// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomColorSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomColorSet : MonoBehaviour
  {
    public Text title;
    public Button button;
    public Image image;
    public bool useAlpha;
    public Action<Color> actUpdateColor;

    public CustomColorSet()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    protected ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    public void SetColor(Color color)
    {
      ((Graphic) this.image).set_color(color);
      this.customBase.customColorCtrl.SetColor(this, color);
    }

    public void EnableColorAlpha(bool enable)
    {
      this.useAlpha = enable;
      this.customBase.customColorCtrl.EnableAlpha(this.useAlpha);
    }

    public void Reset()
    {
      this.title = (Text) ((Component) ((Component) this).get_transform()).GetComponentInChildren<Text>();
      Image[] componentsInChildren = (Image[]) ((Component) ((Component) this).get_transform()).GetComponentsInChildren<Image>();
      if (componentsInChildren != null)
        this.image = ((IEnumerable<Image>) componentsInChildren).Where<Image>((Func<Image, bool>) (x => ((Object) x).get_name() == "imgColor")).FirstOrDefault<Image>();
      this.button = (Button) ((Component) ((Component) this).get_transform()).GetComponentInChildren<Button>();
    }

    public void Start()
    {
      if (!Object.op_Implicit((Object) this.button))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.button), (Action<M0>) (_ => this.customBase.customColorCtrl.Setup(this, ((Graphic) this.image).get_color(), (Action<Color>) (color =>
      {
        ((Graphic) this.image).set_color(color);
        if (this.actUpdateColor == null)
          return;
        this.actUpdateColor(color);
      }), this.useAlpha)));
    }
  }
}
