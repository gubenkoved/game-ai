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
                const string key = "moves-checked";
                return Data.ContainsKey(key) ? (int)Data[key] : 0;
            }
            set
            {
                const string key = "moves-checked";
                Data[key] = value;
            }
        }

        public double ElapsedSec
        {
            get
            {
                const string key = "elapsed-sec";
                return Data.ContainsKey(key) ? (double)Data[key] : 0;
            }
            set
            {
                const string key = "elapsed-sec";
                Data[key] = value;
            }
        }
    }
}
