using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSimulatiorV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            Console.ReadKey();
        }
    }
    /// <summary>
    /// Basic class
    /// </summary>
    public class Actor
    {
        //properties for name and HP
        public string Name { get; set; }
        public int HP { get; set; }

        //read only property that checks if actor is still alive
        public bool IsAlive
        {
            get
            {
                if (this.HP > 0)
                {
                    return true;
                }
                return false;
            }
        }
        //random number generator property
        public Random RNG { get; set; }
        /// <summary>
        /// Constructor that takes name and HP as parameters and initialize them as well as rng
        /// </summary>
        /// <param name="name">Name of actor</param>
        /// <param name="HP">Actor's hp amount</param>
        public Actor(string name, int HP)
        {
            this.Name = name;
            this.HP = HP;
            this.RNG = new Random();
        }
        //method for attack
        public virtual void DoAttack(Actor actor) {}
        /// <summary>
        /// Method that is used for data validation
        /// </summary>
        /// <param name="userInput">Input to be checked</param>
        /// <returns>Returns true if input is valid and false if not</returns>
        public bool InputValidation(string userInput)
        {
            //checking if input is a number
            int result = 0;
            if (int.TryParse(userInput, out result))
            {
                //checking if input is between 1 and 3
                if (int.Parse(userInput) >= 1 && int.Parse(userInput) <= 3)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Class for enemy or "computer"
    /// </summary>
    public class Enemy : Actor
    {
        //properties for lowest and highest damage, sleep length and type of game version
        private int OutPutPause { get; set; }
        private int EnemyLowestDmg { get; set; }
        private int EnemyHighestDmg { get; set; }
        private string Version { get; set; }
        /// <summary>
        /// Constructor that takes in name, hp amount and type of version.
        /// </summary>
        /// <param name="name">Name that is passed to base class</param>
        /// <param name="HP">HP amount that is passed to base class</param>
        /// <param name="version">Type of game version</param>
        public Enemy(string name, int HP, string version)
            : base(name, HP)
        {
            //initializing lowest and highest damage amounts and pause duration
            this.EnemyLowestDmg = 5;
            this.EnemyHighestDmg = 15;
            this.OutPutPause = 700;
            //if version is hardcore then damage is doubled
            if (version == "hardcore")
            {
                this.Version = "hardcore";
                this.EnemyLowestDmg *= 2;
                this.EnemyHighestDmg *= 2;
            }
        }
        /// <summary>
        /// Method that performs an attack
        /// </summary>
        /// <param name="actor">Actor for attack</param>
        public override void DoAttack(Actor actor)
        {
            //initializing random number for enemy's damage
            int enemyHit = this.RNG.Next(this.EnemyLowestDmg, this.EnemyHighestDmg + 1);
            //if version is hardcore then enemy never misses and gets a 20% crit chance with doubled damage
            if (this.Version == "hardcore")
            {
                //initializing a random number for crit chance
                int enemyCritChance = this.RNG.Next(1, 11);
                //checking if a 20% crit chance happened
                if (enemyCritChance <= 2)
                {
                    //if crit chance happened, then damage is doubled. Decreasing player HP
                    actor.HP -= enemyHit * 2;
                    //printing out message and executes sound
                    Console.WriteLine("Dragon did a critical hit on you - {0}! Damage is doubled!".ToUpper(), enemyHit * 2);
                    Console.Beep(700, 2000);
                }
                else
                {
                    //decreasing player HP by the amount of enemy hit and printing the message
                    actor.HP -= enemyHit;
                    Console.WriteLine("Dragon hit you for {0} DMG! Now he never misses and his damage is higher!", enemyHit);
                }
            }
            //this part executes if game version is easy
            else
            {
                //initializing enemy's hit chance
                int enemyHitChance = this.RNG.Next(1, 11);
                //checking if enemy's hit chance of 80% has happened
                if (enemyHitChance <= 8)
                {
                    //executes if hit was successful, then decreasing playe's HP, printing message and waiting
                    actor.HP -= enemyHit;
                    Console.WriteLine("Dragon hit you for {0} DMG!", enemyHit);
                    System.Threading.Thread.Sleep(this.OutPutPause);
                }
                else
                {
                    //executes if hit was unsuccessful, printing message and waiting
                    Console.WriteLine("Dragon missed his hit on you!");
                    System.Threading.Thread.Sleep(this.OutPutPause);
                }
            }
        }
    }

    /// <summary>
    /// Class for player
    /// </summary>
    public class Player : Actor
    {
        //properties for lowest and highest swond and magic damages, heal amount, pause length and type of version
        private int OutPutPause { get; set; }
        private int SwordLowestDmg { get; set; }
        private int SwordHighestDmg { get; set; }
        private int MagicLowestDmg { get; set; }
        private int MagicHighestDmg { get; set; }
        private int HealLowest { get; set; }
        private int HealHighest { get; set; }
        private string Version { get; set; }

        /// <summary>
        /// Enums fpr types of attack
        /// </summary>
        public enum AttackType
        {
            Sword = 1, Magic, Heal, InvalidInput
        }
        /// <summary>
        /// Constructor that takes in name, hp amount and type of version.
        /// </summary>
        /// <param name="name">Name that is passed to base class</param>
        /// <param name="HP">HP amount that is passed to base class</param>
        /// <param name="version">Type of game version</param>
        public Player(string name, int HP, string version)
            : base(name, HP)
        {
            //initializing lowest and highest swond and magic damages, heal amount and pause length
            this.OutPutPause = 700;
            this.SwordLowestDmg = 20;
            this.SwordHighestDmg = 35;
            this.MagicLowestDmg = 10;
            this.MagicHighestDmg = 15;
            this.HealLowest = 10;
            this.HealHighest = 20;
            //if version is hardcore then heal amount is doubled
            if (version == "hardcore")
            {
                this.Version = "hardcore";
                this.HealLowest *= 2;
                this.HealHighest *= 2;
            }
        }
        /// <summary>
        /// Method that performs an attack
        /// </summary>
        /// <param name="actor">Actor for attack</param>
        public override void DoAttack(Actor actor)
        {
            //calling method to ask user for attack choice
            AttackType userChoice = ChooseAttack();
            //executes if user choice was 1 (sword)
            if (userChoice == AttackType.Sword)
            {
                //initializing player's hit chance
                int playerHitChance = this.RNG.Next(1, 11);
                //initializing random number for player's damage
                int playerHit = RNG.Next(this.SwordLowestDmg, this.SwordHighestDmg + 1);
                //checking if player's hit chance of 70% has happened
                if (playerHitChance <= 7)
                {
                    //executes if hit was successful, then decreasing enemy's HP and printing message
                    actor.HP -= playerHit;
                    Console.WriteLine("You hit the dragon for {0} DMG with your sword!", playerHit);
                    System.Threading.Thread.Sleep(this.OutPutPause);
                }
                else
                {
                    //executes if hit was unsuccessful
                    Console.WriteLine("You missed!");
                    System.Threading.Thread.Sleep(this.OutPutPause);
                }
            }
            //executes if choice was 2 (magic)
            else if (userChoice == AttackType.Magic)
            {
                //initializing random number for player's damage, decreasing enemy's HP and printing message
                int playerHit = this.RNG.Next(this.MagicLowestDmg, this.MagicHighestDmg + 1);
                actor.HP -= playerHit;
                Console.WriteLine("You hit the dragon for {0} DMG with your magic!", playerHit);
                System.Threading.Thread.Sleep(this.OutPutPause);
            }
            //executes if choice was 3 (heal)
            else if (userChoice == AttackType.Heal)
            {
                //initializing random number for player's heal
                //increasing player's HP and printing message
                int playerHeal = this.RNG.Next(this.HealLowest, this.HealHighest);
                HP += playerHeal;
                if (this.Version == "hardcore")
                {
                    Console.WriteLine("You healed yourself for {0} points! Now your heal is stronger!", playerHeal);
                }
                else
                {
                    Console.WriteLine("You healed yourself for {0} points!", playerHeal);
                }
                System.Threading.Thread.Sleep(this.OutPutPause);
            }
            //executes if input was invalid
            else
            {
                //printing out message and process enemy move
                Console.WriteLine("Your input is invalid, and dragon doesn't care!");
                System.Threading.Thread.Sleep(this.OutPutPause);
            }
        }
        /// <summary>
        /// Methods that checks the user input and returns a respective attack type
        /// </summary>
        /// <returns>Returns an attack type</returns>
        private AttackType ChooseAttack()
        {
            //asking user for input
            Console.Write("\nPlease enter your choise: ");
            string userInput = Console.ReadLine();
            Console.WriteLine("\n");
            //checking if input was valid
            if (this.InputValidation(userInput))
            {
                return (AttackType)int.Parse(userInput);
            }
            else
            {
                return AttackType.InvalidInput;
            }
        }
    }
    /// <summary>
    /// Class that performs the game
    /// </summary>
    public class Game
    {
        //property created for choice reminder message
        private int Counter { get; set; }
        //properties for player and enemy classes
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }
        /// <summary>
        /// Constructor that runs a game
        /// </summary>
        public Game()
        {
            //asking for player's name
            Console.Write("Please enter your name: ");
            string name = Console.ReadLine();
            Console.Clear();
            //printing out greeting and instructions
            string greeting = "Greeting player " + name + "!";
            for (int i = 0; i < greeting.Length; i++)
            {
                Console.Write(greeting[i]);
                System.Threading.Thread.Sleep(50);
            }
            Console.WriteLine("\n");
            Console.WriteLine(@"How to play:
You are going to fight with a dragon who wants to kill you. You will have a choice of easy and hardcore version. 
In easy version, you will have three options to use at each move:
1 - Sword. Sword does a big damage between 20 and 35, but hits only 70% of the time.
2 - Magic. Magic does lower damage between 10 and 15, but hits all the time.
3 - Heal. You heal yourself for 10 to 20 HP.
Dragon will hit you back each move between 5 and 15 damage, and he hits only 80% of the time.
You start with 100HP and Dragon starts with 200HP. 

In hardcore version, dragon is stronger and few new features added. You will figure it out. Choices and HP amounts stays the same.");
            //asking for user's choice
            Console.Write("\nFor easy version enter 1 and for hardcore enter 2: ");
            string userChoice = Console.ReadLine();
            //creating a boolean for user choice loop
            bool userChoiceBool = true;
            //user choice loop
            while (userChoiceBool)
            {
                //checking if input is valid and if it isn't 3
                if (this.InputValidation(userChoice) && int.Parse(userChoice) <= 2)
                {
                    //executes if user input was 1
                    if (int.Parse(userChoice) == 1)
                    {
                        //clearing the console and printing out message
                        Console.Clear();
                        Console.WriteLine("You chose easy version. Remember: you must enter a number between 1 and 3 for your choice. Good luck!\n");
                        //stops user choice loop and calls function of easy version
                        userChoiceBool = false;
                        //creating objects of player and enemy, and runs an easy version of the game
                        this.Player = new Player(name, 100, "easy");
                        this.Enemy = new Enemy("Dragon", 200, "easy");
                        this.PlayEasyVersion();
                    }
                    //executes if user input was 2
                    else if (int.Parse(userChoice) == 2)
                    {
                        //increasing console window size
                        Console.WindowHeight = 45;
                        Console.WindowWidth = 115;
                        //clearing the console and printing out message
                        Console.Clear();
                        Console.WriteLine("You chose hardcore version. Remember: you must enter a number between 1 and 3 for your choice. Good luck!");
                        //stops user choice loop and calls function of hardcore version
                        userChoiceBool = false;
                        //creating objects of player and enemy, and runs a hardcore version of the game
                        this.Player = new Player(name, 100, "hardcore");
                        this.Enemy = new Enemy("Dragon", 200, "hardcore");
                        this.PlayHardVersion();
                    }
                }
                else
                {
                    //executes if input was invalid
                    //printing out a message and asks for another input
                    Console.WriteLine("Your input is invalid. Please enter 1 or 2.");
                    Console.Write("\nFor easy version enter 1 and for hardcore enter 2: ");
                    userChoice = Console.ReadLine();
                }
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// Method for displaying the current combat info
        /// </summary>
        /// <param name="counter">Counter for reminder message</param>
        public void DisplayCombatInfo(int counter)
        {
            //prints out a current HP status
            Console.WriteLine("Current HP status -> Your HP: {0}    Dragon HP: {1}", Player.HP, Enemy.HP);
            //executes if it wasn't a first print out
            if (counter == 1)
            {
                //prints out a reminder message
                Console.WriteLine("\nReminder about choises: ");
                Console.WriteLine("1 - Sword, 2 - Magic, 3 - Heal");
            }
        }

        public bool InputValidation(string userInput)
        {
            //checking if input is a number
            int result = 0;
            if (int.TryParse(userInput, out result))
            {
                //checking if input is between 1 and 3
                if (int.Parse(userInput) >= 1 && int.Parse(userInput) <= 3)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Function for easy version
        /// </summary>
        public void PlayEasyVersion()
        {
            //checking if dragon and player are still alive
            while (this.Player.IsAlive && this.Enemy.IsAlive)
            {
                //calling method for displaying combat info
                DisplayCombatInfo(this.Counter);
                //prints out a picture
                Console.WriteLine(@"   
                                \||/
                                |  @___oo          
                    /\  /\   / (__,,,,|      
                    ) /^\) ^\/ _)                             /
                    )   /^\/   _)                      ,~~   /
                    )   _ /  / _)                  _  <=)  _/_ 
                /\  )/\/ ||  | )_)          VS    /I\.=""==.{>
                <  >      |(,,) )__)               \I/-\T/-'
                ||      /    \)___)\                  /_\
                | \____(      )___) )___             // \\_     
                \______(_______;;; __;;;           _I    /      ");

                //performing attacks and clearing the console
                this.Player.DoAttack(this.Enemy);
                this.Enemy.DoAttack(this.Player);
                this.Counter = 1;
                Console.Clear();
            }
            //printing out messages if player or enemy are dead
            if (this.Player.IsAlive)
            {
                Console.WriteLine("You won!");
            }
            else
            {
                Console.WriteLine("You lost!");
            }
        }
        /// <summary>
        /// Function for hardcore version
        /// </summary>
        public void PlayHardVersion()
        {
            //checking if dragon and player are still alive
            while (this.Player.IsAlive && this.Enemy.IsAlive)
            {
                //calling method for displaying combat info
                DisplayCombatInfo(this.Counter);
                //prints out a picture
                Console.WriteLine(@"
                                                                 ^                       ^
                                                                 |\   \        /        /|
                                                                /  \  |\__  __/|       /  \
                                                               / /\ \ \ _ \/ _ /      /    \
                                                              / / /\ \ {*}\/{*}      /  / \ \
                                                              | | | \ \( (00) )     /  // |\ \
                                                              | | | |\ \(V""V)\    /  / | || \| 
                                                              | | | | \ |^--^| \  /  / || || || 
                                                             / / /  | |( WWWW__ \/  /| || || ||
                                            _______         | | | | | |  \______\  / / || || || 
                                  |\     /|(  ____ \        | | | / | | )|______\ ) | / | || ||
                                  | )   ( || (    \/        / / /  / /  /______/   /| \ \ || ||
         ,;~;,  ))                | |   | || (_____        / / /  / /  /\_____/  |/ /__\ \ \ \ \
            /\_                   ( (   ) )(_____  )      | | | / /  /\______/    \   \__| \ \ \
           (  /                    \ \_/ /       ) |      | | | | | |\______ __    \_    \__|_| \
           (()      ;,;             \   /  /\____) |      | | ,___ /\______ _  _     \_       \  |
           | \\  ,,;;'(              \_/   \_______)      | |/    /\_____  /    \      \__     \ |    /\
       __ _(  )m=(((((((======-------                     |/ |   |\______ |      |        \___  \ |__/  \
     /'  '\'()/~' ' /'\.                                  v  |   |\______ |      |            \___/     |
  ,;(      )||     (                                         |   |\______ |      |                    __/
 ,;' \    /-(.;,   )                                          \   \________\_    _\               ____/
      ) /       ) /                                         __/   /\_____ __/   /   )\_,      _____/
     //         ||                                         /  ___/  \uuuu/  ___/___)    \______/
    (_\         (_\                                        VVV  V        VVV  V ");

                //performing attacks and clearing the console
                this.Player.DoAttack(this.Enemy);
                this.Enemy.DoAttack(this.Player);
                this.Counter = 1;
                //extra pause before going to next move
                if (this.Player.IsAlive && this.Enemy.IsAlive)
                {
                    Console.Write("\nPress any key to keep going...");
                    Console.ReadKey();
                }
                Console.Clear();
            }
            //printing out messages if player or enemy are dead
            if (this.Player.IsAlive)
            {
                Console.WriteLine("You won!");
            }
            else
            {
                Console.WriteLine("You lost!");
            }
        }
    }
}
