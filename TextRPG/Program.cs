using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using System.Reflection.Emit;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TextRPG
{
    // 게임 시작
    internal class Program
    {
        static int battleTurn = 0;
        static Character player;
        static Item[] items;
        static Monster[] monsters;
        static Random rand = new Random();
        static int monsterCount = 0;

        static void Main(string[] args)
        {
            GameDataSetting();
            StartMenu();
        }

        static void GameDataSetting()
        {
            // 캐릭터 정보 세팅
            player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);
            items = new Item[10];
            monsters = new Monster[5];

            // 아이템 정보 세팅
            AddItem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 5, 0,100));
            AddItem (new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 1, 2, 0, 0, 10));

        }

        static void AddItem(Item item)
        {
            if (Item.ItemCnt == 10) return;
            items[Item.ItemCnt] = item;
            Item.ItemCnt++;
        }

        static void StartMenu()
        {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine("\n1.상태보기\n2.인벤토리\n3.퀘스트 받기\n4.던전입장");
            Console.WriteLine("");

            switch (CheckValidInput(1, 4))
            {
                case 1:
                    MyInfoMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
                case 3:
                    QuestMenu();
                    break;
                case 4:
                    DungeonMenu();
                    break;

            }
        }
        static int CheckValidInput(int min, int max)
        {
            int keyInput;
            bool result;
            do
            {
                Console.WriteLine("");
                result = int.TryParse(Console.ReadLine(), out keyInput);
            } while (result == false || CheckIfValid(keyInput, min, max) == false);

            return keyInput;
        }

        static bool CheckIfValid(int checkable, int min, int max)
        {
            if (min <= checkable && checkable <= max) return true;
            else
            {
                Console.WriteLine("");
                Console.WriteLine("잘못 입력하셨습니다.");
                return false;
            }
            
        }

        static void MyInfoMenu()
        {

            Console.Clear();

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");

            int bonusAtk = getSumBonusAtk();
            Console.Write($"공격력 : {player.Atk + bonusAtk}");
            if (bonusAtk > 0) Console.WriteLine(" (+{0})", bonusAtk);
            else Console.WriteLine("");

            int bonusDef = getSumBonusDef();
            Console.Write($"방어력 : {player.Def + bonusDef}");
            if (bonusDef > 0) Console.WriteLine(" (+{0})", bonusDef);
            else Console.WriteLine("");

            int bonusHp = getSumBonusHp();
            Console.Write($"체 력 : {player.Atk + bonusHp}");
            if (bonusHp > 0) Console.WriteLine(" (+{0})", bonusHp);
            else Console.WriteLine("");

            Console.WriteLine($"Gold : {player.Gold}");
            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");
            switch (CheckValidInput(0, 0))
            {
                case 0:
                    StartMenu();
                    break;
            }
        }

        private static int getSumBonusAtk()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].ItemEquip) sum += items[i].Atk;
            }
            return sum;
        }

        private static int getSumBonusDef()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].ItemEquip) sum += items[i].Def;
            }
            return sum;
            
        }

        private static int getSumBonusHp()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].ItemEquip) sum += items[i].Hp;
            }
            return sum;
        }

        static void InventoryMenu()
        {
            Console.Clear();

            Console.WriteLine("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();

            for (int i = 0; i < Item.ItemCnt; i++)
            {
                items[i].PrintItemStatDescription();
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("");
            switch (CheckValidInput(0, 1))
            {
                case 0:
                    StartMenu();
                    break;
                case 1:
                    EquipMenu();
                    break;
            }
        }
        static void EquipMenu()
        {
            Console.Clear();

            Console.WriteLine("■ 인벤토리 - 장착 관리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                items[i].PrintItemStatDescription(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");

            int keyInput = CheckValidInput(0, Item.ItemCnt);

            switch (keyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    ToggleEquipStatus(keyInput - 1);
                    EquipMenu();
                    break;
            }
        }

        static void ToggleEquipStatus(int idx)
        {
            items[idx].ItemEquip= !items[idx].ItemEquip;
        }

        static void QuestMenu()
        { 

        }

        static void DungeonMenu()
        {
            Console.Clear();
            
            monsterCount = rand.Next(0, 4);

            Console.WriteLine("Battle!!");
            Console.WriteLine("");

            CreateRandomMonster();

            for (int i = 0; i <= monsterCount; i++)
            {
                monsters[i].PrintMonsterStatDescription();

            }

            Console.WriteLine("");
            Console.WriteLine("0. 도망가기");
            Console.WriteLine("1. 전투");

            switch (CheckValidInput(0, 1))
            {
                case 0:
                    StartMenu();
                    break;
                case 1:
                    Console.Clear();
                    BattleMenu();
                    break;
            }
        }

        static void BattleMenu()
        {
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            Console.WriteLine($"HP {player.Hp + getSumBonusHp()}/{player.NowHp} ");

            Console.WriteLine("");

            for (int i = 0; i <= monsterCount; i++)
            {
                monsters[i].PrintMonsterStatDescription(i + 1);

            }

            if (monsterCount >= 0)
            {
                if (battleTurn == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("player의 차례");
                    Console.WriteLine("");
                    Console.WriteLine("0. 공격하기");
                    Console.WriteLine("1. 도망가기");
                    Console.WriteLine("");

                    switch (CheckValidInput(0, 1))
                    {
                        case 0:
                            Console.WriteLine("대상을 선택하세요.");
                            int keyInput = CheckValidInput(1,monsterCount);
                            switch (keyInput)
                            {
                                default:
                                    int nowAttcak = BattleAtk(player.Atk);
                                    monsters[keyInput].Hp -= nowAttcak;
                                    battleTurn += 1;
                                    break;
                            }
                            break;
            

                        case 1:
                            Console.Clear();
                            StartMenu();
                            break;
                    }
                }
            }
        }

        private static int BattleAtk(int atk)
        {
            float minAtkf = atk - (atk * 0.1f);
            float maxAtkf = atk + (atk * 0.1f);
            int minAtk = (int)minAtkf;
            int maxAtk = (int)maxAtkf;
            int finalAtk = rand.Next(minAtk,maxAtk);

            return finalAtk;

        }

        static void CreateRandomMonster()
        {
            
            for (int i= 0; i<=monsterCount; i++)
            {
                int randValue = rand.Next(1, 3);
                
                switch (randValue)
                {
                    case 1:
                        monsters[i] = new Monster("미니언", 2, 5, 15, 100, 1);
                        break;

                    case 2:
                        monsters[i] = new Monster("공허충", 3, 9, 10, 300, 2);
                        break;

                    case 3:
                        monsters[i] = new Monster("대포 미니언", 4, 8, 25, 500, 3);
                        break;
                        
                }

            }
        }

    }


    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; set; }
        public int Gold { get; set; }

        public int NowHp { get; set; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            NowHp = hp;
        }
    }

    public class Item
    {

        public string ItemName { get; } // 아이템 이름 
        public string ItemInform { get; } // 아이템 설명
        public int ItemType { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }

        public bool ItemEquip { get; set; }
        public int ItemPrice { get; }

        public static int ItemCnt = 0;
 
        public Item(string name, string inform, int type, int atk, int def, int hp,int price, bool equip = false)
        {
            ItemName = name;
            ItemInform = inform;
            ItemType = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            ItemEquip = equip;
            ItemPrice = price;


        }

        public void PrintItemStatDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");
            // 장착관리 전용
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }
            if (ItemEquip)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
                Console.Write(PadRightForMixedText(ItemName, 9));
            }
            else Console.Write(PadRightForMixedText(ItemName, 12));

            Console.Write(" | ");

            if (Atk != 0) Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk} ");
            if (Def != 0) Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def} ");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write(" | ");

            Console.WriteLine(ItemInform);
        }
        public static int GetPrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2; 
                }
                else
                {
                    length += 1;
                }
            }

            return length;
        }

        public static string PadRightForMixedText(string str, int totalLength)
        {
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);

        }



    }

    public class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Hp { get; set; }
        public int Gold { get; }
        public int MonsterNum { get; }

        public int NowHp { get; set; }

        public Monster(string name, int level, int atk, int hp, int gold, int monsterNum)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Hp = hp;
            Gold = gold;
            MonsterNum = monsterNum;
        }

        public void PrintMonsterStatDescription()
        {
            Console.WriteLine("Lv.{0} {1} HP {2}", Level, Name, Hp);
        }
        public void PrintMonsterStatDescription(int idx = 0)
        {
            Console.Write("- ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("{0} ", idx);
            Console.ResetColor();
            Console.WriteLine("Lv.{0} {1} HP {2}", Level, Name, Hp);
        }

        public class Quest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int RewardGold { get; set; }
            public int TargetNum { get; set; }
            public int TagetValue { get; set; }
            public bool QuestGet { get; set; }

            public Quest(string name, string description, int rewardGold, int targetNum, int targetValue, bool questGet = false)
            {
                Name = name;
                Description = description;
                RewardGold = rewardGold;
                TargetNum = targetNum;
                TagetValue = targetValue;
                QuestGet = questGet;

            }

            public void PrintQuestDescription()
            {
                if (QuestGet == true) return;
                Console.WriteLine(Name);
                Console.WriteLine(Description);
                Console.WriteLine(RewardGold);

            }
        }

    }
}
