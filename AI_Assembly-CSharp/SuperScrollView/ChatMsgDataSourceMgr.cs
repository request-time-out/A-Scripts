// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ChatMsgDataSourceMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
  public class ChatMsgDataSourceMgr : MonoBehaviour
  {
    private static ChatMsgDataSourceMgr instance = (ChatMsgDataSourceMgr) null;
    private static string[] mChatDemoStrList = new string[5]
    {
      "Support ListView and GridView.",
      "Support Infinity Vertical and Horizontal ScrollView.",
      "Support items in different sizes such as widths or heights. Support items with unknown size at init time.",
      "Support changing item count and item size at runtime. Support looping items such as spinners. Support item padding.",
      "Use only one C# script to help the UGUI ScrollRect to support any count items with high performance."
    };
    private static string[] mChatDemoPicList = new string[4]
    {
      "grid_pencil_128_g2",
      "grid_flower_200_3",
      "grid_pencil_128_g3",
      "grid_flower_200_7"
    };
    private Dictionary<int, PersonInfo> mPersonInfoDict;
    private List<ChatMsg> mChatMsgList;

    public ChatMsgDataSourceMgr()
    {
      base.\u002Ector();
    }

    public static ChatMsgDataSourceMgr Get
    {
      get
      {
        if (Object.op_Equality((Object) ChatMsgDataSourceMgr.instance, (Object) null))
          ChatMsgDataSourceMgr.instance = (ChatMsgDataSourceMgr) Object.FindObjectOfType<ChatMsgDataSourceMgr>();
        return ChatMsgDataSourceMgr.instance;
      }
    }

    private void Awake()
    {
      this.Init();
    }

    public PersonInfo GetPersonInfo(int personId)
    {
      PersonInfo personInfo = (PersonInfo) null;
      return this.mPersonInfoDict.TryGetValue(personId, out personInfo) ? personInfo : (PersonInfo) null;
    }

    public void Init()
    {
      this.mPersonInfoDict.Clear();
      PersonInfo personInfo1 = new PersonInfo();
      personInfo1.mHeadIcon = "grid_pencil_128_g8";
      personInfo1.mId = 0;
      personInfo1.mName = "Jaci";
      this.mPersonInfoDict.Add(personInfo1.mId, personInfo1);
      PersonInfo personInfo2 = new PersonInfo();
      personInfo2.mHeadIcon = "grid_pencil_128_g5";
      personInfo2.mId = 1;
      personInfo2.mName = "Toc";
      this.mPersonInfoDict.Add(personInfo2.mId, personInfo2);
      this.InitChatDataSource();
    }

    public ChatMsg GetChatMsgByIndex(int index)
    {
      return index < 0 || index >= this.mChatMsgList.Count ? (ChatMsg) null : this.mChatMsgList[index];
    }

    public int TotalItemCount
    {
      get
      {
        return this.mChatMsgList.Count;
      }
    }

    private void InitChatDataSource()
    {
      this.mChatMsgList.Clear();
      int length1 = ChatMsgDataSourceMgr.mChatDemoStrList.Length;
      int length2 = ChatMsgDataSourceMgr.mChatDemoPicList.Length;
      for (int index = 0; index < 100; ++index)
        this.mChatMsgList.Add(new ChatMsg()
        {
          mMsgType = (MsgTypeEnum) (Random.Range(0, 99) % 2),
          mPersonId = Random.Range(0, 99) % 2,
          mSrtMsg = ChatMsgDataSourceMgr.mChatDemoStrList[Random.Range(0, 99) % length1],
          mPicMsgSpriteName = ChatMsgDataSourceMgr.mChatDemoPicList[Random.Range(0, 99) % length2]
        });
    }

    public void AppendOneMsg()
    {
      int length1 = ChatMsgDataSourceMgr.mChatDemoStrList.Length;
      int length2 = ChatMsgDataSourceMgr.mChatDemoPicList.Length;
      this.mChatMsgList.Add(new ChatMsg()
      {
        mMsgType = (MsgTypeEnum) (Random.Range(0, 99) % 2),
        mPersonId = Random.Range(0, 99) % 2,
        mSrtMsg = ChatMsgDataSourceMgr.mChatDemoStrList[Random.Range(0, 99) % length1],
        mPicMsgSpriteName = ChatMsgDataSourceMgr.mChatDemoPicList[Random.Range(0, 99) % length2]
      });
    }
  }
}
