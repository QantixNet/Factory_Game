using System;
using System.Collections.Generic;
using System.Text;

namespace Diiage2022.Factory_Games.Client.Entities
{
    public class Game
    {
        public int Round { set; get; }
        public List<Company> Companies { set; get; }
        public List<Project> Projects { set; get; }
        public List<School> Schools { set; get; }
        public List<Developer> Developers { set; get; }
        public List<Skill> Skills { set; get; }
        public bool IsInAction { get; set; }
        public Company LocalPlayer { get; set; }
        public int LocalPlayerId { get; set; }
        public TrainingSession SelectedTrainingSession { get; set; }
        public Project SelectedMyProject { get; set; }
        public List<string> PlayersActions { get; set; }

        public int PlayerTurn { get; set; }

        public Game()
        {
            Projects = new List<Project>();
            Companies = new List<Company>();
            Schools = new List<School>();
            Developers = new List<Developer>();
            Skills = new List<Skill>();
            PlayersActions = new List<string>();
            IsInAction = false;
            PlayerTurn = 0; // set to 0 for nothing
            //Init();
        }

        public void Init()
        {
            Developer developper = new Developer()
            {
                DeveloperId = 1,
                DeveloperName = "jean",
                DeveloperSalary = 1000,
                DeveloperSkills = new List<Skill>(),
                Hired = false,
                ImageUrl = "https://dev.azure.com/martinbalme/_apis/GraphProfile/MemberAvatars/aad.MzIzMDEyNjEtYjIxMS03NWJiLWIzYTgtZGE2NDEyOTQzNGEz"
            };

            Developer developper1 = new Developer()
            {
                DeveloperId = 1,
                DeveloperName = "kevin",
                DeveloperSalary = 10000000,
                DeveloperSkills = new List<Skill>(),
                Hired = true,
                ImageUrl = "https://dev.azure.com/martinbalme/_apis/GraphProfile/MemberAvatars/aad.MzIzMDEyNjEtYjIxMS03NWJiLWIzYTgtZGE2NDEyOTQzNGEz"
            };

            Developer developper2 = new Developer()
            {
                DeveloperId = 1,
                DeveloperName = "poplololopo",
                DeveloperSalary = 10000,
                DeveloperSkills = new List<Skill>(),
                Hired = false,
                ImageUrl = "https://dev.azure.com/martinbalme/_apis/GraphProfile/MemberAvatars/aad.MzIzMDEyNjEtYjIxMS03NWJiLWIzYTgtZGE2NDEyOTQzNGEz"
            };

            Skill java = new Skill()
            {
                SkillId = 1,
                SkillLevel = 3,
                SkillName = "java"
            };

            Skill csharp = new Skill()
            {
                SkillId = 2,
                SkillLevel = 1,
                SkillName = "csharp"
            };

            Skill jquery = new Skill()
            {
                SkillId = 3,
                SkillLevel = 2,
                SkillName = "jquery"
            };

            developper.DeveloperSkills.Add(java);
            developper.DeveloperSkills.Add(jquery);
            developper.DeveloperSkills.Add(csharp);
            developper1.DeveloperSkills.Add(csharp);

            Company company = new Company()
            {
                CompanyId = 1,
                Developers = new List<Developer>(),
                Projects = new List<Project>(),
                Funds = 10000,
                Username = "player1"
            };

            Company company1 = new Company()
            {
                CompanyId = 2,
                Developers = new List<Developer>(),
                Funds = 100000,
                Username = "player2"
            };

            company.Developers.Add(developper1);

            Project project = new Project()
            {
                ProjectDuration = 2,
                ProjectID = 1,
                ProjectName = "app wib",
                ProjectPenality = 0,
                ProjectRemuneration = 10,
                Skills = new List<Skill>(),
                Developers = new List<Developer>(),
                Company = null,
                ImageUrl = "https://massif-du-jura.developpement-edf.com/images/icones-projet/edf-projet-process00.png"
            };

            project.Skills.Add(java);
            project.Skills.Add(jquery);

            School school = new School()
            {
                SchoolId = 1,
                SchoolName = "Diiage",
                SchoolTrainingSessions = new List<TrainingSession>(),
                ImageUrl = "http://diiage.cucdb.fr/wp-content/uploads/sites/4/2014/10/diiage-couleur1.png"
            };

            TrainingSession trainingSession = new TrainingSession()
            {
                Name = "Formation Cobol",
                School = school,
                TrainingSessionDevelopers = new List<Developer>(),
                TrainingSessionDuration = 2,
                TrainingSessionId = 1,
                TrainingSessionSkill = null,
                TraningSessionCost = 100
            };

            TrainingSession trainingSession1 = new TrainingSession()
            {
                Name = "Formation C#",
                School = school,
                TrainingSessionDevelopers = new List<Developer>(),
                TrainingSessionDuration = 2,
                TrainingSessionId = 2,
                TrainingSessionSkill = null,
                TraningSessionCost = 1000
            };

            Skill cobol = new Skill()
            {
                SkillId = 4,
                SkillLevel = 1,
                SkillName = "cobol"
            };

            trainingSession.TrainingSessionSkill = cobol;
            trainingSession1.TrainingSessionSkill = csharp;
            school.SchoolTrainingSessions.Add(trainingSession);
            school.SchoolTrainingSessions.Add(trainingSession1);

            LocalPlayer = company;
            Companies.Add(company);
            Companies.Add(company1);
            Developers.Add(developper);
            Developers.Add(developper1);
            Developers.Add(developper2);
            Projects.Add(project);
            Schools.Add(school);
        }
    }
}
