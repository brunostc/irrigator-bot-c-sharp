using System;

namespace IrrigatorBot
{
    class Field
    {
        public static int XValue;
        public static int YValue;
        public static int NumberOfPlants;
        public static string[] PlantsPositions;
        public static string[,] FieldArr;

        public Field(int xValuex, int yValue, int numberOfPlants, string[] plantsPositions)
        {
            XValue = xValuex;
            YValue = yValue;
            NumberOfPlants = numberOfPlants;
            PlantsPositions = plantsPositions;

            Field.CreateFieldArray();
        }

        private static void CreateFieldArray()
        {
            string[,] FieldArray = new string[XValue, YValue];

            foreach (string plant in PlantsPositions)
            {
                string[] positions = plant.Split(',');

                if (int.Parse(positions[0]) <= Field.XValue && int.Parse(positions[1]) <= Field.YValue)
                {
                    int posX = (int.Parse(positions[0]) - 1) >= 0 ? int.Parse(positions[0]) - 1 : 0;
                    int posY = (int.Parse(positions[1]) - 1) >= 0 ? int.Parse(positions[1]) - 1 : 0;

                    FieldArray[posX, posY] = "P";
                } else
                {
                    Console.WriteLine("------");
                    Console.WriteLine("({0}) position was out of bounds and was discarded", plant);
                    Console.WriteLine("------");
                }
            }

            SetFieldArray(FieldArray);
        }

        public string[,] GetFieldArray()
        {
            return FieldArr;
        }

        private static void SetFieldArray(string[,] arr)
        {
            Field.FieldArr = arr;
        }
    }
}
