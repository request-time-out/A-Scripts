// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.CustomSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class CustomSlider : Slider, ICustomSelectable, ICancelHandler, IEventSystemHandler
  {
    [SerializeField]
    private Sprite _disabledHighlightedSprite;
    [SerializeField]
    private Color _disabledHighlightedColor;
    [SerializeField]
    private string _disabledHighlightedTrigger;
    [SerializeField]
    private bool _autoNavUp;
    [SerializeField]
    private bool _autoNavDown;
    [SerializeField]
    private bool _autoNavLeft;
    [SerializeField]
    private bool _autoNavRight;
    private bool isHighlightDisabled;

    public CustomSlider()
    {
      base.\u002Ector();
    }

    public Sprite disabledHighlightedSprite
    {
      get
      {
        return this._disabledHighlightedSprite;
      }
      set
      {
        this._disabledHighlightedSprite = value;
      }
    }

    public Color disabledHighlightedColor
    {
      get
      {
        return this._disabledHighlightedColor;
      }
      set
      {
        this._disabledHighlightedColor = value;
      }
    }

    public string disabledHighlightedTrigger
    {
      get
      {
        return this._disabledHighlightedTrigger;
      }
      set
      {
        this._disabledHighlightedTrigger = value;
      }
    }

    public bool autoNavUp
    {
      get
      {
        return this._autoNavUp;
      }
      set
      {
        this._autoNavUp = value;
      }
    }

    public bool autoNavDown
    {
      get
      {
        return this._autoNavDown;
      }
      set
      {
        this._autoNavDown = value;
      }
    }

    public bool autoNavLeft
    {
      get
      {
        return this._autoNavLeft;
      }
      set
      {
        this._autoNavLeft = value;
      }
    }

    public bool autoNavRight
    {
      get
      {
        return this._autoNavRight;
      }
      set
      {
        this._autoNavRight = value;
      }
    }

    private bool isDisabled
    {
      get
      {
        return !((Selectable) this).IsInteractable();
      }
    }

    private event UnityAction _CancelEvent
    {
      add
      {
        UnityAction comparand = this._CancelEvent;
        UnityAction unityAction;
        do
        {
          unityAction = comparand;
          comparand = Interlocked.CompareExchange<UnityAction>(ref this._CancelEvent, (UnityAction) Delegate.Combine((Delegate) unityAction, (Delegate) value), comparand);
        }
        while (comparand != unityAction);
      }
      remove
      {
        UnityAction comparand = this._CancelEvent;
        UnityAction unityAction;
        do
        {
          unityAction = comparand;
          comparand = Interlocked.CompareExchange<UnityAction>(ref this._CancelEvent, (UnityAction) Delegate.Remove((Delegate) unityAction, (Delegate) value), comparand);
        }
        while (comparand != unityAction);
      }
    }

    public event UnityAction CancelEvent
    {
      add
      {
        this._CancelEvent += value;
      }
      remove
      {
        this._CancelEvent -= value;
      }
    }

    public virtual Selectable FindSelectableOnLeft()
    {
      Navigation navigation = ((Selectable) this).get_navigation();
      return (((Navigation) ref navigation).get_mode() & 1) != null || this._autoNavLeft ? UISelectionUtility.FindNextSelectable((Selectable) this, ((Component) this).get_transform(), Selectable.get_allSelectables(), Vector3.get_left()) : base.FindSelectableOnLeft();
    }

    public virtual Selectable FindSelectableOnRight()
    {
      Navigation navigation = ((Selectable) this).get_navigation();
      return (((Navigation) ref navigation).get_mode() & 1) != null || this._autoNavRight ? UISelectionUtility.FindNextSelectable((Selectable) this, ((Component) this).get_transform(), Selectable.get_allSelectables(), Vector3.get_right()) : base.FindSelectableOnRight();
    }

    public virtual Selectable FindSelectableOnUp()
    {
      Navigation navigation = ((Selectable) this).get_navigation();
      return (((Navigation) ref navigation).get_mode() & 2) != null || this._autoNavUp ? UISelectionUtility.FindNextSelectable((Selectable) this, ((Component) this).get_transform(), Selectable.get_allSelectables(), Vector3.get_up()) : base.FindSelectableOnUp();
    }

    public virtual Selectable FindSelectableOnDown()
    {
      Navigation navigation = ((Selectable) this).get_navigation();
      return (((Navigation) ref navigation).get_mode() & 2) != null || this._autoNavDown ? UISelectionUtility.FindNextSelectable((Selectable) this, ((Component) this).get_transform(), Selectable.get_allSelectables(), Vector3.get_down()) : base.FindSelectableOnDown();
    }

    protected virtual void OnCanvasGroupChanged()
    {
      ((Selectable) this).OnCanvasGroupChanged();
      if (Object.op_Equality((Object) EventSystem.get_current(), (Object) null))
        return;
      this.EvaluateHightlightDisabled(Object.op_Equality((Object) EventSystem.get_current().get_currentSelectedGameObject(), (Object) ((Component) this).get_gameObject()));
    }

    protected virtual void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      if (this.isHighlightDisabled)
      {
        Color highlightedColor = this._disabledHighlightedColor;
        Sprite highlightedSprite = this._disabledHighlightedSprite;
        string highlightedTrigger = this._disabledHighlightedTrigger;
        if (!((Component) this).get_gameObject().get_activeInHierarchy())
          return;
        Selectable.Transition transition = ((Selectable) this).get_transition();
        if (transition != 1)
        {
          if (transition != 2)
          {
            if (transition != 3)
              return;
            this.TriggerAnimation(highlightedTrigger);
          }
          else
            this.DoSpriteSwap(highlightedSprite);
        }
        else
        {
          Color color = highlightedColor;
          ColorBlock colors = ((Selectable) this).get_colors();
          double colorMultiplier = (double) ((ColorBlock) ref colors).get_colorMultiplier();
          this.StartColorTween(Color.op_Multiply(color, (float) colorMultiplier), instant);
        }
      }
      else
        ((Selectable) this).DoStateTransition(state, instant);
    }

    private void StartColorTween(Color targetColor, bool instant)
    {
      if (Object.op_Equality((Object) ((Selectable) this).get_targetGraphic(), (Object) null))
        return;
      Graphic targetGraphic = ((Selectable) this).get_targetGraphic();
      Color color = targetColor;
      double num;
      if (instant)
      {
        num = 0.0;
      }
      else
      {
        ColorBlock colors = ((Selectable) this).get_colors();
        num = (double) ((ColorBlock) ref colors).get_fadeDuration();
      }
      targetGraphic.CrossFadeColor(color, (float) num, true, true);
    }

    private void DoSpriteSwap(Sprite newSprite)
    {
      if (Object.op_Equality((Object) ((Selectable) this).get_image(), (Object) null))
        return;
      ((Selectable) this).get_image().set_overrideSprite(newSprite);
    }

    private void TriggerAnimation(string triggername)
    {
      if (Object.op_Equality((Object) ((Selectable) this).get_animator(), (Object) null) || !((Behaviour) ((Selectable) this).get_animator()).get_enabled() || (!((Behaviour) ((Selectable) this).get_animator()).get_isActiveAndEnabled() || Object.op_Equality((Object) ((Selectable) this).get_animator().get_runtimeAnimatorController(), (Object) null)) || string.IsNullOrEmpty(triggername))
        return;
      ((Selectable) this).get_animator().ResetTrigger(this._disabledHighlightedTrigger);
      ((Selectable) this).get_animator().SetTrigger(triggername);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
      ((Selectable) this).OnSelect(eventData);
      this.EvaluateHightlightDisabled(true);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
      ((Selectable) this).OnDeselect(eventData);
      this.EvaluateHightlightDisabled(false);
    }

    private void EvaluateHightlightDisabled(bool isSelected)
    {
      if (!isSelected)
      {
        if (!this.isHighlightDisabled)
          return;
        this.isHighlightDisabled = false;
        ((Selectable) this).DoStateTransition(!this.isDisabled ? ((Selectable) this).get_currentSelectionState() : (Selectable.SelectionState) 3, false);
      }
      else
      {
        if (!this.isDisabled)
          return;
        this.isHighlightDisabled = true;
        ((Selectable) this).DoStateTransition((Selectable.SelectionState) 3, false);
      }
    }

    public void OnCancel(BaseEventData eventData)
    {
      // ISSUE: reference to a compiler-generated field
      if (this._CancelEvent == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this._CancelEvent.Invoke();
    }
  }
}
