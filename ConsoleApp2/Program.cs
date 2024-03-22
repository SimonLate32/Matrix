using System;
using System.Linq;

public class Program
{
    const int numObj = 7;
    const int numUsers = 6;

    struct Rule
    {
        public string post;
        public string[] rights;
        public byte id;
    }

    struct User
    {
        public string UserId;
        public Rule[] UserRules;
    }

    static string StrSort(string a)
    {
        char xrab;
        for (int j = 0; j < a.Length; j++)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                if (a[i] > a[i + 1])
                {
                    xrab = a[i];
                    a = a.Remove(i, 1).Insert(i, a[i + 1].ToString());
                    a = a.Remove(i + 1, 1).Insert(i + 1, xrab.ToString());
                }
            }
        }
        return a;
    }

    static string StrWithByte(byte num, string str)
    {
        if (num == 0)
        {
            return "";
        }
        else
        {
            while (str.Length < num)
                str += str[0];
            return str;
        }
    }

    static string IntToStr(int I)
    {
        return I.ToString();
    }

    static string DelSame(string s)
    {
        string s_new = "";
        foreach (char c in s)
        {
            if (s_new.IndexOf(c) == -1)
                s_new += c;
        }
        return StrSort(s_new);
    }

    static byte SetToNull(byte len, byte num)
    {
        if ((len - num) > 0)
            return (byte)(len - num);
        else
            return 0;
    }
    static byte SearchById(User[] Users, ref string userId)
    {
        for (byte i = 1; i <= Users.Length; i++)
        {
            if (Users[i - 1].UserId == userId)
            {
                return i;
            }
        }
        return 0;
    }



    static bool SearchRuleInUser(User user, byte ruleId)
    {
        foreach (Rule rule in user.UserRules)
        {
            if (rule.id == ruleId)
                return true;
        }
        return false; // Если роль не найдена у пользователя
    }

    static void EditUserRules(User[] Users, Rule[] Rules)
    {
        bool Execute = false;
        string commandUser;
        byte commandUserId, commandRule, commandDel, commandAdd, j;
        Rule temp;

        while (!Execute)
        {
            Console.Write("Введите ID (логин) пользователя, роли которого необходимо отредактировать (0 - для выхода): ");
            commandUser = Console.ReadLine();
            if (commandUser != "0")
            {
                commandUserId = SearchById(Users, ref commandUser);
            }
            else
            {
                break;
            }
            switch (commandUserId)
            {
                case 0:
                    Console.WriteLine("\nПользователя с таким ID не существует.\n");
                    break;
                default:
                    while (!Execute)
                    {
                        Console.WriteLine();
                        Console.Write($"Список ролей у пользователя {commandUser}: ");
                        if (Users[commandUserId - 1].UserRules.Length == 0)
                        {
                            Console.WriteLine("-");
                        }
                        else
                        {
                            for (int i = 0; i < Users[commandUserId - 1].UserRules.Length - 1; i++)
                            {
                                Console.Write($"{Users[commandUserId - 1].UserRules[i].post}, ");
                            }
                            Console.WriteLine($"{Users[commandUserId - 1].UserRules[Users[commandUserId - 1].UserRules.Length - 1].post}");
                        }
                        Console.WriteLine("Введите действие: ");
                        Console.WriteLine("1 - удаление роли у пользователя");
                        Console.WriteLine("2 - присвоение роли пользователю");
                        Console.WriteLine("0 - выйти из редактирования ролей у пользователя");
                        commandRule = byte.Parse(Console.ReadLine());

                        switch (commandRule)
                        {
                            case 1:
                                // Логика удаления роли у пользователя
                                Console.WriteLine("Выберите номер роли для удаления:");
                                for (int i = 0; i < Users[commandUserId - 1].UserRules.Length; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {Users[commandUserId - 1].UserRules[i].post}");
                                }
                                int roleToRemoveIndex = int.Parse(Console.ReadLine()) - 1;
                                if (roleToRemoveIndex >= 0 && roleToRemoveIndex < Users[commandUserId - 1].UserRules.Length)
                                {
                                    List<Rule> updatedRoles = new List<Rule>(Users[commandUserId - 1].UserRules);
                                    updatedRoles.RemoveAt(roleToRemoveIndex);
                                    Users[commandUserId - 1].UserRules = updatedRoles.ToArray();
                                    Console.WriteLine("Роль успешно удалена.");
                                }
                                else
                                {
                                    Console.WriteLine("Некорректный номер роли.");
                                }
                                break;
                            
                            case 2:
                                // Логика добавления роли пользователю
                                Console.WriteLine("Выберите номер роли для добавления:");
                                for (int i = 0; i < Rules.Length; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {Rules[i].post}");
                                }
                                int roleToAddIndex = int.Parse(Console.ReadLine()) - 1;
                                if (roleToAddIndex >= 0 && roleToAddIndex < Rules.Length)
                                {
                                    // Extend the user's rule array by one
                                    Array.Resize(ref Users[commandUserId - 1].UserRules, Users[commandUserId - 1].UserRules.Length + 1);
                                    // Add the selected rule to the user's rule array
                                    Users[commandUserId - 1].UserRules[Users[commandUserId - 1].UserRules.Length - 1] = Rules[roleToAddIndex];
                                    Console.WriteLine("Роль успешно добавлена.");
                                }
                                else
                                {
                                    Console.WriteLine("Некорректный номер роли.");
                                }
                                break;

                            case 0:
                                Execute = true;
                                break; 
                        }

                    }
                    break;
            }
        }
    }




    static void Main(string[] args)
    {
        bool Execute = false;
        User[] Users = new User[numUsers];
        Rule[] Rules = new Rule[4];
        string[] userList = new string[numUsers];
        string login;

        void CheckUser(string currLogin)
        {
            if (userList.Contains(currLogin))
            {
                HubUser(currLogin);
            }
            else
            {
                Console.WriteLine("Пользователь с таким Id отсутствует");
            }
        }


        void AccessMatr()
        {
            byte maxUserLength = 8;
            string[,] userRights = new string[numUsers, numObj];

            for (int i = 0; i < numUsers; i++)
            {
                maxUserLength = Math.Max(maxUserLength, (byte)Users[i].UserId.Length);

                for (int j = 0; j < Users[i].UserRules.Length; j++)
                {
                    if (Users[i].UserRules[j].rights.Length >= numObj)
                    {
                        for (int k = 0; k < numObj; k++)
                        {
                            if (userRights[i, k] == null)
                                userRights[i, k] = "";
                            userRights[i, k] += Users[i].UserRules[j].rights[k];
                        }
                    }
                }
            }

            Console.Write("Users" + StrWithByte(SetToNull(maxUserLength, 5), " ") + "| ");
            for (int obj = 0; obj < numObj; obj++)
            {
                Console.Write("Obj" + (obj + 1) + " | ");
            }
            Console.WriteLine();

            for (int i = 0; i < numUsers; i++)
            {
                Console.Write(Users[i].UserId + StrWithByte(SetToNull(maxUserLength, (byte)Users[i].UserId.Length), " ") + "| ");
                for (int obj = 0; obj < numObj; obj++)
                {
                    Console.Write(userRights[i, obj] + StrWithByte(SetToNull(5, (byte)(userRights[i, obj]?.Length ?? 0)), " ") + "| ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        void EditRules()
        {
            byte commandRule, commandObj;
            string changedRule;
            while (!Execute)
            {
                Console.WriteLine("Введите номер роли, которую хотите отредактировать: ");
                Console.WriteLine("1 - " + Rules[0].post);
                Console.WriteLine("2 - " + Rules[1].post);
                Console.WriteLine("3 - " + Rules[2].post);
                Console.WriteLine("4 - " + Rules[3].post);
                Console.WriteLine("0 - выход из редактора ролей");
                commandRule = Convert.ToByte(Console.ReadLine());
                switch (commandRule)
                {
                    case 0:
                        return;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        while (!Execute)
                        {
                            Console.WriteLine("Введите объект, который хотите отредактировать: ");
                            for (int i = 0; i < numObj; i++)
                                Console.WriteLine(IntToStr(i + 1) + " - Obj" + IntToStr(i + 1));
                            Console.WriteLine("0 - выход из выбора объекта");
                            commandObj = Convert.ToByte(Console.ReadLine());
                            switch (commandObj)
                            {
                                case 0:
                                    return;
                                default:
                                    while (!Execute)
                                    {
                                        Console.Write("Введите права роли для взаимодействия с объектом Obj" + IntToStr(commandObj) + " (0 - для выхода): ");
                                        changedRule = Console.ReadLine();
                                        Console.WriteLine(changedRule);
                                        if (changedRule == "0")
                                            break;
                                        else
                                            Rules[commandRule - 1].rights[commandObj - 1] = DelSame(StrSort(changedRule));
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            for (int i = 0; i < numUsers; i++)
                for (int j = 0; j < Users[i].UserRules.Length; j++)
                    Users[i].UserRules[j] = Rules[Users[i].UserRules[j].id - 1];
        }


        void HubAdmin()
        {
            byte commandChoice;
            while (!Execute)
            {
                Console.WriteLine("Список доступных команд: ");
                Console.WriteLine("1 - вывести полную матрицу доступа");
                Console.WriteLine("2 - редактирование ролей");
                Console.WriteLine("3 - распределение пользователей по ролям");
                Console.WriteLine("0 - выход из учетной записи");
                commandChoice = Convert.ToByte(Console.ReadLine());
                switch (commandChoice)
                {
                    case 1:
                        AccessMatr();
                        break;
                    case 2:
                        EditRules();
                        break;
                    case 3:
                        EditUserRules(Users, Rules);
                        break;
                    case 0:
                        return;
                }
            }
        }

        void HubUser(string userName)
        {
            byte commandChoice;
            while (!Execute)
            {
                Console.WriteLine("Список доступных команд: ");
                Console.WriteLine("1 - вывести полную матрицу доступа");
                Console.WriteLine("2 - редактирование объектов");
                Console.WriteLine("0 - выход из выбора объекта");
                commandChoice = Convert.ToByte(Console.ReadLine());
                switch (commandChoice)
                {
                    case 1:
                        AccessMatr();
                        break;
                    case 2:
                        EditUserRules(Users, Rules);
                        break;
                    case 0:
                        return;
                }
            }
        }

        // Заводская настройка ролей
        Rules[0].post = "Должность_1";
        Rules[0].id = 1;
        Rules[0].rights = new string[] { StrSort("RW"), StrSort("CA"), "", "", "", "", "" };

        Rules[1].post = "Должность_2";
        Rules[1].id = 2;
        Rules[1].rights = new string[] { StrSort("RW"), "", "", "", "", "", StrSort("RA") };

        Rules[2].post = "Должность_3";
        Rules[2].id = 3;
        Rules[2].rights = new string[] { "", "", StrSort("RWCA"), StrSort("RWAC"), "", "", "" };

        Rules[3].post = "Должность_4";
        Rules[3].id = 4;
        Rules[3].rights = new string[] { StrSort("RWCA"), "", "", "", "", "", "" };

        // Изначальное случайное назначение ролей
        Random random = new Random();
        for (int i = 0; i < numUsers; i++)
        {
            Users[i].UserId = "User" + IntToStr(i + 1);
            Users[i].UserRules = new Rule[1]; // Initialize user rules array
            Users[i].UserRules[0] = new Rule(); // Initialize the rule struct
            Users[i].UserRules[0].post = Rules[random.Next(0, 4)].post; // Assign a random rule to the user
            Users[i].UserRules[0].rights = Rules[random.Next(0, 4)].rights; // Assign rights for the rule
            Users[i].UserRules[0].id = Users[i].UserRules[0].id; // Assign an ID to the rule
            userList[i] = Users[i].UserId;
        }

        login = "";

        while (!Execute)
        {
            Console.Write("Введите, под кем Вы хотите авторизоваться (00 - для выхода из программы): ");
            login = Console.ReadLine();
            switch (login)
            {
                case "00":
                    Execute = true;
                    break;
                case "admin":
                    HubAdmin();
                    break;
                default:
                    CheckUser(login);
                    break;
            }
        }
    }
}