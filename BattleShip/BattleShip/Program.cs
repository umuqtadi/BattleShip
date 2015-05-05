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
        public enum ShipType
        {
            Carrier, Battleship, Cruiser, Submarine, Minesweeper
        }

        public ShipType Type { get; set; }
        public List<Point> OccupiedPoints { get; set; }
        public int Length { get; set; }
        public bool IsDestroyed { get; set; }

        public Ship(ShipType typeOfShip)
        {
            this.OccupiedPoints = new List<Point>();
            this.Type = typeOfShip;

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
        public enum PlaceShipDirection
        {
            Horizontal,
            Vertical
        }

        public Point[,] Ocean { get; set; }
        public List<Ship> ListOfShips { get; set; }
        public bool AllShipsDestroyed { get; }
        public int CombatRound { get; set; }

        public Grid()
        {
            this.Ocean = new  Point[10, 10];

            for (int y = 0; y < Ocean.GetLength(1); y++)
            {
                for (int x = 0; x < Ocean.GetLength(0); x++)
                {
                    Console.WriteLine(Ocean[x, y]);
                }
            }

            this.ListOfShips = new List<Ship>();
            ListOfShips.Add(new Ship(Ship.ShipType.Carrier));
            ListOfShips.Add(new Ship(Ship.ShipType.Battleship));
            ListOfShips.Add(new Ship(Ship.ShipType.Cruiser));
            ListOfShips.Add(new Ship(Ship.ShipType.Submarine));
            ListOfShips.Add(new Ship(Ship.ShipType.Minesweeper));

            PlaceShip(Ship.ShipType.Carrier,PlaceShipDirection.Horizontal, 0, 2 );

            

            
        }

        public void PlaceShip(Ship shipToPlace, PlaceShipDirection direction, int startX, int startY)
        {
            for (int i = 0; i < shipToPlace.Length; i++)
            {
                if (Ocean[startX, startY].Status == Point.PointStatus.Ship)
                {
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
        }
    }
}
