using Gorb.DAL.DB;
using Microsoft.EntityFrameworkCore;

namespace Gorb.Server.Services
{
    public class UserDataServices
    {
        private readonly ApplicationDbContext _dbContext;
        public UserDataServices(ApplicationDbContext context)
        { 
            _dbContext = context;
        }
        public void SetExperience(int Id, int AmountExperience)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == Id);
            if (user != null)
            {

                double newAmount = user.ExperienceBar+Convert.ToDouble(AmountExperience);
                double rawnewlvl = newAmount / 100;
                string str = Convert.ToString(rawnewlvl);
                string[] parts = str.Split(',');
                int newlvl = Convert.ToInt32(parts[0]);
                user.ExperienceBar = newAmount;
                user.CurrentBackLvl = newlvl;
                _dbContext.SaveChanges();
            }
        }
    }
}
