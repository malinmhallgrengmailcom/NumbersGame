using System.ComponentModel;

namespace NumbersGame
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            //Settings up all the variables that are needed globally
            bool gameOngoing = true; 
            bool correctGuess = false;
            bool isAtGameStart = true;
            bool validInput = false;
            int amountTries = 1;
            int attempts = 0;
            int target = 0;
            int currentGuess;

            
            //The game loop, quits if gameOngoing is false
            while (gameOngoing)
            {
                //Only run game start effects if we're at game start
                //Sets up the target, amount of tries and prints a greeting
                if (isAtGameStart)
                {
                    Console.WriteLine("Välkommen! Jag tänker på ett nummer. Kan du gissa vilket?");
                    (target, attempts, isAtGameStart) = GenerateDifficulty();
                }

                //Calls MakeGuess(); to let player make a guess
                //Informs the player if the guess is correct or not 
                //Once game ends, after correct guess or running out of attempts
                //Calls ContinuePlaying(); to ask if player wants to restart game
                //which will also resets variables and put them at game start


                //Uncertain whether best practice is to declare these local using var
                //or declaring global and make sure it is clear what these variables are
                (currentGuess, validInput) = MakeGuess();

                while (validInput && !correctGuess)
                {
                    if (validInput && amountTries < attempts)
                    {
                        correctGuess = CheckGuess(target, currentGuess);
                        amountTries++;
                        if (correctGuess)
                        {
                            Console.WriteLine("Snyggt, du gissade rätt!");

                            
                            (amountTries, gameOngoing, isAtGameStart, correctGuess) = ContinuePlaying();
                            break;
                        }
                        else
                        {
                            Console.WriteLine(RandomizeResponse(target, currentGuess));
                            if (IsClose(target, currentGuess))
                            {
                                Console.WriteLine("...Men du var rätt nära nu!");
                            }
                            (currentGuess, validInput) = MakeGuess();
                        }
                    }
                    else 
                    {
                        correctGuess = CheckGuess(target, currentGuess);
                        if (correctGuess)
                        {
                            Console.WriteLine("Det var i grevens tid, men du gissade rätt! Snyggt jobbat!");

                            (amountTries, gameOngoing, isAtGameStart, correctGuess) = ContinuePlaying();
                            break;
                        }

                        else
                        {
                            Console.WriteLine($"Tyvärr, du lyckades inte gissa talet på {attempts} försök!");

                            (amountTries, gameOngoing, isAtGameStart, correctGuess) = ContinuePlaying();
                            break;
                        }
                    } 
                }
            }
        }

        public static (int guess, bool validInput) MakeGuess()
        {
            int currentGuess;
            bool validInput = false;

            do
            {
                validInput = int.TryParse(Console.ReadLine(), out currentGuess);
                if (!validInput)
                {
                    Console.WriteLine("Felaktig inmatning, gissa om!");
                }

            } while (!validInput);

            return (currentGuess, validInput);
        }

        //Method to evaluate whether a guess is correct or not
        public static bool CheckGuess(int targetNumber, int guessedNamber)
        {
            
            if (guessedNamber < targetNumber)
            {
                return false;
            }
            else if (guessedNamber > targetNumber)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Method to evaluate whether a wrong guess was close or not
        public static bool IsClose(int targetNumber, int guessedNumber)
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

        //Randomizes the response based on whether the guess is too high or too low
        public static string RandomizeResponse(int targetNumber, int guessedNumber)
        {
            string[] belowResponses = { "Nej, ditt nummer är för lågt, gissa igen!", "Inte riktigt, mitt nummer är högre, försök igen!", "Bra gissat, men mitt tal är inte så lågt", "Bra jobbat... haha, gotcha! För lågt, gissa igen!", "För låååågt! Prova igen!"};
            string[] aboveResponses = { "Ditt nummer är högre än mitt! Prova igen!", "Nu gissade du för högt, försök igen!", "Njae... inte riktigt, mitt nummer är lägre, försök igen!", "För högt! Testa igen med ett lägre nummer", "Bra gissat, men fel tyvärr, jag tänker på ett lägre tal" };

            Random random = new Random();
            int responseSelection = random.Next(0, 5);

            if (targetNumber < guessedNumber)
            {
                return aboveResponses[responseSelection];
            }
            else
            {
                return belowResponses[responseSelection];
            }
        }


        //Method to generate numbers for game difficulty, attempts and target
        //Informs the player of what their chosen difficulty means
        public static (int target, int amountAttempts, bool isAtGameStart) GenerateDifficulty()
        {
            bool difficultyInput = false;
            int difficultySetting = 0;
            string difficultyText = " ";
            int minRange = 1;
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
                maxRange = 11;
                amountAttempts = 5;
            }
            else if (difficultySetting == 2)
            {
                difficultyText = "Medelsvår";
                maxRange = 21;
                amountAttempts = 3;
            }
            else
            {
                difficultyText = "Svår";
                maxRange = 51;
                amountAttempts = 3;
            }

            Console.Clear();
            Console.WriteLine($"Okej, vi kör en {difficultyText} omgång, talet är mellan {minRange} och {maxRange - 1}! Du har {amountAttempts} försök på dig!\nBörja gissa!");

            Random random = new Random();
            int target = random.Next(minRange, maxRange);

            return (target, amountAttempts, false);
        }

        //Prompts the player to either restart or exit the game
        public static (int resetTries, bool gameOngoing, bool isAtGameStart, bool correctGuess) ContinuePlaying()
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
                Console.Clear();
                return (keepPlaying, true, true, false);
            }
            else 
            {
                Console.Clear();
                Console.WriteLine("Okej, då ses vi nästa gång!");
                return (keepPlaying, false, false, false);
            }
        }
    }
}
