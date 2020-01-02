// Decompiled with JetBrains decompiler
// Type: AIProject.UI.TutorialGroupListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  [RequireComponent(typeof (CanvasGroup))]
  [RequireComponent(typeof (RectTransform))]
  public class TutorialGroupListUI : MenuUIBehaviour
  {
    [SerializeField]
    private string _lockStr = string.Empty;
    [SerializeField]
    private string _lastElementStr = string.Empty;
    [SerializeField]
    private Color _whiteColor = Color.get_white();
    [SerializeField]
    private Color _yellowColor = Color.get_yellow();
    private List<Tuple<int, Button, Text, bool>> _elements = new List<Tuple<int, Button, Text, bool>>();
    private int _lastIndex = -1;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Transform _elementRoot;
    [SerializeField]
    private GameObject _elementPrefab;
    [SerializeField]
    private Scrollbar _scrollbar;
    [SerializeField]
    private Image _scrollbarHandleImage;
    [SerializeField]
    private TutorialUI _tutorialUI;
    [SerializeField]
    private TutorialLoadingImageUI _loadingImageUI;
    private ReadOnlyDictionary<int, ValueTuple<string, GameObject[]>> _prefabTable;

    public Button CloseButton
    {
      get
      {
        return this._closeButton;
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    public int SelectIndex { get; protected set; } = -1;

    public Tuple<int, Button, Text, bool> SelectedElement { get; protected set; }

    public bool InputEnabled
    {
      get
      {
        return this.EnabledInput && Singleton<Input>.Instance.FocusLevel == this._focusLevel;
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
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Func<M0, bool>) (_ => this.EnabledInput)), (Action<M0>) (_ => this._tutorialUI.DoClose()));
    }

    protected override void OnAfterStart()
    {
      base.OnAfterStart();
      IEnumerator coroutine = this.CreateElements();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (Component) this);
    }

    [DebuggerHidden]
    private IEnumerator CreateElements()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialGroupListUI.\u003CCreateElements\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void RefreshElements()
    {
      if (this._prefabTable.IsNullOrEmpty<int, ValueTuple<string, GameObject[]>>())
        return;
      using (IEnumerator<KeyValuePair<int, ValueTuple<string, GameObject[]>>> enumerator = this._prefabTable.GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          KeyValuePair<int, ValueTuple<string, GameObject[]>> current = enumerator.Current;
          Tuple<int, Button, Text, bool> element = this._elements.GetElement<Tuple<int, Button, Text, bool>>(current.Key);
          if (element != null)
          {
            ValueTuple<string, GameObject[]> valueTuple = current.Value;
            element.Item4 = this.GetTutorialOpenState(current.Key);
            if (Object.op_Inequality((Object) element.Item3, (Object) null))
            {
              bool flag = element.Item4;
              element.Item3.set_text(!flag ? this._lockStr : (string) valueTuple.Item1);
            }
          }
        }
      }
      Tuple<int, Button, Text, bool> element1 = this._elements.GetElement<Tuple<int, Button, Text, bool>>(this._prefabTable.get_Count());
      if (!Object.op_Inequality((Object) element1?.Item3, (Object) null))
        return;
      element1.Item3.set_text(this._lastElementStr);
    }

    private void ChangeGroupUI(Tuple<int, Button, Text, bool> value)
    {
      if (value == null || !value.Item4)
        return;
      if (this._tutorialUI.OpenElementNumber == value.Item1)
      {
        this.ClickSelect((Tuple<int, Button, Text, bool>) null);
        this._tutorialUI.ChangeUIGroup(-1);
      }
      else
        this._tutorialUI.ChangeUIGroup(value.Item1);
    }

    private void ClickSelect(Tuple<int, Button, Text, bool> elm)
    {
      if (elm != null && !elm.Item4 && elm.Item1 != this._lastIndex)
        return;
      if (this.SelectedElement != null && Object.op_Inequality((Object) this.SelectedElement.Item3, (Object) null))
        ((Graphic) this.SelectedElement.Item3).set_color(this._whiteColor);
      this.SelectedElement = elm;
      if (elm != null)
      {
        if (Object.op_Inequality((Object) elm.Item3, (Object) null))
          ((Graphic) elm.Item3).set_color(this._yellowColor);
        this.SelectIndex = elm.Item1;
      }
      else
        this.SelectIndex = -1;
    }

    public void SelectButton(int index)
    {
      this.ClickSelect(this._elements.GetElement<Tuple<int, Button, Text, bool>>(index));
    }

    private void OpenLoadingImageUI()
    {
      this.PlaySE(SoundPack.SystemSE.OK_S);
      this._loadingImageUI.IsActiveControl = true;
    }

    private bool GetTutorialOpenState(int id)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      WorldData worldData = Singleton<Game>.Instance.WorldData;
      Dictionary<int, bool> source = worldData == null ? (Dictionary<int, bool>) null : worldData.TutorialOpenStateTable;
      if (source.IsNullOrEmpty<int, bool>())
        return false;
      bool flag;
      if (!source.TryGetValue(id, out flag))
        flag = false;
      return flag;
    }

    private void SetInteractable(bool active)
    {
      if (this._canvasGroup.get_interactable() == active)
        return;
      this._canvasGroup.set_interactable(active);
    }

    private void SetBlockRaycasts(bool active)
    {
      if (this._canvasGroup.get_blocksRaycasts() == active)
        return;
      this._canvasGroup.set_blocksRaycasts(active);
    }

    private void SetActive(GameObject obj, bool active)
    {
      if (Object.op_Equality((Object) obj, (Object) null) || obj.get_activeSelf() == active)
        return;
      obj.SetActive(active);
    }

    private void SetActive(Component com, bool active)
    {
      if (Object.op_Equality((Object) com, (Object) null) || Object.op_Equality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == active)
        return;
      com.get_gameObject().SetActive(active);
    }

    private void PlaySE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }
  }
}
