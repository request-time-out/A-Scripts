// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ThemeSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [Serializable]
  public class ThemeSettings : ScriptableObject
  {
    [SerializeField]
    private ThemeSettings.ImageSettings _mainWindowBackground;
    [SerializeField]
    private ThemeSettings.ImageSettings _popupWindowBackground;
    [SerializeField]
    private ThemeSettings.ImageSettings _areaBackground;
    [SerializeField]
    private ThemeSettings.SelectableSettings _selectableSettings;
    [SerializeField]
    private ThemeSettings.SelectableSettings _buttonSettings;
    [SerializeField]
    private ThemeSettings.SelectableSettings _inputGridFieldSettings;
    [SerializeField]
    private ThemeSettings.ScrollbarSettings _scrollbarSettings;
    [SerializeField]
    private ThemeSettings.SliderSettings _sliderSettings;
    [SerializeField]
    private ThemeSettings.ImageSettings _invertToggle;
    [SerializeField]
    private Color _invertToggleDisabledColor;
    [SerializeField]
    private ThemeSettings.ImageSettings _calibrationValueMarker;
    [SerializeField]
    private ThemeSettings.ImageSettings _calibrationRawValueMarker;
    [SerializeField]
    private ThemeSettings.TextSettings _textSettings;
    [SerializeField]
    private ThemeSettings.TextSettings _buttonTextSettings;
    [SerializeField]
    private ThemeSettings.TextSettings _inputGridFieldTextSettings;

    public ThemeSettings()
    {
      base.\u002Ector();
    }

    public void Apply(ThemedElement.ElementInfo[] elementInfo)
    {
      if (elementInfo == null)
        return;
      for (int index = 0; index < elementInfo.Length; ++index)
      {
        if (elementInfo[index] != null)
          this.Apply(elementInfo[index].themeClass, elementInfo[index].component);
      }
    }

    private void Apply(string themeClass, Component component)
    {
      if (Object.op_Inequality((Object) (component as Selectable), (Object) null))
        this.Apply(themeClass, (Selectable) component);
      else if (Object.op_Inequality((Object) (component as Image), (Object) null))
        this.Apply(themeClass, (Image) component);
      else if (Object.op_Inequality((Object) (component as Text), (Object) null))
      {
        this.Apply(themeClass, (Text) component);
      }
      else
      {
        if (!Object.op_Inequality((Object) (component as UIImageHelper), (Object) null))
          return;
        this.Apply(themeClass, (UIImageHelper) component);
      }
    }

    private void Apply(string themeClass, Selectable item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      ThemeSettings.SelectableSettings_Base selectableSettingsBase;
      if (Object.op_Inequality((Object) (item as Button), (Object) null))
      {
        switch (themeClass)
        {
          case "inputGridField":
            selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._inputGridFieldSettings;
            break;
          default:
            selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._buttonSettings;
            break;
        }
      }
      else if (Object.op_Inequality((Object) (item as Scrollbar), (Object) null))
        selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._scrollbarSettings;
      else if (Object.op_Inequality((Object) (item as Slider), (Object) null))
        selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._sliderSettings;
      else if (Object.op_Inequality((Object) (item as Toggle), (Object) null))
      {
        switch (themeClass)
        {
          case "button":
            selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._buttonSettings;
            break;
          default:
            selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._selectableSettings;
            break;
        }
      }
      else
        selectableSettingsBase = (ThemeSettings.SelectableSettings_Base) this._selectableSettings;
      selectableSettingsBase.Apply(item);
    }

    private void Apply(string themeClass, Image item)
    {
      if (Object.op_Equality((Object) item, (Object) null) || themeClass == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ThemeSettings.\u003C\u003Ef__switch\u0024map8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ThemeSettings.\u003C\u003Ef__switch\u0024map8 = new Dictionary<string, int>(8)
        {
          {
            "area",
            0
          },
          {
            "popupWindow",
            1
          },
          {
            "mainWindow",
            2
          },
          {
            "calibrationValueMarker",
            3
          },
          {
            "calibrationRawValueMarker",
            4
          },
          {
            "invertToggle",
            5
          },
          {
            "invertToggleBackground",
            6
          },
          {
            "invertToggleButtonBackground",
            7
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!ThemeSettings.\u003C\u003Ef__switch\u0024map8.TryGetValue(themeClass, out num))
        return;
      switch (num)
      {
        case 0:
          this._areaBackground.CopyTo(item);
          break;
        case 1:
          this._popupWindowBackground.CopyTo(item);
          break;
        case 2:
          this._mainWindowBackground.CopyTo(item);
          break;
        case 3:
          this._calibrationValueMarker.CopyTo(item);
          break;
        case 4:
          this._calibrationRawValueMarker.CopyTo(item);
          break;
        case 5:
          this._invertToggle.CopyTo(item);
          break;
        case 6:
          this._inputGridFieldSettings.imageSettings.CopyTo(item);
          break;
        case 7:
          this._buttonSettings.imageSettings.CopyTo(item);
          break;
      }
    }

    private void Apply(string themeClass, Text item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      ThemeSettings.TextSettings textSettings;
      switch (themeClass)
      {
        case "button":
          textSettings = this._buttonTextSettings;
          break;
        case "inputGridField":
          textSettings = this._inputGridFieldTextSettings;
          break;
        default:
          textSettings = this._textSettings;
          break;
      }
      if (Object.op_Inequality((Object) textSettings.font, (Object) null))
        item.set_font(textSettings.font);
      ((Graphic) item).set_color(textSettings.color);
      item.set_lineSpacing(textSettings.lineSpacing);
      if ((double) textSettings.sizeMultiplier != 1.0)
      {
        item.set_fontSize((int) ((double) item.get_fontSize() * (double) textSettings.sizeMultiplier));
        item.set_resizeTextMaxSize((int) ((double) item.get_resizeTextMaxSize() * (double) textSettings.sizeMultiplier));
        item.set_resizeTextMinSize((int) ((double) item.get_resizeTextMinSize() * (double) textSettings.sizeMultiplier));
      }
      if (textSettings.style == ThemeSettings.FontStyleOverride.Default)
        return;
      item.set_fontStyle((FontStyle) (textSettings.style - 1));
    }

    private void Apply(string themeClass, UIImageHelper item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      item.SetEnabledStateColor(this._invertToggle.color);
      item.SetDisabledStateColor(this._invertToggleDisabledColor);
      item.Refresh();
    }

    [Serializable]
    private abstract class SelectableSettings_Base
    {
      [SerializeField]
      protected Selectable.Transition _transition;
      [SerializeField]
      protected ThemeSettings.CustomColorBlock _colors;
      [SerializeField]
      protected ThemeSettings.CustomSpriteState _spriteState;
      [SerializeField]
      protected ThemeSettings.CustomAnimationTriggers _animationTriggers;

      public Selectable.Transition transition
      {
        get
        {
          return this._transition;
        }
      }

      public ThemeSettings.CustomColorBlock selectableColors
      {
        get
        {
          return this._colors;
        }
      }

      public ThemeSettings.CustomSpriteState spriteState
      {
        get
        {
          return this._spriteState;
        }
      }

      public ThemeSettings.CustomAnimationTriggers animationTriggers
      {
        get
        {
          return this._animationTriggers;
        }
      }

      public virtual void Apply(Selectable item)
      {
        Selectable.Transition transition = this._transition;
        bool flag = item.get_transition() != transition;
        item.set_transition(transition);
        ICustomSelectable customSelectable = item as ICustomSelectable;
        if (transition == 1)
        {
          ThemeSettings.CustomColorBlock colors = this._colors;
          colors.fadeDuration = 0.0f;
          item.set_colors((ColorBlock) colors);
          colors.fadeDuration = this._colors.fadeDuration;
          item.set_colors((ColorBlock) colors);
          if (customSelectable != null)
            customSelectable.disabledHighlightedColor = colors.disabledHighlightedColor;
        }
        else if (transition == 2)
        {
          item.set_spriteState((SpriteState) this._spriteState);
          if (customSelectable != null)
            customSelectable.disabledHighlightedSprite = this._spriteState.disabledHighlightedSprite;
        }
        else if (transition == 3)
        {
          item.get_animationTriggers().set_disabledTrigger(this._animationTriggers.disabledTrigger);
          item.get_animationTriggers().set_highlightedTrigger(this._animationTriggers.highlightedTrigger);
          item.get_animationTriggers().set_normalTrigger(this._animationTriggers.normalTrigger);
          item.get_animationTriggers().set_pressedTrigger(this._animationTriggers.pressedTrigger);
          if (customSelectable != null)
            customSelectable.disabledHighlightedTrigger = this._animationTriggers.disabledHighlightedTrigger;
        }
        if (!flag)
          return;
        item.get_targetGraphic().CrossFadeColor(item.get_targetGraphic().get_color(), 0.0f, true, true);
      }
    }

    [Serializable]
    private class SelectableSettings : ThemeSettings.SelectableSettings_Base
    {
      [SerializeField]
      private ThemeSettings.ImageSettings _imageSettings;

      public ThemeSettings.ImageSettings imageSettings
      {
        get
        {
          return this._imageSettings;
        }
      }

      public override void Apply(Selectable item)
      {
        if (Object.op_Equality((Object) item, (Object) null))
          return;
        base.Apply(item);
        if (this._imageSettings == null)
          return;
        this._imageSettings.CopyTo(item.get_targetGraphic() as Image);
      }
    }

    [Serializable]
    private class SliderSettings : ThemeSettings.SelectableSettings_Base
    {
      [SerializeField]
      private ThemeSettings.ImageSettings _handleImageSettings;
      [SerializeField]
      private ThemeSettings.ImageSettings _fillImageSettings;
      [SerializeField]
      private ThemeSettings.ImageSettings _backgroundImageSettings;

      public ThemeSettings.ImageSettings handleImageSettings
      {
        get
        {
          return this._handleImageSettings;
        }
      }

      public ThemeSettings.ImageSettings fillImageSettings
      {
        get
        {
          return this._fillImageSettings;
        }
      }

      public ThemeSettings.ImageSettings backgroundImageSettings
      {
        get
        {
          return this._backgroundImageSettings;
        }
      }

      private void Apply(Slider item)
      {
        if (Object.op_Equality((Object) item, (Object) null))
          return;
        if (this._handleImageSettings != null)
          this._handleImageSettings.CopyTo(((Selectable) item).get_targetGraphic() as Image);
        if (this._fillImageSettings != null)
        {
          RectTransform fillRect = item.get_fillRect();
          if (Object.op_Inequality((Object) fillRect, (Object) null))
            this._fillImageSettings.CopyTo((Image) ((Component) fillRect).GetComponent<Image>());
        }
        if (this._backgroundImageSettings == null)
          return;
        Transform transform = ((Component) item).get_transform().Find("Background");
        if (!Object.op_Inequality((Object) transform, (Object) null))
          return;
        this._backgroundImageSettings.CopyTo((Image) ((Component) transform).GetComponent<Image>());
      }

      public override void Apply(Selectable item)
      {
        base.Apply(item);
        this.Apply(item as Slider);
      }
    }

    [Serializable]
    private class ScrollbarSettings : ThemeSettings.SelectableSettings_Base
    {
      [SerializeField]
      private ThemeSettings.ImageSettings _handleImageSettings;
      [SerializeField]
      private ThemeSettings.ImageSettings _backgroundImageSettings;

      public ThemeSettings.ImageSettings handle
      {
        get
        {
          return this._handleImageSettings;
        }
      }

      public ThemeSettings.ImageSettings background
      {
        get
        {
          return this._backgroundImageSettings;
        }
      }

      private void Apply(Scrollbar item)
      {
        if (Object.op_Equality((Object) item, (Object) null))
          return;
        if (this._handleImageSettings != null)
          this._handleImageSettings.CopyTo(((Selectable) item).get_targetGraphic() as Image);
        if (this._backgroundImageSettings == null)
          return;
        this._backgroundImageSettings.CopyTo((Image) ((Component) item).GetComponent<Image>());
      }

      public override void Apply(Selectable item)
      {
        base.Apply(item);
        this.Apply(item as Scrollbar);
      }
    }

    [Serializable]
    private class ImageSettings
    {
      [SerializeField]
      private Color _color = Color.get_white();
      [SerializeField]
      private Sprite _sprite;
      [SerializeField]
      private Material _materal;
      [SerializeField]
      private Image.Type _type;
      [SerializeField]
      private bool _preserveAspect;
      [SerializeField]
      private bool _fillCenter;
      [SerializeField]
      private Image.FillMethod _fillMethod;
      [SerializeField]
      private float _fillAmout;
      [SerializeField]
      private bool _fillClockwise;
      [SerializeField]
      private int _fillOrigin;

      public Color color
      {
        get
        {
          return this._color;
        }
      }

      public Sprite sprite
      {
        get
        {
          return this._sprite;
        }
      }

      public Material materal
      {
        get
        {
          return this._materal;
        }
      }

      public Image.Type type
      {
        get
        {
          return this._type;
        }
      }

      public bool preserveAspect
      {
        get
        {
          return this._preserveAspect;
        }
      }

      public bool fillCenter
      {
        get
        {
          return this._fillCenter;
        }
      }

      public Image.FillMethod fillMethod
      {
        get
        {
          return this._fillMethod;
        }
      }

      public float fillAmout
      {
        get
        {
          return this._fillAmout;
        }
      }

      public bool fillClockwise
      {
        get
        {
          return this._fillClockwise;
        }
      }

      public int fillOrigin
      {
        get
        {
          return this._fillOrigin;
        }
      }

      public virtual void CopyTo(Image image)
      {
        if (Object.op_Equality((Object) image, (Object) null))
          return;
        ((Graphic) image).set_color(this._color);
        image.set_sprite(this._sprite);
        ((Graphic) image).set_material(this._materal);
        image.set_type(this._type);
        image.set_preserveAspect(this._preserveAspect);
        image.set_fillCenter(this._fillCenter);
        image.set_fillMethod(this._fillMethod);
        image.set_fillAmount(this._fillAmout);
        image.set_fillClockwise(this._fillClockwise);
        image.set_fillOrigin(this._fillOrigin);
      }
    }

    [Serializable]
    private struct CustomColorBlock
    {
      [SerializeField]
      private float m_ColorMultiplier;
      [SerializeField]
      private Color m_DisabledColor;
      [SerializeField]
      private float m_FadeDuration;
      [SerializeField]
      private Color m_HighlightedColor;
      [SerializeField]
      private Color m_NormalColor;
      [SerializeField]
      private Color m_PressedColor;
      [SerializeField]
      private Color m_DisabledHighlightedColor;

      public float colorMultiplier
      {
        get
        {
          return this.m_ColorMultiplier;
        }
        set
        {
          this.m_ColorMultiplier = value;
        }
      }

      public Color disabledColor
      {
        get
        {
          return this.m_DisabledColor;
        }
        set
        {
          this.m_DisabledColor = value;
        }
      }

      public float fadeDuration
      {
        get
        {
          return this.m_FadeDuration;
        }
        set
        {
          this.m_FadeDuration = value;
        }
      }

      public Color highlightedColor
      {
        get
        {
          return this.m_HighlightedColor;
        }
        set
        {
          this.m_HighlightedColor = value;
        }
      }

      public Color normalColor
      {
        get
        {
          return this.m_NormalColor;
        }
        set
        {
          this.m_NormalColor = value;
        }
      }

      public Color pressedColor
      {
        get
        {
          return this.m_PressedColor;
        }
        set
        {
          this.m_PressedColor = value;
        }
      }

      public Color disabledHighlightedColor
      {
        get
        {
          return this.m_DisabledHighlightedColor;
        }
        set
        {
          this.m_DisabledHighlightedColor = value;
        }
      }

      public static implicit operator ColorBlock(ThemeSettings.CustomColorBlock item)
      {
        ColorBlock colorBlock = (ColorBlock) null;
        ((ColorBlock) ref colorBlock).set_colorMultiplier(item.m_ColorMultiplier);
        ((ColorBlock) ref colorBlock).set_disabledColor(item.m_DisabledColor);
        ((ColorBlock) ref colorBlock).set_fadeDuration(item.m_FadeDuration);
        ((ColorBlock) ref colorBlock).set_highlightedColor(item.m_HighlightedColor);
        ((ColorBlock) ref colorBlock).set_normalColor(item.m_NormalColor);
        ((ColorBlock) ref colorBlock).set_pressedColor(item.m_PressedColor);
        return colorBlock;
      }
    }

    [Serializable]
    private struct CustomSpriteState
    {
      [SerializeField]
      private Sprite m_DisabledSprite;
      [SerializeField]
      private Sprite m_HighlightedSprite;
      [SerializeField]
      private Sprite m_PressedSprite;
      [SerializeField]
      private Sprite m_DisabledHighlightedSprite;

      public Sprite disabledSprite
      {
        get
        {
          return this.m_DisabledSprite;
        }
        set
        {
          this.m_DisabledSprite = value;
        }
      }

      public Sprite highlightedSprite
      {
        get
        {
          return this.m_HighlightedSprite;
        }
        set
        {
          this.m_HighlightedSprite = value;
        }
      }

      public Sprite pressedSprite
      {
        get
        {
          return this.m_PressedSprite;
        }
        set
        {
          this.m_PressedSprite = value;
        }
      }

      public Sprite disabledHighlightedSprite
      {
        get
        {
          return this.m_DisabledHighlightedSprite;
        }
        set
        {
          this.m_DisabledHighlightedSprite = value;
        }
      }

      public static implicit operator SpriteState(ThemeSettings.CustomSpriteState item)
      {
        SpriteState spriteState = (SpriteState) null;
        ((SpriteState) ref spriteState).set_disabledSprite(item.m_DisabledSprite);
        ((SpriteState) ref spriteState).set_highlightedSprite(item.m_HighlightedSprite);
        ((SpriteState) ref spriteState).set_pressedSprite(item.m_PressedSprite);
        return spriteState;
      }
    }

    [Serializable]
    private class CustomAnimationTriggers
    {
      [SerializeField]
      private string m_DisabledTrigger;
      [SerializeField]
      private string m_HighlightedTrigger;
      [SerializeField]
      private string m_NormalTrigger;
      [SerializeField]
      private string m_PressedTrigger;
      [SerializeField]
      private string m_DisabledHighlightedTrigger;

      public CustomAnimationTriggers()
      {
        this.m_DisabledTrigger = string.Empty;
        this.m_HighlightedTrigger = string.Empty;
        this.m_NormalTrigger = string.Empty;
        this.m_PressedTrigger = string.Empty;
        this.m_DisabledHighlightedTrigger = string.Empty;
      }

      public string disabledTrigger
      {
        get
        {
          return this.m_DisabledTrigger;
        }
        set
        {
          this.m_DisabledTrigger = value;
        }
      }

      public string highlightedTrigger
      {
        get
        {
          return this.m_HighlightedTrigger;
        }
        set
        {
          this.m_HighlightedTrigger = value;
        }
      }

      public string normalTrigger
      {
        get
        {
          return this.m_NormalTrigger;
        }
        set
        {
          this.m_NormalTrigger = value;
        }
      }

      public string pressedTrigger
      {
        get
        {
          return this.m_PressedTrigger;
        }
        set
        {
          this.m_PressedTrigger = value;
        }
      }

      public string disabledHighlightedTrigger
      {
        get
        {
          return this.m_DisabledHighlightedTrigger;
        }
        set
        {
          this.m_DisabledHighlightedTrigger = value;
        }
      }

      public static implicit operator AnimationTriggers(
        ThemeSettings.CustomAnimationTriggers item)
      {
        AnimationTriggers animationTriggers = new AnimationTriggers();
        animationTriggers.set_disabledTrigger(item.m_DisabledTrigger);
        animationTriggers.set_highlightedTrigger(item.m_HighlightedTrigger);
        animationTriggers.set_normalTrigger(item.m_NormalTrigger);
        animationTriggers.set_pressedTrigger(item.m_PressedTrigger);
        return animationTriggers;
      }
    }

    [Serializable]
    private class TextSettings
    {
      [SerializeField]
      private Color _color = Color.get_white();
      [SerializeField]
      private float _lineSpacing = 1f;
      [SerializeField]
      private float _sizeMultiplier = 1f;
      [SerializeField]
      private Font _font;
      [SerializeField]
      private ThemeSettings.FontStyleOverride _style;

      public Color color
      {
        get
        {
          return this._color;
        }
      }

      public Font font
      {
        get
        {
          return this._font;
        }
      }

      public ThemeSettings.FontStyleOverride style
      {
        get
        {
          return this._style;
        }
      }

      public float lineSpacing
      {
        get
        {
          return this._lineSpacing;
        }
      }

      public float sizeMultiplier
      {
        get
        {
          return this._sizeMultiplier;
        }
      }
    }

    private enum FontStyleOverride
    {
      Default,
      Normal,
      Bold,
      Italic,
      BoldAndItalic,
    }
  }
}
