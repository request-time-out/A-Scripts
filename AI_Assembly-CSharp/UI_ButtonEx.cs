// Decompiled with JetBrains decompiler
// Type: UI_ButtonEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonEx : Button
{
  [SerializeField]
  private Image overImage;
  [SerializeField]
  private Text[] text;
  [SerializeField]
  private bool alpha;

  public UI_ButtonEx()
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
    ((UIBehaviour) this).Start();
    if (!Object.op_Inequality((Object) null, (Object) this.overImage))
      return;
    ((Behaviour) this.overImage).set_enabled(false);
  }

  protected virtual void OnEnable()
  {
    ((Selectable) this).OnEnable();
    if (!Object.op_Inequality((Object) null, (Object) this.overImage))
      return;
    ((Behaviour) this.overImage).set_enabled(false);
  }

  protected virtual void DoStateTransition(Selectable.SelectionState state, bool instant)
  {
    ((Selectable) this).DoStateTransition(state, instant);
    switch ((int) state)
    {
      case 0:
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
      case 1:
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
