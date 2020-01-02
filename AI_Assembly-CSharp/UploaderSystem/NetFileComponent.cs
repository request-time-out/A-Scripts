// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetFileComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UploaderSystem
{
  [Serializable]
  public class NetFileComponent : MonoBehaviour
  {
    [Header("---< 基本 >--------------------------")]
    [SerializeField]
    private GameObject objSortInfo;
    [SerializeField]
    private RawImage rawImage;
    public Toggle tglItem;
    [Header("---< ランク >------------------------")]
    [SerializeField]
    private GameObject objRank;
    [SerializeField]
    private Text textRank;
    [SerializeField]
    private Image[] imgRank;
    [Header("---< 拍手 >--------------------")]
    [SerializeField]
    private GameObject objApplause;
    [SerializeField]
    private UI_ButtonEx btnLike;
    [SerializeField]
    private Text textApplauseNum;
    public Action actApplause;
    [Header("---< 更新日 >------------------------")]
    [SerializeField]
    private GameObject objDate;
    [SerializeField]
    private Text textDateTitle;
    [SerializeField]
    private Text textDate;

    public NetFileComponent()
    {
      base.\u002Ector();
    }

    public void SetState(bool interactable, bool enable)
    {
      if (Object.op_Inequality((Object) null, (Object) this.tglItem))
        ((Selectable) this.tglItem).set_interactable(interactable);
      if (Object.op_Inequality((Object) null, (Object) this.rawImage))
        ((Behaviour) this.rawImage).set_enabled(enable);
      if (Object.op_Inequality((Object) null, (Object) this.objSortInfo))
        this.objSortInfo.SetActiveIfDifferent(interactable);
      if (!Object.op_Inequality((Object) null, (Object) this.btnLike))
        return;
      ((Component) this.btnLike).get_gameObject().SetActiveIfDifferent(enable);
    }

    public void SetImage(Texture tex)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.rawImage))
        return;
      ((Behaviour) this.rawImage).set_enabled(Object.op_Inequality((Object) null, (Object) tex));
      if (Object.op_Inequality((Object) null, (Object) this.rawImage.get_texture()))
        Object.Destroy((Object) this.rawImage.get_texture());
      this.rawImage.set_texture(tex);
    }

    public void UpdateSortType(int type)
    {
      bool[,] flagArray = new bool[3, 7]
      {
        {
          false,
          false,
          false,
          false,
          true,
          true,
          false
        },
        {
          false,
          false,
          false,
          false,
          false,
          false,
          true
        },
        {
          true,
          true,
          true,
          true,
          false,
          false,
          false
        }
      };
      if (Object.op_Inequality((Object) null, (Object) this.objRank))
        this.objRank.SetActiveIfDifferent(flagArray[0, type]);
      if (Object.op_Inequality((Object) null, (Object) this.objApplause))
        this.objApplause.SetActiveIfDifferent(flagArray[1, type]);
      if (!Object.op_Inequality((Object) null, (Object) this.objDate))
        return;
      this.objDate.SetActiveIfDifferent(flagArray[2, type]);
    }

    public void SetRanking(int no)
    {
      for (int index = 0; index < this.imgRank.Length; ++index)
        ((Behaviour) this.imgRank[index]).set_enabled(false);
      if (1 <= no && no <= 3)
      {
        ((Behaviour) this.imgRank[no - 1]).set_enabled(true);
        if (!Object.op_Inequality((Object) null, (Object) this.textRank))
          return;
        this.textRank.set_text(string.Empty);
      }
      else
      {
        if (!Object.op_Inequality((Object) null, (Object) this.textRank))
          return;
        this.textRank.set_text(string.Format("{0}位", (object) no));
      }
    }

    public void SetUpdateTime(DateTime time, int kind)
    {
      if (Object.op_Inequality((Object) null, (Object) this.textDateTitle))
        this.textDateTitle.set_text(kind != 0 ? "更新日時：" : "投稿日時：");
      if (!Object.op_Inequality((Object) null, (Object) this.textDate))
        return;
      this.textDate.set_text(time.ToString("yyyy/MM/dd"));
    }

    public void SetApplauseNum(int num)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.textApplauseNum))
        return;
      this.textApplauseNum.set_text(num.ToString());
    }

    public void EnableApplause(bool enable)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.btnLike))
        return;
      ((Selectable) this.btnLike).set_interactable(enable);
    }

    private void Awake()
    {
    }

    private void Reset()
    {
      Transform transform1 = ((Component) this).get_transform().Find("imgBack/sortinfo");
      if (Object.op_Inequality((Object) null, (Object) transform1))
        this.objSortInfo = ((Component) transform1).get_gameObject();
      Transform transform2 = ((Component) this).get_transform().Find("Image/RawImage");
      if (Object.op_Inequality((Object) null, (Object) transform2))
        this.rawImage = (RawImage) ((Component) transform2).GetComponent<RawImage>();
      this.tglItem = (Toggle) ((Component) this).GetComponent<Toggle>();
      Transform transform3 = ((Component) this).get_transform().Find("imgBack/sortinfo/rank");
      if (Object.op_Inequality((Object) null, (Object) transform3))
        this.objRank = ((Component) transform3).get_gameObject();
      Transform transform4 = ((Component) this).get_transform().Find("imgBack/sortinfo/rank/textRank");
      if (Object.op_Inequality((Object) null, (Object) transform4))
        this.textRank = (Text) ((Component) transform4).GetComponent<Text>();
      this.imgRank = new Image[3];
      Transform transform5 = ((Component) this).get_transform().Find("imgBack/sortinfo/rank/imgRank00");
      if (Object.op_Inequality((Object) null, (Object) transform5))
        this.imgRank[0] = (Image) ((Component) transform5).GetComponent<Image>();
      Transform transform6 = ((Component) this).get_transform().Find("imgBack/sortinfo/rank/imgRank01");
      if (Object.op_Inequality((Object) null, (Object) transform6))
        this.imgRank[1] = (Image) ((Component) transform6).GetComponent<Image>();
      Transform transform7 = ((Component) this).get_transform().Find("imgBack/sortinfo/rank/imgRank02");
      if (Object.op_Inequality((Object) null, (Object) transform7))
        this.imgRank[2] = (Image) ((Component) transform7).GetComponent<Image>();
      Transform transform8 = ((Component) this).get_transform().Find("imgBack/sortinfo/applausenum");
      if (Object.op_Inequality((Object) null, (Object) transform8))
        this.objApplause = ((Component) transform8).get_gameObject();
      Transform transform9 = ((Component) this).get_transform().Find("imgBack/sortinfo/applausenum/textApplauseNum");
      if (Object.op_Inequality((Object) null, (Object) transform9))
        this.textApplauseNum = (Text) ((Component) transform9).GetComponent<Text>();
      Transform transform10 = ((Component) this).get_transform().Find("imgBack/sortinfo/applausenum/btnLike");
      if (Object.op_Inequality((Object) null, (Object) transform10))
        this.btnLike = (UI_ButtonEx) ((Component) transform10).GetComponent<UI_ButtonEx>();
      Transform transform11 = ((Component) this).get_transform().Find("imgBack/sortinfo/date");
      if (Object.op_Inequality((Object) null, (Object) transform11))
        this.objDate = ((Component) transform11).get_gameObject();
      Transform transform12 = ((Component) this).get_transform().Find("imgBack/sortinfo/date/textUpTimeTitle");
      if (Object.op_Inequality((Object) null, (Object) transform12))
        this.textDateTitle = (Text) ((Component) transform12).GetComponent<Text>();
      Transform transform13 = ((Component) this).get_transform().Find("imgBack/sortinfo/date/textUpTime");
      if (!Object.op_Inequality((Object) null, (Object) transform13))
        return;
      this.textDate = (Text) ((Component) transform13).GetComponent<Text>();
    }

    private void Start()
    {
      if (!Object.op_Inequality((Object) null, (Object) this.btnLike))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) this.btnLike), (Action<M0>) (_ =>
      {
        if (this.actApplause == null)
          return;
        this.actApplause();
      }));
    }

    private void Update()
    {
    }
  }
}
