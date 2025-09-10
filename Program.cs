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
            int currentGuess = 0;
            int amountTries = 1;
            int attempts = 0;
            int target = 0;

            
            //The game loop, quits if gameOngoing is false
            while (gameOngoing)
            {
                //Only run game start effects if we're at game start
                //Sets up the target, amount of tries and prints a greeting
                if (isAtGameStart)
                {
                    Console.WriteLine("Välkommen! Jag tänker på ett nummer. Kan du gissa vilket?");
                    var targetAndAttempts = GenerateDifficulty();

                    target = targetAndAttempts.Item1;
                    attempts = targetAndAttempts.Item2;
                    isAtGameStart = false;
                }

                //Makes sure that the default is that we do not have a valid input
                //Once we have a valid input, evaluates input against target
                //Informs the player if the guess is correct or not 
                //Once game ends, after correct guess or running out of attempts
                //Calls method to ask if player wants to restart game
                //And resets values
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

                            //This code is repeated to many  times and would
                            //benefit from being broken out into a class with methods
                            //A class called GameStart making all central variables
                            //more readily available would make for an easier time 
                            //maintaining without having to repeat code or returning
                            //arrays or tuples and hoping the data is consistently 
                            //structured and that indexes will remain stable and consistent.
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
                            Console.WriteLine(RandomizeResponse(target, currentGuess));
                            if (IsClose(target, currentGuess))
                            {
                                Console.WriteLine("...Men du var rätt nära nu!");
                            }
                            validInput = int.TryParse(Console.ReadLine(), out currentGuess);
                        }
                    }
                    else 
                    {
                        correctGuess = CheckGuess(target, currentGuess);
                        if (correctGuess)
                        {
                            Console.WriteLine("Det var i grevens tid, men du gissade rätt! Snyggt jobbat!");

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
        }

        //Method to evaluate whether a guess is correct or not
        static bool CheckGuess(int targetNumber, int guessedNamber)
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

        //Randomizes the response based on whether the guess is too high or too low
        static string RandomizeResponse(int targetNumber, int guessedNumber)
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
        static (int target, int amountAttempts) GenerateDifficulty()
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
                maxRange = 11;
                amountAttempts = 5;
            }
            else if (difficultySetting == 2)
            {
                difficultyText = "Medelsvår";

                minRange = 1;
                maxRange = 21;
                amountAttempts = 3;
            }
            else
            {
                difficultyText = "Svår";

                minRange = 1;
                maxRange = 51;
                amountAttempts = 3;
            }

            //Reminnder that - 1 must be kept as maxValue of a random is exclusive, and minValue is inclusive
            Console.WriteLine($"Okej, vi kör en {difficultyText} omgång, talet är mellan {minRange} och {maxRange - 1}! Du har {amountAttempts} försök på dig!");

            Random random = new Random();
            int target = random.Next(minRange, maxRange);
            (int, int) targetAndAttempts = (target, amountAttempts);

            return targetAndAttempts;
        }

        //Prompts the player to either restart or exit the game
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
