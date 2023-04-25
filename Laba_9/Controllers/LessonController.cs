using Laba_9.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web.Mvc;

namespace Laba_9.Controllers
{
    public class LessonController : Controller
    {
        //private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Инсаф\Desktop\Laba_9\Laba_9\db.mdb";
        private string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Инсаф\Desktop\123\Laba_8\Laba_9\db.mdb";
        public ActionResult Index()
        {
            DataSet dataSet = FillDataSet();
            List<Lesson> lessons = new List<Lesson>();
            DataRow[] studentRows;
            List<Lesson> ll = new List<Lesson>();
            studentRows = dataSet.Tables["Студенты"].Select("Курс = '" + 2 + "'");
            if (studentRows.Length == 0)
                return View();
            foreach (DataRow studRow in studentRows)
            {
                DataRow[] marksRows = studRow.GetChildRows("marks");
                foreach (DataRow markRow in marksRows)
                {
                    DataRow[] lessonsRows = markRow.GetParentRows("lessons1");
                    var less = from lsn in lessonsRows
                                select
                                new Lesson
                                {
                                    id = (int)lsn.ItemArray[0],
                                    lesson_name = (String)lsn.ItemArray[1],
                                    description = (String)lsn.ItemArray[2]
                                };
                    foreach (Lesson l in less) 
                        ll.Add(l);
                }
            }
            IEnumerable<Lesson> distinctList = ll 
            .GroupBy(l => l.id)
            .Select(g => g.First())
            .ToList();
            ViewBag.Lessons = distinctList;
            return View();
        }
        [HttpGet]
        public ActionResult Change(int id)
        {
            DataSet dataSet = FillDataSet();
            DataRow[] studentRows = dataSet.Tables["Студенты"].Select("Курс = '" + 2 + "'");
            List<Mark> mm = new List<Mark>();
            if (studentRows.Length == 0)
                return View();
            foreach (DataRow studRow in studentRows)
            {
                DataRow[] marksRows = studRow.GetChildRows("marks");
                var mrks = from mrk in marksRows
                            where (int)mrk.ItemArray[3] == id
                            select
                            new Mark
                            {
                                lesson_id = (int)mrk.ItemArray[3],
                                mark = (int)mrk.ItemArray[4],
                                name = (String)studRow.ItemArray[1]
                            };
                foreach (Mark m in mrks)
                    mm.Add(m);
            }
            ViewBag.LessonID = id;
            ViewBag.Marks = mm;
            return View();
        }

        [HttpPost]
        public string Change(Lesson lesson)
        {
            OleDbConnection connection = new
            OleDbConnection(connectionString);
            OleDbCommand query = new OleDbCommand($"UPDATE Оценки SET Оценка=2 WHERE Код_предмета={lesson.id} AND Код_студента IN (SELECT Код FROM Студенты WHERE Курс=2)", connection);
 
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();

            return "Теперь все двочечники!";
        }

        private DataSet FillDataSet()
        {
            OleDbConnection connection = new
            OleDbConnection(connectionString);
            OleDbDataAdapter studentsAdapter = new OleDbDataAdapter("SELECT * FROM [Студенты]", connection);
            OleDbDataAdapter marksAdapter = new OleDbDataAdapter("SELECT * FROM [Оценки]", connection);
            OleDbDataAdapter lessonsAdapter = new OleDbDataAdapter("SELECT * FROM [Предметы]", connection);

            DataSet dataSet = new DataSet();
            studentsAdapter.Fill(dataSet, "Студенты");
            marksAdapter.Fill(dataSet, "Оценки");
            lessonsAdapter.Fill(dataSet, "Предметы");
            dataSet.Relations.Add("marks", dataSet.Tables["Студенты"].Columns["Код"], dataSet.Tables["Оценки"].Columns["Код_студента"]);
            dataSet.Relations.Add("lessons1", dataSet.Tables["Предметы"].Columns["Код"], dataSet.Tables["Оценки"].Columns["Код_предмета"]);
            
            return dataSet;
        }
    }
}