using System;

namespace _1._9.使用Monitor类锁定资源;

/// <summary>
/// 玩家
/// </summary>
public class Player
{
    public string Name { get; private set; }

    public int Atk { get; private set; }

    public Player(string name, int atk)
    {
        Name = name;
        Atk = atk;
    }

    public void Attack(YaoGuai yaoGuai)
    {
        while (yaoGuai.Blood > 0)
        {
            Console.WriteLine($"我是{Name}，我来打妖怪~");
            yaoGuai.BeAttacked(Atk);
        }
    }
}
