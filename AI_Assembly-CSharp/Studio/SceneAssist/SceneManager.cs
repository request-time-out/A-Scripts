// Decompiled with JetBrains decompiler
// Type: Studio.SceneAssist.SceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio.SceneAssist
{
  public class SceneManager : MonoBehaviour
  {
    [SerializeField]
    private Image imageNowLoading;
    [SerializeField]
    private Slider sliderProgress;
    [SerializeField]
    private Image imageLoadingAnime;

    public SceneManager()
    {
      base.\u002Ector();
    }

    public Image ImageNowLoading
    {
      get
      {
        return this.imageNowLoading;
      }
    }

    public Slider SliderProgress
    {
      get
      {
        return this.sliderProgress;
      }
    }

    public Image ImageLoadingAnime
    {
      get
      {
        return this.imageLoadingAnime;
      }
    }

    public bool NowLoadingActive
    {
      set
      {
        if (!Object.op_Implicit((Object) this.imageNowLoading) || ((Component) this.imageNowLoading).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this.imageNowLoading).get_gameObject().SetActive(value);
      }
    }

    public bool ProgressActive
    {
      set
      {
        if (!Object.op_Implicit((Object) this.sliderProgress) || ((Component) this.sliderProgress).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this.sliderProgress).get_gameObject().SetActive(value);
      }
      get
      {
        return Object.op_Inequality((Object) this.sliderProgress, (Object) null) && ((UIBehaviour) this.sliderProgress).IsActive();
      }
    }

    public bool LoadingAnimeActive
    {
      set
      {
        if (!Object.op_Implicit((Object) this.imageLoadingAnime) || ((Behaviour) this.imageLoadingAnime).get_enabled() == value)
          return;
        ((Behaviour) this.imageLoadingAnime).set_enabled(value);
      }
    }

    public bool Active
    {
      set
      {
        if (Object.op_Implicit((Object) this.imageNowLoading) && ((Component) this.imageNowLoading).get_gameObject().get_activeSelf() != value)
          ((Component) this.imageNowLoading).get_gameObject().SetActive(value);
        if (Object.op_Implicit((Object) this.sliderProgress) && ((Component) this.sliderProgress).get_gameObject().get_activeSelf() != value)
          ((Component) this.sliderProgress).get_gameObject().SetActive(value);
        if (!Object.op_Implicit((Object) this.imageLoadingAnime) || ((Behaviour) this.imageLoadingAnime).get_enabled() == value)
          return;
        ((Behaviour) this.imageLoadingAnime).set_enabled(value);
      }
    }

    public float ProgressValue
    {
      get
      {
        return Object.op_Inequality((Object) this.sliderProgress, (Object) null) ? this.sliderProgress.get_value() : 1f;
      }
      set
      {
        if (!Object.op_Implicit((Object) this.sliderProgress))
          return;
        this.sliderProgress.set_value(value);
      }
    }

    public float NowLoadingAlpha
    {
      set
      {
        if (!Object.op_Implicit((Object) this.imageNowLoading))
          return;
        Color color = ((Graphic) this.imageNowLoading).get_color();
        color.a = (__Null) (double) value;
        ((Graphic) this.imageNowLoading).set_color(color);
      }
    }

    public float LoadingAnimeAlpha
    {
      set
      {
        if (!Object.op_Implicit((Object) this.imageLoadingAnime))
          return;
        Color color = ((Graphic) this.imageLoadingAnime).get_color();
        color.a = (__Null) (double) value;
        ((Graphic) this.imageLoadingAnime).set_color(color);
      }
    }

    public void SetAlpha(float _a)
    {
      this.NowLoadingAlpha = _a;
      this.LoadingAnimeAlpha = _a;
    }

    private void Reset()
    {
      this.imageNowLoading = ((IEnumerable<Image>) ((Component) this).GetComponentsInChildren<Image>()).FirstOrDefault<Image>((Func<Image, bool>) (p => ((Object) p).get_name() == "NowLoading"));
      this.sliderProgress = ((IEnumerable<Slider>) ((Component) this).GetComponentsInChildren<Slider>()).FirstOrDefault<Slider>((Func<Slider, bool>) (p => ((Object) p).get_name() == "Progress"));
      this.imageLoadingAnime = ((IEnumerable<Image>) ((Component) this).GetComponentsInChildren<Image>()).FirstOrDefault<Image>((Func<Image, bool>) (p => ((Object) p).get_name() == "LoadingAnime"));
    }
  }
}
