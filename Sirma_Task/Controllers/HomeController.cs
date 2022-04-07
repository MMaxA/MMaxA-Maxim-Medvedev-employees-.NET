using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sirma_Task.Models;
using System.IO;

namespace Sirma_Task.Controllers
{
    public class HomeController : Controller
    {
        List<Employee> employees = new List<Employee>();
        List<pairEmp> pairEmp = new List<pairEmp>();

        [HttpGet]
        // GET: Home
        public ActionResult Index()
        {
            return View(new List<pairEmp>());
        }
        [HttpPost]
        // POST: Home
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            int duration = 0;
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                try
                {
                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            employees.Add(new Employee
                            {
                                EmpId = Convert.ToInt32(row.Split(';')[0]),
                                ProjectID = Convert.ToInt32(row.Split(';')[1]),
                                DateFrom = Convert.ToDateTime(row.Split(';')[2]),
                                DateTo = Convert.ToDateTime(row.Split(';')[3])
                            });
                        }
                    }

                    for (int x = 0; x < employees.Count; x++)
                    {
                        for (int y = 0; y < employees.Count; y++)
                        {
                            if (employees[x].ProjectID == employees[y].ProjectID && employees[x].EmpId != employees[y].EmpId)
                            {
                                if (employees[y].DateFrom > employees[x].DateFrom && employees[y].DateFrom < employees[x].DateTo)
                                {
                                    for (DateTime a = employees[x].DateFrom; a <= employees[x].DateTo; a = a.AddDays(1))
                                    {
                                        for (DateTime b = employees[y].DateFrom; b <= employees[y].DateTo; b = b.AddDays(1))
                                        {
                                            if (a == b)
                                                duration += 1;
                                        }
                                    }

                                    if (duration != 0)
                                    {
                                        pairEmp.Add(new pairEmp
                                        {
                                            ProjectID = employees[x].ProjectID,
                                            FirstEmpID = employees[x].EmpId,
                                            SecondEmpID = employees[y].EmpId,
                                            Duration = duration
                                        });
                                        duration = 0;
                                    }
                                }

                                if (employees[x].DateFrom > employees[y].DateFrom && employees[x].DateFrom <= employees[y].DateTo)
                                {
                                    for (DateTime a = employees[x].DateFrom; a <= employees[x].DateTo; a = a.AddDays(1))
                                    {
                                        for (DateTime b = employees[y].DateFrom; b <= employees[y].DateTo; b = b.AddDays(1))
                                        {
                                            if (a == b)
                                                duration += 1;
                                        }
                                    }

                                    if (duration != 0)
                                    {
                                        pairEmp.Add(new pairEmp
                                        {
                                            ProjectID = employees[x].ProjectID,
                                            FirstEmpID = employees[x].EmpId,
                                            SecondEmpID = employees[y].EmpId,
                                            Duration = duration
                                        });
                                        duration = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.ToString();
                }
            }
            return View(pairEmp);
        }

    }
}