// Decompiled with JetBrains decompiler
// Type: AIProject.UI.JukeBoxAudioListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using Illusion.Game.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (CanvasGroup))]
  [RequireComponent(typeof (RectTransform))]
  public class JukeBoxAudioListUI : MenuUIBehaviour
  {
    [SerializeField]
    private string _noneStr = string.Empty;
    [SerializeField]
    private List<string> _extensionList = new List<string>();
    [SerializeField]
    private Color _whiteColor = Color.get_white();
    [SerializeField]
    private Color _yellowColor = Color.get_yellow();
    private List<JukeBoxAudioListUI.AudioData> _elementPool = new List<JukeBoxAudioListUI.AudioData>();
    private List<string> _filePathList = new List<string>();
    private List<JukeBoxAudioListUI.AudioData> _elementList = new List<JukeBoxAudioListUI.AudioData>();
    private CompositeDisposable _allDisposable = new CompositeDisposable();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Toggle _sortToggle;
    [SerializeField]
    private Image _sortSelectedImage;
    [SerializeField]
    private Button _sortButton;
    [SerializeField]
    private Transform _elementRoot;
    [SerializeField]
    private GameObject _elementPrefab;
    [SerializeField]
    private Button _setButton;
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _pauseButton;
    [SerializeField]
    private JukeBoxUI _mainUI;
    [SerializeField]
    private AudioSortUI _sortUI;
    private Button _noneButton;
    private JukeBoxAudioListUI.AudioData _noneData;
    private JukeBoxAudioListUI.AudioData _selectedElement;
    private AudioSource _currentPlayAudio;
    private uAudio.uAudio_backend.uAudio _uAudio;
    private JukeBoxAudioListUI.AudioData _lastAudioData;

    public string NoneStr
    {
      get
      {
        return this._noneStr;
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    public bool InputEnabled
    {
      get
      {
        return this.EnabledInput && this._focusLevel == Singleton<Input>.Instance.FocusLevel;
      }
    }

    public IReadOnlyList<JukeBoxAudioListUI.AudioData> ElementList
    {
      get
      {
        return (IReadOnlyList<JukeBoxAudioListUI.AudioData>) this._elementList;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        this._canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (!Object.op_Equality((Object) this._rectTransform, (Object) null))
        return;
      this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
    }

    protected override void OnBeforeStart()
    {
      base.OnBeforeStart();
      ObservableExtensions.Subscribe<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Component) this), (Action<M0>) (x => this.SetActiveControl(x)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Func<M0, bool>) (_ => this.IsActiveControl)), (Action<M0>) (_ => this.DoClose()));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._sortButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this._sortUI.IsActiveControl = !this._sortUI.IsActiveControl));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._setButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.DoSetAudio()));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._rightButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.DoMove(1)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._leftButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.DoMove(-1)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._playButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.DoPlay()));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._pauseButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.AudioStop()));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this._sortToggle), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.ListSort()));
      if (Object.op_Inequality((Object) ((Selectable) this._sortToggle).get_targetGraphic(), (Object) null))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this._sortToggle), (Action<M0>) (isOn => ((Behaviour) ((Selectable) this._sortToggle).get_targetGraphic()).set_enabled(!isOn)));
      this._sortUI.ToggleIndexChanged = (Action<int>) (x => this.ListSort());
      if (!Object.op_Inequality((Object) this._sortSelectedImage, (Object) null))
        return;
      ((Behaviour) this._sortSelectedImage).set_enabled(false);
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._sortToggle), (Action<M0>) (_ => ((Behaviour) this._sortSelectedImage).set_enabled(true)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._sortToggle), (Action<M0>) (_ => ((Behaviour) this._sortSelectedImage).set_enabled(false)));
    }

    protected override void OnAfterStart()
    {
      base.OnAfterStart();
    }

    public void Hide()
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        return;
      this._canvasGroup.SetBlocksRaycasts(false);
      this._canvasGroup.SetInteractable(false);
      this._canvasGroup.set_alpha(0.0f);
    }

    public bool IsPlayingAudio
    {
      get
      {
        return Object.op_Inequality((Object) this._currentPlayAudio, (Object) null) && Object.op_Inequality((Object) ((Component) this._currentPlayAudio).get_gameObject(), (Object) null) && this._currentPlayAudio.get_isPlaying();
      }
    }

    private void DoPlay()
    {
      if (this._lastAudioData != null && this._selectedElement == this._lastAudioData)
        return;
      this.AudioStop();
      if (this._selectedElement == null)
        return;
      if (this._uAudio != null)
        this._uAudio.Dispose();
      bool loadedTarget = false;
      AudioClip clip = SoundPlayer.LoadAudioClip(this._selectedElement.filePath, ref loadedTarget, this._uAudio);
      if (Object.op_Inequality((Object) clip, (Object) null))
      {
        this._currentPlayAudio = Illusion.Game.Utils.Sound.Play(Manager.Sound.Type.BGM, clip, 0.0f);
        this._lastAudioData = this._selectedElement;
      }
      if (!Object.op_Inequality((Object) this._currentPlayAudio, (Object) null))
        return;
      ((Component) this._playButton).SetActiveSelf(false);
      ((Component) this._pauseButton).SetActiveSelf(true);
    }

    public void DoPause()
    {
      this.AudioStop();
    }

    public void AudioStop()
    {
      if (this.IsPlayingAudio)
        this._currentPlayAudio.Stop();
      this._currentPlayAudio = (AudioSource) null;
      ((Component) this._playButton).SetActiveSelf(true);
      ((Component) this._pauseButton).SetActiveSelf(false);
      this._lastAudioData = (JukeBoxAudioListUI.AudioData) null;
    }

    private void DoMove(int move)
    {
      if (this._selectedElement != null || this._lastAudioData != null)
      {
        if (((IReadOnlyList<JukeBoxAudioListUI.AudioData>) this._elementList).IsNullOrEmpty<JukeBoxAudioListUI.AudioData>() || 2 > this._elementList.Count)
          return;
        int index = (this._selectedElement ?? this._lastAudioData).index + move;
        if (this._elementList.Count <= index)
          index = 0;
        else if (index < 0)
          index = this._elementList.Count - 1;
        this.ChangeSelectedElement(this._elementList.GetElement<JukeBoxAudioListUI.AudioData>(index));
      }
      else
        this.ChangeSelectedElement(this._elementList.GetElement<JukeBoxAudioListUI.AudioData>(0));
    }

    private void DoSetAudio()
    {
      if (this._selectedElement == null)
        return;
      this._mainUI.SetAudioName(this._selectedElement.text.get_text(), this._selectedElement.fileName);
      this.PlaySE(SoundPack.SystemSE.OK_L);
    }

    private void DoClose()
    {
      this.IsActiveControl = false;
    }

    private void SetActiveControl(bool active)
    {
      this._allDisposable.Clear();
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Component) this)), (ICollection<IDisposable>) this._allDisposable);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new JukeBoxAudioListUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new JukeBoxAudioListUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void UISetting()
    {
      this.Release();
      if (this._noneData == null)
      {
        this._noneData = this.GetElement();
        this._noneData.text.set_text(this._noneData.fileName = this._noneStr);
        this._noneData.filePath = string.Empty;
      }
      ((Component) this._playButton).SetActiveSelf(true);
      ((Component) this._pauseButton).SetActiveSelf(false);
      this._filePathList.Clear();
      if (!((IReadOnlyList<string>) this._extensionList).IsNullOrEmpty<string>())
      {
        foreach (string extension in this._extensionList)
        {
          if (!extension.IsNullOrEmpty())
          {
            string[] files = System.IO.Directory.GetFiles(SoundPlayer.Directory.AudioFile, string.Format("*.{0}", (object) extension));
            if (!((IReadOnlyList<string>) files).IsNullOrEmpty<string>())
              this._filePathList.AddRange((IEnumerable<string>) files);
          }
        }
      }
      if (!((IReadOnlyList<string>) this._filePathList).IsNullOrEmpty<string>())
      {
        foreach (string filePath in this._filePathList)
        {
          if (!filePath.IsNullOrEmpty())
          {
            JukeBoxAudioListUI.AudioData element = this.GetElement();
            element.filePath = filePath;
            element.fileName = Path.GetFileName(filePath);
            element.text.set_text(Path.GetFileNameWithoutExtension(filePath));
            element.dateTime = File.GetLastWriteTime(filePath);
            this._elementList.Add(element);
          }
        }
      }
      this._elementList.Insert(0, this._noneData);
      this.ListSort();
      foreach (JukeBoxAudioListUI.AudioData element in this._elementList)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        JukeBoxAudioListUI.\u003CUISetting\u003Ec__AnonStorey5 settingCAnonStorey5 = new JukeBoxAudioListUI.\u003CUISetting\u003Ec__AnonStorey5();
        // ISSUE: reference to a compiler-generated field
        settingCAnonStorey5.elm = element;
        // ISSUE: reference to a compiler-generated field
        settingCAnonStorey5.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        ((Component) settingCAnonStorey5.elm.button).SetActiveSelf(true);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        settingCAnonStorey5.elm.ClickAction = new Action<JukeBoxAudioListUI.AudioData>(settingCAnonStorey5.\u003C\u003Em__0);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent) settingCAnonStorey5.elm.button.get_onClick()).AddListener(new UnityAction((object) settingCAnonStorey5, __methodptr(\u003C\u003Em__1)));
        // ISSUE: reference to a compiler-generated field
        settingCAnonStorey5.elm.doubleClickDisposable?.Dispose();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        settingCAnonStorey5.elm.doubleClickDisposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<IList<double>>((IObservable<M0>) UnityEventExtensions.AsObservable((UnityEvent) settingCAnonStorey5.elm.button.get_onClick()).DoubleInterval<Unit>(250f, false), (Action<M0>) new Action<IList<double>>(settingCAnonStorey5.\u003C\u003Em__2)), (Component) settingCAnonStorey5.elm.button);
      }
    }

    private void ListSort()
    {
      if (((IReadOnlyList<JukeBoxAudioListUI.AudioData>) this._elementList).IsNullOrEmpty<JukeBoxAudioListUI.AudioData>() || this._elementList.Count <= 2)
        return;
      if (this._noneData != null && this._elementList.Contains(this._noneData))
        this._elementList.Remove(this._noneData);
      switch (this._sortUI.SortIndex())
      {
        case 0:
          if (this._sortToggle.get_isOn())
          {
            this._elementList.Sort((Comparison<JukeBoxAudioListUI.AudioData>) ((a, b) => a.dateTime.CompareTo(b.dateTime)));
            break;
          }
          this._elementList.Sort((Comparison<JukeBoxAudioListUI.AudioData>) ((a, b) => b.dateTime.CompareTo(a.dateTime)));
          break;
        case 1:
          if (this._sortToggle.get_isOn())
          {
            this._elementList.Sort((Comparison<JukeBoxAudioListUI.AudioData>) ((a, b) => a.fileName.CompareTo(b.fileName)));
            break;
          }
          this._elementList.Sort((Comparison<JukeBoxAudioListUI.AudioData>) ((a, b) => b.fileName.CompareTo(a.fileName)));
          break;
      }
      if (this._noneData != null)
        this._elementList.Insert(0, this._noneData);
      for (int index = 0; index < this._elementList.Count; ++index)
      {
        JukeBoxAudioListUI.AudioData element = this._elementList[index];
        element.index = index;
        ((Component) element.button).get_transform().SetAsLastSibling();
      }
    }

    private void ChangeSelectedElement(JukeBoxAudioListUI.AudioData data)
    {
      if (data == null && this._selectedElement != null)
      {
        ((Graphic) this._selectedElement.text).set_color(this._whiteColor);
        this._selectedElement = (JukeBoxAudioListUI.AudioData) null;
        this.AudioStop();
      }
      else if (data != null && data == this._selectedElement)
      {
        ((Graphic) this._selectedElement.text).set_color(this._whiteColor);
        this._selectedElement = (JukeBoxAudioListUI.AudioData) null;
      }
      else
      {
        if (data == null)
          return;
        if (this._selectedElement != null)
          ((Graphic) this._selectedElement.text).set_color(this._whiteColor);
        this._selectedElement = data;
        ((Graphic) data.text).set_color(this._yellowColor);
        if (!this.IsPlayingAudio)
          return;
        this.DoPlay();
      }
    }

    private void OnDoubleClick(JukeBoxAudioListUI.AudioData data)
    {
      Debug.Log((object) "ダブルクリック押された");
      if (data == null)
        return;
      bool isPlayingAudio = this.IsPlayingAudio;
      if (this._selectedElement == data)
      {
        this.DoSetAudio();
      }
      else
      {
        if (this._selectedElement != null && this._selectedElement != data)
        {
          ((Graphic) this._selectedElement.text).set_color(this._whiteColor);
          this._selectedElement = (JukeBoxAudioListUI.AudioData) null;
          this.AudioStop();
        }
        this._selectedElement = data;
        ((Graphic) data.text).set_color(this._yellowColor);
        this.DoSetAudio();
      }
    }

    private JukeBoxAudioListUI.AudioData GetElement()
    {
      JukeBoxAudioListUI.AudioData audioData = this._elementPool.PopFront<JukeBoxAudioListUI.AudioData>();
      if (audioData == null)
      {
        Button component = (Button) ((GameObject) Object.Instantiate<GameObject>((M0) this._elementPrefab, this._elementRoot, false))?.GetComponent<Button>();
        Text componentInChildren = (Text) ((Component) component)?.GetComponentInChildren<Text>();
        if (Object.op_Inequality((Object) component, (Object) null) && Object.op_Inequality((Object) componentInChildren, (Object) null))
          audioData = new JukeBoxAudioListUI.AudioData()
          {
            button = component,
            text = componentInChildren
          };
      }
      return audioData;
    }

    private void ReturnElement(JukeBoxAudioListUI.AudioData elm)
    {
      if (elm == null || this._elementPool.Contains(elm))
        return;
      if (Object.op_Inequality((Object) elm.text, (Object) null))
        ((Graphic) elm.text).set_color(this._whiteColor);
      elm.Clear();
      elm.Deactivate();
      this._elementPool.Add(elm);
    }

    private void Release()
    {
      foreach (JukeBoxAudioListUI.AudioData element in this._elementList)
      {
        if (element != null)
          this.ReturnElement(element);
      }
      this._elementList.Clear();
      this._noneData = (JukeBoxAudioListUI.AudioData) null;
      this._selectedElement = (JukeBoxAudioListUI.AudioData) null;
    }

    private void PlaySE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }

    public class AudioData
    {
      public int index = -1;
      public DateTime dateTime = DateTime.MinValue;
      public string filePath = string.Empty;
      public string fileName = string.Empty;
      public Button button;
      public Text text;
      public IDisposable doubleClickDisposable;

      public Action<JukeBoxAudioListUI.AudioData> ClickAction { get; set; }

      public void Clear()
      {
        this.index = -1;
        this.ClickAction = (Action<JukeBoxAudioListUI.AudioData>) null;
        this.dateTime = DateTime.MinValue;
        ((UnityEventBase) this.button.get_onClick()).RemoveAllListeners();
        this.text.set_text(string.Empty);
        this.filePath = string.Empty;
        this.fileName = string.Empty;
      }

      public void Activate()
      {
        ((Component) this.button).SetActiveSelf(true);
      }

      public void Deactivate()
      {
        ((Component) this.button).SetActiveSelf(false);
      }
    }
  }
}
