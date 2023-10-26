using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cDataReader<T>: IDataReader
    {
        private readonly IList<T> aoItems;
        private List<string> atColumnNames;
        private int nCurrentIndex;

        public cDataReader(IList<T> paoItems)
        {
            aoItems = paoItems;
            nCurrentIndex = -1;
            FieldDictionary = PrepareFieldLookup();
        }

        private OrderedDictionary FieldDictionary { get; }

        public List<string> ColumnNames
        {
            get
            {
                if (atColumnNames == null)
                {
                    atColumnNames = new List<string>();
                    foreach (var kvp in FieldDictionary)
                    {
                        var colName = ((dynamic)kvp).Key;
                        atColumnNames.Add(colName);
                    }
                }
                return atColumnNames;
            }
        }

        public void Dispose()
        {
        }

        public object GetValue(int i)
        {
            string propName = ((dynamic)FieldDictionary[i]).Value;
            var currentItem = aoItems[nCurrentIndex];
            var propertyInfo = currentItem.GetType().GetProperty(propName);
            object value = null;
            if (propertyInfo != null)
            {
                value = propertyInfo.GetValue(currentItem, null);
            }
            return value;
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        public int GetOrdinal(string name)
        {
            return ((dynamic)FieldDictionary[name]).Key;
        }

        public int FieldCount => FieldDictionary.Count;

        public bool Read()
        {
            if (nCurrentIndex < aoItems.Count - 1)
            {
                nCurrentIndex++;
                return true;
            }
            return false;
        }

        public int Depth { get; } = 0;

        public bool IsClosed => aoItems.Count == nCurrentIndex;
        public int RecordsAffected => -1;

        private static OrderedDictionary PrepareFieldLookup()
        {
            var fieldLookup = new OrderedDictionary();
            var i = 0;

            foreach (var property in typeof(T).GetProperties())
            {
                fieldLookup.Add(property.Name, new KeyValuePair<int, string>(i++, property.Name));
            }
            return fieldLookup;
        }

        #region Not Implemented

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        object IDataRecord.this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        object IDataRecord.this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        #endregion Not Implemented
    }
}
