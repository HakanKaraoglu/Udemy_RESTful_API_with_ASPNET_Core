using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Models.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(x => x.NationalPark).FirstOrDefault(y => y.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.OrderBy(a => a.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(x => x.NationalPark).Where(x => x.NationalParkId == npId).ToList();
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }


    }
}
