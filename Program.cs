using System;
using System.IO;

class Program
{
    static string[] lines;

    static void Main()
    {
        // Read all lines from the CSV file into the lines array
        string filePath = "input.csv";        
        lines = File.ReadAllLines(filePath);

        // Display menu and handle user input
        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Add Character");
            Console.WriteLine("3. Level Up Character");
            Console.WriteLine("4. Exit");
            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllCharacters(lines);
                    break;
                case "2":
                    AddCharacter(ref lines);
                    break;
                case "3":
                    LevelUpCharacter(lines);
                    break;
                case "4":
                    Console.WriteLine("Closing application...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllCharacters(string[] lines)
    {
        // Skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            string name;
            int commaIndex;

            // Check if the name is quoted
            if (line.StartsWith("\""))
            {
                // Find the closing quote
                int closingQuoteIndex = line.IndexOf("\"", 1);
                // Get the name inside quotes
                name = line.Substring(1, closingQuoteIndex - 1); 

                // Find the comma after the closing quote
                commaIndex = line.IndexOf(",", closingQuoteIndex);
                // Get the rest of the fields after the name
                string remainingFields = line.Substring(commaIndex + 1);
                string[] fields = remainingFields.Split(',');

                // Parse the rest
                string characterClass = fields[0];
                int level = int.Parse(fields[1]);
                int hitPoints = int.Parse(fields[2]);
                string[] equipment = fields[3].Split('|');

                // Display character information
                Console.WriteLine();
                Console.WriteLine($"Name: {name}");
                Console.WriteLine($"Class: {characterClass}");
                Console.WriteLine($"Level: {level}");
                Console.WriteLine($"HP: {hitPoints}");
                Console.WriteLine($"Equipment: ");
                foreach (var eq in equipment)
                {
                    Console.WriteLine($"\t{eq}");
                }

                Console.WriteLine("\n----------------------------");
            }
            else
            {
                // Name is not quoted, so up to the first comma
                commaIndex = line.IndexOf(",");
                name = line.Substring(0, commaIndex);

                // Get the rest of the fields after the name
                string remainingFields = line.Substring(commaIndex + 1);
                string[] fields = remainingFields.Split(',');

                // Parse the rest
                string characterClass = fields[0];
                int level = int.Parse(fields[1]);
                int hitPoints = int.Parse(fields[2]);
                string[] equipment = fields[3].Split('|');

                // Display character information
                Console.WriteLine();
                Console.WriteLine($"Name: {name}");
                Console.WriteLine($"Class: {characterClass}");
                Console.WriteLine($"Level: {level}");
                Console.WriteLine($"HP: {hitPoints}");
                Console.WriteLine($"Equipment: ");
                foreach (var eq in equipment)
                {
                    Console.WriteLine($"\t{eq}");
                }

                Console.WriteLine("\n----------------------------");
            }
        }
    }

    static void AddCharacter(ref string[] lines)
    {
        // Get user input for the new character
        Console.Write("Enter your character's name: ");
        string name = Console.ReadLine();

        Console.Write("Enter your character's class: ");
        string characterClass = Console.ReadLine();

        Console.Write("Enter your character's level: ");
        int level = int.Parse(Console.ReadLine());

        Console.Write("Enter your character's hit points: ");
        int hitPoints = int.Parse(Console.ReadLine());

        string[] equipment = new string[3];
        for (int i = 0; i < equipment.Length; i++)
        {
            Console.Write($"Enter equipment item {i + 1}: ");
            equipment[i] = Console.ReadLine().Trim();
        }

        // Add quotes to the name if it contains a comma
        if (name.Contains(","))
        {
            name = $"\"{name}\"";
        }

        // Format equipment as a single string separated by '|'
        string formatEquipment = string.Join("|", equipment);

        // Create the new character line
        string userEnteredData = ($"{name},{characterClass},{level},{hitPoints},{formatEquipment}");

        // Increase the size of the array and add the new character to the end
        Array.Resize(ref lines, lines.Length + 1);
        lines[lines.Length - 1] = userEnteredData;

        // Save the updated array back to the file
        using (StreamWriter writer = new StreamWriter("input.csv"))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }

        // Refresh the lines array to include the new character
        lines = File.ReadAllLines("input.csv");

        Console.WriteLine("\nCharacter added successfully!");
        Console.WriteLine();

    }

    static void LevelUpCharacter(string[] lines)
    {
        Console.Write("Enter the name of the character to level up: ");
        string nameToLevelUp = Console.ReadLine();

        // Loop through characters to find the one to level up, skipping the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            // Check if the name matches the one to level up
            if (line.Contains(nameToLevelUp))
            {
                string name;
                int commaIndex;

                // Check if the name is quoted
                if (line.StartsWith("\""))
                {
                    // Find the closing quote
                    int closingQuoteIndex = line.IndexOf("\"", 1);
                    // Get the name inside quotes
                    name = line.Substring(1, closingQuoteIndex - 1);

                    // Find the comma after the closing quote
                    commaIndex = line.IndexOf(",", closingQuoteIndex);
                    // Get the rest of the fields after the name
                    string remainingFields = line.Substring(commaIndex + 1);
                    string[] fields = remainingFields.Split(',');

                    // Increase the level by 1
                    int level = int.Parse(fields[1]);
                    level++;
                    fields[1] = level.ToString();

                    // Rebuild the line with the updated level
                    string formatEquipment = fields[3];
                    lines[i] = ($"\"{name}\",{fields[0]},{fields[1]},{fields[2]},{formatEquipment}");

                    Console.WriteLine($"\n{name} leveled up to {level}!\n");
                    break;
                }
                else
                {
                    // Name is not quoted, so up to the first comma
                    commaIndex = line.IndexOf(",");
                    name = line.Substring(0, commaIndex);

                    // Get the rest of the fields after the name
                    string remainingFields = line.Substring(commaIndex + 1);
                    string[] fields = remainingFields.Split(',');

                    // Increase the level by 1
                    int level = int.Parse(fields[1]);
                    level++;
                    fields[1] = level.ToString();

                    // Rebuild the line with the updated level
                    string formatEquipment = fields[3];
                    lines[i] = ($"{name},{fields[0]},{fields[1]},{fields[2]},{formatEquipment}");

                    Console.WriteLine($"\n{name} leveled up to {level}!\n");
                    break;
                }
            }           
        }
        // Save the updated array back to the file
        using (StreamWriter writer = new StreamWriter("input.csv"))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }

}