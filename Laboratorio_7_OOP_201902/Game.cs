﻿using Laboratorio_7_OOP_201902.Cards;
using Laboratorio_7_OOP_201902.Enums;
using Laboratorio_7_OOP_201902.Static;
using Laboratorio_7_OOP_201902.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
// colocar los using respectivos para serializacion
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Laboratorio_7_OOP_201902
{
    public class Game
    {
        //Constantes
        private const int DEFAULT_CHANGE_CARDS_NUMBER = 3;

        //Atributos
        private Player[] players;
        private Player activePlayer;
        private List<Deck> decks;
        private List<SpecialCard> captains;
        private Board boardGame;
        internal int turn;
        static List<Deck> statusDecks;
        static Player player1Status;
        static Player player2Status;
        static Board boardGameStatus;
        static int turnStatus;


        //Constructor
        public Game()
        {
            // Check for previous data
            if (!LoadData())
            {
                Console.WriteLine("No previous data");
                Random random = new Random();
                decks = new List<Deck>();
                captains = new List<SpecialCard>();
                AddDecks();
                AddCaptains();
                players = new Player[2] { new Player(), new Player() };
                ActivePlayer = Players[random.Next(2)];
                boardGame = new Board();
                //Add board to players
                players[0].Board = boardGame;
                players[1].Board = boardGame;
                turn = 0;
                statusDecks = decks;
                player1Status = players[0];
                player2Status = players[1];
                boardGameStatus = boardGame;
                turnStatus = turn;
            }
            else
            {
                Console.WriteLine("Game loaded");
                Random random = new Random();
                decks = statusDecks;
                AddDecks();
                AddCaptains();
                players[0] = player1Status;
                players[1] = player2Status;
                ActivePlayer = Players[random.Next(2)];
                boardGame = boardGameStatus;
                //Add board to players
                players[0].Board = boardGame;
                players[1].Board = boardGame;
                turn = turnStatus;
              
            }
            
            
        }
        //Propiedades
        public Player[] Players
        {
            get
            {
                return this.players;
            }
        }
        public Player ActivePlayer
        {
            get
            {
                return this.activePlayer;
            }
            set
            {
                activePlayer = value;
            }
        }
        public List<Deck> Decks
        {
            get
            {
                return this.decks;
            }
        }
        public List<SpecialCard> Captains
        {
            get
            {
                return this.captains;
            }
        }
        public Board BoardGame
        {
            get
            {
                return this.boardGame;
            }
        }


        //Metodos
        public bool CheckIfEndGame()
        {
            if (players[0].LifePoints == 0 || players[1].LifePoints == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetWinner()
        {
            if (players[0].LifePoints == 0 && players[1].LifePoints > 0)
            {
                return 1;
            }
            else if (players[1].LifePoints == 0 && players[0].LifePoints > 0)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        public int GetRoundWinner()
        {
            if (Players[0].GetAttackPoints()[0] == Players[1].GetAttackPoints()[0])
            {
                return -1;
            }
            else
            {
                int winner = Players[0].GetAttackPoints()[0] > Players[1].GetAttackPoints()[0] ? 0 : 1;
                return winner;
            }
        }
        public void Play()
        {
            int userInput = 0;
            int firstOrSecondUser = ActivePlayer.Id == 0 ? 0 : 1;
            int winner = -1;
            bool bothPlayersPlayed = false;
            

            while (turn < 4 && !CheckIfEndGame())
            {
                bool drawCard = false;
                //turno 0 o configuracion
                if (turn == 0)
                {
                    for (int _ = 0; _<Players.Length; _++)
                    {
                        ActivePlayer = Players[firstOrSecondUser];
                        Visualization.ClearConsole();
                        //Mostrar mensaje de inicio
                        Visualization.ShowProgramMessage($"Player {ActivePlayer.Id+1} select Deck and Captain:");
                        //Preguntar por deck
                        Visualization.ShowDecks(this.Decks);
                        userInput = Visualization.GetUserInput(this.Decks.Count - 1);
                        Deck deck = new Deck();
                        deck.Cards = new List<Card>(Decks[userInput].Cards);
                        ActivePlayer.Deck = deck;
                        // mostrar cartas del mazo

                        List<string> ab1 = deck.GetCharacteristics();
                        // no coloco los console write ya que ya estan en la funcion

                        //Preguntar por capitan
                        Visualization.ShowCaptains(Captains);
                        userInput = Visualization.GetUserInput(this.Captains.Count - 1);
                        ActivePlayer.ChooseCaptainCard(new SpecialCard(Captains[userInput].Name, Captains[userInput].Type, Captains[userInput].Effect));

                        // mostrar info del capi

                        List<string> ab2 = ActivePlayer.Captain.GetCharacteristics();
                        //  no coloco los console write ya que ya estan en la funcion

                        //Asignar mano
                        ActivePlayer.FirstHand();
                        //Mostrar mano
                        Visualization.ShowHand(ActivePlayer.Hand);
                        //Mostar opciones, cambiar carta o pasar
                        Visualization.ShowListOptions(new List<string>() { "Change Card", "Pass" }, "Change 3 cards or ready to play:");
                        userInput = Visualization.GetUserInput(1);
                        if (userInput == 0)
                        {
                            Visualization.ClearConsole();
                            Visualization.ShowProgramMessage($"Player {ActivePlayer.Id+1} change cards:");
                            Visualization.ShowHand(ActivePlayer.Hand);
                            for (int i = 0; i < DEFAULT_CHANGE_CARDS_NUMBER; i++)
                            {
                                Visualization.ShowProgramMessage($"Input the numbers of the cards to change (max {DEFAULT_CHANGE_CARDS_NUMBER}). To stop enter -1");
                                userInput = Visualization.GetUserInput(ActivePlayer.Hand.Cards.Count, true);
                                if (userInput == -1) break;
                                ActivePlayer.ChangeCard(userInput);
                                Visualization.ShowHand(ActivePlayer.Hand);
                            }
                        }
                        firstOrSecondUser = ActivePlayer.Id == 0 ? 1 : 0;
                    }
                    // guardar turno 0
                    SaveData();

                    turn += 1;
                }
                //turnos siguientes
                else
                {
                    while (true)
                    {
                        ActivePlayer = Players[firstOrSecondUser];
                        //Obtener lifePoints
                        int[] lifePoints = new int[2] { Players[0].LifePoints, Players[1].LifePoints };
                        //Obtener total de ataque:
                        int[] attackPoints = new int[2] { Players[0].GetAttackPoints()[0], Players[1].GetAttackPoints()[0] };
                        //Mostrar tablero, mano y solicitar jugada
                        Visualization.ClearConsole();
                        Visualization.ShowBoard(boardGame, ActivePlayer.Id, lifePoints,attackPoints);
                        //Robar carta
                        if (!drawCard)
                        {
                            ActivePlayer.DrawCard();
                            drawCard = true;
                        }
                        Visualization.ShowHand(ActivePlayer.Hand);
                        Visualization.ShowListOptions(new List<string> { "Play Card", "Pass" }, $"Make your move player {ActivePlayer.Id+1}:");
                        userInput = Visualization.GetUserInput(1);
                        if (userInput == 0)
                        {
                            //Si la carta es un buff solicitar a la fila que va.
                            Visualization.ShowProgramMessage($"Input the number of the card to play. To cancel enter -1");
                            userInput = Visualization.GetUserInput(ActivePlayer.Hand.Cards.Count, true);
                            if (userInput != -1)
                            {
                                if (ActivePlayer.Hand.Cards[userInput].Type == EnumType.buff)
                                {
                                    Visualization.ShowListOptions(new List<string> { "Melee", "Range", "LongRange" }, $"Choose row to buff {ActivePlayer.Id}:");
                                    int cardId = userInput;
                                    userInput = Visualization.GetUserInput(2);
                                    if (userInput == 0)
                                    {
                                        ActivePlayer.PlayCard(cardId, EnumType.buffmelee);
                                    }
                                    else if (userInput == 1)
                                    {
                                        ActivePlayer.PlayCard(cardId, EnumType.buffrange);
                                    }
                                    else
                                    {
                                        ActivePlayer.PlayCard(cardId, EnumType.bufflongRange);
                                    }
                                }
                                else
                                {
                                    ActivePlayer.PlayCard(userInput);
                                }
                            }
                            //Revisar si le quedan cartas, si no le quedan obligar a pasar.
                            if (ActivePlayer.Hand.Cards.Count == 0)
                            {
                                firstOrSecondUser = ActivePlayer.Id == 0 ? 1 : 0;
                                // guardar antes de pasar
                                SaveData();
                                break;
                            }
                        }
                        else
                        {
                            firstOrSecondUser = ActivePlayer.Id == 0 ? 1 : 0;
                            // guardar antes de pasar
                            SaveData();

                            break;
                        }
                    }
                    //Cambiar al oponente si no ha jugado
                    if (!bothPlayersPlayed)
                    {
                        bothPlayersPlayed = true;
                        drawCard = false;
                    }
                    //Si ambos jugaron obtener el ganador de la ronda, reiniciar el tablero y pasar de turno.
                    else
                    {
                        winner = GetRoundWinner();
                        if (winner == 0)
                        {
                            Players[1].LifePoints -= 1;
                        }
                        else if (winner == 1)
                        {
                            Players[0].LifePoints -= 1;
                        }
                        else
                        {
                            Players[0].LifePoints -= 1;
                            Players[1].LifePoints -= 1;
                        }
                        bothPlayersPlayed = false;
                        turn += 1;
                        //Destruir Cartas
                        BoardGame.DestroyCards();
                    }
                }
            }
            //Definir cual es el ganador.
            winner = GetWinner();
            if (winner == 0)
            {
                Visualization.ShowProgramMessage($"Player 1 is the winner!");
            }
            else if (winner == 1)
            {
                Visualization.ShowProgramMessage($"Player 2 is the winner!");
            }
            else
            {
                Visualization.ShowProgramMessage($"Draw!");
            }

        }
        public void AddDecks()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Decks.txt";
            StreamReader reader = new StreamReader(path);
            int deckCounter = 0;
            List<Card> cards = new List<Card>();


            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string [] cardDetails = line.Split(",");

                if (cardDetails[0] == "END")
                {
                    decks[deckCounter].Cards = new List<Card>(cards);
                    deckCounter += 1;
                }
                else
                {
                    if (cardDetails[0] != "START")
                    {
                        if (cardDetails[0] == nameof(CombatCard))
                        {
                            cards.Add(new CombatCard(cardDetails[1], (EnumType) Enum.Parse(typeof(EnumType),cardDetails[2]), cardDetails[3], Int32.Parse(cardDetails[4]), bool.Parse(cardDetails[5])));
                        }
                        else
                        {
                            cards.Add(new SpecialCard(cardDetails[1], (EnumType)Enum.Parse(typeof(EnumType), cardDetails[2]), cardDetails[3]));
                        }
                    }
                    else
                    {
                        decks.Add(new Deck());
                        cards = new List<Card>();
                    }
                }

            }
            
        }
        public void AddCaptains()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Captains.txt";
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] cardDetails = line.Split(",");
                captains.Add(new SpecialCard(cardDetails[1], (EnumType)Enum.Parse(typeof(EnumType), cardDetails[2]), cardDetails[3]));
            }
        }
        // metodos para guardar
        private static void SaveDataDecks()
        {
            // Creamos el Stream donde guardaremos nuestro estado de juego
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../DeckStatus.txt");
            FileStream fs = new FileStream(fileName, FileMode.Create);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, statusDecks);
            fs.Close();
        }
        private static void SaveDataPlayer1()
        {
            // Creamos el Stream donde guardaremos nuestro estado de juego
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Player1Status.txt");
            FileStream fs = new FileStream(fileName, FileMode.Create);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, player1Status);
            fs.Close();
        }
        private static void SaveDataPlayer2()
        {
            // Creamos el Stream donde guardaremos nuestro estado de juego
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Player2Status.txt");
            FileStream fs = new FileStream(fileName, FileMode.Create);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, player2Status);
            fs.Close();
        }
        private static void SaveDataBoardGame()
        {
            // Creamos el Stream donde guardaremos nuestro estado de juego
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../BoardGameStatus.txt");
            FileStream fs = new FileStream(fileName, FileMode.Create);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, boardGameStatus);
            fs.Close();
        }
        private static void SaveDataTurn()
        {
            // Creamos el Stream donde guardaremos nuestro estado de juego
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../TurnStatus.txt");
            FileStream fs = new FileStream(fileName, FileMode.Create);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, turnStatus);
            fs.Close();
        }

        private static void SaveData()
        {
            SaveDataDecks();
            SaveDataPlayer1();
            SaveDataPlayer2();
            SaveDataBoardGame();
            SaveDataTurn();
        }
        
        // metodo para cargar
        private static Boolean LoadDataDecks()
        {
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../DeckStatus.txt");
            if (!File.Exists(fileName))
            {
                return false;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);
            IFormatter formatter = new BinaryFormatter();
            statusDecks = formatter.Deserialize(fs) as List<Deck>;
            fs.Close();
            return true;
        }
        private static Boolean LoadDataPlayer1()
        {
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Player1Status.txt");
            if (!File.Exists(fileName))
            {
                return false;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);
            IFormatter formatter = new BinaryFormatter();
            player1Status = formatter.Deserialize(fs) as Player;
            fs.Close();
            return true;
        }
        private static Boolean LoadDataPlayer2()
        {
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Player2Status.txt");
            if (!File.Exists(fileName))
            {
                return false;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);
            IFormatter formatter = new BinaryFormatter();
            player2Status = formatter.Deserialize(fs) as Player;
            fs.Close();
            return true;
        }
        private static Boolean LoadDataBoardGame()
        {
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../BoardGameStatus.txt");
            if (!File.Exists(fileName))
            {
                return false;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);
            IFormatter formatter = new BinaryFormatter();
            boardGameStatus = formatter.Deserialize(fs) as Board;
            fs.Close();
            return true;
        }
        private static Boolean LoadData()
        {
            LoadDataDecks();
            LoadDataPlayer1();
            LoadDataPlayer2();
            LoadDataBoardGame();
        }
    }
}
