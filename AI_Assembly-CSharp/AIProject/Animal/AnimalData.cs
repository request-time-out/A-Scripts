// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEx;

namespace AIProject.Animal
{
  public static class AnimalData
  {
    private static Dictionary<AnimalTypes, string> animalNameTable_ = new Dictionary<AnimalTypes, string>()
    {
      {
        AnimalTypes.Cat,
        "cat"
      },
      {
        AnimalTypes.Chicken,
        "chicken"
      },
      {
        AnimalTypes.Fish,
        "fish"
      },
      {
        AnimalTypes.Mecha,
        "mecha"
      },
      {
        AnimalTypes.Frog,
        "frog"
      },
      {
        AnimalTypes.Butterfly,
        "butterfly"
      },
      {
        AnimalTypes.BirdFlock,
        "birdflock"
      },
      {
        AnimalTypes.CatWithFish,
        "catwithfish"
      },
      {
        AnimalTypes.CatTank,
        "cattank"
      },
      {
        AnimalTypes.Chick,
        "chick"
      },
      {
        AnimalTypes.Fairy,
        "fairy"
      },
      {
        AnimalTypes.DarkSpirit,
        "darkspirit"
      }
    };
    private static ReadOnlyDictionary<AnimalTypes, string> _animalNameTable = (ReadOnlyDictionary<AnimalTypes, string>) null;
    private static Dictionary<BreedingTypes, string> breedingStr_ = new Dictionary<BreedingTypes, string>()
    {
      {
        BreedingTypes.Wild,
        "wild"
      },
      {
        BreedingTypes.Pet,
        "pet"
      }
    };
    private static ReadOnlyDictionary<BreedingTypes, string> _breedingStr = (ReadOnlyDictionary<BreedingTypes, string>) null;
    private static Dictionary<int, AnimalTypes> animalTypeIDTable_ = new Dictionary<int, AnimalTypes>()
    {
      {
        0,
        AnimalTypes.Cat
      },
      {
        1,
        AnimalTypes.Chicken
      },
      {
        2,
        AnimalTypes.Fish
      },
      {
        3,
        AnimalTypes.Butterfly
      },
      {
        4,
        AnimalTypes.Mecha
      },
      {
        5,
        AnimalTypes.Frog
      },
      {
        6,
        AnimalTypes.BirdFlock
      },
      {
        7,
        AnimalTypes.CatWithFish
      },
      {
        8,
        AnimalTypes.CatTank
      },
      {
        9,
        AnimalTypes.Chick
      },
      {
        10,
        AnimalTypes.Fairy
      },
      {
        11,
        AnimalTypes.DarkSpirit
      }
    };
    private static ReadOnlyDictionary<int, AnimalTypes> _animalTypeIDTable = (ReadOnlyDictionary<int, AnimalTypes>) null;
    private static Dictionary<int, AnimalState> animalStateIDTable_ = new Dictionary<int, AnimalState>()
    {
      {
        0,
        AnimalState.None
      },
      {
        1,
        AnimalState.Start
      },
      {
        2,
        AnimalState.Repop
      },
      {
        3,
        AnimalState.Depop
      },
      {
        4,
        AnimalState.Idle
      },
      {
        5,
        AnimalState.Wait
      },
      {
        6,
        AnimalState.SitWait
      },
      {
        7,
        AnimalState.Locomotion
      },
      {
        8,
        AnimalState.LovelyIdle
      },
      {
        9,
        AnimalState.LovelyFollow
      },
      {
        10,
        AnimalState.Escape
      },
      {
        11,
        AnimalState.Swim
      },
      {
        12,
        AnimalState.Sleep
      },
      {
        13,
        AnimalState.Toilet
      },
      {
        14,
        AnimalState.Rest
      },
      {
        15,
        AnimalState.Eat
      },
      {
        16,
        AnimalState.Drink
      },
      {
        17,
        AnimalState.Actinidia
      },
      {
        18,
        AnimalState.Grooming
      },
      {
        19,
        AnimalState.MoveEars
      },
      {
        20,
        AnimalState.Roar
      },
      {
        21,
        AnimalState.Peck
      },
      {
        22,
        AnimalState.ToIndoor
      },
      {
        90,
        AnimalState.Action0
      },
      {
        91,
        AnimalState.Action1
      },
      {
        92,
        AnimalState.Action2
      },
      {
        93,
        AnimalState.Action3
      },
      {
        94,
        AnimalState.Action4
      },
      {
        95,
        AnimalState.Action5
      },
      {
        96,
        AnimalState.Action6
      },
      {
        97,
        AnimalState.Action7
      },
      {
        98,
        AnimalState.Action8
      },
      {
        99,
        AnimalState.Action9
      },
      {
        100,
        AnimalState.WithPlayer
      },
      {
        101,
        AnimalState.WithAgent
      },
      {
        999,
        AnimalState.Destroyed
      }
    };
    private static ReadOnlyDictionary<int, AnimalState> _animalStateIDTable = (ReadOnlyDictionary<int, AnimalState>) null;
    private static List<ValueTuple<string, AnimalTypes>> animalNameList_;
    private static IReadOnlyList<ValueTuple<string, AnimalTypes>> _animalNameList;
    private static List<ValueTuple<string, BreedingTypes>> breedingNameList_;
    private static IReadOnlyList<ValueTuple<string, BreedingTypes>> _breedingNameList;

