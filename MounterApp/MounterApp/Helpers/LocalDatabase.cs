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
            return _database.Table<Mounts>().Where(x => x.MounterID == _mounter).ToList();
        }
        public List<Mounts> GetMounts(int _id) {
            return _database.Table<Mounts>().Where(x => x.ID == _id).ToList();
        }
        public int SaveMount(Mounts mount) {
            return _database.Insert(mount);
        }
        public int UpdateMount(Mounts mount) {
            return _database.Update(mount);
        }
        public int DeleteMount(int pk) {
            return _database.Delete<Mounts>(pk);
        }
        public int GetCurrentID() {
            int ret = 1;
            try {
                ret = _database.Table<Mounts>().OrderByDescending(x => x.ID).FirstOrDefault().ID + 1;
            }
            catch { }
            return ret;
        }
        public int ClearDatabase() {
            return _database.DeleteAll<Mounts>();
        }
    }
}
