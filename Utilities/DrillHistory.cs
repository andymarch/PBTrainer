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

using PBTrainer.Model;
using System.Xml;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using System.IO;

namespace PBTrainer.Utilities
{
    public class DrillHistory
    {
        public static void AddToDrillHistory(Drill drill)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    bool fileexist = store.FileExists("DrillHistory.xml");
                    FileMode Fmode;
                    FileAccess Faccess;
                    if (fileexist)
                    {
                        Fmode = FileMode.Open;
                        Faccess = FileAccess.ReadWrite;
                    }
                    else
                    {
                        Fmode = FileMode.Create;
                        Faccess = FileAccess.Write;
                    }

                    using (IsolatedStorageFileStream fs = store.OpenFile("DrillHistory.xml", Fmode, Faccess))
                    {
                        XDocument xDoc;
                        if (!fileexist)
                        {
                            xDoc = new XDocument();
                            xDoc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
                            XElement root = new XElement("DrillHistory");
                            xDoc.Add(root);
                        }
                        else
                        {
                            xDoc = XDocument.Load(fs);
                        }

                        XElement EL_Drill = new XElement("Drill");
                        EL_Drill.Add(new XAttribute("ID",new Guid()));
                        EL_Drill.Add(new XElement("Description", drill.description));
                        EL_Drill.Add(new XElement("Feedback", drill.feedback));
                        EL_Drill.Add(new XElement("Timestamp", DateTime.Now));
                        string repValue = drill.Template_RepNumber.ToString();
                        if (repValue == "-1")
                        {
                            repValue = "Infinite";
                        }
                        EL_Drill.Add(new XElement("Reps", repValue));
                        EL_Drill.Add(new XElement("RepsCompleted",drill.repPosition));
                        EL_Drill.Add(new XElement("DrillDuration", drill.Template_DrillDuration.ToString()));
                        EL_Drill.Add(new XElement("ReadyTime", drill.Template_ReadyTime.ToString()));
                        EL_Drill.Add(new XElement("WarningTime", drill.Template_WarningTime.ToString()));
                        EL_Drill.Add(new XElement("ResetTime", drill.ResetTime.ToString()));
                        xDoc.Root.AddFirst(EL_Drill);
                        
                        fs.Position = 0;
                        xDoc.Save(fs);
                        fs.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went worng saving that drill, sorry. Please contact the developer.");
            }
        }

        public static XDocument ReadDrillHistory()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if(myIsolatedStorage.FileExists("DrillHistory.xml"))
                    {
                        using (IsolatedStorageFileStream isoFileStream = myIsolatedStorage.OpenFile("DrillHistory.xml", FileMode.Open))
                        {
                            XDocument doc = XDocument.Load(isoFileStream);
                            return doc;
                        }
                    }
                    else
                    {
                        //document does not exist
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Whoops can't load your drill history. Please contact the developer.");
                return null;
            }
        }

    }
}
