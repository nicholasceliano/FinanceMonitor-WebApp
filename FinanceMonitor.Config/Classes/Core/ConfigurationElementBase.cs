using System;
using System.Configuration;

namespace FinanceMonitor.Config
{
    public abstract class ConfigurationElementCollectionBase<T> : ConfigurationElementCollection where T : ConfigurationElementBase
    {
        #region Method Overrides

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return (ConfigurationElement)Activator.CreateInstance(typeof(T));
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)element).GetKeyValue();
        }

        #endregion

        #region Collection Methods

        public T this[int index]
        {
            get { return (T)this.BaseGet(index); }
            set
            {
                if ((this.BaseGet(index) != null))
                    this.BaseRemoveAt(index);
                this.BaseAdd(index, value);
            }
        }

        public T this[object key]
        {
            get { return (T)this.BaseGet(key); }
        }

        public int IndexOf(T item)
        {
            return this.BaseIndexOf(item);
        }

        public void Add(T item)
        {
            this.BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            this.BaseAdd(element, false);
        }

        public void Remove(T item)
        {
            if (this.BaseIndexOf(item) >= 0)
                this.BaseRemove(item.GetKeyValue());
        }

        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        public void Remove(object key)
        {
            this.BaseRemove(key);
        }

        public void Clear()
        {
            this.BaseClear();
        }

        #endregion
    }

    public abstract class ConfigurationElementBase : global::System.Configuration.ConfigurationElement
    {
        #region Abstract Methods

        public abstract object GetKeyValue();

        #endregion
    }
}