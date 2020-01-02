// Decompiled with JetBrains decompiler
// Type: UI_ToggleEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ToggleEx : Toggle
{
  [SerializeField]
  private Image baseImageEx;
  [SerializeField]
  private Image overImage;
  [SerializeField]
  private Color selectedColor;
  [SerializeField]
  private Text[] text;
  [SerializeField]
  private bool alpha;

  public UI_ToggleEx()
  {
    base.\u002Ector();
  }

  protected virtual void Awake()
  {
    if (this.text != null)
      return;
    this.text = (Text[]) ((Component) this).GetComponentsInChildren<Text>();
  }

  protected virtual void Start()
  {
    base.Start();
    ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable((Toggle) this), (Action<M0>) (isOn =>
    {
      if (this.text == null)
        return;
      foreach (Text text in this.text)
      {
        Color color;
        if (isOn)
        {
          color = this.selectedColor;
        }
        else
        {
          ColorBlock colors = ((Selectable) this).get_colors();
          color = ((ColorBlock) ref colors).get_normalColor();
        }
        ((Graphic) text).set_color(color);
      }
    }));
    if (!Object.op_Inequality((Object) null, (Object) this.overImage))
      return;
    ((Behaviour) this.overImage).set_enabled(false);
  }

  protected virtual void OnEnable()
  {
    base.OnEnable();
    if (!Object.op_Inequality((Object) null, (Object) this.overImage))
      return;
    ((Behaviour) this.overImage).set_enabled(false);
  }

  public void SetTextColor(int state)
  {
    switch (state)
    {
      case 0:
        if (this.text == null)
          break;
        foreach (Text text1 in this.text)
        {
          Text text2 = text1;
          ColorBlock colors1 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null r = ((ColorBlock) ref colors1).get_highlightedColor().r;
          ColorBlock colors2 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null g = ((ColorBlock) ref colors2).get_highlightedColor().g;
          ColorBlock colors3 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null b = ((ColorBlock) ref colors3).get_highlightedColor().b;
          // ISSUE: variable of the null type
          __Null a;
          if (this.alpha)
          {
            ColorBlock colors4 = ((Selectable) this).get_colors();
            a = ((ColorBlock) ref colors4).get_highlightedColor().a;
          }
          else
            a = ((Graphic) text1).get_color().a;
          Color color = new Color((float) r, (float) g, (float) b, (float) a);
          ((Graphic) text2).set_color(color);
        }
        break;
      case 1:
        if (this.text == null)
          break;
        foreach (Text text1 in this.text)
        {
          Text text2 = text1;
          ColorBlock colors1 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null r = ((ColorBlock) ref colors1).get_normalColor().r;
          ColorBlock colors2 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null g = ((ColorBlock) ref colors2).get_normalColor().g;
          ColorBlock colors3 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null b = ((ColorBlock) ref colors3).get_normalColor().b;
          // ISSUE: variable of the null type
          __Null a;
          if (this.alpha)
          {
            ColorBlock colors4 = ((Selectable) this).get_colors();
            a = ((ColorBlock) ref colors4).get_normalColor().a;
          }
          else
            a = ((Graphic) text1).get_color().a;
          Color color = new Color((float) r, (float) g, (float) b, (float) a);
          ((Graphic) text2).set_color(color);
        }
        break;
      case 2:
        if (this.text == null)
          break;
        foreach (Text text1 in this.text)
        {
          Text text2 = text1;
          ColorBlock colors1 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null r = ((ColorBlock) ref colors1).get_pressedColor().r;
          ColorBlock colors2 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null g = ((ColorBlock) ref colors2).get_pressedColor().g;
          ColorBlock colors3 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null b = ((ColorBlock) ref colors3).get_pressedColor().b;
          // ISSUE: variable of the null type
          __Null a;
          if (this.alpha)
          {
            ColorBlock colors4 = ((Selectable) this).get_colors();
            a = ((ColorBlock) ref colors4).get_pressedColor().a;
          }
          else
            a = ((Graphic) text1).get_color().a;
          Color color = new Color((float) r, (float) g, (float) b, (float) a);
          ((Graphic) text2).set_color(color);
        }
        break;
      case 3:
        if (this.text == null)
          break;
        foreach (Text text1 in this.text)
        {
          Text text2 = text1;
          ColorBlock colors1 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null r = ((ColorBlock) ref colors1).get_disabledColor().r;
          ColorBlock colors2 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null g = ((ColorBlock) ref colors2).get_disabledColor().g;
          ColorBlock colors3 = ((Selectable) this).get_colors();
          // ISSUE: variable of the null type
          __Null b = ((ColorBlock) ref colors3).get_disabledColor().b;
          // ISSUE: variable of the null type
          __Null a;
          if (this.alpha)
          {
            ColorBlock colors4 = ((Selectable) this).get_colors();
            a = ((ColorBlock) ref colors4).get_disabledColor().a;
          }
          else
            a = ((Graphic) text1).get_color().a;
          Color color = new Color((float) r, (float) g, (float) b, (float) a);
          ((Graphic) text2).set_color(color);
        }
        break;
    }
  }

  protected virtual void DoStateTransition(Selectable.SelectionState state, bool instant)
  {
    ((Selectable) this).DoStateTransition(state, instant);
    if (this.get_isOn())
      return;
    switch ((int) state)
    {
      case 0:
        if (this.text != null)
        {
          foreach (Text text1 in this.text)
          {
            Text text2 = text1;
            ColorBlock colors1 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null r = ((ColorBlock) ref colors1).get_normalColor().r;
            ColorBlock colors2 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null g = ((ColorBlock) ref colors2).get_normalColor().g;
            ColorBlock colors3 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null b = ((ColorBlock) ref colors3).get_normalColor().b;
            // ISSUE: variable of the null type
            __Null a;
            if (this.alpha)
            {
              ColorBlock colors4 = ((Selectable) this).get_colors();
              a = ((ColorBlock) ref colors4).get_normalColor().a;
            }
            else
              a = ((Graphic) text1).get_color().a;
            Color color = new Color((float) r, (float) g, (float) b, (float) a);
            ((Graphic) text2).set_color(color);
          }
        }
        if (!Object.op_Inequality((Object) null, (Object) this.baseImageEx))
          break;
        Image baseImageEx1 = this.baseImageEx;
        ColorBlock colors5 = ((Selectable) this).get_colors();
        Color normalColor = ((ColorBlock) ref colors5).get_normalColor();
        ((Graphic) baseImageEx1).set_color(normalColor);
        break;
      case 1:
        if (this.text != null)
        {
          foreach (Text text1 in this.text)
          {
            Text text2 = text1;
            ColorBlock colors1 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null r = ((ColorBlock) ref colors1).get_highlightedColor().r;
            ColorBlock colors2 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null g = ((ColorBlock) ref colors2).get_highlightedColor().g;
            ColorBlock colors3 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null b = ((ColorBlock) ref colors3).get_highlightedColor().b;
            // ISSUE: variable of the null type
            __Null a;
            if (this.alpha)
            {
              ColorBlock colors4 = ((Selectable) this).get_colors();
              a = ((ColorBlock) ref colors4).get_highlightedColor().a;
            }
            else
              a = ((Graphic) text1).get_color().a;
            Color color = new Color((float) r, (float) g, (float) b, (float) a);
            ((Graphic) text2).set_color(color);
          }
        }
        if (!Object.op_Inequality((Object) null, (Object) this.baseImageEx))
          break;
        Image baseImageEx2 = this.baseImageEx;
        ColorBlock colors6 = ((Selectable) this).get_colors();
        Color highlightedColor = ((ColorBlock) ref colors6).get_highlightedColor();
        ((Graphic) baseImageEx2).set_color(highlightedColor);
        break;
      case 2:
        if (this.text != null)
        {
          foreach (Text text1 in this.text)
          {
            Text text2 = text1;
            ColorBlock colors1 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null r = ((ColorBlock) ref colors1).get_pressedColor().r;
            ColorBlock colors2 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null g = ((ColorBlock) ref colors2).get_pressedColor().g;
            ColorBlock colors3 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null b = ((ColorBlock) ref colors3).get_pressedColor().b;
            // ISSUE: variable of the null type
            __Null a;
            if (this.alpha)
            {
              ColorBlock colors4 = ((Selectable) this).get_colors();
              a = ((ColorBlock) ref colors4).get_pressedColor().a;
            }
            else
              a = ((Graphic) text1).get_color().a;
            Color color = new Color((float) r, (float) g, (float) b, (float) a);
            ((Graphic) text2).set_color(color);
          }
        }
        if (!Object.op_Inequality((Object) null, (Object) this.baseImageEx))
          break;
        Image baseImageEx3 = this.baseImageEx;
        ColorBlock colors7 = ((Selectable) this).get_colors();
        Color pressedColor = ((ColorBlock) ref colors7).get_pressedColor();
        ((Graphic) baseImageEx3).set_color(pressedColor);
        break;
      case 3:
        if (this.text != null)
        {
          foreach (Text text1 in this.text)
          {
            Text text2 = text1;
            ColorBlock colors1 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null r = ((ColorBlock) ref colors1).get_disabledColor().r;
            ColorBlock colors2 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null g = ((ColorBlock) ref colors2).get_disabledColor().g;
            ColorBlock colors3 = ((Selectable) this).get_colors();
            // ISSUE: variable of the null type
            __Null b = ((ColorBlock) ref colors3).get_disabledColor().b;
            // ISSUE: variable of the null type
            __Null a;
            if (this.alpha)
            {
              ColorBlock colors4 = ((Selectable) this).get_colors();
              a = ((ColorBlock) ref colors4).get_disabledColor().a;
            }
            else
              a = ((Graphic) text1).get_color().a;
            Color color = new Color((float) r, (float) g, (float) b, (float) a);
            ((Graphic) text2).set_color(color);
          }
        }
        if (!Object.op_Inequality((Object) null, (Object) this.baseImageEx))
          break;
        Image baseImageEx4 = this.baseImageEx;
        ColorBlock colors8 = ((Selectable) this).get_colors();
        Color disabledColor = ((ColorBlock) ref colors8).get_disabledColor();
        ((Graphic) baseImageEx4).set_color(disabledColor);
        break;
    }
  }

  public virtual void OnPointerEnter(PointerEventData eventData)
  {
    ((Selectable) this).OnPointerEnter(eventData);
    if (!Object.op_Inequality((Object) null, (Object) this.overImage))
      return;
    ((Behaviour) this.overImage).set_enabled(((Selectable) this).get_interactable());
  }

  public virtual void OnPointerExit(PointerEventData eventData)
  {
    ((Selectable) this).OnPointerExit(eventData);
    if (!Object.op_Inequality((Object) null, (Object) this.overImage))
      return;
    ((Behaviour) this.overImage).set_enabled(false);
  }
}
