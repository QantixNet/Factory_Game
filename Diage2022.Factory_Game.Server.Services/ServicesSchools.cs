using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Diiage2022.Factory_Games.Entities;
using System.Linq;
using System.Threading;


namespace Diage2022.Factory_Game.Server.Services
{
    partial class Services
    {
        /// <summary>
        /// Add a developer to a training session
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="developerId"></param>
        /// <param name="schoolID"></param>
        /// <param name="trainingSessionID"></param>
        /// <returns></returns>
        public int AddDeveloperToTrainingSession(int companyId, int developerId, int schoolID, int trainingSessionID)
        {
            School school = Game.Schools.FirstOrDefault(s=> s.SchoolId==schoolID);
            TrainingSession trainingSession = school.SchoolTrainingSessions.FirstOrDefault(t=> t.TrainingSessionId==trainingSessionID);
            Company currentCompany = Game.Companies.FirstOrDefault(c => c.CompanyId == companyId);
            Developer developer = currentCompany.Developers.FirstOrDefault(d => d.DeveloperId == developerId);
            developer.TrainingSession = trainingSession;
            developer.InATrainingSession = true;
            trainingSession.TrainingSessionDevelopers.Add(developer);
            return trainingSession.TrainingSessionDevelopers.Count();
        }
        /// <summary>
        /// When the turn of the all players is done this method is called in order to 
        /// update the number of rounds remaining before the end of the training session
        /// </summary>
        /// <returns></returns>
        public int UpdateTrainingSessions()
        {
            List<School> schools = Game.Schools;
            List<TrainingSession> trainingSessions = schools.SelectMany(s => s.SchoolTrainingSessions).ToList();
            trainingSessions.ForEach(t =>
            {
                t.TrainingSessionDuration -= 1;
            });
            List<TrainingSession> finishedTrainingSession = trainingSessions.Where(t => t.TrainingSessionDuration == 0).ToList();
            finishedTrainingSession.ForEach(f => 
            {
                f.TrainingSessionDevelopers.ForEach(t => 
                {
                    t.DeveloperSkills.Add(t.TrainingSession.TrainingSessionSkill);
                    t.TrainingSession = null;
                    t.InATrainingSession = false;
                }
                );
            }
            );
            trainingSessions.RemoveAll(t => t.TrainingSessionDuration == 0);
            return trainingSessions.Count;
        }

        /// <summary>
        /// This method generate Schools an Training Sessions
        /// </summary>
        /// <returns></returns>
        public int GenerateSchoolTrainingSessions()
        {
            Random random = new Random();
            School school = new School()
            {
                SchoolId = Game.Schools.Count + 1,
                SchoolName = schoolsNames[random.Next(schoolsNames.Count)],
                SchoolTrainingSessions = new List<TrainingSession>(),
                ImageUrl = schoolsImages[random.Next(schoolsImages.Count)]
            };

            for(int index = 0; index < random.Next(3); index++)
            {
                Skill skill = Game.Skills[random.Next(Game.Skills.Count)];
                int sessionDuration = skill.SkillLevel + 1;
                int sessionCost = 1;
                string sessionName = "Formation " + skill.SkillName;
                TrainingSession trainingSession = new TrainingSession()
                {
                    Accessibility = true,
                    TrainingSessionDevelopers = new List<Developer>(),
                    TrainingSessionDuration = sessionDuration,
                    TrainingSessionId = school.SchoolTrainingSessions.Count + 1,
                    TrainingSessionSkill = skill,
                    TraningSessionCost = sessionCost,
                    Name = sessionName,
                    School = school
                };

                school.SchoolTrainingSessions.Add(trainingSession);
            }
            Game.Schools.Add(school);
            return Game.Schools.Count;
        }
    }
}
