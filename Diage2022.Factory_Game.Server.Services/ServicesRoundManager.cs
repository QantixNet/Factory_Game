using Diiage2022.Factory_Games.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Diage2022.Factory_Game.Server.Services
{
    public partial class Services
    {
        /// <summary>
        /// Initialize A new turn:
        /// Shuffle the List of the Player
        /// Generate the developers, the projects, the training sessions
        /// </summary>
        /// <returns>
        /// Return the list of Id of the player shuffled 
        /// </returns>
        public List<int> StartTurn()
        {
            try
            {
                Game.Round += 1;
                Game.PlayerHaveToPlay = Game.Companies.Count;
                List<Company> companiesToShuffle = Game.Companies.ToList();
                List<Company> ShuffledPlayerList = ShuffleList(companiesToShuffle);
                if(Game.Round>=2)
                {
                    GenerateDevelopers(3, 3000);
                    GenerateProjects(3);
                    GenerateSchoolTrainingSessions();
                }
                return ShuffledPlayerList.Select(p=> p.CompanyId).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                List<int> error = new List<int> { -1 };
                return error;
            }
        }
        public List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
        /// <summary>
        /// Initialize send to a player that it's his turn
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="tcpClients"></param>
        /// <returns></returns>
        public int SetPlayerTurn(int playerId, List<TcpClient> tcpClients)
        {
            Game.CurrentPlayer = playerId;
            Communication communication = new Communication();
            communication.RequestType = 9;//Set Player Turn
            communication.PlayerId = playerId;
            SendToAll(communication, tcpClients);
            return 1;
        }
        /// <summary>
        /// Update the Funds of all the company by substracting all developers salary
        /// </summary>
        /// <returns></returns>
        public int UpdatingCompanies()
        {
            try
            {
                Game.Companies.ForEach(c=>
                {
                    double salarySpending = c.Developers.Sum(d=> d.DeveloperSalary);
                    c.Funds -= salarySpending;
                }
                );
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }
        /// <summary>
        /// Finish the turn of a player
        /// if all the player have played update the training sessions, projects
        /// </summary>
        /// <returns></returns>
        public int FinishTurn()
        {
            try
            {
                if (Thread.CurrentThread.ManagedThreadId == Game.CurrentPlayer)
                {
                    Game.PlayerHaveToPlay -= 1;
                    
                    TurnFinished = true;
                    if (Game.PlayerHaveToPlay <= 0)
                    {
                        UpdateTrainingSessions();
                        UpdatingProject();
                        UpdatingCompanies();
                        if(Game.Round==Game.RoundMax)
                        {

                            return 0;
                        }
                        return 0;
                    }
                    else
                        return 1;                    
                }
                else
                    return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }
    }
}