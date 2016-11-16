using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Engines
{
    public class Metadata
    {
        #region Data
        public Dictionary<string, object> Data { get; private set; } = new Dictionary<string, object>();
        #endregion

        public int MovesChecked
        {
            get
            {
                return Get<int>("moves-checked");
            }
            set
            {
                Set("moves-checked", value);
            }
        }

        public double ElapsedSec
        {
            get
            {
                return Get<int>("elapsed-sec");
            }
            set
            {
                Set("elapsed-sec", value);
            }
        }

        public double Rate
        {
            get
            {
                return Get<double>("rate");
            }
            set
            {
                Set("rate", value);
            }
        }

        #region Private methods
        private T Get<T>(string key)
        {
            if (Data.ContainsKey(key))
            {
                return (T)Data[key];
            }
            else
            {
                return default(T);
            }
        }

        private void Set(string key, object value)
        {
            Data[key] = value;
        } 
        #endregion
    }
}
