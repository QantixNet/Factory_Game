﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Diiage2022.Factory_Games.Client.Entities
{
    public class Communication
    {
        public int CommunicationId { set; get; }
        public int PlayerId { set; get; }
        public int RequestType { set; get; }
        public int DeveloperId { set; get; }
        public int ProjectId { set; get; }
        public string NamePlayer { set; get; }
        public int TrainingSessionId { set; get; }
        public int SchoolID { set; get; }
        public List<School> Schools { set; get; }
        public List<Developer> Developers { set; get; }
        public List<Project> Projects { set; get; }
        public List<Company> Companies { set; get; }
        public int ErrorChoice { set; get; }
        public string ErrorMessage { set; get; }
        public Configuration Configuration { get; set; }
        public int ActualRound { get; set; }

        public Communication()
        {
            Schools = new List<School>();
            Developers = new List<Developer>();
            Projects = new List<Project>();
        }
    }
}
