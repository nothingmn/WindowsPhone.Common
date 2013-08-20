using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercisr.Contracts.Configuration;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Repository;

namespace Exercisr.Domain.Configuration
{
    public class Settings : ISettings
    {

        
        public event SettingsChanged OnSettingsChanged;
        private readonly IRepository _repository;
        private readonly ILog _log;

        public Settings(IRepository repository, ILog log) : this()
        {
            _repository = repository;
            _log = log;
        }
        public Settings()
        {
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if(_log!=null) _log.Info("Setting updated: Id=" + value.ToString());
                _id = value;
                if (OnSettingsChanged != null) OnSettingsChanged(this, "Id");
            }
        }

        private bool _IsMetric;
        public bool IsMetric
        {
            get { return _IsMetric; }
            set
            {
                if (_log != null) _log.Info("Setting updated: IsMetric=" + value.ToString());
                _IsMetric = value;
                if (OnSettingsChanged != null) OnSettingsChanged(this, "IsMetric");
            }
        }

        private bool _AutoPostToRunKeeper;
        public bool AutoPostToRunKeeper
        {
            get { return _AutoPostToRunKeeper; }
            set
            {
                if (_log != null) _log.Info("Setting updated: AutoPostToRunKeeper=" + value.ToString());
                _AutoPostToRunKeeper = value;
                if (OnSettingsChanged != null) OnSettingsChanged(this, "AutoPostToRunKeeper");
            }
        }

        public void Save()
        {
            if (Id <= 0)
            {
                _log.Info("Saving new settings");
                _repository.Insert<Settings>(this);

            }
            else
            {
                _log.Info("Update existing settings");
                _repository.Update<Settings>(this, this.Id);
            }
        }

        public void Load()
        {
            _log.Info("loading settings from db");
            _repository.Query<Settings>("select * from Settings").ContinueWith(t =>
                {
                    var first = t.Result.FirstOrDefault();
                    if (first != null)
                    {
                        _log.Info("loaded settings from db, updating current values");
                        this.Id = first.Id;
                        this.AutoPostToRunKeeper = first.AutoPostToRunKeeper;
                        this.IsMetric = first.IsMetric;
                    }
                });
        }
    }
}
