using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;

namespace PBTrainer.Utilities
{
    public static class Logger
    {
        public static void logMessage(string message)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    FileMode Fmode;
                    FileAccess Faccess;
                    if (store.FileExists("pbtrainer.log"))
                    {
                        Fmode = FileMode.Open;
                        Faccess = FileAccess.ReadWrite;
                    }
                    else
                    {
                        Fmode = FileMode.Create;
                        Faccess = FileAccess.Write;
                    }

                    using (StreamWriter writer = new StreamWriter(store.OpenFile("pbtrainer.log", Fmode, Faccess)))
                    {
                        writer.WriteLine(message);
                        writer.Close();
                    }
                }
            }
            catch (Exception)
            {
                //smother exception so we don't crash while logging
                //need a better way to log this
            }
        }
    }
}
