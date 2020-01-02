// Decompiled with JetBrains decompiler
// Type: Studio.Anime.TextMeshSlideEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio.Anime
{
  public class TextMeshSlideEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
  {
    [SerializeField]
    private RectTransform transBase;
    [SerializeField]
    private TextMeshProUGUI textMesh;
    public float speed;
    private SingleAssignmentDisposable sildeDisposable;
    private bool isPlay;
    private bool isMove;

    public TextMeshSlideEffect()
    {
      base.\u002Ector();
    }

    private float ParentWidth
    {
      get
      {
        return Object.op_Inequality((Object) this.transBase, (Object) null) ? (float) this.transBase.get_sizeDelta().x : 0.0f;
      }
    }

    private float PreferredWidth
    {
      get
      {
        return Object.op_Inequality((Object) this.textMesh, (Object) null) ? ((TMP_Text) this.textMesh).get_preferredWidth() : 0.0f;
      }
    }

    public void Stop()
    {
      if (!this.isPlay)
        return;
      this.isPlay = false;
      ((TMP_Text) this.textMesh).set_margin(Vector4.get_zero());
      ((TMP_Text) this.textMesh).set_overflowMode((TextOverflowModes) 1);
    }

    public void OnChangedText()
    {
      if (this.sildeDisposable != null)
      {
        this.sildeDisposable.Dispose();
        this.sildeDisposable = (SingleAssignmentDisposable) null;
      }
      ((TMP_Text) this.textMesh).set_margin(Vector4.get_zero());
      ((TMP_Text) this.textMesh).set_alignment((TextAlignmentOptions) 514);
      ((TMP_Text) this.textMesh).set_overflowMode((TextOverflowModes) 0);
      ((TMP_Text) this.textMesh).set_enableWordWrapping(false);
      this.CheckText();
    }

    private void CheckText()
    {
      // ISSUE: variable of a compiler-generated type
      TextMeshSlideEffect.\u003CCheckText\u003Ec__async0 checkTextCAsync0;
      // ISSUE: reference to a compiler-generated field
      checkTextCAsync0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      checkTextCAsync0.\u0024builder = AsyncVoidMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: cast to a reference type
      ((AsyncVoidMethodBuilder) ref checkTextCAsync0.\u0024builder).Start<TextMeshSlideEffect.\u003CCheckText\u003Ec__async0>((M0&) ref checkTextCAsync0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (!this.isMove)
        return;
      this.isPlay = true;
      ((TMP_Text) this.textMesh).set_overflowMode((TextOverflowModes) 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      if (!this.isMove)
        return;
      this.isPlay = false;
      ((TMP_Text) this.textMesh).set_margin(Vector4.get_zero());
      ((TMP_Text) this.textMesh).set_overflowMode((TextOverflowModes) 1);
    }
  }
}
