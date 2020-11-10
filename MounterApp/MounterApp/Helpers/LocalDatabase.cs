using MounterApp.InternalModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MounterApp.Helpers {
    public class LocalDatabase {
        readonly SQLiteConnection _database;
        public LocalDatabase(string dbPath) {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<Mounts>();
        }
        //public Task<List<Mounts>> GetMountsAsync() {
        //    return _database.Table<Mounts>().ToListAsync();
        //}
        public List<Mounts> GetMounts(Guid _mounter) {
            return _database.Table<Mounts>().Where(x=>x.MounterID ==_mounter).ToList();
        }
        public int SaveMount(Mounts mount) {
            return _database.Insert(mount);
        }
    }
}