    public static ReadOnlyDictionary<AnimalTypes, string> AnimalNameTable
    {
      get
      {
        return AnimalData._animalNameTable ?? (AnimalData._animalNameTable = new ReadOnlyDictionary<AnimalTypes, string>((IDictionary<AnimalTypes, string>) AnimalData.animalNameTable_));
      }
    }

    public static ReadOnlyDictionary<BreedingTypes, string> BreedingStr
    {
      get
      {
        return AnimalData._breedingStr ?? (AnimalData._breedingStr = new ReadOnlyDictionary<BreedingTypes, string>((IDictionary<BreedingTypes, string>) AnimalData.breedingStr_));
      }
    }

    public static ReadOnlyDictionary<int, AnimalTypes> AnimalTypeIDTable
    {
      get
      {
        return AnimalData._animalTypeIDTable ?? (AnimalData._animalTypeIDTable = new ReadOnlyDictionary<int, AnimalTypes>((IDictionary<int, AnimalTypes>) AnimalData.animalTypeIDTable_));
      }
    }

    public static ReadOnlyDictionary<int, AnimalState> AnimalStateIDTable
    {
      get
      {
        return AnimalData._animalStateIDTable ?? (AnimalData._animalStateIDTable = new ReadOnlyDictionary<int, AnimalState>((IDictionary<int, AnimalState>) AnimalData.animalStateIDTable_));
      }
    }

    public static IReadOnlyList<ValueTuple<string, AnimalTypes>> AnimalNameList
    {
      get
      {
        return AnimalData._animalNameList ?? (AnimalData._animalNameList = (IReadOnlyList<ValueTuple<string, AnimalTypes>>) AnimalData.animalNameList_);
      }
    }

    public static IReadOnlyList<ValueTuple<string, BreedingTypes>> BreedingNameList
    {
      get
      {
        return AnimalData._breedingNameList ?? (AnimalData._breedingNameList = (IReadOnlyList<ValueTuple<string, BreedingTypes>>) AnimalData.breedingNameList_);
      }
    }

    public static int GetAnimalTypeID(AnimalTypes animalType)
    {
      int num = 32;
      for (int index = 0; index < num; ++index)
      {
        if ((int) animalType >> index == 1 && animalType == (AnimalTypes) (1 << index))
          return index;
      }
      return -1;
    }

    static AnimalData()
    {
      List<ValueTuple<string, AnimalTypes>> valueTupleList1 = new List<ValueTuple<string, AnimalTypes>>();
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("cat", AnimalTypes.Cat));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("chicken", AnimalTypes.Chicken));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("fish", AnimalTypes.Fish));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("mecha", AnimalTypes.Mecha));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("frog", AnimalTypes.Frog));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("butterfly", AnimalTypes.Butterfly));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("birdflock", AnimalTypes.BirdFlock));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("catwithfish", AnimalTypes.CatWithFish));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("cattank", AnimalTypes.CatTank));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("chick", AnimalTypes.Chick));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("fairy", AnimalTypes.Fairy));
      valueTupleList1.Add(new ValueTuple<string, AnimalTypes>("darkspirit", AnimalTypes.DarkSpirit));
      AnimalData.animalNameList_ = valueTupleList1;
      AnimalData._animalNameList = (IReadOnlyList<ValueTuple<string, AnimalTypes>>) null;
      List<ValueTuple<string, BreedingTypes>> valueTupleList2 = new List<ValueTuple<string, BreedingTypes>>();
      valueTupleList2.Add(new ValueTuple<string, BreedingTypes>("wild", BreedingTypes.Wild));
      valueTupleList2.Add(new ValueTuple<string, BreedingTypes>("pet", BreedingTypes.Pet));
      AnimalData.breedingNameList_ = valueTupleList2;
      AnimalData._breedingNameList = (IReadOnlyList<ValueTuple<string, BreedingTypes>>) null;
    }
  }
}
