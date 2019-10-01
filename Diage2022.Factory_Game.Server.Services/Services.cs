using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Diiage2022.Factory_Games.Entities;
using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace Diage2022.Factory_Game.Server.Services
{
    public partial class Services
    {
        private const int CREATE_PLAYER = 0;
        private const int CHOOSE_DEVELOPER = 1;
        private const int CHOOSE_TRAINING_SESSION = 2;
        private const int CHOOSE_PROJECT = 3;
        private const int ASSOCIATE_DEV_PROJECT = 4;
        private const int ANALYSE_MARKET = 5;
        private const int FINISH_TURN = 6;
        private const int FIRE_DEVELOPPER = 7;
        private const int GAME_START = 8;
        private const int PLAYER_TURN = 9;
        private const int ADMIN_START_GAME = 10;
        Game Game;
        public bool TurnFinished { set; get; }
        public List<string> developersNames;
        public List<string> developersImages;
        public List<string> projectsNames;
        public List<string> projectsImages;
        public List<string> schoolsNames;
        public List<string> schoolsImages;
        public List<string> skillsNames;
        public List<string> skillsImages;
        public bool adminStartGame = false;

        public Services()
        {
            LoadData();
        }

        public void LoadData()
        {
            var pathDevelopersNames = Path.Combine(Directory.GetCurrentDirectory(), "Data/developersNames.json");
            FillList(out developersNames, pathDevelopersNames);
            var pathDevelopersImages = Path.Combine(Directory.GetCurrentDirectory(), "Data/developersImages.json");
            FillList(out developersImages, pathDevelopersImages);


            var pathProjectsNames = Path.Combine(Directory.GetCurrentDirectory(), "Data/projectsNames.json");
            FillList(out projectsNames, pathProjectsNames);
            var pathProjectsImages = Path.Combine(Directory.GetCurrentDirectory(), "Data/projectsImages.json");
            FillList(out projectsImages, pathProjectsImages);

            var pathSchoolsNames = Path.Combine(Directory.GetCurrentDirectory(), "Data/schoolsNames.json");
            FillList(out schoolsNames, pathSchoolsNames);
            var pathSchoolsImages = Path.Combine(Directory.GetCurrentDirectory(), "Data/schoolsImages.json");
            FillList(out schoolsImages, pathSchoolsImages);

            var pathSkillsNames = Path.Combine(Directory.GetCurrentDirectory(), "Data/skillsNames.json");
            FillList(out skillsNames, pathSkillsNames);
            var pathSkillsImages = Path.Combine(Directory.GetCurrentDirectory(), "Data/skillsImages.json");
            FillList(out skillsImages, pathSkillsImages);

        }

        public void FillList(out List<string> list, string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<List<string>>(json);
            }
        }


        /// <summary>
        /// Initialize a Game Object which contains all the informations for the game
        /// </summary>
        /// <param name="gamedifficulty"></param>
        /// <param name="maxround"></param>
        public int CreateGame(int gamedifficulty, int maxround, double treasuryStart)
        {
            try
            {
                Game = new Game(gamedifficulty, maxround, treasuryStart);
                GenerateSkills();
     
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }
        /// <summary>
        /// Send the Datas of the game to all the players
        /// </summary>
        /// <param name="tcpClients"></param>
        /// <returns></returns>
        public int GameInitialization(List<TcpClient> tcpClients)
        {
            try
            {
                Game.Round = 0;
                Game.Companies.ForEach(c => c.Funds = Game.GameTreasuryStart);
                Communication communication = new Communication();
                communication.Developers = Game.Developers;
                communication.Companies = Game.Companies;
                communication.RequestType = 8;
                SendToAll(communication, tcpClients);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }

        /// <summary>
        /// Create an object Company which correspond to a player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="idcompany"></param>
        public int AddCompany(string username, int idcompany)
        {
            try
            {
                Company company = new Company();
                company.Username = username;
                company.CompanyId = idcompany;
                company.Funds = Game.GameTreasuryStart;
                Game.Companies.Add(company);
                return Game.Companies.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }
        /// <summary>
        /// This method send the object communication to all the clients connected
        /// </summary>
        /// <param name="communication"></param>
        /// <param name="clients"></param>
        public int SendToAll(Communication communication, List<TcpClient> clients)
        {
            try
            {
                communication.Developers = Game.Developers;
                communication.Projects = Game.Projects;
                communication.Companies = Game.Companies;
                string query = JsonConvert.SerializeObject(communication);
                byte[] response = Encoding.UTF8.GetBytes(query);

                clients.ForEach(c => c.GetStream().Write(response, 0, response.Length));

                //Console.WriteLine("Sent to {0} client(s) : {1}", clients.Count, communication);
                Console.WriteLine(communication.PlayerId + " " + communication.NamePlayer + " " + communication.RequestType);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }
        public int SendToOneClient(TcpClient tcpClient, Communication communication)
        {
            try
            {
                communication.PlayerId = Thread.CurrentThread.ManagedThreadId;
                string query = JsonConvert.SerializeObject(communication);
                byte[] response = Encoding.UTF8.GetBytes(query);
                tcpClient.GetStream().Write(response, 0, response.Length);
                Console.WriteLine(communication.PlayerId + " " + communication.NamePlayer);
                //Console.WriteLine(query);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }

        /// <summary>
        /// Analyse the datas sended by the clients and call the appropriate methods
        /// </summary>
        /// <param name="communication"></param>
        /// <returns></returns>
        public int AnalyseQuery(Communication communication)
        {
            try
            {
                switch (communication.RequestType)
                {
                    case CREATE_PLAYER:// Create the player
                        int result = AddCompany(communication.NamePlayer, Thread.CurrentThread.ManagedThreadId);
                        if (result != -1)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    case CHOOSE_DEVELOPER:// Choose a dev
                        result = AddDeveloperToCompany(communication.DeveloperId, communication.PlayerId);
                        if(result!=-1)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }
                        
                    case CHOOSE_TRAINING_SESSION:// Choose a training Session
                        result = AddDeveloperToTrainingSession(communication.PlayerId, communication.DeveloperId, communication.SchoolID, communication.TrainingSessionId);
                        if (result != -1)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }

                    case CHOOSE_PROJECT:// Choose a project
                        result = AddProjectToCompany(communication.ProjectId, communication.PlayerId);
                        if (result != -1)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }

                    case ASSOCIATE_DEV_PROJECT:// Associate dev and project
                        result = AddDeveloperToProject(communication.DeveloperId, communication.ProjectId, communication.PlayerId);
                        if (result != -1)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }

                    case ANALYSE_MARKET:// Analyse market
                        return 0;
                    case FINISH_TURN:// Finish Turn
                        FinishTurn();
                        return 0;
                    case FIRE_DEVELOPPER:
                        result = FireDeveloper(communication.DeveloperId, communication.PlayerId);
                        if (result != -1)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }
                    case ADMIN_START_GAME:
                        AdminStartGame(communication);
                        return 12;
                    default:// Something go wrong
                        return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }
        }
    }
}
