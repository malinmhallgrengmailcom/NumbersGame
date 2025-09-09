namespace NumbersGame
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            bool gameOngoing = true; 
            bool correctGuess = false;
            bool isAtGameStart = true;
            int currentGuess = 0;
            int amountTries = 1;
            int attempts = 0;
            int target = 0;

            

            while (gameOngoing)
            {
                if (isAtGameStart)
                {
                    Console.WriteLine("Välkommen! Jag tänker på ett nummer. Kan du gissa vilket?");
                    var targetAndAttempts = GenerateRange();

                    target = targetAndAttempts.Item1;
                    attempts = targetAndAttempts.Item2;
                    isAtGameStart = false;
                }


                bool validInput = false;

                while (!validInput)
                {
                    validInput = int.TryParse(Console.ReadLine(), out int guessInput);
                    currentGuess = guessInput;
                }

                while (validInput && !correctGuess)
                {
                    if (validInput && amountTries < attempts)
                    {
                        correctGuess = CheckGuess(target, currentGuess);
                        amountTries++;
                        if (correctGuess)
                        {
                            Console.WriteLine("Snyggt, du gissade rätt!");

                            if (ContinuePlaying())
                            {
                                amountTries = 1;
                                gameOngoing = true;
                                isAtGameStart = true;
                                correctGuess = false;
                                break;
                            }
                            else
                            {
                                gameOngoing = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nope, försök igen!");
                            validInput = int.TryParse(Console.ReadLine(), out currentGuess);
                        }
                    }
                    else 
                    {
                        Console.WriteLine("Tyvärr, du lyckades inte gissa talet på fem försök!");
                        if (ContinuePlaying())
                        {
                            amountTries = 1;
                            gameOngoing = true;
                            isAtGameStart = true;
                            correctGuess = false;
                            break;
                        }
                        else
                        {
                            gameOngoing = false;
                            break;
                        }
                    } 
                }
            }
        }

        static bool CheckGuess(int targetNumber, int guessedNamber)
        {
            
            if (guessedNamber < targetNumber)
            {
                //return "Ditt number är lite för lågt";
                return false;
            }
            else if (guessedNamber > targetNumber)
            {
                //return "Ditt nummer är lite för högt";
                return false;
            }
            else
            {
                //return "Woohoo! Du klarade det!";
                return true;
            }
        }

        static bool IsClose(int targetNumber, int guessedNumber)
        {
            if (targetNumber - guessedNumber <= 2 && targetNumber - guessedNumber >= -2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static (int target, int amountAttempts) GenerateRange()
        {
            bool difficultyInput = false;
            int difficultySetting = 0;
            string difficultyText = " ";
            int minRange = 0;
            int maxRange = 0;
            int amountAttempts = 0;

            Console.WriteLine("Välj först svårighetsgrad:\n [1] Lätt\n [2] Medel\n [3] Svårt");

            while (!difficultyInput)
            {
                difficultyInput = int.TryParse(Console.ReadLine(), out difficultySetting);

                if(!difficultyInput || (difficultySetting != 1 && difficultySetting != 2 && difficultySetting != 3))
                {
                    Console.WriteLine("Felaktig inmatning, försök igen");
                }
            }

            if (difficultySetting == 1)
            {
                difficultyText = "Lätt";

                minRange = 1;
                maxRange = 10;
                amountAttempts = 5;
            }
            else if (difficultySetting == 2)
            {
                difficultyText = "Medelsvår";

                minRange = 1;
                maxRange = 20;
                amountAttempts = 3;
            }
            else
            {
                difficultyText = "Svår";

                minRange = 1;
                maxRange = 50;
                amountAttempts = 3;
            }

            Console.WriteLine($"Okej, vi kör en {difficultyText} omgång, talet är mellan {minRange} och {maxRange}! Du har {amountAttempts} försök på dig!");

            Random random = new Random();
            int target = random.Next(minRange, maxRange);
            (int, int) targetAndAttempts = (target, amountAttempts);

            return targetAndAttempts;
        }

        static bool ContinuePlaying()
        {
            bool inputGiven = false;
            int keepPlaying = 0;

            Console.WriteLine("Vill du fortsätta spela?\n[1] Ja\n[2] Nej");

            while (!inputGiven)
            {
                inputGiven = int.TryParse(Console.ReadLine(), out keepPlaying);

                if (!inputGiven || (keepPlaying != 1 && keepPlaying != 2))
                {
                    Console.WriteLine("Felaktig inmatning, försök igen");
                }
            }
            
            if (keepPlaying == 1)
            {
                return true;
            }
            else 
            { 
                Console.WriteLine("Okej, då ses vi nästa gång!");
                return false;
            }
        }
    }
}
