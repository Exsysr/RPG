using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    // Global variablar
    static int t1 = 0;
    static int t2 = 0;
    static int t3 = 0;
    static int t4 = 0;
    static int WeaponLev = 0;
    static int ArmourLev = 0;
    static int LastEquippedC = 10;
    static bool HasArmour = false;
    static bool HasWeapon = false;
    static bool HasCharms = false;
    static bool BoosterActive1 = false;
    static bool BoosterActive2 = false;
    static bool answer;
    static bool ForestComplete = false;
    static bool CavesComplete = false;
    static bool MountainsComplete = false;
    static int ForestCoins = 100;
    static int CavesCoins = 150;
    static int MountainsCoins = 200;

    // Mina listor
    static readonly Charms[] CharmInv =
    {
        new Charms("Heart Charm", false, false, 0, 1.3, 200, "1"),
        new Charms("Sheild Charm", false, false, 1, 1.5, 100, "2"),
        new Charms("Sword Charm", false, false, 2, 1.5, 100, "3"),
        new Charms("Monster Charm", false, false, 3, 2, 150, "4"),
        new Charms("Gold Charm", false, false, 4, 2, 500, "5"),
    };
    static readonly Potion[] PotionInv = {
        new Potion("Tiny Potion", 0, 1, 5, 5, 1),
        new Potion("Small Potion", 0, 0, 10, 10, 2),
        new Potion("Medium Potion", 0, 0, 20, 20, 3),
        new Potion("Large Potion", 0, 0, 40, 40, 4),
        new Potion("Giant Potion", 0, 0, 50, 80, 5),
        new Potion("Max Potion", 0, 0, 0, 120, 6),
    };
    static readonly Armour[] Armourlev = {
        new Armour("Wooden Armour", 2, 5),
        new Armour("Chainmaile Armour", 4, 10),
        new Armour("Copper Armour", 8, 20),
        new Armour("Iron Armour", 16, 40),
        new Armour("Crystal Armour", 32, 80),
        new Armour("Dragon Armour", 64, 120)};
    static readonly Weapon[] Weaponlev = {
        new Weapon("Wooden Sword", 2, 5),
        new Weapon("Stone Sword", 4, 10),
        new Weapon("Copper Sword", 8, 20),
        new Weapon("Iron Sword", 16, 40),
        new Weapon("Crystal Sword", 32, 80),
        new Weapon("Dragon Sword", 64, 120)};
    static readonly Enemy[] Enemies = {
        new Enemy("Deamon", 1000000000, 1000000000, 1000000000, 0, 0, false),
        new Enemy("Slime", 10, 5, 5, 5, 3, false),
        new Enemy("Goblin", 20, 10, 5, 10, 5, false),
        new Enemy("Cave Clrawlers", 30, 15, 10, 20, 7, false),
        new Enemy("Death Worm", 50, 20, 10, 30, 10, false),
        new Enemy("Warden", 70, 30, 20, 60, 15, false),
        new Enemy("Pheonix", 100, 40, 20, 120, 20, false),
        new Enemy("Dragon", 120, 50, 30, 240, 30, false) };
    static readonly string[] enemyNames = { "Deamon", "Slime", "Goblin", "Cave Crawlers", "Death Worm", "Warden", "Pheonix", "Dragon" };
    static readonly string[] place = { "Shop", "Forest", "Caves", "Mountain", "DragonsDen", "Open Stats", "Inventory" };
    static readonly string[] booster = { "Normal", "Greed", "Glutton" };

    static void Main()
    {
        Console.WriteLine("Hello!");
        Player player = CreatePlayer();
        AddBooster();
        Town(player);
    }

    static void AddBooster()
    {
        int addbooster = ChooseBooster();
        if (addbooster == 1)
        {
            BoosterActive1 = true;
            for (int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i].Name += ": Greed";
                Enemies[i].Health = Math.Round(Enemies[i].Health * 1.5, 2);
                Enemies[i].Defense = Math.Round(Enemies[i].Defense * 1.3, 2);
                Enemies[i].Strength = Math.Round(Enemies[i].Strength * 1.3, 2);
                Enemies[i].Money *= 2;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ALL ENEMIES GIVE MORE COINS");
            Console.ForegroundColor = ConsoleColor.White;
            Wait(500);
        }
        else if (addbooster == 2)
        {
            BoosterActive2 = true;
            for (int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i].Name += ": Glutton";
                Enemies[i].Health = Math.Round(Enemies[i].Health * 1.5, 2);
                Enemies[i].Defense = Math.Round(Enemies[i].Defense * 1.3, 2);
                Enemies[i].Strength = Math.Round(Enemies[i].Strength * 1.3, 2);
                Enemies[i].XP *= 2;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ALL ENEMIES GIVE MORE EXP");
            Console.ForegroundColor = ConsoleColor.White;
            Wait(500);
        }
    }
    static int ChooseBooster()
    {
        Console.WriteLine("Choose a booster you would like to add:");
        for (int i = 0; i < booster.Length; i++)
        {
            Console.Write(i + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(booster[i]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
            Wait(100);
        }
        Console.Write("Choose an option: ");
        string Choice = Console.ReadLine();
        Console.Clear();

        for (int i = 0; i < booster.Length; i++)
        {
            try
            {
                if (Command(Choice, booster[i]) || int.Parse(Choice) - 1 == i)
                {
                    return i;
                }
            }
            catch (Exception)
            {

            }
        }
        return 0;
    }

    // skapar spelare
    static Player CreatePlayer()
    {
        Console.Write("What is your name: ");
        string? name = Console.ReadLine();
        name ??= "Aetherius";
        Console.Clear();

        Console.WriteLine("Choose a diffeculty");
        string[] dif = { "Easy", "Medium", "Hard" };
        for (int i = 0; i < dif.Length; i++)
        {
            Console.Write(i + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(dif[i]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
            Wait(100);
        }

        int PlayerChoice = Choose(dif);

        Player player = new(name, 0, 0, 0, 0);


        if (PlayerChoice == 0)
        {
            player = new(name, 40, 10, 10, 10);
            player.TrueHealth = player.Health;
            player.TrueDefense = player.Defense;
            player.TrueStrength = player.Strength;
            return player;
        }
        else if (PlayerChoice == 1)
        {
            player = new(name, 35, 10, 10, 5);
            player.TrueHealth = player.Health;
            player.TrueDefense = player.Defense;
            player.TrueStrength = player.Strength;
            return player;
        }
        else if (PlayerChoice == 2)
        {
            player = new(name, 30, 10, 5, 5);
            player.TrueHealth = player.Health;
            player.TrueDefense = player.Defense;
            player.TrueStrength = player.Strength;
            return player;
        }
        return player;
    }

    // I Town får spelaren välja vad de vill göra
    static void Town(Player player)
    {
        Console.WriteLine($"Where do you want to go {player.Name}?");

        for (int i = 0; i < place.Length; i++)
        {
            Console.Write(i + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(place[i]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
            Wait(100);
        }

        int PlayerChoice = Choose(place);
        if (PlayerChoice == 0)
        {
            Places(player, 0, 0);
        }
        else if (PlayerChoice == 1)
        {
            Places(player, 0, 1);
        }
        else if (PlayerChoice == 2)
        {
            Places(player, 5, 2);
        }
        else if (PlayerChoice == 3)
        {
            Places(player, 10, 3);
        }
        else if (PlayerChoice == 4)
        {
            Places(player, 15, 4);
        }
        else if (PlayerChoice == 5)
        {
            ShowPlayerStats(player);

            if (HasArmour == true)
            {
                ShowArmourStats(player);
            }
            if (HasWeapon == true)
            {
                ShowWeaponStats(player);
            }
            Town(player);
        }
        else if (PlayerChoice == 6)
        {
            Console.WriteLine("Choose Inventory");
            string[] inv = { "Potions", "Charms" };
            for (int i = 0; i < inv.Length; i++)
            {
                Console.Write(i + 1 + ":[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(inv[i]);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("]");
            }

            int PlayerOption = Choose(inv);

            if (PlayerOption == 0)
            {
                Console.WriteLine("Would you like to use a potion?");
                Yn();
                    
                // använd en potion utanför strid
                if (answer)
                {
                    UsePotion(player);
                    Town(player);
                }
                else
                {
                    for (int i = 0; i < PotionInv.Length - player.EndGameC; i++)
                    {
                        Console.Write(i + 1 + ":[");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{PotionInv[i].Name}: {PotionInv[i].Amount}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("]");
                    }
                    Wait(2000);
                    
                    Town(player);
                }
            }
            else if (PlayerOption == 1)
            {
                if (HasCharms == true)
                {
                    Console.WriteLine("Would you like to equip a charm?");
                    Yn();
                    if (answer)
                    {
                        ChooseCharmEquip(player);
                    }
                    else
                    {
                        ShowOwnedCharms();
                        Town(player);
                    }
                }
                else
                {
                    Console.WriteLine("You don't own any charms");
                    Town(player);
                }
            }
        }
        else
        {
            Town(player);
        }
    } // en hub för spelaren där de kan göra olika saker
    static void Places(Player player, int rq, int p)
    {
        if (player.Level >= rq)
        {
            if (p == 0) { Shop(player); }
            else if (p == 1) { Forest(player); }
            else if (p == 2) { Caves(player); }
            else if (p == 3) { Mountains(player); }
            else if (p == 4) { DragonDen(player); }

        }
        else
        {
            Console.WriteLine($"You need to reach level {rq}!!!");
            Console.WriteLine($"You are level {player.Level}!!!");
            Wait(700);
            Console.Clear();
            Town(player);
        }
    } // Spelaren väljer var de ska till

    // En funktion för att köpa utrustning, vapen, potions och charms
    static void Shop(Player player)
    {
        Console.WriteLine("What do you need?");

        string[] choice = { "Town", "Armour", "Weapons", "Potions", "Charms", "Tell me about you", "Gamble" };
        for (int i = 0; i < choice.Length; i++)
        {
            Console.Write(i + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(choice[i]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
            Wait(100);
        }

        int PlayerChoice = Choose(choice);
        
        if (PlayerChoice == 0)
        {
            Wait(750);
            Console.Clear();
            Town(player);
        }
        else if (PlayerChoice == 1)
        {
            if (ArmourLev < Armourlev.Length)
            {
                BuyArmour(player);
            }
            else if (ArmourLev >= Armourlev.Length)
            {
                Console.WriteLine("You have already acquired all armour uppgrades!");
                Wait(750);
                Console.Clear();
                Shop(player);
            }

        }
        else if (PlayerChoice == 2)
        {
            if (WeaponLev < Weaponlev.Length)
            {
                BuyWeapon(player);
            }
            else if (WeaponLev >= Weaponlev.Length)
            {
                Console.WriteLine("You have already acquired all weapon uppgrades!");
                Wait(750);
                Console.Clear();
                Shop(player);
            }
        }
        else if (PlayerChoice == 3)
        {
            BuyPotion(player);
        }
        else if (PlayerChoice == 4)
        {
            if (player.Coins > 0)
            {
                BuyCharms(player);
            }
            else
            {
                Console.WriteLine("You don't have any money!!!");
                Console.WriteLine("You can get money by slaying monsters!!!");
                Wait(750);
                Console.Clear();
            }
        }
        else if (PlayerChoice == 5)
        {
            Console.WriteLine("Want to know about me?");
            Wait(500);
            Yn();
            if (answer)
            {
                Console.WriteLine("Alright!");
                Console.WriteLine("You asked for it!");
                Enemy ene = EnemyAttacked(0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("YOU SHOULDN'T HAVE ASKED!!!!");
                Console.ForegroundColor = ConsoleColor.White;
                Wait(1000);
                Console.Clear();
                Battle(player, ene);
            }
            else
            {
                Wait(750);
                Console.Clear();
                Shop(player);
            }
        }
        else if (PlayerChoice == 6)
        {
            GambleCoins(player);
        }
        else
        {
            Wait(750);
            Console.Clear();
            Shop(player);
        }
    }

    static void Wait(int i) { Thread.Sleep(i); } // sleep funktion istället för att behöva skriva ut thread.sleep
    static bool Yn()
    {
        string choice;
        string[] yns = { "YES", "NO" };
        for (int i = 0; i < yns.Length; i++)
        {
            Console.Write(i + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(yns[i]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
            Wait(100);
        }
        int PlayerChoice = Choose(yns);

        if (PlayerChoice == 0)
        {
            answer = true;
        }
        else if (PlayerChoice == 1)
        {
            answer = false;
        }
        return answer;
    }  // frågar ja eller nej

    // områden för fiender
    static void Forest(Player player)
    {
            Enemy enemy = SelectEnemy(1, 2);
            Wait(1000);
            Console.Clear();
            Battle(player, enemy);
            Wait(500);
            Forest(player);
    }
    static void Caves(Player player)
    {
            Enemy enemy = SelectEnemy(3, 4);
            Wait(1000);
            Console.Clear();
            Battle(player, enemy);
            Wait(500);
            Caves(player);
    }
    static void Mountains(Player player)
    {
            Enemy enemy = SelectEnemy(5, 6);
            Wait(1000);
            Console.Clear();
            Battle(player, enemy);
            Wait(500);
            Mountains(player);
    }
    static void DragonDen(Player player)
    {
            Enemy enemy = EnemyAttacked(7);
            Wait(1000);
            Console.Clear();
            Battle(player, enemy);
            Wait(500);
            DragonDen(player);
    }

    // Funktion för att köpa vapen, utrustning, Potions och charms
    static void BuyArmour(Player player)
    {
        Console.WriteLine($"Uppgrade costs {Armourlev[ArmourLev].Price} Coins");
        Console.WriteLine($"You have {player.Coins} Coins");
        Console.WriteLine("Would you like to uppgrade?");
        Yn();

        if (answer)
        {
            if (player.Coins >= Armourlev[ArmourLev].Price)
            {
                player.EquipArmour(Armourlev[ArmourLev]);
                Console.WriteLine($"You uppgraded to {Armourlev[ArmourLev].Name}");
                Console.WriteLine($"Your Strength uppgraded to {Armourlev[ArmourLev].Defense}");
                player.Coins -= Armourlev[ArmourLev].Price;
                ArmourLev++;
                HasArmour = true;
                player.TrueDefense = player.EquippedArmour.Defense + player.Defense;
                Shop(player);
            }
            if (Armourlev[ArmourLev].Price >= player.Coins)
            {
                NotEnoughCoins(player, Armourlev[ArmourLev].Price);
                Shop(player);
            }
        }
        else        
        {
            Shop(player);
        }
    }
    static void BuyWeapon(Player player)
    {
        Console.WriteLine($"Uppgrade costs {Weaponlev[WeaponLev].Price} Coins");
        Console.WriteLine($"You have {player.Coins} Coins");
        Console.WriteLine("Would you like to uppgrade?");

        Yn();

        if (answer == true)
        {
            if (player.Coins >= Weaponlev[WeaponLev].Price)
            {
                player.EquipWeapon(Weaponlev[WeaponLev]);
                Console.WriteLine($"You uppgraded to {Weaponlev[WeaponLev].Name}");
                Console.WriteLine($"Your Strength uppgraded to {Weaponlev[WeaponLev].Strength}");
                player.Coins -= Weaponlev[WeaponLev].Price;
                WeaponLev++;
                HasWeapon = true;
                player.TrueStrength = player.EquippedWeapon.Strength + player.Defense;
                Shop(player);
            }
            if (Weaponlev[WeaponLev].Price >= player.Coins)
            {
                NotEnoughCoins(player, Weaponlev[WeaponLev].Price);
                Shop(player);
            }
        }
        else
        {
            Shop(player);
        }
    }
    static void BuyCharms(Player player)
    {
        Console.WriteLine("Do you want to buy a Charm?");

        Yn();

        if (answer)
        {
            Console.WriteLine("Choose a charm to buy: ");
            for (int i = 0; i < CharmInv.Length; i++)
            {
                Console.Write(i + 1 + ":[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{CharmInv[i].Name}: {CharmInv[i].Price} Coins");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("]");
            }

            int PlayerChoice = ChooseCharm();

            for (int i = 0; i < CharmInv.Length; i++)
            {
                if (PlayerChoice == i)
                {
                    if (player.Coins >= CharmInv[i].Price && !CharmInv[i].Owned)
                    {
                        Console.WriteLine($"You bought {CharmInv[i].Name}");
                        player.Coins -= CharmInv[i].Price;
                        CharmInv[i].Owned = true;
                        HasCharms = true;

                        Console.WriteLine($"Would you like to equip {CharmInv[i].Name}");
                        Yn();
                        if (answer)
                        {
                            EquipCharm(i);
                            AddAndRemoveCharms(player);
                            ShowCharmStats(player);
                            Yn();

                            if (answer)
                            {
                                BuyCharms(player);
                                break;
                            }
                            else
                            {
                                Shop(player);
                                break;
                            }
                        }
                    }
                    if (player.Coins <= CharmInv[i].Price)
                    {
                        NotEnoughCoins(player, CharmInv[i].Price);
                        Shop(player);
                        break;
                    }
                    if (CharmInv[i].Owned == true)
                    {
                        Console.WriteLine($"You already own {CharmInv[i].Name}");
                        Shop(player);
                        break;
                    }
                }
            }
        }
        else
        {
            Shop(player);
        }
    }
    static void BuyPotion(Player player)
    {
        Console.WriteLine("What potion do you want to buy?");
        for (int x = 0; x < PotionInv.Length - player.EndGameC; x++)
        {
            Console.Write(x + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{PotionInv[x].Name}: {PotionInv[x].Price} Coins");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
        }
        string potionchoice = Console.ReadLine();
        Console.Clear();
        for (int i = 0; i < PotionInv.Length - player.EndGameC; i++)
        {
            if (ChoosePotion(i, potionchoice))
            {
                int amount = BuyPotionAm(player, i);
                if (player.Coins >= PotionInv[i].Price * amount)
                {
                    player.Coins -= PotionInv[i].Price * amount;
                    if (amount > 1)
                    {
                        Console.WriteLine($"You bought {amount} {PotionInv[i].Name}s");
                        PotionInv[i].Amount += amount;
                    }
                    else if (amount == 1)
                    {
                        Console.WriteLine($"You bought a {PotionInv[i].Name}");
                        PotionInv[i].Amount += amount;
                    }
                    else
                    {
                        Console.WriteLine("You didn't buy any Potions... Why?");
                    }
                }
                else
                {
                    NotEnoughCoins(player, PotionInv[i].Price * amount);
                }
            }
        }
        Shop(player);
    }
    static int BuyPotionAm(Player player, int i)
    {

        Console.Write($"How many {PotionInv[i].Name} would you like to buy:");

        try
        {
            int amount = int.Parse(Console.ReadLine());
            return amount;
        }
        catch (Exception)
        {
            Console.WriteLine("Please spcify a amount");
            return BuyPotionAm(player, i);
        }
    }


    // Funktioner för att visa statestik
    static void ShowPlayerStats(Player player)
    {
        Console.WriteLine($"Player: {player.Name}");
        Console.WriteLine($"Health: {player.Health} out of {player.TrueHealth}");
        Console.WriteLine($"Strength: {player.Strength}");
        Console.WriteLine($"Total Strength: {player.TrueStrength}");
        Console.WriteLine($"Defense: {player.Defense}");
        Console.WriteLine($"Total Defense: {player.TrueDefense}");
        Console.WriteLine($"Coins: {player.Coins}");
        Console.WriteLine($"Level: {player.Level}");
        Console.WriteLine($"Exp to level {player.Level + 1}: {player.XpToNextLevel - player.Experience} xp");
    }
    static void ShowArmourStats(Player player)
    {
        Console.WriteLine($"Armour: {player.EquippedArmour.Name}");
        Console.WriteLine($"Defence: {player.EquippedArmour.Defense}");
    }
    static void ShowWeaponStats(Player player)
    {
        Console.WriteLine($"Weapon: {player.EquippedWeapon.Name}");
        Console.WriteLine($"Strength: {player.EquippedWeapon.Strength}");
    }
    static void ShowOwnedCharms()
    {
        string owned = "";
        string equiped = "";
        for (int i = 0; i < CharmInv.Length; i++)
        {
            if (CharmInv[i].Owned == true)
            {
                owned = "Owned";
            }
            if (CharmInv[i].Owned == false)
            {
                owned = "Not Owned";
            }
            if (CharmInv[i].Equipped == true)
            {
                equiped = "Equipped";
            }
            if (CharmInv[i].Equipped == false)
            {
                equiped = "Not Equipped";
            }
            Console.Write(i + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{CharmInv[i].Name} Charm| Owned: {owned} | Equipped: {equiped}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
        }
    }
    static void ShowCharmStats(Player player)
    {
        double ShowStat;
        double Rounded;
        for (int i = 0; i < CharmInv.Length; i++)
        {
            if (CharmInv[i].Equipped == true)
            {
                Console.WriteLine($"Charm: {CharmInv[i].Name}");
                if (i == CharmInv[0].Type)
                {
                    ShowStat = player.TrueHealth;
                    Rounded = ShowStat - (ShowStat / CharmInv[0].Value);
                    Console.WriteLine($"Added Health: {Math.Round(Rounded, 2)}");
                }
                if (i == CharmInv[1].Type)
                {
                    ShowStat = player.TrueDefense;
                    Rounded = ShowStat - (ShowStat / CharmInv[1].Value);
                    Console.WriteLine($"Added Defense: {Math.Round(Rounded, 2)}");
                }
                if (i == CharmInv[2].Type)
                {
                    ShowStat = player.TrueStrength;
                    Rounded = ShowStat - (ShowStat / CharmInv[2].Value);
                    Console.WriteLine($"Added Strength: {Math.Round(Rounded, 2)}");
                }
                if (i == CharmInv[3].Type)
                {
                    Console.WriteLine($"Xp Multiplier: {CharmInv[3].Value}");
                }
                if (i == CharmInv[4].Type)
                {
                    Console.WriteLine($"Coin Multiplier: {CharmInv[4].Value}");
                }
            }
        }
    }

    // Funktioner för att sätta på och ta av charms
    static void EquipCharm(int i)
    {

        for (int x = 0; x < CharmInv.Length; x++)
        {
            CharmInv[x].Equipped = false;
        }
        CharmInv[i].Equipped = true;
    }
    static void ChooseCharmEquip(Player player)
    {
        Console.WriteLine("Choose a charm to equip");
        ShowOwnedCharms();

        int PlayerChoice = ChooseCharm();

        for (int i = 0; i < CharmInv.Length; i++)
        {
            if (PlayerChoice == i && CharmInv[i].Equipped == false)
            {
                if (CharmInv[i].Owned)
                {
                    EquipCharm(i);
                    AddAndRemoveCharms(player);
                    ShowCharmStats(player);
                    break;
                }
                else
                {
                    Console.WriteLine($"You don't own {CharmInv[i].Name}!!!");
                }
            }
            else if (PlayerChoice == i && CharmInv[i].Equipped == true)
            {
                Console.WriteLine($"You alredy have {CharmInv[i].Name} equipped");
                Town(player);
            }
        }
    }
    static void AddAndRemoveCharms(Player player)
    {
        if (LastEquippedC == 0)
        {
            player.TrueHealth /= CharmInv[0].Value;
            Math.Round(player.TrueHealth, 2);
        }
        else if (LastEquippedC == 1)
        {
            player.TrueDefense /= CharmInv[1].Value;
            Math.Round(player.Defense, 2);
        }
        else if (LastEquippedC == 2)
        {
            player.TrueStrength /= CharmInv[2].Value;
            Math.Round(player.Strength, 2);
        }
        else if (LastEquippedC == 3)
        {
            player.XpMultiplier = 1;
        }
        else if (LastEquippedC == 4)
        {
            player.CoinMultiplier = 1;
        }

        if (CharmInv[0].Equipped == true)
        {
            player.TrueHealth *= CharmInv[0].Value;
            Math.Round(player.TrueHealth, 2);
            LastEquippedC = 0;
        }
        else if (CharmInv[1].Equipped == true)
        {
            player.TrueDefense *= CharmInv[1].Value;
            Math.Round(player.TrueDefense, 2);
            LastEquippedC = 1;
        }
        else if (CharmInv[2].Equipped == true)
        {
            player.TrueStrength *= CharmInv[2].Value;
            Math.Round(player.TrueStrength, 2);
            LastEquippedC = 2;
        }
        else if (CharmInv[3].Equipped == true)
        {
            player.XpMultiplier = CharmInv[3].Value;
            LastEquippedC = 3;
        }
        else if (CharmInv[4].Equipped == true)
        {
            player.CoinMultiplier = CharmInv[4].Value;
            LastEquippedC = 4;
        }
    }

    // Använda en potion
    static bool ChoosePotion(int x, string c)
    {
        try
        {
            if (Command(c, PotionInv[x].Name) || int.Parse(c) == PotionInv[x].Strength)
            {
                return true;
            }
        }
        catch (Exception)
        {

        }
        return false;
    }
    static void UsePotion(Player player)
    {
        PotionInv[5].Value = player.TrueHealth;
        Console.WriteLine("Choose a potion:");
        for (int i = 0; i < PotionInv.Length - player.EndGameC; i++)
        {
            Console.Write(i + 1 + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{PotionInv[i].Name}: {PotionInv[i].Amount}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
        }
        string potionchoice = Console.ReadLine();
        Console.Clear();
        for (int i = 0; i < PotionInv.Length - player.EndGameC; i++)
        {
            if (ChoosePotion(i, potionchoice))
            {
                if (PotionInv[i].Amount > 0)
                {

                    if (PotionInv[i].Type == 0 && player.Health < player.TrueHealth)
                    {
                        PotionInv[i].Amount--;
                        player.Health += PotionInv[i].Value;
                        Console.WriteLine($"You used a potion");
                        Console.WriteLine($"You have {PotionInv[i].Amount} {PotionInv[i].Name}");
                        if (player.Health > player.TrueHealth)
                        {
                            double playerextrahp = player.TrueHealth - player.Health;
                            player.Health = player.TrueHealth;
                            Console.WriteLine($"You healed {PotionInv[i].Value - playerextrahp}");
                        }
                        else
                        {
                            Console.WriteLine($"You healed {PotionInv[i].Value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You are at max health");
                    }
                }
                else
                {
                    Console.WriteLine($"You dont have any {PotionInv[i].Name} potions to use");
                    Console.WriteLine("You can buy potions from the shop");
                }
            }
        }
    }

    // fiende attackerar eller får hälsa
    static bool EnemyTurn(int x, int y)
    {
        Random rnd = new();
        int choice = rnd.Next(x, y);

        if (choice == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    static void EnemyHeal(Enemy enemy, double x, double y)
    {
        enemy.Health += x * y;
        if (enemy.Health > x) //fienden ska inte kunna få mer hp än dens manx hp
        {
            enemy.Health = x;
            Console.WriteLine($"{enemy.Name} healed and is now at max health");
            Wait(1000);
        }
        else
        {
            Console.WriteLine($"{enemy.Name} healed!!\n{enemy.Name} gained {x * y} hp");
            Wait(1000);
        }
    }

    //fienden blir starkare efter när du klarar av mountains
    static void StrongerEnemies(Player player)
    {
        if (player.EndGame == true)
        {
            Console.WriteLine($"You start to hear monsters growl in the distance...");
            Console.WriteLine($"The monsters grow to be stronger");
            for (int i = 1; i < Enemies.Length - 1; i++)
            {
                Enemies[i].Health *= 1.3;
                Enemies[i].Strength *= 1.5;
                Enemies[i].Defense *= 1.5;
                Math.Round(Enemies[i].Health, 2);
                Math.Round(Enemies[i].Strength, 2);
                Math.Round(Enemies[i].Defense, 2);
            }
        }
    }

    // Välja fienden
    static Enemy EnemyAttacked(int x)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Oh watchout you are being attacked by {Enemies[x].Name}!");
        Console.ForegroundColor = ConsoleColor.White;

        return Enemies[x];
    }
    static Enemy SelectEnemy(int x, int y)
    {
        int o = x;
        int p = y;
        int q = 1;
        Console.WriteLine("Choose an enemy you would like to battle");
        for (int i = x; i < y + 1; i++)
        {
            Console.Write(q + ":[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Enemies[i].Name);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
            q++;
        }
        Console.WriteLine("Choose an option: ");
        string Choice = Console.ReadLine();
        Console.Clear();
        for (int i = x; i <= y; i++)
        {
            try
            {
                if (Command(Choice, Enemies[i].Name) || int.Parse(Choice) == i - x + 1)
                {
                    return Enemies[i];
                }
            }
            catch
            {
                
            }
        }                    
        
        return SelectEnemy(o, p);
    }

    // Coins
    static void NotEnoughCoins(Player player, int i)
    {
        if (player.Coins >= 0)
        {
            Console.WriteLine("Looks like you don't have enough Coins");
            Console.WriteLine("You can get coins from slaying monsters");
            Console.WriteLine($"You needs: {i - player.Coins} more Coins");
            Console.WriteLine($"You currently have: {player.Coins} Coins");
        }
        else
        {
            Console.WriteLine("Looks like you don't have enough Coins");
            Console.WriteLine("You can get coins from slaying monsters or gambeling");
            Console.WriteLine($"You needs: {i - player.Coins} more Coins");
            Console.WriteLine($"You currently have: {player.Coins} Coins");
        }
    }
    static void GambleCoins(Player player)
    {
        Console.WriteLine("Choose a diffeculty or leave before its too late.");
        string[] game = { "Leave", "Easy", "Medium", "Hard", "Ultimate"};
        int[] gc = { 20, 50, 100, 1000 };
        for (int i = 0; i < game.Length; i++)
        {
            if (i == 0)
            {
                Console.Write(i + 1 + ":[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(game[i]);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("]");
            }
            else
            {
                Console.Write(i + 1 + ":[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{game[i]}: {gc[i - 1]} Coins");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("]");
            }
        }

        int PlayerChoice = Choose(game);

        if (PlayerChoice == 0)
        {
            Shop(player);
        }
        else if (PlayerChoice == 1)
        {
            if (player.Coins >= gc[0])
            {
                player.Coins -= gc[0];
                t1++;

                if (GambleInput(1, 10))
                {
                    player.Coins += gc[0] * t1;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"YOU WIN: {gc[0] * t1} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    t1 = 0;
                    Shop(player);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"YOU LOST: {gc[0]} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    Shop(player);
                }
            }
            else
            {
                NotEnoughCoins(player, gc[0]);
                Shop(player);
            }

        }
        else if (PlayerChoice == 2)
        {
            if (player.Coins >= gc[1])
            {
                player.Coins -= gc[1];
                t2++;

                if (GambleInput(1, 25))
                {
                    player.Coins += gc[1] * t2;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"YOU WIN: {gc[1] * t2} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    t2 = 0;
                    Shop(player);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"YOU LOST: {gc[1]} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    Shop(player);
                }
            }
            else
            {
                NotEnoughCoins(player, gc[1]);
                Shop(player);
            }
        }
        else if (PlayerChoice == 3)
        {
            if (player.Coins >= gc[2])
            {
                player.Coins -= gc[2];
                t3++;

                if (GambleInput(1, 50))
                {
                    player.Coins += gc[2] * t3;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"YOU WIN: {gc[2] * t3} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    t3 = 0;
                    Shop(player);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"YOU LOST: {gc[2]} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    Shop(player);
                }
            }
            else
            {
                NotEnoughCoins(player, gc[2]);
                Shop(player);
            }
        }
        else if (PlayerChoice == 4)
        {
            if (player.Coins >= gc[3])
            {
                player.Coins -= gc[3];
                t4++;

                if (GambleInput(1, 100))
                {
                    player.Coins += gc[3] * t4;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"YOU WIN: {gc[3] * t4} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    t4 = 0;
                    Shop(player);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"YOU LOST: {gc[3]} Coins!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"You have: {player.Coins} Coins");
                    Shop(player);
                }
            }
            else
            {
                NotEnoughCoins(player, gc[3]);
                Shop(player);
            }
        }
    }

    static bool Run(int x, int y)
    {
        Random rnd = new();
        int r = rnd.Next(x, y);

        if (r == x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    static bool GambleInput(int x, int y)
    {
        Random rnd = new();
        int random = rnd.Next(x, y);
        Console.Write($"Choose a number between {x} and {y}: ");
        try
        {
            int PlayerChoice = int.Parse(Console.ReadLine());
            if (PlayerChoice == random)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return GambleInput(x, y);
        }
    }
    static bool Command(string s, string c)
    {
        bool valid = false;
        if (c[0..s.Length].ToLower() == s.ToLower())
        {
            valid = true;
        }
        else if (s is null)
        {
            valid = false;
        }
        return valid;
    }
    static int Choose(string[] s)
    {
        Console.Write("Choose an option: ");
        string PlayerChoice = Console.ReadLine();
        Console.Clear();
        for (int i = 0; i < s.Length; i++)
        {
            try // Försöker köra koden och om det inte går så fångar den felet och kör om koden
            {
                if (Command(PlayerChoice, s[i]) || int.Parse(PlayerChoice) - 1 == i) // Kollar om spelaren skriver in en siffra.
                {
                    return i;
                }
            }
            catch (Exception)
            {
                // Om det är fel inmatning så fortsätts koden att köras.
            }
        }
        return 0;
    }
    static int ChooseCharm()
    {
        Console.Write("Choose a Charm: ");
        string PlayerChoice = Console.ReadLine();
        Console.Clear();
        for (int i = 0; i < CharmInv.Length; i++)
        {
            if (Command(PlayerChoice, CharmInv[i].Name) || int.Parse(PlayerChoice) - 1 == i)
            {
                return i;
            }
        }
        return ChooseCharm();
    }

    // Startar om spelet
    static void PlayAgain(Player player)
    {
        Console.WriteLine("Do you want to play again?");
        Yn();
        if (answer)
        {
            Wait(1000);
            RestartGame(player);
            Console.Clear();
            Main();
        }
        else
        {
            Environment.Exit(0);
        }
    }
    static void RestartGame(Player player)
    {
        if (BoosterActive1 == true)
        {
            BoosterActive1 = false;
            for (int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i].Name = enemyNames[i];
                Enemies[i].Health = Math.Round(Enemies[i].Health / 1.5, 2);
                Enemies[i].Defense = Math.Round(Enemies[i].Defense / 1.3, 2);
                Enemies[i].Strength = Math.Round(Enemies[i].Strength / 1.3, 2);
                Enemies[i].Money /= 2;
            }
        }
        if (BoosterActive2 == true)
        {
            BoosterActive2 = false;
            for (int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i].Name = enemyNames[i];
                Enemies[i].Health = Math.Round(Enemies[i].Health / 1.5, 2);
                Enemies[i].Defense = Math.Round(Enemies[i].Defense / 1.3, 2);
                Enemies[i].Strength = Math.Round(Enemies[i].Strength / 1.3, 2);
                Enemies[i].XP /= 2;
            }
        }
        if (player.EndGame == true)
        {
            player.EndGame = false;
            player.EndGameC = 1;
            for (int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i].Health = Math.Round(Enemies[i].Health / 1.3, 2);
                Enemies[i].Defense = Math.Round(Enemies[i].Defense / 1.5, 2);
                Enemies[i].Strength = Math.Round(Enemies[i].Strength / 1.5, 2);
            }
        }
        for (int i = 0; i < PotionInv.Length; i++)
        {
            if (i == 0)
            {
                PotionInv[i].Amount += 1;
            }
            PotionInv[i].Amount = 0;
        }
        PotionInv[0].Amount += 1;
        for (int i = 0; i < CharmInv.Length; i++)
        {
            CharmInv[i].Owned = false;
            CharmInv[i].Equipped = false;
        }
        AddAndRemoveCharms(player);
        player.Coins = 0;
        player.Level = 1;
        player.Experience = 0;
        ArmourLev = 0;
        WeaponLev = 0;
        HasCharms = false;
        HasArmour = false;
        HasWeapon = false;
    }


    static void Battle(Player player, Enemy enemy) // Metod för att starta en strid mellan spelaren och en fiende
    {
        // Skriv ut stridsmeddelanden
        Console.WriteLine($"A battle starts! {player.Name} vs {enemy.Name}");
        Wait(1000);
        double TrueEnemyHp = enemy.Health;
        double AddedHp;
        while (!player.IsDead() && enemy.Health > 0)
        {
            // Spelaren attackerar fienden
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("##############################################");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[YOUR TURN]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("##############################################");
            Console.ForegroundColor = ConsoleColor.White;
            Wait(500);
            

            string[] choices = { "Attack", "Potions", "Run" };
            for (int i = 0; i < choices.Length; i++)
            {
                Console.Write(i + 1 + ":[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(choices[i]);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("]");
                Wait(100);
            }

            int PlayerChoice = Choose(choices);

            if (PlayerChoice == 0) // Funktion för att attackera
            {
                double playerDamage = player.Attack(enemy.Defense);
                enemy.TakeDamage(playerDamage);
                Console.WriteLine($"{player.Name} attacks {enemy.Name} and does {playerDamage} damage.");
                Wait(750);
            }
            else if (PlayerChoice == 1) // Funktion för att använda Potions
            {
                UsePotion(player);
            }
            else if (PlayerChoice == 2) // Funktion för att springa iväg
            {
                bool ran;
                if (enemy.Name == Enemies[0].Name && Run(-999999999, 999999999))
                {
                    ran = true;
                }
                else if ((enemy.Name == Enemies[1].Name || enemy.Name == Enemies[2].Name) && Run(1, 5))
                {
                    ran = true;
                }
                else if ((enemy.Name == Enemies[3].Name || enemy.Name == Enemies[4].Name) && Run(1, 10))
                {
                    ran = true;
                }
                else if ((enemy.Name == Enemies[5].Name || enemy.Name == Enemies[6].Name) && Run(1, 15))
                {
                    ran = true;
                }
                else if (enemy.Name == Enemies[7].Name && Run(1, 20))
                {
                    ran = true;
                }
                else
                {
                    ran = false;
                }

                if (ran == true)
                {
                    Console.WriteLine($"You successfully escaped {enemy.Name}");
                    Wait(1000);
                    Console.Clear();
                    Console.WriteLine("Do you want to go back to town?");
                    Yn();
                    if (answer)
                    {
                        Town(player);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Fienden får välja att attackera eller få hälsa
            if (enemy.Name == Enemies[0].Name && EnemyTurn(1, 3) && enemy.Health < TrueEnemyHp)
            {
                AddedHp = 0.1;
                EnemyHeal(enemy, TrueEnemyHp, AddedHp);
            }
            else if ((enemy.Name == Enemies[1].Name || enemy.Name == Enemies[2].Name) && EnemyTurn(1, 20) && enemy.Health < TrueEnemyHp)
            {
                AddedHp = 0.10;
                EnemyHeal(enemy, TrueEnemyHp, AddedHp);
            }
            else if ((enemy.Name == Enemies[3].Name || enemy.Name == Enemies[4].Name) && EnemyTurn(1, 15) && enemy.Health < TrueEnemyHp)
            {
                AddedHp = 0.15;
                EnemyHeal(enemy, TrueEnemyHp, AddedHp);
            }
            else if ((enemy.Name == Enemies[5].Name || enemy.Name == Enemies[6].Name) && EnemyTurn(1, 10) && enemy.Health < TrueEnemyHp)
            {
                AddedHp = 0.20;
                EnemyHeal(enemy, TrueEnemyHp, AddedHp);
            }
            else if (enemy.Name == Enemies[7].Name && EnemyTurn(1, 5) && enemy.Health < TrueEnemyHp)
            {
                AddedHp = 0.25;
                EnemyHeal(enemy, TrueEnemyHp, AddedHp);
            }
            else
            {
                double enemyDamage = enemy.Attack(player.Defense);
                player.TakeDamage(enemyDamage);
                Console.WriteLine($"{enemy.Name} attacks {player.Name} and does {enemyDamage} damage");
                Wait(750);
            }


            // Skriv ut hälsan för spelaren och fienden
            Console.WriteLine($"{player.Name} has {player.Health} hp left.");
            Wait(750);


            Console.WriteLine($"{enemy.Name} has {enemy.Health} hp left.");
            Wait(750);

            // Om spelaren vinner, skriv ut en vinstmeddelande

            if (player.IsDead())
            {
                // Om fienden vinner, skriv ut ett förlorad meddelande
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{player.Name} got killed by {enemy.Name}!".ToUpper());
                Console.ForegroundColor = ConsoleColor.White;
                Wait(1000);
                Console.Clear();
                PlayAgain(player);

            }
            else if (enemy.Health <= 0)
            {
                enemy.Health = TrueEnemyHp; // man kan spela mot samma fiende flera gånger
                enemy.Defeated = true;
                Console.WriteLine($"{player.Name} killed {enemy.Name}!");
                Console.WriteLine($"{player.Name} recieved {enemy.Money} Coins");
                Console.WriteLine($"{player.Name} recieved {enemy.XP} EXP");
                Math.Round(player.Health, 2);
                player.GiveXP(enemy.XP);
                player.PlayerMoney(enemy.Money);

                if (Enemies[1].Defeated && Enemies[2].Defeated && !ForestComplete)
                {
                    ForestComplete = true;
                    player.PlayerMoney(ForestCoins);
                    Console.WriteLine($"You receaved {ForestCoins} coins for compleating the forest!!".ToUpper());
                }
                else if (Enemies[3].Defeated && Enemies[4].Defeated && !CavesComplete)
                {
                    CavesComplete = true;
                    player.PlayerMoney(CavesCoins);
                    Console.WriteLine($"You receaved {CavesCoins} coins for compleating the caves!!".ToUpper());
                }
                else if (Enemies[5].Defeated && Enemies[6].Defeated && !MountainsComplete)
                {
                    MountainsComplete = true;
                    player.PlayerMoney(MountainsCoins);
                    Console.WriteLine($"You receaved {MountainsCoins} coins for compleating the mountains!!".ToUpper());
                }

                Wait(1000);
                Console.Clear();


                bool Upgraded = false;
                if (Enemies[6].Defeated == true && !Upgraded)
                {
                    Upgraded = true;
                    player.EndGame = true;
                    player.EndGameC = 0;
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ENDGAME: UNLOCKED!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    StrongerEnemies(player);
                    Wait(1000);
                }
                Console.WriteLine("Do you want to go back to town?");
                Yn();
                if (answer)
                {
                    Town(player);
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }
}

class Player
{
    public string Name { get; set; }
    public double Health { get; set; }
    public double Strength { get; set; }
    public double Defense { get; set; }
    public int Level { get; set; }
    public double Experience { get; set; }
    public double Coins { get; set; }
    public double XpMultiplier { get; set; }
    public double CoinMultiplier { get; set; }
    public double XpToNextLevel { get; set; }
    public double TrueHealth { get; set; }
    public double TrueStrength { get; set; }
    public double TrueDefense { get; set; }
    public bool EndGame { get; set; }
    public int EndGameC { get; set; }
    public Armour EquippedArmour { get; set; }
    public Weapon EquippedWeapon { get; set; }
    public Charms EquippedCharm { get; set; }

    public Player(string name, int health, int strength, int defense, int coins)
    {
        Name = name;
        Health = health;
        Strength = strength;
        Defense = defense;
        Level = 1;
        Experience = 0;
        Coins = coins;
        XpToNextLevel = 50 * Level;
        XpMultiplier = 1;
        CoinMultiplier = 1;
        TrueHealth = 0;
        TrueStrength = 0;
        TrueDefense = 0;
        EndGame = false;
        EndGameC = 1;
    }

    public double Attack(double defence)
    {
        double totalDamage = Math.Round(TrueStrength - (defence / 4), 2);
        return totalDamage;
    }

    public double TakeDamage(double damage)
    {
        // här är skadan räknad, spelarens försvar är halverad för att få ut hur mycket skada den blockerar
        double totalDamage = Math.Round(damage, 2);
        return Health -= totalDamage;
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public void GiveXP(double xp)
    {
        Experience += xp * XpMultiplier;
        LevelUp();
    }

    public void PlayerMoney(int money)
    {
        Coins += money * CoinMultiplier;
    }

    public void LevelUp()
    {
        while (Experience >= XpToNextLevel)
        {
            Level++;
            Experience -= XpToNextLevel * Level;
            TrueHealth += Level * 1.3;
            Defense += Level * 1.5;
            Strength += Level * 1.5;
            TrueDefense += Level * 1.5;
            TrueStrength += Level * 1.5;
            Console.WriteLine($"{Name} got a level!");
            Console.WriteLine($"{Name} is now level: {Level}!");// Spelaren blir starkare när de får en nivå
            Console.WriteLine($"{Name} went up in health: {TrueHealth}"); 
            Console.WriteLine($"{Name} went up in defense: {Defense}");
            Console.WriteLine($"{Name} went up in strength: {Strength}");
        }
    }

    public void EquipArmour(Armour armour)
    {
        EquippedArmour = armour;
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
    }

    public void EquipCharm(Charms charms)
    {
        EquippedCharm = charms;
    }
}

class Enemy
{
    public string Name { get; set; }
    public double Health { get; set; } 
    public double Strength { get; set; }
    public double Defense { get; set; }
    public double XP { get; set; }
    public int Money { get; set; }
    public bool Defeated { get; set; }

    public Enemy(string name, double health, double strength, double defense, double xp, int money, bool defeated)
    {
        Name = name;
        Health = health;
        Strength = strength;
        Defense = defense;
        XP = xp; // Antal XP som spelaren får för att besegra fienden
        Money = money; // Hur mycket pengar du får av att besegra en fiende
        Defeated = defeated;
    }

    public double Attack(double defence)
    {
        double DamageDone = Math.Round(Strength - (defence / 4));
        return DamageDone;
    }

    public double TakeDamage(double damage)
    {
        Health -= Math.Round(damage, 2); // Lägger till försvar för fienden
        return Health;
    }
}

class Armour
{
    public string Name { get; set; }
    public int Defense { get; set; }
    public int Price { get; set; }
    public bool Bought { get; set; }

    public Armour(string name, int defense, int price)
    {
        Name = name;
        Defense = defense;
        Price = price;
    }
}

class Weapon
{
    public string Name { get; set; }
    public int Strength { get; set; }
    public int Price { get; set; }
    public bool Bought { get; set; }

    public Weapon(string name, int strength, int price)
    {
        Name = name;
        Strength = strength;
        Price = price;
    }
}

class Potion
{
    public string Name { get; set; }
    public int Type { get; set; }
    public int Amount { get; set; }
    public double Value { get; set; }
    public int Price { get; set; }
    public int Strength { get; set; }

    public Potion(string name, int type, int amount, double value, int price, int strength)
    {
        Name = name;
        Type = type;
        Amount = amount;
        Value = value;
        Price = price;
        Strength = strength;
    }
}

class Charms
{
    // Charms kan ge: Health, Defense, Strength, Xp, Coins
    public string Name { get; set; }
    public int Type { get; set; }
    public bool Owned { get; set; }
    public double Value { get; set; }
    public int Price { get; set; }
    public string Choose { get; set; }
    public bool Equipped { get; set; }

    public Charms(string name, bool owned, bool equiped, int type, double value, int price, string chose)
    {
        Name = name;
        Owned = owned;        
        Equipped = equiped;
        Type = type;
        Value = value;
        Price = price;
        Choose = chose;
    }
}
