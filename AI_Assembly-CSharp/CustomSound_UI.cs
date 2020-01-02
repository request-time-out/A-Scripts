// Decompiled with JetBrains decompiler
// Type: CustomSound_UI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomSound_UI : MonoBehaviour
{
  public CustomSound customSound;
  public Button bn_SongSet;
  public Button bn_Back;
  public Button bn_Stop;
  public Button bn_Prev;
  public Button bn_Next;
  public Image cantImageFilter;
  public Image SelectCursol;
  public Button[] bn_DecidedSong;
  public Button bt_PageNum;
  public GameObject PageNoScroll;
  private string[] SongPath;
  private string[] SetList;
  private bool bCanUseList;
  private int nSetBGMArea;
  private bool bPageNoScroll;
  [SerializeField]
  private GameObject SongListLine;
  [SerializeField]
  private GameObject PageListColumn;
  [SerializeField]
  private GameObject PageNoListLine;
  private int nCanLookPageIdx;
  private int nSongNum;
  private int nPageNum;
  private const int nCanLookPageNoNum = 5;
  private const int nSongNumPerPage = 2;
  private List<GameObject> SongList;
  private List<GameObject> PageList;
  private List<GameObject> PageNoList;
  private ScrollRect parent;

  public CustomSound_UI()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.bCanUseList = false;
    this.nSetBGMArea = -1;
    this.SetList = new string[this.bn_DecidedSong.Length];
    this.customSound.FileToBGMList(this.SetList);
    for (int index = 0; index < this.bn_DecidedSong.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomSound_UI.\u003CStart\u003Ec__AnonStorey0 startCAnonStorey0 = new CustomSound_UI.\u003CStart\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey0.ID = index;
      if (Object.op_Inequality((Object) this.bn_DecidedSong[index], (Object) null))
      {
        ((Text) ((Component) this.bn_DecidedSong[index]).GetComponentInChildren<Text>()).set_text(Path.GetFileNameWithoutExtension(this.SetList[index]));
        // ISSUE: method pointer
        ((UnityEvent) this.bn_DecidedSong[index].get_onClick()).AddListener(new UnityAction((object) startCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      }
    }
    string[] files1 = Directory.GetFiles(contents.AudioFileDirectory, "*.wav");
    string[] files2 = Directory.GetFiles(contents.AudioFileDirectory, "*.mp3");
    this.nSongNum = files1.Length + files2.Length;
    this.nPageNum = this.nSongNum / 2;
    if (this.nSongNum % 2 != 0)
      ++this.nPageNum;
    this.SongPath = new string[this.nSongNum];
    RectTransform component = (RectTransform) GameObject.Find("Canvas/SongList/Viewport/Content").GetComponent<RectTransform>();
    float spacing1 = ((HorizontalOrVerticalLayoutGroup) ((Component) component).GetComponent<HorizontalOrVerticalLayoutGroup>()).get_spacing();
    float preferredWidth = ((LayoutElement) this.PageListColumn.GetComponent<LayoutElement>()).get_preferredWidth();
    component.set_sizeDelta(new Vector2((preferredWidth + spacing1) * (float) this.nPageNum, 0.0f));
    for (int index = 0; index < this.nPageNum; ++index)
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.PageListColumn);
      this.PageList.Add(gameObject);
      gameObject.get_transform().SetParent((Transform) component, false);
    }
    RectTransform content = ((ScrollRect) this.PageList[0].GetComponentInChildren<ScrollRect>()).get_content();
    float spacing2 = ((HorizontalOrVerticalLayoutGroup) ((Component) content).GetComponent<VerticalLayoutGroup>()).get_spacing();
    float preferredHeight = ((LayoutElement) this.SongListLine.GetComponent<LayoutElement>()).get_preferredHeight();
    content.set_sizeDelta(new Vector2(0.0f, (preferredHeight + spacing2) * (float) this.nSongNum));
    for (int index = 0; index < this.nSongNum; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomSound_UI.\u003CStart\u003Ec__AnonStorey1 startCAnonStorey1 = new CustomSound_UI.\u003CStart\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey1.\u0024this = this;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.SongListLine);
      this.SongList.Add(gameObject);
      gameObject.get_transform().SetParent((Transform) ((ScrollRect) this.PageList[index / 2].GetComponentInChildren<ScrollRect>()).get_content(), false);
      if (index < files1.Length)
      {
        this.SongPath[index] = files1[index].Remove(0, Application.get_dataPath().ToString().Length - "Assets".Length);
        ((Text) gameObject.GetComponentInChildren<Text>()).set_text(Path.GetFileNameWithoutExtension(this.SongPath[index]));
      }
      else
      {
        this.SongPath[index] = files2[index - files1.Length].Remove(0, Application.get_dataPath().ToString().Length - "Assets".Length);
        ((Text) gameObject.GetComponentInChildren<Text>()).set_text(Path.GetFileNameWithoutExtension(this.SongPath[index]));
      }
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey1.i = index;
      // ISSUE: method pointer
      ((UnityEvent) ((Button) gameObject.GetComponentInChildren<Button>()).get_onClick()).AddListener(new UnityAction((object) startCAnonStorey1, __methodptr(\u003C\u003Em__0)));
      ((Toggle) gameObject.GetComponentInChildren<Toggle>()).set_group(((Toggle) this.SongList[0].GetComponentInChildren<Toggle>()).get_group());
      if (((Text) gameObject.GetComponentInChildren<Text>()).get_text() == Path.GetFileNameWithoutExtension(this.SetList[0]))
        ((Toggle) gameObject.GetComponentInChildren<Toggle>()).set_isOn(true);
      else
        ((Toggle) gameObject.GetComponentInChildren<Toggle>()).set_isOn(false);
    }
    this.nCanLookPageIdx = 0;
    this.PageNoScroll.SetActive(false);
    if (Object.op_Inequality((Object) this.bn_SongSet, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.bn_SongSet.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(BGMSet)));
    }
    if (Object.op_Inequality((Object) this.bn_Back, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.bn_Back.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(Back)));
    }
    if (Object.op_Inequality((Object) this.bn_Stop, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.bn_Stop.get_onClick()).AddListener(new UnityAction((object) this.customSound, __methodptr(SongEnd)));
    }
    if (Object.op_Inequality((Object) this.bn_Prev, (Object) null))
    {
      if (this.nSongNum < 2)
        ((Selectable) this.bn_Prev).set_interactable(false);
      // ISSUE: method pointer
      ((UnityEvent) this.bn_Prev.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__0)));
    }
    if (Object.op_Inequality((Object) this.bn_Next, (Object) null))
    {
      if (this.nSongNum < 2)
        ((Selectable) this.bn_Next).set_interactable(false);
      // ISSUE: method pointer
      ((UnityEvent) this.bn_Next.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__1)));
    }
    if (Object.op_Inequality((Object) this.cantImageFilter, (Object) null))
      ((Behaviour) this.cantImageFilter).set_enabled(true);
    if (Object.op_Inequality((Object) this.SelectCursol, (Object) null))
      ((Behaviour) this.SelectCursol).set_enabled(false);
    if (Object.op_Inequality((Object) this.bt_PageNum, (Object) null))
    {
      ((Text) ((Component) this.bt_PageNum).GetComponentInChildren<Text>()).set_text(string.Format("{0}/{1}", (object) (this.nCanLookPageIdx + 1), (object) this.nPageNum));
      // ISSUE: method pointer
      ((UnityEvent) this.bt_PageNum.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(SetPageNoList)));
    }
    this.parent = (ScrollRect) GameObject.Find("Canvas/SongList").GetComponent<ScrollRect>();
    this.bPageNoScroll = false;
  }

  private void Update()
  {
    if (this.bCanUseList)
    {
      ((Behaviour) this.cantImageFilter).set_enabled(false);
      this.SetSelectCursol();
    }
    else
      ((Behaviour) this.cantImageFilter).set_enabled(true);
  }

  public void BGMListOpen(int ID)
  {
    if (ID == this.nSetBGMArea)
      return;
    this.bCanUseList = true;
    this.nSetBGMArea = ID;
    this.ChangeListMark();
    this.SetSelectCursol();
  }

  public bool IsUse()
  {
    return this.bCanUseList;
  }

  public void SongPlay(string szTarget)
  {
    this.customSound.LoadFile(szTarget);
    this.customSound.SongPlay();
  }

  public void BGMSet()
  {
    if (!this.bCanUseList)
      return;
    this.bCanUseList = false;
    for (int index = 0; index < this.nSongNum; ++index)
    {
      if (((Toggle) this.SongList[index].GetComponentInChildren<Toggle>()).get_isOn())
      {
        this.SetList[this.nSetBGMArea] = this.SongPath[index];
        ((Text) ((Component) this.bn_DecidedSong[this.nSetBGMArea]).GetComponentInChildren<Text>()).set_text(((Text) this.SongList[index].GetComponentInChildren<Text>()).get_text());
        break;
      }
    }
    this.nSetBGMArea = -1;
    ((Behaviour) this.SelectCursol).set_enabled(false);
  }

  public void Back()
  {
    if (this.bCanUseList)
    {
      this.bCanUseList = false;
      ((Behaviour) this.SelectCursol).set_enabled(false);
      this.nSetBGMArea = -1;
    }
    else
    {
      this.customSound.BGMListToFile(this.SetList);
      this.customSound.SongEnd();
    }
  }

  private void ChangeListMark()
  {
    for (int index = 0; index < this.nSongNum; ++index)
    {
      if (((Text) this.SongList[index].GetComponentInChildren<Text>()).get_text() == Path.GetFileNameWithoutExtension(this.SetList[this.nSetBGMArea]))
        ((Toggle) this.SongList[index].GetComponentInChildren<Toggle>()).set_isOn(true);
    }
  }

  private void SetSelectCursol()
  {
    Vector3 position = ((Component) this.SelectCursol).get_transform().get_position();
    position.y = ((Component) this.bn_DecidedSong[this.nSetBGMArea]).get_transform().get_position().y;
    ((Component) this.SelectCursol).get_transform().set_position(position);
    ((Behaviour) this.SelectCursol).set_enabled(true);
  }

  private void Scroll(bool next)
  {
    if (!this.bCanUseList || this.nPageNum == 1)
      return;
    float num = 1f / (float) (this.nPageNum - 1);
    if (next && this.nCanLookPageIdx < this.nPageNum - 1)
      ++this.nCanLookPageIdx;
    else if (!next && this.nCanLookPageIdx > 0)
      --this.nCanLookPageIdx;
    this.parent.set_horizontalNormalizedPosition(num * (float) this.nCanLookPageIdx);
    ((Text) ((Component) this.bt_PageNum).GetComponentInChildren<Text>()).set_text(string.Format("{0}/{1}", (object) (this.nCanLookPageIdx + 1), (object) this.nPageNum));
  }

  private void SetPageNoList()
  {
    this.PageNoScroll.SetActive(true);
    if (this.nPageNum < 5)
      ((RectTransform) this.PageNoScroll.GetComponent<RectTransform>()).set_sizeDelta(new Vector2((float) ((RectTransform) this.PageNoScroll.GetComponent<RectTransform>()).get_sizeDelta().x, (float) ((RectTransform) this.PageNoListLine.GetComponent<RectTransform>()).get_sizeDelta().y * (float) this.nPageNum));
    else
      ((RectTransform) this.PageNoScroll.GetComponent<RectTransform>()).set_sizeDelta(new Vector2((float) ((RectTransform) this.PageNoScroll.GetComponent<RectTransform>()).get_sizeDelta().x, (float) (((RectTransform) this.PageNoListLine.GetComponent<RectTransform>()).get_sizeDelta().y * 5.0)));
    if (this.bPageNoScroll)
      return;
    RectTransform component = (RectTransform) GameObject.Find("Canvas/PageNumList/Viewport/Content").GetComponent<RectTransform>();
    float spacing = ((HorizontalOrVerticalLayoutGroup) ((Component) component).GetComponent<HorizontalOrVerticalLayoutGroup>()).get_spacing();
    float preferredHeight = ((LayoutElement) this.PageNoListLine.GetComponent<LayoutElement>()).get_preferredHeight();
    component.set_sizeDelta(new Vector2((spacing + preferredHeight) * (float) this.nPageNum, 0.0f));
    for (int index = 0; index < this.nPageNum; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomSound_UI.\u003CSetPageNoList\u003Ec__AnonStorey2 listCAnonStorey2 = new CustomSound_UI.\u003CSetPageNoList\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey2.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey2.No = index;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.PageNoListLine);
      this.PageNoList.Add(gameObject);
      gameObject.get_transform().SetParent((Transform) component, false);
      // ISSUE: reference to a compiler-generated field
      ((Text) gameObject.GetComponentInChildren<Text>()).set_text((listCAnonStorey2.No + 1).ToString());
      // ISSUE: method pointer
      ((UnityEvent) ((Button) gameObject.GetComponent<Button>()).get_onClick()).AddListener(new UnityAction((object) listCAnonStorey2, __methodptr(\u003C\u003Em__0)));
    }
    this.bPageNoScroll = true;
  }

  private void PageJump(int JumpPageNo)
  {
    float num = 1f / (float) (this.nPageNum - 1);
    this.nCanLookPageIdx = JumpPageNo;
    this.parent.set_horizontalNormalizedPosition(num * (float) this.nCanLookPageIdx);
    ((Text) ((Component) this.bt_PageNum).GetComponentInChildren<Text>()).set_text(string.Format("{0}/{1}", (object) (this.nCanLookPageIdx + 1), (object) this.nPageNum));
    this.PageNoScroll.SetActive(false);
  }
}
