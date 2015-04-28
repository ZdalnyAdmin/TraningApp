﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class Training
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TrainingID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int TrainingTypeID { get; set; }
        public TrainingType TrainingType { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserID { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserID { get; set; }



        private string _createUserName;
        public string CreateUserName
        {
            get { return _createUserName; }
        }
        public void SetCreateUserName(string name)
        {
            _createUserName = name;
        }

        private int _runTrainingStats;
        public int RunTrainingStats
        {
            get { return _runTrainingStats; }
        }
        public void SetRunTrainingStats(int value)
        {
            _runTrainingStats = value;
        }


        private string _assignedGroups;
        public string AssignedGroups
        {
            get { return _assignedGroups; }
        }
        public void SetAssignedGroups(List<string> groups)
        {
            if (groups != null && groups.Any())
            {
                _assignedGroups = String.Join(",", groups);
            }
        }

    }
}