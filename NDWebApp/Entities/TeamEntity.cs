﻿using System.ComponentModel.DataAnnotations.Schema;

namespace NDWebApp.Entities
{
    public class TeamEntity
    {
        public int TeamId { get; set; }

        public string? TeamName { get; set; }

        public int  LeaderEmpNr { get; set; }
    }
}
