using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Class representing register for holding flags
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Enumeration of all flags which can be stored in register
        /// </summary>
        public enum EFlags
        {
            /// <summary>
            /// Flag which informs about expression beeing an argument of function
            /// </summary>
            FARG = 0,

            /// <summary>
            /// Flag which informs that function is running
            /// </summary>
            FRUN = 1
        }

        /// <summary>
        /// Array with stored flags
        /// </summary>
        private bool[] flags;

        /// <summary>
        /// Creates new register for holding flags
        /// </summary>
        public Register()
        {
            this.flags = new bool[Enum.GetNames(typeof(EFlags)).Length];
            for (int i = 0; i < this.flags.Length; i++)
            {
                this.flags[i] = false;
            }
        }

        /// <summary>
        /// Sets flag of register
        /// </summary>
        /// <param name="flag">Flag which will be set</param>
        public void Set(EFlags flag)
        {
            this.flags[(int)flag] = true;
        }

        /// <summary>
        /// Unsets flag of register
        /// </summary>
        /// <param name="flag">Flag whihc will be unset</param>
        public void Unset(EFlags flag)
        {
            this.flags[(int)flag] = false;
        }

        /// <summary>
        /// Checks, whether flag is set or not
        /// </summary>
        /// <param name="flag">Flag which will be checked</param>
        /// <returns>TRUE if flag is set, FALSE otherwise</returns>
        public bool Get(EFlags flag)
        {
            return this.flags[(int)flag];
        }
    }
}
