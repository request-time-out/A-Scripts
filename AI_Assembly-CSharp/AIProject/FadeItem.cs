// Decompiled with JetBrains decompiler
// Type: AIProject.FadeItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace AIProject
{
  public class FadeItem : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Graphic _graphic;
    [SerializeField]
    private Text _uiText;

    public FadeItem()
    {
      base.\u002Ector();
    }

    public CanvasGroup CanvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
      set
      {
        this._canvasGroup = value;
      }
    }

    public Graphic Graphic
    {
      get
      {
        return this._graphic;
      }
    }

    public Text UIText
    {
      get
      {
        return this._uiText;
      }
    }
  }
}
