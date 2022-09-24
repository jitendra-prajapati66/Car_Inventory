using Car_Inventory.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Car_Inventory.Initialize
{
    public class Initializer : IInitializer
    {
        private readonly ApplicationDbContext _db;
        public Initializer(ApplicationDbContext db)
        {
            _db = db;
        }

        public void initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
