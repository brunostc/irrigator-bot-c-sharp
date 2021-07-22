using System;

namespace IrrigatorBot
{
    class Robot
    {
        public static int xPosition;
        public static int yPosition;
        public static string Orientation;
        public static string[,] FieldArray = null;
        public static string robotPath = "";
        public static int NumberOfPlants;
        public static int NumberOfPlantsIrrigated = 0;

        public Robot(int x, int y, string orientation, int numberOfPlants)
        {
            xPosition = x;
            yPosition = y;
            Orientation = orientation;
            NumberOfPlants = numberOfPlants;
        }

        public void SetFieldArray(string[,] fieldArray)
        {
            FieldArray = fieldArray;
        }

        public void StartRobot()
        {
            string[,] field = FieldArray;

            int posX = xPosition >= 0 ? xPosition : 0;
            int posY = yPosition >= 0 ? yPosition : 0;

            FieldArray[posX, posY] = "R";

            int rows = FieldArray.GetLength(0);
            int columns = FieldArray.GetLength(1);

            PrintField(rows, columns, FieldArray);

            Console.WriteLine("");

            if (GetHorizontalSizeOfField() == 1 && GetVerticalSizeOfField() == 1)
            {
                TryToIrrigate();
                PrintSuccessMessage();
            }
            else
            {
                SetRobotToFaceLeft();
                SetRobotToTopLeftBorder();

                Console.WriteLine("Robo em ({0},{1})", xPosition, yPosition);
                MoveRobot();
            }
        }

        private static void PrintField(int rows, int columns, string[,] fieldArray)
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    Console.Write("----");
                }

                Console.WriteLine("");

                for (int y = 0; y < columns; y++)
                {
                    Console.Write(" {0} |", FieldArray[x, y] != null ? FieldArray[x, y] : " ");
                }

                Console.WriteLine("");
            }
        }

        private void MoveRobot()
        {
            string robotOrientation = Robot.Orientation.ToUpper();

            if (NumberOfPlantsIrrigated != NumberOfPlants)
            {
                if (robotOrientation == "S" && RobotIsAtTopOfField())
                {
                    MoveSouth();
                }

                if (robotOrientation == "S" && !RobotIsAtTopOfField() && !RobotIsAtBottomOfField())
                {
                    MoveSouth();
                }

                if (robotOrientation == "S" && RobotIsAtBottomOfField())
                {
                    robotPath += " E";

                    Orientation = "L";

                    MoveEast();
                }

                if (robotOrientation == "L" && RobotIsAtBottomOfField())
                {
                    robotPath += " E";

                    Orientation = "N";

                    MoveNorth();
                }

                if (robotOrientation == "N" && !RobotIsAtTopOfField() && !RobotIsAtBottomOfField())
                {
                    MoveNorth();
                }

                if (robotOrientation == "N" && RobotIsAtTopOfField())
                {
                    robotPath += " D";

                    Orientation = "L";

                    MoveEast();
                }

                if (robotOrientation == "L" && RobotIsAtTopOfField())
                {
                    robotPath += " D";

                    Orientation = "S";

                    MoveSouth();
                }
            }

            if (NumberOfPlantsIrrigated == NumberOfPlants)
            {
                PrintSuccessMessage();
            } else
            {
                MoveRobot();
            }
        }

        private void PrintSuccessMessage()
        {
            Console.WriteLine("");
            Console.WriteLine("{0} Plantas irrigadas", NumberOfPlantsIrrigated);
            Console.WriteLine("Terminou de irrigar todas as plantas...");
            Console.WriteLine("Desativando o robô.");
            Console.WriteLine("");
            Console.WriteLine("CAMINHO: {0}", robotPath);
        }

        private Boolean RobotIsAtBottomOfField()
        {
            return xPosition == (FieldArray.GetLength(0) - 1);
        }

        private Boolean RobotIsAtTopOfField()
        {
            return xPosition == 0;
        }

        private static int GetVerticalSizeOfField()
        {
            return FieldArray.GetLength(0);
        }

        private static int GetHorizontalSizeOfField()
        {
            return FieldArray.GetLength(1);
        }

        private void SetRobotToFaceLeft()
        {
            string robotOrientation = Robot.Orientation;

            if (robotOrientation.ToUpper() == "S")
            {
                robotPath += " D";

                Orientation = "O";
            }

            if (robotOrientation.ToUpper() == "N")
            {
                robotPath += " E";

                Orientation = "O";
            }

            if (robotOrientation.ToUpper() == "L")
            {
                robotPath += " E E";

                Orientation = "O";
            }
        }

        private void SetRobotToTopLeftBorder()
        {
            while (yPosition != 0)
            {
                MoveWest();
            }

            Orientation = "N";

            robotPath += " D";

            while (xPosition != 0)
            {
                MoveNorth();
            }

            Orientation = "S";

            robotPath += " D D";
        }

        private void MoveNorth()
        {
            int newXPosition = xPosition - 1;

            xPosition = newXPosition < 0 ? xPosition : newXPosition;

            robotPath += " M";

            TryToIrrigate();
        }

        private void MoveSouth()
        {
            int newXPosition = xPosition + 1;

            xPosition = newXPosition > (GetVerticalSizeOfField()-1) ? xPosition : newXPosition;

            robotPath += " M";

            TryToIrrigate();
        }

        private void MoveWest()
        {
            int newYPosition = yPosition - 1;

            yPosition = newYPosition < 0 ? yPosition : newYPosition;

            robotPath += " M";

            TryToIrrigate();
        }

        private void MoveEast()
        {
            int newYPosition = yPosition + 1;

            yPosition = (newYPosition > GetHorizontalSizeOfField()-1) ? yPosition : newYPosition;

            robotPath += " M";

            TryToIrrigate();
        }

        private void TryToIrrigate()
        {
            if (FieldArray[xPosition, yPosition] == "P")
            {
                Console.WriteLine("Irrigando planta na posição ({0},{1})", xPosition+1, yPosition+1);
                FieldArray[xPosition, yPosition] = "I";
                Console.WriteLine("Planta irrigada");

                NumberOfPlantsIrrigated++;

                robotPath += " I";
            }
        }
    }
}
