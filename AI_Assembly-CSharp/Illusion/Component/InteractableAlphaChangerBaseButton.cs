// Decompiled with JetBrains decompiler
// Type: Illusion.Component.InteractableAlphaChangerBaseButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Illusion.Component
{
  internal class InteractableAlphaChangerBaseButton : MonoBehaviour
  {
    [Header("Interactable参照用ボタン")]
    [SerializeField]
    private Button flagButton;
    [Header("カラー変更対象TextMesh")]
    [SerializeField]
    private List<TextMeshProUGUI> targetTextMesh;
    [Header("カラー変更対象Text")]
    [SerializeField]
    private List<Text> targetText;
    [Header("カラー変更対象Image")]
    [SerializeField]
    private List<Image> targetImage;
    [Header("カラー変更対象RawImage")]
    [SerializeField]
    private List<RawImage> targetRawImage;

    public InteractableAlphaChangerBaseButton()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (!Object.op_Equality((Object) this.flagButton, (Object) null))
        return;
      Debug.LogError((object) "flagButton none", (Object) this);
      Object.Destroy((Object) this);
    }

    private void Start()
    {
      List<Color> baseTextMeshColor = ((IEnumerable<TextMeshProUGUI>) this.targetTextMesh).Select<TextMeshProUGUI, Color>((Func<TextMeshProUGUI, Color>) (t => ((Graphic) t).get_color())).ToList<Color>();
      List<Color> baseTextColor = ((IEnumerable<Text>) this.targetText).Select<Text, Color>((Func<Text, Color>) (t => ((Graphic) t).get_color())).ToList<Color>();
      List<Color> baseImageColor = ((IEnumerable<Image>) this.targetImage).Select<Image, Color>((Func<Image, Color>) (t => ((Graphic) t).get_color())).ToList<Color>();
      Color[] baseRawImageColor = ((IEnumerable<RawImage>) this.targetRawImage).Select<RawImage, Color>((Func<RawImage, Color>) (t => ((Graphic) t).get_color())).ToArray<Color>();
      BoolReactiveProperty isInteract = new BoolReactiveProperty(((Selectable) this.flagButton).get_interactable());
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) isInteract, (Action<M0>) (isOn =>
      {
        ColorBlock colors = ((Selectable) this.flagButton).get_colors();
        List<Color> colorList1 = new List<Color>((IEnumerable<Color>) baseTextMeshColor);
        List<Color> colorList2 = new List<Color>((IEnumerable<Color>) baseTextColor);
        List<Color> colorList3 = new List<Color>((IEnumerable<Color>) baseImageColor);
        List<Color> colorList4 = new List<Color>((IEnumerable<Color>) baseRawImageColor);
        if (!isOn)
        {
          for (int index = 0; index < this.targetTextMesh.Count; ++index)
            colorList1[index] = new Color(Mathf.Clamp01((float) (colorList1[index].r * ((ColorBlock) ref colors).get_disabledColor().r)), Mathf.Clamp01((float) (colorList1[index].g * ((ColorBlock) ref colors).get_disabledColor().g)), Mathf.Clamp01((float) (colorList1[index].b * ((ColorBlock) ref colors).get_disabledColor().b)), Mathf.Clamp01((float) (colorList1[index].a * ((ColorBlock) ref colors).get_disabledColor().a)));
          for (int index = 0; index < this.targetText.Count; ++index)
            colorList2[index] = new Color(Mathf.Clamp01((float) (colorList2[index].r * ((ColorBlock) ref colors).get_disabledColor().r)), Mathf.Clamp01((float) (colorList2[index].g * ((ColorBlock) ref colors).get_disabledColor().g)), Mathf.Clamp01((float) (colorList2[index].b * ((ColorBlock) ref colors).get_disabledColor().b)), Mathf.Clamp01((float) (colorList2[index].a * ((ColorBlock) ref colors).get_disabledColor().a)));
          for (int index = 0; index < this.targetImage.Count; ++index)
            colorList3[index] = new Color(Mathf.Clamp01((float) (colorList3[index].r * ((ColorBlock) ref colors).get_disabledColor().r)), Mathf.Clamp01((float) (colorList3[index].g * ((ColorBlock) ref colors).get_disabledColor().g)), Mathf.Clamp01((float) (colorList3[index].b * ((ColorBlock) ref colors).get_disabledColor().b)), Mathf.Clamp01((float) (colorList3[index].a * ((ColorBlock) ref colors).get_disabledColor().a)));
          for (int index = 0; index < this.targetRawImage.Count; ++index)
            colorList4[index] = new Color(Mathf.Clamp01((float) (colorList4[index].r * ((ColorBlock) ref colors).get_disabledColor().r)), Mathf.Clamp01((float) (colorList4[index].g * ((ColorBlock) ref colors).get_disabledColor().g)), Mathf.Clamp01((float) (colorList4[index].b * ((ColorBlock) ref colors).get_disabledColor().b)), Mathf.Clamp01((float) (colorList4[index].a * ((ColorBlock) ref colors).get_disabledColor().a)));
        }
        for (int index = 0; index < this.targetTextMesh.Count; ++index)
          ((Graphic) this.targetTextMesh[index]).set_color(colorList1[index]);
        for (int index = 0; index < this.targetText.Count; ++index)
          ((Graphic) this.targetText[index]).set_color(colorList2[index]);
        for (int index = 0; index < this.targetImage.Count; ++index)
          ((Graphic) this.targetImage[index]).set_color(colorList3[index]);
        for (int index = 0; index < this.targetRawImage.Count; ++index)
          ((Graphic) this.targetRawImage[index]).set_color(colorList4[index]);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnEnableAsObservable((UnityEngine.Component) this), (Action<M0>) (_ => ((ReactiveProperty<bool>) isInteract).set_Value(((Selectable) this.flagButton).get_interactable())));
      ObservableExtensions.Subscribe<bool>(Observable.DistinctUntilChanged<bool>((IObservable<M0>) Observable.Select<Unit, bool>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((UnityEngine.Component) this), (Func<M0, M1>) (_ => ((Selectable) this.flagButton).get_interactable()))), (Action<M0>) (interactable => ((ReactiveProperty<bool>) isInteract).set_Value(interactable)));
    }
  }
}
