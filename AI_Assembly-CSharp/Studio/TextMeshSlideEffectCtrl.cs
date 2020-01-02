// Decompiled with JetBrains decompiler
// Type: Studio.TextMeshSlideEffectCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  [RequireComponent(typeof (TextMeshProUGUI))]
  public class TextMeshSlideEffectCtrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
  {
    [SerializeField]
    private RectTransform transBase;
    [SerializeField]
    private TextMeshProUGUI textMesh;
    public float speed;
    public bool assist;
    private bool isPlay;

    public TextMeshSlideEffectCtrl()
    {
      base.\u002Ector();
    }

    private float preferredWidth
    {
      get
      {
        return Object.op_Implicit((Object) this.textMesh) ? ((TMP_Text) this.textMesh).get_preferredWidth() : 0.0f;
      }
    }

    private void MoveText()
    {
      float x = (float) this.transBase.get_sizeDelta().x;
      float preferredWidth = this.preferredWidth;
      if ((double) x >= (double) preferredWidth)
        return;
      Vector4 margin = ((TMP_Text) this.textMesh).get_margin();
      if ((double) Mathf.Abs((float) margin.x) > (double) preferredWidth)
      {
        ref Vector4 local = ref margin;
        local.x = (__Null) (local.x + ((double) preferredWidth + (double) x));
      }
      ref Vector4 local1 = ref margin;
      local1.x = (__Null) (local1.x - (double) this.speed * (double) Time.get_deltaTime());
      ((TMP_Text) this.textMesh).set_margin(margin);
    }

    private void Check()
    {
      if ((double) (float) this.transBase.get_sizeDelta().x >= (double) this.preferredWidth)
      {
        ObservableLateUpdateTrigger component = (ObservableLateUpdateTrigger) ((Component) this).GetComponent<ObservableLateUpdateTrigger>();
        if (Object.op_Inequality((Object) component, (Object) null))
          Object.Destroy((Object) component);
        Object.Destroy((Object) this);
      }
      else
      {
        ((TMP_Text) this.textMesh).set_alignment((TextAlignmentOptions) 4097);
        ((TMP_Text) this.textMesh).set_overflowMode((TextOverflowModes) 1);
        ((TMP_Text) this.textMesh).set_enableWordWrapping(false);
        this.AddFunc();
      }
    }

    private void AddFunc()
    {
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, bool>) (_ => this.isPlay)), (Action<M0>) (_ => this.MoveText())), (Component) this);
    }

    private void Start()
    {
      if (this.assist)
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.First<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this)), (Action<M0>) (_ => this.Check())), (Component) this);
      else
        this.AddFunc();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      this.isPlay = true;
      ((TMP_Text) this.textMesh).set_overflowMode((TextOverflowModes) 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      this.isPlay = false;
      ((TMP_Text) this.textMesh).set_margin(Vector4.get_zero());
      ((TMP_Text) this.textMesh).set_overflowMode((TextOverflowModes) 1);
    }
  }
}
