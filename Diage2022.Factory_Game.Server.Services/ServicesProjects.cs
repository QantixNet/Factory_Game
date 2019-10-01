using Diiage2022.Factory_Games.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diage2022.Factory_Game.Server.Services
{
    public partial class Services
    {
        /// <summary>
        /// this method allow to Generate Projects
        /// </summary>
        /// <param name="numberProjects"></param>
        /// <returns></returns>
        public int GenerateProjects(int numberProjects)
        {
            try
            {
                List<string> names = new List<string> { "WebSite", "WebApp", "UwpApp", "GameApp", "Forum" };

                for (int i = 1; i <= numberProjects; i++)
                {
                    Random random = new Random();
                    int index = random.Next(names.Count);

                    Project project = new Project()
                    {
                        ProjectID = Game.Projects.Count + 1,
                        ProjectName = names[index],
                        ProjectPenality = 0,
                    };

                    int skillsToGenerate = random.Next(1, 4);

                    for (int x = 1; x <= skillsToGenerate; x++)
                    {
                        List<Skill> currentSkills = Game.Skills.Intersect(project.Skills).ToList();
                        int skillIndex = random.Next(Game.Skills.Count);

                        while (currentSkills.Contains(Game.Skills[skillIndex]))
                        {
                            skillIndex = random.Next(Game.Skills.Count);
                        }

                        project.Skills.Add(Game.Skills[skillIndex]);
                    }

                    int variable = 0;
                    foreach (Skill oneSkill in project.Skills)
                    {
                        variable += (project.Skills.Count * oneSkill.SkillLevel);
                    }
                    double customDuration = Math.Floor(Game.RoundMax * ((double)variable / 27d));
                    project.ProjectDuration = Convert.ToInt16(customDuration);
                    Game.Projects.Add(project);
                }
                return Game.Projects.Count();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return -1;
            }

        }

        /// <summary>
        /// Add a project to a company
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public int AddProjectToCompany(int projectid, int companyid)
        {
            try
            {
                if (companyid == Game.CurrentPlayer)
                {
                    var companies = Game.Companies;
                    Company currentCompany = companies.FirstOrDefault(c => c.CompanyId == companyid);
                    var projects = Game.Projects;
                    Project project = projects.FirstOrDefault(p => p.ProjectID == projectid);
                    currentCompany.Projects.Add(project);
                    return currentCompany.Projects.Count;
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
        /// <summary>
        /// Assign a developer to a project
        /// </summary>
        /// <param name="developerid"></param>
        /// <param name="projectid"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public int AddDeveloperToProject(int developerid, int projectid, int companyid)
        {
            try
            {
                if (companyid == Game.CurrentPlayer)
                {
                    var companies = Game.Companies;
                    Company currentCompany = companies.FirstOrDefault(c => c.CompanyId == companyid);
                    var projects = currentCompany.Projects;
                    Project project = projects.FirstOrDefault(p => p.ProjectID == projectid);
                    var developers = currentCompany.Developers;
                    Developer developer = developers.FirstOrDefault(d => d.DeveloperId == developerid);
                    project.Developers.Add(developer);
                    return project.Developers.Count;
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

        public int UpdatingProject()
        {
            List<Project> projects = Game.Projects;
            projects.ForEach(p =>
            {
                List<Skill> developerSkills = p.Developers.SelectMany(d => d.DeveloperSkills).ToList();
                int skillRequired = 0;
                p.Skills.ForEach(s =>
                {
                    if (developerSkills.Contains(s))
                        skillRequired += 1;
                }
                );
                if (skillRequired == p.Skills.Count())
                    p.ProjectDuration -= 1;
            }
            );
            projects.RemoveAll(p => p.ProjectDuration == 0);
            return projects.Count;
        }
    }
}
