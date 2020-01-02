// Decompiled with JetBrains decompiler
// Type: AIProject.SpendMoneyUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject
{
  [RequireComponent(typeof (CanvasGroup))]
  [RequireComponent(typeof (RectTransform))]
  public class SpendMoneyUI : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Sprite _onIcon;
    [SerializeField]
    private Sprite _offIcon;
    [SerializeField]
    private Image[] _icons;
    [SerializeField]
    private Text _moneyText;
    [SerializeField]
    private Text _maxText;
    [SerializeField]
    private float _openFadeTime;
    [SerializeField]
    private float _closeFadeTime;
    private CompositeDisposable _fadeDisposable;
    private bool _visibled;

    public SpendMoneyUI()
    {
      base.\u002Ector();
    }

    public bool Visibled
    {
      get
      {
        return this._visibled;
      }
      set
      {
        if (this._visibled == value)
          return;
        this._visibled = value;
        this._fadeDisposable.Clear();
        IEnumerator coroutine = !this._visibled ? this.CloseCoroutine() : this.OpenCoroutine();
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex))), (ICollection<IDisposable>) this._fadeDisposable);
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

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SpendMoneyUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SpendMoneyUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void SettingUI()
    {
      PlayerData playerData = !Singleton<Manager.Map>.IsInstance() ? (PlayerData) null : Singleton<Manager.Map>.Instance.Player?.PlayerData;
      MerchantProfile merchantProfile = !Singleton<Resources>.IsInstance() ? (MerchantProfile) null : Singleton<Resources>.Instance.MerchantProfile;
      int[] spendMoneyBorder = merchantProfile?.SpendMoneyBorder;
      if (playerData == null || spendMoneyBorder.IsNullOrEmpty<int>())
        return;
      playerData.SpendMoney = Mathf.Min(merchantProfile.SpendMoneyMax, playerData.SpendMoney);
      int num = ((IEnumerable<int>) spendMoneyBorder).Last<int>();
      bool active = false;
      if (!this._icons.IsNullOrEmpty<Image>())
      {
        bool flag1 = true;
        for (int index = 0; index < this._icons.Length; ++index)
        {
          if (index == 0)
            active = true;
          bool flag2 = index < spendMoneyBorder.Length && spendMoneyBorder[index] <= playerData.SpendMoney;
          this._icons[index].set_sprite(!flag2 ? this._offIcon : this._onIcon);
          active &= flag2;
          if (!flag2 && flag1)
            num = index >= spendMoneyBorder.Length ? ((IEnumerable<int>) spendMoneyBorder).Last<int>() : spendMoneyBorder[index];
          flag1 = flag2;
        }
      }
      this._moneyText.set_text(string.Format("{0}/{1,4}", (object) playerData.SpendMoney, (object) num));
      this.SetActive((Component) this._moneyText, !active);
      this.SetActive((Component) this._maxText, active);
    }

    private void Awake()
    {
      if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
        return;
      this._canvasGroup.set_blocksRaycasts(false);
      this._canvasGroup.set_alpha(0.0f);
    }

    private void OnDestroy()
    {
      if (this._fadeDisposable == null)
        return;
      this._fadeDisposable.Clear();
    }

    private void SetActive(Component com, bool active)
    {
      if (!Object.op_Inequality((Object) com, (Object) null) || !Object.op_Inequality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == active)
        return;
      com.get_gameObject().SetActive(active);
    }

    private void SetActive(GameObject obj, bool active)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_activeSelf() == active)
        return;
      obj.SetActive(active);
    }
  }
}
