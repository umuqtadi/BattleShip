using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid newGame = new Grid();

            newGame.PlayGame();
        }
    }
    class Point
    {
        
        public enum PointStatus
        {
            Empty,
            Ship,
            Hit,
            Miss
        }
        //properties of the point class
        public int X { get; set; }
        public int Y { get; set; }
        public PointStatus Status { get; set; }

        
        public Point(int x, int y, PointStatus p)
        {
            this.X = x;
            this.Y = y;
            this.Status = p;
        }
    }
    class Ship
    {
        //showing the different types of ships in the game
        public enum ShipType
        {
            Carrier, Battleship, Cruiser, Submarine, Minesweeper
        }

        //the properties of a ship
        public ShipType Type { get; set; }
        public List<Point> OccupiedPoints { get; set; }
        public int Length { get; set; }
        public bool IsDestroyed { get; set; }


        public Ship(ShipType typeOfShip)
        {
            this.OccupiedPoints = new List<Point>();
            this.Type = typeOfShip;

            //switch statements that determines the length of the ship
            switch (typeOfShip)
            {
                case ShipType.Carrier:
                    this.Length = 5;
                    break;
                case ShipType.Battleship:
                    this.Length = 4;
                    break;
                case ShipType.Cruiser:
                case ShipType.Submarine:
                    this.Length = 3;
                    break;
                case ShipType.Minesweeper:
                    this.Length = 2;
                    break;
                default:
                    break;
            }

        }
    }

    class Grid
    {
        //giving the ship 2 options horizontal or vertical
        public enum PlaceShipDirection
        {
            Horizontal,
            Vertical
        }
        //multi-dimensional array
        public Point[,] Ocean { get; set; }
        //list to hold the ships
        public List<Ship> ListOfShips { get; set; }
        public bool AllShipsDestroyed { get {
            return false;
        } }
        public int CombatRound { get; set; }

        public Grid()
        {
            this.Ocean = new  Point[10, 10];
            
            //creating the points that can be used on the grid
            for (int y = 0; y < Ocean.GetLength(1); y++)
            {
                for (int x = 0; x < Ocean.GetLength(0); x++)
                {
                    //making the grid blue
                    Console.ForegroundColor= ConsoleColor.Blue;
                    Ocean[x, y] = new Point(x, y, Point.PointStatus.Empty);
                }
            }

            //adding all the ships to the list of ships
            this.ListOfShips = new List<Ship>();
            ListOfShips.Add(new Ship(Ship.ShipType.Carrier));
            ListOfShips.Add(new Ship(Ship.ShipType.Battleship));
            ListOfShips.Add(new Ship(Ship.ShipType.Cruiser));
            ListOfShips.Add(new Ship(Ship.ShipType.Submarine));
            ListOfShips.Add(new Ship(Ship.ShipType.Minesweeper));

            Random rngX = new Random();
            Random rngY = new Random();
            int XX = rngX.Next(0, 10);
            int YY = rngY.Next(0, 10);

            //setting the ships at specific points
            PlaceShip(ListOfShips[0], PlaceShipDirection.Horizontal, 0, 9);
            PlaceShip(ListOfShips[1], PlaceShipDirection.Horizontal, 0, 1);
            PlaceShip(ListOfShips[2], PlaceShipDirection.Horizontal, 0, 7);
            PlaceShip(ListOfShips[3], PlaceShipDirection.Horizontal, 2, 4);
            PlaceShip(ListOfShips[4], PlaceShipDirection.Horizontal, 7, 3);
            
        }

        /// <summary>
        /// will place the ships on the grid
        /// </summary>
        /// <param name="shipToPlace"></param>
        /// <param name="direction"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        public void PlaceShip(Ship shipToPlace, PlaceShipDirection direction, int startX, int startY)
        {
            for (int i = 0; i < shipToPlace.Length; i++)
            {
                Ocean[startX, startY].Status = Point.PointStatus.Ship;
                
                    if (direction == PlaceShipDirection.Horizontal)
                    {
                        startX++;
                    }
                    else
                    {
                        startY++;
                    }
                
                
            }
        }

        /// <summary>
        /// will create the grid and change the grid letting you know if there is a hit or miss
        /// </summary>
        public void DisplayOcean()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            //will display above grid to help with grid identification
            Console.WriteLine("     0  1  2  3  4  5  6  7  8  9 ");
            Console.WriteLine("     ----------------------------");

            for (int y = 0; y < this.Ocean.GetLength(1); y++)
            { 
                Console.Write("{0}|| ", y);

                for (int x = 0; x < this.Ocean.GetLength(0); x++)
                {
                    //will display if not hit or miss
                    Point pointNow = this.Ocean[x, y];
                    if (pointNow.Status == Point.PointStatus.Ship)
                    {
                        Console.Write("[ ]");
                    }
                    if (pointNow.Status == Point.PointStatus.Empty)
                    {
                        Console.Write("[ ]");
                    }
                    // will display if hit
                    else if (pointNow.Status == Point.PointStatus.Hit)
                    {
                        Console.Write("[X]");
                        
                    }
                    //will display if miss
                    else if (pointNow.Status == Point.PointStatus.Miss)
                    {
                        Console.Write("[O]");
                       
                    }
                }
                Console.WriteLine();
            }
        }

        public bool Target(int x, int y)
        {
            int destroyedShips = ListOfShips.Where(X => X.IsDestroyed == true).Count();

            Point whereHit = this.Ocean[x , y ];

                //determing whether a hit or not
                if (whereHit.Status == Point.PointStatus.Ship)
                {
                    whereHit.Status = Point.PointStatus.Hit;
                }
                if (whereHit.Status == Point.PointStatus.Empty)
                {
                    whereHit.Status = Point.PointStatus.Miss;
                }
            //new list of ships that have been destroyed
            int newlyDestroyed = ListOfShips.Where(X => X.IsDestroyed == true).Count();

            if (newlyDestroyed > destroyedShips)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
   
        /// <summary>
        /// contains the logic for the game, keep playing until all ships destroyed
        /// </summary>
        public void PlayGame()
        {
            string inputX = string.Empty;
            string inputY = string.Empty;

            //game keeps going until all ships have been destroyed
            while (!AllShipsDestroyed)
            {
                Console.Clear();
                //makes the grid show up
               
                this.DisplayOcean();

                while (true)
                {
                    
                    Console.WriteLine("Please enter an x coordinate");
                    inputX = Console.ReadLine();
                    Console.WriteLine("Please enter a Y coordinate");
                    inputY = Console.ReadLine();
                    if (("0123456789").Contains(inputX) && inputX.Length == 1 && ("0123456789").Contains(inputY) && inputY.Length == 1)
                    {
                        Target(int.Parse(inputX), int.Parse(inputY));
                        CombatRound++;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid input");
                    }
                }
            }
        }
    }
}

