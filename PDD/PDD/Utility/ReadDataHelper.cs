using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using PDD.Models;
using SQLite;

namespace PDD.Utility
{
    internal class ReadDataHelper
    {
        internal static List<Picture> Pictures;

        public static ObservableCollection<T> GetAll<T>() where T : class, new()
        {
            try
            {
                using (var dbConn = new SQLiteConnection(App.DbPath))
                {
                    List<T> myCollection = dbConn.Table<T>().ToList<T>();
                    return new ObservableCollection<T>(myCollection);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public static void GetDataFromDb()
        {
            Pictures = GetAll<Picture>().ToList();
        }
    }
}
