﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;
using DataContainer.MySortedDictionary;

namespace DataContainer
{
    /// <summary>
    /// Информация о моменте времени
    /// </summary>
    [System.Serializable]
    public class AbstractTimeStampInfo<T> : IEnumerable<KeyValuePair<int, T>>, IDisposable
    {
        protected Dictionary<int, T> info;
        public Dictionary<int, T> Info
        {
            get { return info; }
            set { info = value; }
        }

        public AbstractTimeStampInfo(Dictionary<int, T> info)
        {
            this.info = info;
        }

        public AbstractTimeStampInfo(T value)
        {
            this.info = new Dictionary<int, T>(2);
            info.Add(0, value);
        }

        public AbstractTimeStampInfo()
        {
            this.info = new Dictionary<int, T>();
        }

        public int Count
        {
            get
            {
                return info.Count;
            }
        }

        public T this[Int32 index]
        {
            get
            {
                return info[index];
            }
            set
            {
                info[index] = value;
            }
        }

        public T LastValue
        {
            get 
            {
                if (info.Count != 0)
                    return info.Values.ElementAt(info.Count - 1);
                else
                    return default(T);
            }
        }

        public T FirstValue
        {
            get 
            {
                if (info.Count != 0)
                    return info.Values.ElementAt(0);
                else
                    return default(T);
            }
        }

        public int LastDeltaCycleNumber
        {
            get 
            {
                if (info.Count != 0)
                    return info.Keys.ElementAt(info.Count - 1);
                else
                    return 0;                    
            }
        }

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            foreach (var v in info)
                yield return v;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var v in info)
                yield return v;
        }

        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            if (info != null)
            {
                info.Clear();
            }
            info = null;
        }

        ~AbstractTimeStampInfo()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }
    }
}
