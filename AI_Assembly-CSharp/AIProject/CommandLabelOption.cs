// Decompiled with JetBrains decompiler
// Type: AIProject.CommandLabelOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ReMotion;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject
{
  public class CommandLabelOption : MonoBehaviour
  {
    private static readonly Vector2 _sizeOffset = new Vector2(20f, 20f);
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private CanvasGroup _targetCanvasGroup;
    [SerializeField]
    private CanvasGroup _targetSelectedCanvasGroup;
    [SerializeField]
    private Image _targetImage;
    [SerializeField]
    private Image _targetSelectedImage;
    [SerializeField]
    private ImageSpriteSheet _targetImageSS;
    [SerializeField]
    private ImageSpriteSheet _targetSelectedImageSS;
    [SerializeField]
    private CanvasGroup _progressBackground;
    [SerializeField]
    private CanvasGroup _progressCanvasGroup;
    [SerializeField]
    private Image _progressImage;
    [SerializeField]
    private Image _coolTimeImage;
    [SerializeField]
    private RectTransform _labelRect;
    [SerializeField]
    private RectTransform _rect;
    [SerializeField]
    private Image _panel;
    [SerializeField]
    private Text _textComponent;
    [SerializeField]
    private GameObject[] _blankSpaceRects;
    [SerializeField]
    private RectTransform _iconRect;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private CanvasGroup _panelCanvasGroup;
    [SerializeField]
    private CanvasGroup _iconCanvasGroup;
    [SerializeField]
    private CanvasGroup _labelCanvasGroup;
    [SerializeField]
    private Vector2 _labelDisplayOffset;
    private CommandLabel.CommandInfo _commandInfo;
    private const float _fadeTime = 0.3f;
    private Rect _canvasBaseRect;
    private int _visibility;
    private IDisposable _disposable;
    private Vector2 _smoothVelocityVector;
    private float _smoothVelocity;

    public CommandLabelOption()
    {
      base.\u002Ector();
    }

    public CommandLabel.LabelPool PoolSource { get; set; }

    public CommandLabel.CommandInfo CommandInfo
    {
      get
      {
        return this._commandInfo;
      }
      set
      {
        this._commandInfo = value;
        if (value == null)
          return;
        if (Object.op_Inequality((Object) this._textComponent, (Object) null))
        {
          if (value.OnText != null)
            this._textComponent.set_text(value.OnText());
          else
            this._textComponent.set_text(value.Text);
        }
        CommandTargetSpriteInfo targetSpriteInfo = value.TargetSpriteInfo;
        if (targetSpriteInfo != null)
        {
          ImageSpriteSheet targetImageSs = this._targetImageSS;
          float fps = targetSpriteInfo.FPS;
          this._targetSelectedImageSS.set_FPS(fps);
          double num = (double) fps;
          targetImageSs.set_FPS((float) num);
          this._targetImageSS.set_Sprites(targetSpriteInfo.Sprites);
          this._targetSelectedImageSS.set_Sprites(targetSpriteInfo.SelectedSprites);
        }
        if (Object.op_Inequality((Object) this._targetImage, (Object) null))
          ((Component) this._targetImage).get_gameObject().SetActive(targetSpriteInfo != null && !targetSpriteInfo.Sprites.IsNullOrEmpty<Sprite>());
        if (Object.op_Inequality((Object) this._targetSelectedImage, (Object) null))
          ((Component) this._targetSelectedImage).get_gameObject().SetActive(targetSpriteInfo != null && !targetSpriteInfo.SelectedSprites.IsNullOrEmpty<Sprite>());
        if (Object.op_Inequality((Object) this._progressBackground, (Object) null))
          this._progressBackground.set_alpha(!value.IsHold ? 0.0f : 1f);
        if (Object.op_Inequality((Object) this._progressCanvasGroup, (Object) null))
          this._progressCanvasGroup.set_alpha(!value.IsHold ? 0.0f : 1f);
        if (!Object.op_Inequality((Object) this._icon, (Object) null))
          return;
        this._icon.set_sprite(value.Icon);
        ((Component) this._icon).get_gameObject().SetActive(Object.op_Inequality((Object) value.Icon, (Object) null));
      }
    }

    public int Visibility
    {
      get
      {
        return this._visibility;
      }
      set
      {
        if (this._visibility == value)
          return;
        this._visibility = value;
        if (this._disposable != null)
          this._disposable.Dispose();
        float startValueA = this._canvasGroup.get_alpha();
        float startValueB = !Object.op_Inequality((Object) this._labelCanvasGroup, (Object) null) ? 0.0f : this._labelCanvasGroup.get_alpha();
        float startValueC = !Object.op_Inequality((Object) this._targetCanvasGroup, (Object) null) ? 0.0f : this._targetCanvasGroup.get_alpha();
        float startValueD = !Object.op_Inequality((Object) this._targetSelectedCanvasGroup, (Object) null) ? 0.0f : this._targetSelectedCanvasGroup.get_alpha();
        IObservable<TimeInterval<float>> observable = (IObservable<TimeInterval<float>>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.3f, false), ((Component) this._canvasGroup).get_gameObject()), false);
        switch (value)
        {
          case 0:
            this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (Action<M0>) (x =>
            {
              this._canvasGroup.set_alpha(Mathf.Lerp(startValueA, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._labelCanvasGroup, (Object) null))
                this._labelCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._targetCanvasGroup, (Object) null))
                this._targetCanvasGroup.set_alpha(Mathf.Lerp(startValueC, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._targetSelectedCanvasGroup, (Object) null))
                this._targetSelectedCanvasGroup.set_alpha(Mathf.Lerp(startValueD, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._panelCanvasGroup, (Object) null))
                this._panelCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (!Object.op_Inequality((Object) this._iconCanvasGroup, (Object) null))
                return;
              this._iconCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
            }), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() =>
            {
              if (Object.op_Inequality((Object) this._targetImage, (Object) null) && ((Component) this._targetImage).get_gameObject().get_activeSelf())
                ((Component) this._targetImage).get_gameObject().SetActive(false);
              if (this.PoolSource != null)
                this.PoolSource.Return(this);
              else
                ((Component) this).get_gameObject().SetActive(false);
              this.SetActiveText(false);
            }));
            break;
          case 1:
            if (Object.op_Inequality((Object) this._targetImage, (Object) null) && !((Component) this._targetImage).get_gameObject().get_activeSelf() && Object.op_Inequality((Object) this._commandInfo.Transform, (Object) null))
              ((Component) this._targetImage).get_gameObject().SetActive(true);
            if (Object.op_Inequality((Object) this._targetImage, (Object) null))
              ((Behaviour) this._targetImage).set_enabled(true);
            this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (Action<M0>) (x =>
            {
              this._canvasGroup.set_alpha(Mathf.Lerp(startValueA, 1f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._labelCanvasGroup, (Object) null))
                this._labelCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._targetCanvasGroup, (Object) null))
                this._targetCanvasGroup.set_alpha(Mathf.Lerp(startValueC, 1f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._targetSelectedCanvasGroup, (Object) null))
                this._targetSelectedCanvasGroup.set_alpha(Mathf.Lerp(startValueD, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._panelCanvasGroup, (Object) null))
                this._panelCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              if (!Object.op_Inequality((Object) this._iconCanvasGroup, (Object) null))
                return;
              this._iconCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
            }), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() => this.SetActiveText(false)));
            break;
          case 2:
            ((Component) this).get_transform().SetAsLastSibling();
            this.SetActiveText(true);
            if (Object.op_Inequality((Object) this._targetImage, (Object) null) && !((Component) this._targetImage).get_gameObject().get_activeSelf() && Object.op_Inequality((Object) this._commandInfo.Transform, (Object) null))
              ((Component) this._targetImage).get_gameObject().SetActive(true);
            if (Object.op_Inequality((Object) this._targetImage, (Object) null))
              ((Behaviour) this._targetImage).set_enabled(false);
            this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (Action<M0>) (x =>
            {
              this._canvasGroup.set_alpha(Mathf.Lerp(startValueA, 1f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._labelCanvasGroup, (Object) null))
                this._labelCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 1f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._targetCanvasGroup, (Object) null))
                this._targetCanvasGroup.set_alpha(Mathf.Lerp(startValueC, 1f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._targetSelectedCanvasGroup, (Object) null))
                this._targetSelectedCanvasGroup.set_alpha(Mathf.Lerp(startValueD, 1f, ((TimeInterval<float>) ref x).get_Value()));
              if (Object.op_Inequality((Object) this._panelCanvasGroup, (Object) null))
                this._panelCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 1f, ((TimeInterval<float>) ref x).get_Value()));
              if (!Object.op_Inequality((Object) this._iconCanvasGroup, (Object) null))
                return;
              this._iconCanvasGroup.set_alpha(Mathf.Lerp(startValueB, 1f, ((TimeInterval<float>) ref x).get_Value()));
            }));
            break;
        }
        if (value <= 0)
          return;
        if (Object.op_Inequality((Object) this._commandInfo.Transform, (Object) null))
        {
          if (Singleton<Manager.Map>.IsInstance())
            ((Component) this).get_transform().set_position(Vector2.op_Implicit(RectTransformUtility.WorldToScreenPoint(Singleton<Manager.Map>.Instance.Player.CameraControl.CameraComponent, this._commandInfo.Transform.get_position())));
          else
            ((Component) this).get_transform().set_position(Vector3.get_zero());
        }
        else
          (((Component) this).get_transform() as RectTransform).set_anchoredPosition(Vector2.get_zero());
      }
    }

    private void Start()
    {
      this._canvasBaseRect = (((Component) ((Graphic) this._icon).get_canvas()).get_transform() as RectTransform).get_rect();
    }

    private void OnEnable()
    {
      if (!Object.op_Inequality((Object) this._panel, (Object) null))
        return;
      ((Graphic) this._panel).get_rectTransform().SetSizeWithCurrentAnchors((RectTransform.Axis) 0, (float) this._rect.get_sizeDelta().x);
    }

    private void LateUpdate()
    {
      float unscaledDeltaTime = Time.get_unscaledDeltaTime();
      if (Object.op_Inequality((Object) this._panel, (Object) null))
        ((Graphic) this._panel).get_rectTransform().SetSizeWithCurrentAnchors((RectTransform.Axis) 0, Mathf.SmoothDamp((float) ((Graphic) this._panel).get_rectTransform().get_sizeDelta().x, (float) this._rect.get_sizeDelta().x, ref this._smoothVelocity, 0.3f, float.PositiveInfinity, unscaledDeltaTime));
      if (Object.op_Inequality((Object) this._icon, (Object) null) && Object.op_Inequality((Object) this._iconRect, (Object) null))
        ((Component) this._icon).get_transform().set_position(Vector2.op_Implicit(Vector2.SmoothDamp(Vector2.op_Implicit(((Component) this._icon).get_transform().get_position()), Vector2.op_Implicit(((Transform) this._iconRect).get_position()), ref this._smoothVelocityVector, 0.3f, float.PositiveInfinity, unscaledDeltaTime)));
      if (this._commandInfo == null)
        return;
      CommandTargetSpriteInfo targetSpriteInfo = this._commandInfo.TargetSpriteInfo;
      Func<float> coolTimeFillRate = this._commandInfo.CoolTimeFillRate;
      float? nullable1 = coolTimeFillRate != null ? new float?(coolTimeFillRate()) : new float?();
      float num1 = !nullable1.HasValue ? 0.0f : nullable1.Value;
      bool flag = (double) num1 > 0.0;
      Func<PlayerActor, bool> condition = this._commandInfo.Condition;
      bool? nullable2 = condition != null ? new bool?(condition(Singleton<Manager.Map>.Instance.Player)) : new bool?();
      if ((!nullable2.HasValue ? 1 : (nullable2.Value ? 1 : 0)) != 0)
      {
        ((Graphic) this._textComponent).set_color(Color32.op_Implicit(new Color32((byte) 235, (byte) 225, (byte) 215, byte.MaxValue)));
        if (targetSpriteInfo != null)
        {
          if (this._targetImageSS.get_Sprites() != targetSpriteInfo.Sprites)
            this._targetImageSS.set_Sprites(targetSpriteInfo.Sprites);
          if (this._targetSelectedImageSS.get_Sprites() != targetSpriteInfo.SelectedSprites)
            this._targetSelectedImageSS.set_Sprites(targetSpriteInfo.SelectedSprites);
        }
      }
      else
      {
        ((Graphic) this._textComponent).set_color(Color32.op_Implicit(new Color32((byte) 235, (byte) 225, (byte) 215, (byte) 127)));
        if (targetSpriteInfo != null)
        {
          if (flag)
          {
            if (this._targetImageSS.get_Sprites() != targetSpriteInfo.CoolTimeSprites)
              this._targetImageSS.set_Sprites(targetSpriteInfo.CoolTimeSprites);
            if (this._targetSelectedImageSS.get_Sprites() != targetSpriteInfo.CoolTimeSprites)
              this._targetSelectedImageSS.set_Sprites(targetSpriteInfo.CoolTimeSprites);
          }
          else
          {
            if (this._targetImageSS.get_Sprites() != targetSpriteInfo.DisableSprites)
              this._targetImageSS.set_Sprites(targetSpriteInfo.DisableSprites);
            if (this._targetSelectedImageSS.get_Sprites() != targetSpriteInfo.DisableSprites)
              this._targetSelectedImageSS.set_Sprites(targetSpriteInfo.DisableSprites);
          }
        }
      }
      if (Object.op_Inequality((Object) this._progressImage, (Object) null))
        this._progressImage.set_fillAmount(this._commandInfo.FillRate);
      if (Object.op_Inequality((Object) this._coolTimeImage, (Object) null))
      {
        if (((Behaviour) this._coolTimeImage).get_enabled() != flag)
          ((Behaviour) this._coolTimeImage).set_enabled(flag);
        if (flag)
          this._coolTimeImage.set_fillAmount(num1);
      }
      if (!Object.op_Inequality((Object) this._commandInfo.Transform, (Object) null))
        return;
      Camera cameraComponent = Singleton<Manager.Map>.Instance.Player.CameraControl.CameraComponent;
      Vector3 screenPoint = cameraComponent.WorldToScreenPoint(this._commandInfo.Transform.get_position());
      RectTransform rectTransform = ((Graphic) this._targetImage).get_rectTransform();
      Rect canvasBaseRect = this._canvasBaseRect;
      ref Rect local1 = ref canvasBaseRect;
      Rect rect1 = rectTransform.get_rect();
      double num2 = (double) ((Rect) ref rect1).get_width() * 0.5 + CommandLabelOption._sizeOffset.x;
      ((Rect) ref local1).set_x((float) num2);
      ((Rect) ref canvasBaseRect).set_y((float) CommandLabelOption._sizeOffset.y);
      ref Rect local2 = ref canvasBaseRect;
      double width1 = (double) ((Rect) ref local2).get_width();
      // ISSUE: variable of the null type
      __Null local3 = CommandLabelOption._sizeOffset.x + this._labelDisplayOffset.x;
      Rect rect2 = this._rect.get_rect();
      double width2 = (double) ((Rect) ref rect2).get_width();
      double num3 = local3 + width2;
      ((Rect) ref local2).set_width((float) (width1 - num3));
      ref Rect local4 = ref canvasBaseRect;
      double height1 = (double) ((Rect) ref local4).get_height();
      // ISSUE: variable of the null type
      __Null local5 = CommandLabelOption._sizeOffset.y + this._labelDisplayOffset.y;
      Rect rect3 = this._rect.get_rect();
      double height2 = (double) ((Rect) ref rect3).get_height();
      double num4 = local5 + height2;
      ((Rect) ref local4).set_height((float) (height1 - num4));
      Vector3 position1 = ((Component) cameraComponent).get_transform().get_position();
      position1.y = (__Null) 0.0;
      Vector3 position2 = this._commandInfo.Transform.get_position();
      position2.y = (__Null) 0.0;
      if ((double) Quaternion.Angle(Quaternion.LookRotation(Vector3.op_Subtraction(position2, position1)), ((Component) cameraComponent).get_transform().get_rotation()) >= 120.0)
        return;
      Vector2 vector2 = Vector2.op_Implicit(screenPoint);
      vector2.x = (__Null) (double) Mathf.Clamp((float) vector2.x, ((Rect) ref canvasBaseRect).get_x(), ((Rect) ref canvasBaseRect).get_width());
      vector2.y = (__Null) (double) Mathf.Clamp((float) vector2.y, ((Rect) ref canvasBaseRect).get_y(), ((Rect) ref canvasBaseRect).get_height());
      ((Transform) rectTransform).set_position(Vector2.op_Implicit(vector2));
      ((Transform) this._labelRect).set_position(Vector2.op_Implicit(Vector2.op_Addition(vector2, this._labelDisplayOffset)));
    }

    private void SetActiveText(bool active)
    {
      if (Object.op_Inequality((Object) this._panel, (Object) null) && ((Component) this._panel).get_gameObject().get_activeSelf() != active)
        ((Component) this._panel).get_gameObject().SetActive(active);
      if (active)
      {
        bool flag = Object.op_Inequality((Object) this._commandInfo.Icon, (Object) null);
        if (((Component) this._icon).get_gameObject().get_activeSelf() != flag)
          ((Component) this._icon).get_gameObject().SetActive(flag);
      }
      else if (((Component) this._icon).get_gameObject().get_activeSelf())
        ((Component) this._icon).get_gameObject().SetActive(false);
      if (((Component) this._textComponent).get_gameObject().get_activeSelf() != active)
        ((Component) this._textComponent).get_gameObject().SetActive(active);
      foreach (GameObject blankSpaceRect in this._blankSpaceRects)
      {
        if (blankSpaceRect.get_gameObject().get_activeSelf() != active)
          blankSpaceRect.SetActive(active);
      }
    }

    private void Reset()
    {
      this._rect = ((Component) this).get_transform() as RectTransform;
    }
  }
}
