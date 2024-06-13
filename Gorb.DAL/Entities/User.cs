using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Gorb.DAL.Entities
{

    public class User
    {
        public int Id { get; set; }

        public string Nickname { get; set; }

        public double ExperienceBar { get; set; }

        public string HashedPassword { get; set; }

        public string Salt { get; set; }

        public int CurrentBackLvl { get; set; }

        public int Avatar { get; set; }

        public int NotificationTimeOut { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime {  get; set; }
    }


}