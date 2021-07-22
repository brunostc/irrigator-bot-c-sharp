using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IrrigatorBot
{
    class Program
    {
        static void Main(string[] args)
        {
            int xAxisValue = 0;
            int yAxisValue = 0;

            int robotX = 0;
            int robotY = 0;

            string robotOrientation = "";

            Boolean hasSubmittedPositions = false;

            String[] sanitizedStrings = null;

            Console.WriteLine("Configurando robô...");
            Console.WriteLine("");

            string input = "";

            while (xAxisValue == 0 && yAxisValue == 0)
            {
                Console.WriteLine("Por favor insira um tamanho para o campo no formato correto (x,y) (restrição: x,y >= 1)");
                Console.WriteLine("");

                Regex regex = new Regex("[^0-9 ,]");

                input = Console.ReadLine();

                Console.WriteLine($"Você inseriu: {input}");
                Console.WriteLine("");

                input = regex.Replace(input, "");

                string[] sizes = input.Split(',');

                if (sizes.Length == 2 && int.Parse(sizes[0]) > 0 && int.Parse(sizes[1]) > 0)
                {
                    xAxisValue = int.Parse(sizes[0]);
                    yAxisValue = int.Parse(sizes[1]);
                }
                else
                {
                    Console.WriteLine("Formato invalido, por favor tente novamente..");
                    Console.WriteLine("");
                }
            }

            while (robotX == 0 && robotY == 0)
            { 
                Console.WriteLine("Por favor insira a posicao inicial do robo no formato correto (x,y) (restrição: x,y >= 1)");
                Console.WriteLine("");

                Regex regex = new Regex("[^0-9 ,]");

                input = Console.ReadLine();

                Console.WriteLine($"Você inseriu: {input}");
                Console.WriteLine("");

                input = regex.Replace(input, "");

                string[] positions = input.Split(',');

                if (positions.Length == 2 && int.Parse(positions[0]) > 0 && int.Parse(positions[1]) > 0)
                {
                    if (int.Parse(positions[0]) <= xAxisValue || int.Parse(positions[1]) <= yAxisValue)
                    {
                        robotX = int.Parse(positions[0]);
                        robotY = int.Parse(positions[1]);
                    } else
                    {
                        Console.WriteLine("Posição do robo está fora dos limites, por favor tente novamente!");
                        Console.WriteLine("");
                    }
                } else
                {
                    Console.WriteLine("Formato invalido, por favor tente novamente..");
                    Console.WriteLine("");
                }
            }

            robotX = robotX - 1;
            robotY = robotY - 1;

            while (robotOrientation == "")
            {
                Console.WriteLine("Por favor insira a orientação inicial do robo (N, S, L, O)");
                Console.WriteLine("");

                input = Console.ReadLine();

                Boolean validated = Program.ValidateOrientation(input);

                if (validated == true)
                {
                    robotOrientation = input;
                } else
                {
                    Console.WriteLine("Formato invalido, por favor tente novamente..");
                    Console.WriteLine("");
                }
            }

            while (!hasSubmittedPositions)
            {
                Console.WriteLine("Por favor insira as posições das plantas.");
                Console.WriteLine("Para inserir uma posução de planta você insere or argumentos de posição separados por virgula.");
                Console.WriteLine("Exemplo: '(1,3),(3,8),(5,1)' sem as aspas simples.");
                Console.WriteLine("Se você inserir posições no formato incorreto elas serão descartadas.");
                Console.WriteLine("");

                Regex regex = new Regex("[^0-9 ,()]");

                input = Console.ReadLine();

                Console.WriteLine($"Você inseriu: {input}");
                Console.WriteLine("");

                input = regex.Replace(input, "");

                sanitizedStrings = Program.SanitizePositionString(input);

                if (sanitizedStrings.Length > 0)
                {
                    hasSubmittedPositions = true;
                } else
                {
                    Console.WriteLine("Formato invalido, por favor tente novamente..");
                    Console.WriteLine("");
                }
            }

            Robot robot = new Robot(robotX, robotY, robotOrientation, sanitizedStrings.Length);
            Field field = new Field(xAxisValue, yAxisValue, sanitizedStrings.Length, sanitizedStrings);

            robot.SetFieldArray(field.GetFieldArray());
            robot.StartRobot();
        }

        private static string[] SanitizePositionString(string input)
        {
            String[] positions = input.Split(')');

            List<string> positionsArray = new List<string>();

            foreach (string position in positions)
            {
                string[] positionsAgain = position.Split('(');

                foreach (string positionAgain in positionsAgain)
                {
                    if (positionAgain != ",")
                    {
                        positionsArray.Add(positionAgain);
                    }
                }
            }

            positionsArray.RemoveAll(item => (item == null || item == ""));

            return positionsArray.ToArray();
        }

        private static Boolean ValidateOrientation(string input)
        {
            if (input == "N" || input == "n" || input == "S" || input == "s" || input == "L" || input == "l" || input == "O" || input == "o")
            {
                return true;
            }

            return false;
        }
    }
}
