// Decompiled with JetBrains decompiler
// Type: Studio.RoutePointComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class RoutePointComponent : MonoBehaviour
  {
    [SerializeField]
    private Image imageBack;
    [SerializeField]
    private TextMeshProUGUI _textName;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [Space]
    [SerializeField]
    private GameObject _objAid;

    public RoutePointComponent()
    {
      base.\u002Ector();
    }

    public Color color
    {
      set
      {
        ((Graphic) this.imageBack).set_color(value);
      }
    }

    public string textName
    {
      set
      {
        ((TMP_Text) this._textName).set_text(value);
      }
    }

    public bool visible
    {
      get
      {
        return (double) this.canvasGroup.get_alpha() != 0.0;
      }
      set
      {
        this.canvasGroup.Enable(value, false);
      }
    }

    public GameObject objAid
    {
      get
      {
        return this._objAid;
      }
    }
  }
}
