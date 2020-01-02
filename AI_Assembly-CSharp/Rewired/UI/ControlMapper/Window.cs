// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.Window
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (CanvasGroup))]
  public class Window : MonoBehaviour
  {
    public Image backgroundImage;
    public GameObject content;
    private bool _initialized;
    private int _id;
    private RectTransform _rectTransform;
    private Text _titleText;
    private List<Text> _contentText;
    private GameObject _defaultUIElement;
    private Action<int> _updateCallback;
    private Func<int, bool> _isFocusedCallback;
    private Window.Timer _timer;
    private CanvasGroup _canvasGroup;
    public UnityAction cancelCallback;
    private GameObject lastUISelection;

    public Window()
    {
      base.\u002Ector();
    }

    public bool hasFocus
    {
      get
      {
        return this._isFocusedCallback != null && this._isFocusedCallback(this._id);
      }
    }

    public int id
    {
      get
      {
        return this._id;
      }
    }

    public RectTransform rectTransform
    {
      get
      {
        if (Object.op_Equality((Object) this._rectTransform, (Object) null))
          this._rectTransform = (RectTransform) ((Component) this).get_gameObject().GetComponent<RectTransform>();
        return this._rectTransform;
      }
    }

    public Text titleText
    {
      get
      {
        return this._titleText;
      }
    }

    public List<Text> contentText
    {
      get
      {
        return this._contentText;
      }
    }

    public GameObject defaultUIElement
    {
      get
      {
        return this._defaultUIElement;
      }
      set
      {
        this._defaultUIElement = value;
      }
    }

    public Action<int> updateCallback
    {
      get
      {
        return this._updateCallback;
      }
      set
      {
        this._updateCallback = value;
      }
    }

    public Window.Timer timer
    {
      get
      {
        return this._timer;
      }
    }

    public int width
    {
      get
      {
        return (int) this.rectTransform.get_sizeDelta().x;
      }
      set
      {
        Vector2 sizeDelta = this.rectTransform.get_sizeDelta();
        sizeDelta.x = (__Null) (double) value;
        this.rectTransform.set_sizeDelta(sizeDelta);
      }
    }

    public int height
    {
      get
      {
        return (int) this.rectTransform.get_sizeDelta().y;
      }
      set
      {
        Vector2 sizeDelta = this.rectTransform.get_sizeDelta();
        sizeDelta.y = (__Null) (double) value;
        this.rectTransform.set_sizeDelta(sizeDelta);
      }
    }

    protected bool initialized
    {
      get
      {
        return this._initialized;
      }
    }

    private void OnEnable()
    {
      this.StartCoroutine("OnEnableAsync");
    }

    protected virtual void Update()
    {
      if (!this._initialized || !this.hasFocus)
        return;
      this.CheckUISelection();
      if (this._updateCallback == null)
        return;
      this._updateCallback(this._id);
    }

    public virtual void Initialize(int id, Func<int, bool> isFocusedCallback)
    {
      if (this._initialized)
      {
        Debug.LogError((object) "Window is already initialized!");
      }
      else
      {
        this._id = id;
        this._isFocusedCallback = isFocusedCallback;
        this._timer = new Window.Timer();
        this._contentText = new List<Text>();
        this._canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
        this._initialized = true;
      }
    }

    public void SetSize(int width, int height)
    {
      this.rectTransform.set_sizeDelta(new Vector2((float) width, (float) height));
    }

    public void CreateTitleText(GameObject prefab, Vector2 offset)
    {
      this.CreateText(prefab, ref this._titleText, "Title Text", UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), offset);
    }

    public void CreateTitleText(GameObject prefab, Vector2 offset, string text)
    {
      this.CreateTitleText(prefab, offset);
      this.SetTitleText(text);
    }

    public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
    {
      Text textComponent = (Text) null;
      this.CreateText(prefab, ref textComponent, "Content Text", pivot, anchor, offset);
      this._contentText.Add(textComponent);
    }

    public void AddContentText(
      GameObject prefab,
      UIPivot pivot,
      UIAnchor anchor,
      Vector2 offset,
      string text)
    {
      this.AddContentText(prefab, pivot, anchor, offset);
      this.SetContentText(text, this._contentText.Count - 1);
    }

    public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
    {
      this.CreateImage(prefab, "Image", pivot, anchor, offset);
    }

    public void AddContentImage(
      GameObject prefab,
      UIPivot pivot,
      UIAnchor anchor,
      Vector2 offset,
      string text)
    {
      this.AddContentImage(prefab, pivot, anchor, offset);
    }

    public void CreateButton(
      GameObject prefab,
      UIPivot pivot,
      UIAnchor anchor,
      Vector2 offset,
      string buttonText,
      UnityAction confirmCallback,
      UnityAction cancelCallback,
      bool setDefault)
    {
      if (Object.op_Equality((Object) prefab, (Object) null))
        return;
      ButtonInfo buttonInfo;
      GameObject button = this.CreateButton(prefab, "Button", anchor, pivot, offset, out buttonInfo);
      if (Object.op_Equality((Object) button, (Object) null))
        return;
      Button component = (Button) button.GetComponent<Button>();
      if (confirmCallback != null)
        ((UnityEvent) component.get_onClick()).AddListener(confirmCallback);
      CustomButton customButton = component as CustomButton;
      if (cancelCallback != null && Object.op_Inequality((Object) customButton, (Object) null))
        customButton.CancelEvent += cancelCallback;
      if (Object.op_Inequality((Object) buttonInfo.text, (Object) null))
        buttonInfo.text.set_text(buttonText);
      if (!setDefault)
        return;
      this._defaultUIElement = button;
    }

    public string GetTitleText(string text)
    {
      return Object.op_Equality((Object) this._titleText, (Object) null) ? string.Empty : this._titleText.get_text();
    }

    public void SetTitleText(string text)
    {
      if (Object.op_Equality((Object) this._titleText, (Object) null))
        return;
      this._titleText.set_text(text);
    }

    public string GetContentText(int index)
    {
      return this._contentText == null || this._contentText.Count <= index || Object.op_Equality((Object) this._contentText[index], (Object) null) ? string.Empty : this._contentText[index].get_text();
    }

    public float GetContentTextHeight(int index)
    {
      return this._contentText == null || this._contentText.Count <= index || Object.op_Equality((Object) this._contentText[index], (Object) null) ? 0.0f : (float) ((Graphic) this._contentText[index]).get_rectTransform().get_sizeDelta().y;
    }

    public void SetContentText(string text, int index)
    {
      if (this._contentText == null || this._contentText.Count <= index || Object.op_Equality((Object) this._contentText[index], (Object) null))
        return;
      this._contentText[index].set_text(text);
    }

    public void SetUpdateCallback(Action<int> callback)
    {
      this.updateCallback = callback;
    }

    public virtual void TakeInputFocus()
    {
      if (Object.op_Equality((Object) EventSystem.get_current(), (Object) null))
        return;
      EventSystem.get_current().SetSelectedGameObject(this._defaultUIElement);
      this.Enable();
    }

    public virtual void Enable()
    {
      this._canvasGroup.set_interactable(true);
    }

    public virtual void Disable()
    {
      this._canvasGroup.set_interactable(false);
    }

    public virtual void Cancel()
    {
      if (!this.initialized || this.cancelCallback == null)
        return;
      this.cancelCallback.Invoke();
    }

    private void CreateText(
      GameObject prefab,
      ref Text textComponent,
      string name,
      UIPivot pivot,
      UIAnchor anchor,
      Vector2 offset)
    {
      if (Object.op_Equality((Object) prefab, (Object) null) || Object.op_Equality((Object) this.content, (Object) null))
        return;
      if (Object.op_Inequality((Object) textComponent, (Object) null))
      {
        Debug.LogError((object) ("Window already has " + name + "!"));
      }
      else
      {
        GameObject gameObject = UITools.InstantiateGUIObject<Text>(prefab, this.content.get_transform(), name, UIPivot.op_Implicit(pivot), (Vector2) anchor.min, (Vector2) anchor.max, offset);
        if (Object.op_Equality((Object) gameObject, (Object) null))
          return;
        textComponent = (Text) gameObject.GetComponent<Text>();
      }
    }

    private void CreateImage(
      GameObject prefab,
      string name,
      UIPivot pivot,
      UIAnchor anchor,
      Vector2 offset)
    {
      if (Object.op_Equality((Object) prefab, (Object) null) || Object.op_Equality((Object) this.content, (Object) null))
        return;
      UITools.InstantiateGUIObject<Image>(prefab, this.content.get_transform(), name, UIPivot.op_Implicit(pivot), (Vector2) anchor.min, (Vector2) anchor.max, offset);
    }

    private GameObject CreateButton(
      GameObject prefab,
      string name,
      UIAnchor anchor,
      UIPivot pivot,
      Vector2 offset,
      out ButtonInfo buttonInfo)
    {
      buttonInfo = (ButtonInfo) null;
      if (Object.op_Equality((Object) prefab, (Object) null))
        return (GameObject) null;
      GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(prefab, this.content.get_transform(), name, UIPivot.op_Implicit(pivot), (Vector2) anchor.min, (Vector2) anchor.max, offset);
      if (Object.op_Equality((Object) gameObject, (Object) null))
        return (GameObject) null;
      buttonInfo = (ButtonInfo) gameObject.GetComponent<ButtonInfo>();
      if (Object.op_Equality((Object) gameObject.GetComponent<Button>(), (Object) null))
      {
        Debug.Log((object) "Button prefab is missing Button component!");
        return (GameObject) null;
      }
      if (!Object.op_Equality((Object) buttonInfo, (Object) null))
        return gameObject;
      Debug.Log((object) "Button prefab is missing ButtonInfo component!");
      return (GameObject) null;
    }

    [DebuggerHidden]
    private IEnumerator OnEnableAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Window.\u003COnEnableAsync\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void CheckUISelection()
    {
      if (!this.hasFocus || Object.op_Equality((Object) EventSystem.get_current(), (Object) null))
        return;
      if (Object.op_Equality((Object) EventSystem.get_current().get_currentSelectedGameObject(), (Object) null))
        this.RestoreDefaultOrLastUISelection();
      this.lastUISelection = EventSystem.get_current().get_currentSelectedGameObject();
    }

    private void RestoreDefaultOrLastUISelection()
    {
      if (!this.hasFocus)
        return;
      if (Object.op_Equality((Object) this.lastUISelection, (Object) null) || !this.lastUISelection.get_activeInHierarchy())
        this.SetUISelection(this._defaultUIElement);
      else
        this.SetUISelection(this.lastUISelection);
    }

    private void SetUISelection(GameObject selection)
    {
      if (Object.op_Equality((Object) EventSystem.get_current(), (Object) null))
        return;
      EventSystem.get_current().SetSelectedGameObject(selection);
    }

    public class Timer
    {
      private bool _started;
      private float end;

      public bool started
      {
        get
        {
          return this._started;
        }
      }

      public bool finished
      {
        get
        {
          if (!this.started || (double) Time.get_realtimeSinceStartup() < (double) this.end)
            return false;
          this._started = false;
          return true;
        }
      }

      public float remaining
      {
        get
        {
          return !this._started ? 0.0f : this.end - Time.get_realtimeSinceStartup();
        }
      }

      public void Start(float length)
      {
        this.end = Time.get_realtimeSinceStartup() + length;
        this._started = true;
      }

      public void Stop()
      {
        this._started = false;
      }
    }
  }
}
