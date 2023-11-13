using System.Numerics;

namespace TextRPG
{
    // 게임 시작
    internal class Program
    {
        private static Character player;

        private static Item item101;
        private static Item item201;

        private static List<int> itemList = new List<int>();
        private static List<int> effectAtk = new List<int>();
        private static List<int> effectDef = new List<int>();

        static void Main(string[] args)
        {
            GameDataSetting();
            DisplayGameIntro();
        }

        static void GameDataSetting()
        {
            // 캐릭터 정보 세팅
            player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);

            // 아이템 정보 세팅
            item101 = new Item(101, "무쇠갑옷", "방어력 +5", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1, 5, 1, true, 100);
            item201 = new Item(201, "낡은 검", "공격력 +2", "쉽게 볼 수 있는 낡은 검 입니다.", 2, 2, 2, false, 50);

            itemList.Add(101);
            itemList.Add(201);

        }

        static void DisplayGameIntro()
        {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine("\n1. 상태보기\n2.인벤토리\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>\n");
            
            // 입력 체크
            int input = CheckValidInput(1, 2);
            switch (input)
            {
                case 1:
                    DisplayMyInfo();
                    break;
                case 2:
                    DisplayInventory();
                    break;

            }


        }
        static void DisplayMyInfo()
        {
            
            Console.Clear();

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            Console.WriteLine($"공격력 :{player.Atk}");
            Console.WriteLine($"방어력 : {player.Def}");
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 0);
            switch (input)
            {
                case 0:
                    DisplayGameIntro();
                    break;
            }
        }

        static void DisplayInventory()
        {
            Console.Clear();

            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();

            Console.WriteLine($"- 1 {item101.ItemName}  |  {item101.ItemEffectInform}  |  {item101.ItemInform}");
            Console.WriteLine($"- 2 {item201.ItemName}  |  {item201.ItemEffectInform}  |  {item201.ItemInform}");
            
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 0);
            switch (input)
            {
                case 0:
                    DisplayGameIntro();
                    break;
            }
        }

        static int CheckValidInput(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
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
        public int Hp { get; }
        public int Gold { get; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }

    public class Item
    {

        public int ItemNum { get; }
        public string ItemName { get; }
        public string ItemEffectInform { get; }
        public string ItemInform { get; }
        public int ItemType { get; }
        public int ItemEffect { get; }
        public int ItemEffecType { get; }

        public bool ItemEquip { get; }
        public int ItemPrice { get; }

        public List<int> effectDef = new List<int>();
        public List<int> effectAtk = new List<int>();

        public Item(int itemNum,string name, string effectInform, string inform, int type, int effect, int effectType, bool equip,int price)
        {
            ItemNum = itemNum;
            ItemName = name;
            ItemEffectInform = effectInform;
            ItemInform = inform;
            ItemType = type;
            ItemEffect = effect;
            ItemEffecType = effectType;
            ItemEquip = equip;
            ItemPrice = price;

            if (equip == true)
            {
                ItemName = "[E]" + name;

            }



        }
    }
        

}
